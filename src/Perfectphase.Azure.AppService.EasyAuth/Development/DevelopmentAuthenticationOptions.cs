using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Perfectphase.Azure.AppService.EasyAuth.Development;

public class DevelopmentAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The AccessDeniedPath property is used by the handler for the redirection target when handling ForbidAsync.
    /// </summary>
    public PathString AccessDeniedPath { get; set; }
    public string? RoleClaimType { get; set; }
    public string? NameClaimType { get; set; }
    public List<Claim> Claims { get; init; }

    public DevelopmentAuthenticationOptions()
    {
        // Add default claims
        Claims =
        [
            new("http://schemas.microsoft.com/identity/claims/objectidentifier",
                "3BC18C21-220E-4210-9842-FA047B30E5AD"),
            new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", "fred@anon.com"),
            new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "fred@anon.com"),
            new("name", "Fred Bloggs"),
            new("aud", "4FF16EB1-E80C-4A5D-8525-7DD6B8EB0376")
        ];
    }
    
    
}
