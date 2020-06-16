using Sitecore.Plugin.IdentityProvider.AzureB2C.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sitecore.Framework.Runtime.Configuration;

namespace Sitecore.Plugin.IdentityProvider.AzureB2C
{
    public class ConfigureSitecore
    {
        private readonly ILogger<ConfigureSitecore> _logger;
        private readonly AppSettings _settings;


        public ConfigureSitecore(ISitecoreConfiguration scConfig, ILogger<ConfigureSitecore> logger)
        {
            this._logger = logger;
            this._settings = new AppSettings();
            scConfig.GetSection(AppSettings.SectionName)
            .Bind((object)this._settings.AzureB2CIdentityProvider);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var provider = _settings.AzureB2CIdentityProvider;
            if (!provider.Enabled) return;
            this._logger.LogInformation($"Configure '{provider.DisplayName}' with the following setup: ClientId='{provider.ClientId}', ClientSecret='{provider.ClientSecret}', Tenant='{provider.Tenant}', SignUpSignInPolicyId='{provider.SignUpSignInPolicyId}', RedirectUrl='{provider.RedirectUri}' ");

            new AuthenticationBuilder(services).AddOpenIdConnect(provider.AuthenticationScheme, provider.DisplayName, options =>
            {
                //This is required by Sitecore's implementation of IDS
                options.SignInScheme = "idsrv.external";
                options.UseTokenLifetime = true;
                options.SaveTokens = true;
                options.Authority = $"{provider.AzureADB2CInstance}/{provider.Tenant}/{provider.SignUpSignInPolicyId}/v2.0/";
                options.ClientId = provider.ClientId;
                options.ClientSecret = provider.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                //Azure AD requires us to pass the ClientId as a claim for the integration to work
                options.Scope.Add(provider.ClientId);
                options.Scope.Add(OpenIdConnectScope.OpenIdProfile);
                options.Scope.Add(OpenIdConnectScope.OfflineAccess);
            });
        }
    }
}
