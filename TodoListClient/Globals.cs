namespace TodoListClient
{
    public static class Globals
    {
        // TODO: Replace these with your own configuration values
        public static string tenant = "[Tenant name of your Azure AD B2C e.g. contoso.onmicrosoft.com]";
        public static string clientId = "[Client ID of B2C App created in Portal e.g. ac12a187-c653-4236-970c-65807dc4413d]";
        public static string signInPolicy = "[Sign in policy name as created in Portal e.g. B2C_1_todolist_signin]";
        public static string signUpPolicy = "[Sign up policy name as created in Portal e.g. B2C_1_todolist_signup]";
        public static string editProfilePolicy = "[Edit profile policy name as created in Portal e.g. B2C_1_todolist_profile]";

        public static string aadInstance = "https://login.microsoftonline.com/";
        public static string redirectUri = "urn:ietf:wg:oauth:2.0:oob";

        public static string todoListBaseAddress = "[Base URL of ToDo List of service published to Azure App Service e.g. https://my-todolist-service.azurewebsites.net]";

    }
}
