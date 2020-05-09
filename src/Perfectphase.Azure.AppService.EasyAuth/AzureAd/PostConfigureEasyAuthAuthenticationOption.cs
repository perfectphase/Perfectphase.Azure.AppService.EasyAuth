using System;
using Microsoft.Extensions.Options;

namespace Perfectphase.Azure.AppService.EasyAuth.AzureAd
{
    public class PostConfigureEasyAuthAzureAdOption : IPostConfigureOptions<AzureAdOptions>
    {
        public void PostConfigure(string name, AzureAdOptions options)
        {
            if (!options.LoginPath.HasValue)
            {
                options.LoginPath = AzureAdDefaults.LoginPath;
            }
            if (!options.LogoutPath.HasValue)
            {
                options.LogoutPath = AzureAdDefaults.LogoutPath;
            }
            if (!options.AccessDeniedPath.HasValue)
            {
                options.AccessDeniedPath = AzureAdDefaults.AccessDeniedPath;
            }
        }
    }
}
