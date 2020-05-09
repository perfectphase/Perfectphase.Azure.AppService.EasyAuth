using Microsoft.AspNetCore.Http;

namespace Perfectphase.Azure.AppService.EasyAuth.Development
{
    public class DevelopmentAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Development";
        public static readonly PathString AccessDeniedPath = "/AccessDenied";

        public static readonly string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public static readonly string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    }
}
