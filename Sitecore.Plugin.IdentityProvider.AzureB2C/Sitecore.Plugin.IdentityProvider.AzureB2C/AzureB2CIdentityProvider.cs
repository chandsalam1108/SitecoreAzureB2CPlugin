namespace Sitecore.Plugin.IdentityProvider.AzureB2C
{
    public class AzureB2CIdentityProvider : Sitecore.Plugin.IdentityProviders.IdentityProvider
    {
        public string ClientId { get; set; }
        public string Tenant { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string AzureADB2CInstance { get; set; }
        public string RedirectUri { get; set; }
        public string ClientSecret { get; set; }
    }
}
