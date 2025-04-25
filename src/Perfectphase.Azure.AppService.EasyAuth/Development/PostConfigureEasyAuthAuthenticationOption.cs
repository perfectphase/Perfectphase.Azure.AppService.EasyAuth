using Microsoft.Extensions.Options;

namespace Perfectphase.Azure.AppService.EasyAuth.Development;

public class PostConfigureDevelopmentAuthenticationOption : IPostConfigureOptions<DevelopmentAuthenticationOptions>
{
    public void PostConfigure(string? name, DevelopmentAuthenticationOptions options)
    {
        if (!options.AccessDeniedPath.HasValue)
        {
            options.AccessDeniedPath = DevelopmentAuthenticationDefaults.AccessDeniedPath;
        }

        options.RoleClaimType ??= DevelopmentAuthenticationDefaults.RoleClaimType;

        options.NameClaimType ??= DevelopmentAuthenticationDefaults.NameClaimType;
    }
}
