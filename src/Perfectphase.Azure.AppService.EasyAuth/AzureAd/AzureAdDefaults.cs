using Microsoft.AspNetCore.Http;

namespace Perfectphase.Azure.AppService.EasyAuth.AzureAd
{
    public class AzureAdDefaults
    {
        public const string AuthenticationScheme = "EasyAuthAzureAd";
        public static readonly PathString LoginPath = "/.auth/login/aad";
        public static readonly PathString LogoutPath = "/.auth/logout";
        public static readonly PathString AccessDeniedPath = "/AccessDenied";
        public static readonly string LoginReturnUrlParameter = "post_login_redirect_url";
        public static readonly string LogoutReturnUriParameter = "post_logout_redirect_uri";
    }
}
