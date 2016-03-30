
# All we should have to do is
* Start from this code sample & the patterns used therein: https://github.com/Azure-Samples/active-directory-dotnet-windowsphone-8.1 
* Replace the adal library in ^that sample with this one: https://www.nuget.org/packages/Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory/4.0.209160138-alpha (which will involve some API changes, like going from sync  async)
* -> Install-Package Microsoft.Experimental.IdentityModel.Clients.ActiveDirectory -Pre 
* Re-create the user flows from this app: https://github.com/AzureADQuickStarts/B2C-NativeClient-DotNet, or whatever user flows you think make the most sense.

