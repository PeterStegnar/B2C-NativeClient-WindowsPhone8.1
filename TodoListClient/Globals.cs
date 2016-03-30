using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListClient
{
    public static class Globals
    {
        // TODO: Replace these with your own configuration values
        public static string tenant = "[Enter the name of your B2C directory, e.g. contoso.onmicrosoft.com]";
        public static string clientId = "[Enter the Application Id assinged to your app by the Azure portal, e.g.580e250c-8f26-49d0-bee8-1c078add1609]";
        public static string signInPolicy = "[Enter your sign in policy name, e.g. b2c_1_sign_in]";
        public static string signUpPolicy = "[Enter your sign up policy name, e.g. b2c_1_sign_up]";
        public static string editProfilePolicy = "[Enter your edit profile policy name, e.g. b2c_1_profile_edit]";

        public static string taskServiceUrl = "https://localhost:44321";
        public static string aadInstance = "https://login.microsoftonline.com/";
        public static string redirectUri = "urn:ietf:wg:oauth:2.0:oob";
    }
}
