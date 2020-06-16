namespace Sitecore.Plugin.IdentityProvider.AzureB2C.Configuration
{
    public class AppSettings
    {
        public static readonly string SectionName = "Sitecore:ExternalIdentityProviders:IdentityProviders:AzureB2C";

        public AzureB2CIdentityProvider AzureB2CIdentityProvider { get; set; } = new AzureB2CIdentityProvider();
    }
}
