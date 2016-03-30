//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Globalization;
using System.Net.Http.Headers;
using Windows.Data.Json;
using System.Linq;

namespace TodoListClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// The page implements IWebAuthenticationContinuable, as it is necessary for pages containing actions that can trigger authentication
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region init

        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The Tenant is the name of the Azure AD tenant in which this application is registered.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        //

        const string aadInstance = "https://login.microsoftonline.com/{0}";
        
        private HttpClient httpClient = new HttpClient();
        private AuthenticationContext authContext = null;

        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            // ADAL for Windows Phone 8.1 builds AuthenticationContext instances through a factory, which performs authority validation at creation time
            authContext = new AuthenticationContext(Globals.aadInstance + Globals.tenant);
        }

        #region Callbacks
        // Retrieve the user's To Do list.
        public async void GetTodoList(AuthenticationResult result)
        {
            //
            // Add the access token to the Authorization Header of the call to the To Do list service, and call the service.
            //
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(result.TokenType, result.Token);
            HttpResponseMessage response = await httpClient.GetAsync(Globals.todoListBaseAddress + "/api/todolist");

            if (response.IsSuccessStatusCode)
            {
                // Read the response as a Json Array and databind to the GridView to display todo items
                var todoArray = JsonArray.Parse(await response.Content.ReadAsStringAsync());

                TodoList.ItemsSource = from todo in todoArray
                                        select new
                                        {
                                            Title = todo.GetObject()["Title"].GetString()
                                        };
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // If the To Do list service returns access denied, clear the token cache and have the user sign-in again.
                    MessageDialog dialog = new MessageDialog("Sorry, you don't have access to the To Do Service.  Please sign-in again.");
                    await dialog.ShowAsync();
                    authContext.TokenCache.Clear();
                }
                else
                {
                    MessageDialog dialog = new MessageDialog("Sorry, an error occurred accessing your To Do list.  Please try again.");
                    await dialog.ShowAsync();
                }
            }
        }

        // Post a new item to the To Do list.
        public async void AddTodo(AuthenticationResult result)
        {
            //
            // Add the access token to the Authorization Header of the call to the To Do list service, and call the service.
            //
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(result.TokenType, result.Token);
            HttpContent content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("Title", txtTodo.Text) });

            // Call the todolist web api
            var response = await httpClient.PostAsync(Globals.todoListBaseAddress + "/api/todolist", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the response as a Json Array and databind to the GridView to display todo items
                txtTodo.Text = "";
                GetTodoList(result);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // If the To Do list service returns access denied, clear the token cache and have the user sign-in again.
                    MessageDialog dialog = new MessageDialog("Sorry, you don't have access to the To Do Service.  Please sign-in again.");
                    await dialog.ShowAsync();
                    authContext.TokenCache.Clear();
                }
                else
                {
                    MessageDialog dialog = new MessageDialog("Sorry, an error occurred accessing your To Do list.  Please try again.");
                    await dialog.ShowAsync();
                }
            }
        }
        #endregion

        #region AppBar buttons

        // clear the token cache
        private void RemoveAppBarButton_Click(object sender, RoutedEventArgs e)
        {
             // Clear session state from the token cache.
            authContext.TokenCache.Clear();

            // Reset UI elements
            TodoList.ItemsSource = null;
            SignInButton.Visibility = Visibility.Visible;
            SignUpButton.Visibility = Visibility.Visible;
            EditProfileButton.Visibility = Visibility.Collapsed;
            SignOutButton.Visibility = Visibility.Collapsed;
            UsernameLabel.Text = String.Empty;
        }

        // fetch the user's To Do list from the service. If no tokens are present in the cache, trigger the authentication experience before performing the call
        private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult result = await GetTokenSilent();

            // A token was successfully retrieved. Get the To Do list for the current user
            GetTodoList(result);
        }
        #endregion

        private async Task<AuthenticationResult> GetTokenSilent()
        {
            //// Try to get a token without triggering any user prompt. 
            //// ADAL will check whether the requested token is in the cache or can be obtained without user itneraction (e.g. via a refresh token).
            AuthenticationResult result = null;
            try
            {
                result = await authContext.AcquireTokenSilentAsync(new string[] { Globals.clientId }, Globals.clientId);
            }
            catch (AdalException ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Inner Exception : " + ex.InnerException.Message;
                }

                MessageDialog dialog = new MessageDialog(message);
                await dialog.ShowAsync();
            }

            return result;
        }

        // Post a new item to the To Do list. If no tokens are present in the cache, trigger the authentication experience before performing the call
        private async void btnAddTodo_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult result = await GetTokenSilent();

            // A token was successfully retrieved. Post the new To Do item
            AddTodo(result);
        }

        private async void SignUp(object sender, RoutedEventArgs e)
        {
            AuthenticationResult result = null;
            try
            {
                result = await authContext.AcquireTokenAsync(new string[] { Globals.clientId },
                    null, Globals.clientId, new Uri(Globals.redirectUri),
                    null, Globals.signUpPolicy);

                SignInButton.Visibility = Visibility.Collapsed;
                SignUpButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                SignOutButton.Visibility = Visibility.Visible;
                UsernameLabel.Text = result.UserInfo.Name;
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode == "authentication_canceled")
                {
                    MessageDialog dialog = new MessageDialog("Sign up was canceled by the user");
                    await dialog.ShowAsync();
                }
                else
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageDialog dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                }

                return;
            }
        }

        private async void SignIn(object sender, RoutedEventArgs e)
        {
            AuthenticationResult result = null;
            try
            {
                var platformParams = new PlatformParameters();

                result = await authContext.AcquireTokenAsync(new string[] { Globals.clientId },
                    null, Globals.clientId, new Uri(Globals.redirectUri),
                    platformParams, Globals.signInPolicy);

                SignInButton.Visibility = Visibility.Collapsed;
                SignUpButton.Visibility = Visibility.Collapsed;
                EditProfileButton.Visibility = Visibility.Visible;
                SignOutButton.Visibility = Visibility.Visible;
                UsernameLabel.Text = result.UserInfo.Name;
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode == "authentication_canceled")
                {
                    MessageDialog dialog = new MessageDialog("Sign in was canceled by the user");
                    await dialog.ShowAsync();
                }
                else
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }

                    MessageDialog dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                }

                return;
            }
        }

        private void EditProfile(object sender, RoutedEventArgs e)
        {

        }

        private void SignOut(object sender, RoutedEventArgs e)
        {

        }
    }
}
