namespace TodoListClient
{
    public static class Globals
    {
        // TODO: Replace these with your own configuration values
        public static string tenant = "zeissb2cdev.onmicrosoft.com";
        public static string clientId = "ac55a187-c663-4236-970c-65807dc4413d";
        public static string signInPolicy = "B2C_1_todolist_signin";
        public static string signUpPolicy = "B2C_1_todolist_signup";
        public static string editProfilePolicy = "B2C_1_todolist_profile";

        public static string taskServiceUrl = "https://localhost:44321";
        public static string aadInstance = "https://login.microsoftonline.com/";
        public static string redirectUri = "urn:ietf:wg:oauth:2.0:oob";
    }
}
