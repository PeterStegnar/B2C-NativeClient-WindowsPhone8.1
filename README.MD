# Azure Active Directory B2C Native Client Windows Phone 8.1 sample

This simple Windows Phone 8.1 app shows how to authenticate using Azure AD B2C policies. 
The Web Api accessed by the app is secured    

# Use this sample

## Step 1 : download the Windows Phone 8.1 B2C Native Client Sample code

From your shell or command line:

```
git clone https://github.com/KaiWalter/B2C-NativeClient-WindowsPhone8.1.git
```

## Step 2 : configure App in B2C tenant


## Step 3 : configure settings in Web.Config

``` XML
    <add key="ida:Tenant" value="[Tenant name of your Azure AD B2C e.g. contoso.onmicrosoft.com]" />
    <add key="ida:ClientId" value="[Client ID of B2C App created in Portal e.g. ac12a187-c653-4236-970c-65807dc4413d]" />
    <add key="ida:PolicyId" value="[Sign in policy name as created in Portal e.g. B2C_1_todolist_signin]" />
```



...

## Step 4 : publish service

...

## Step 5 : Configure settings in Globals.cs


``` C#
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
```
