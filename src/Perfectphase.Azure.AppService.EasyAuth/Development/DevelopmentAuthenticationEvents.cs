using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Perfectphase.Azure.AppService.EasyAuth.Development;


public class DevelopmentAuthenticationEvents
{
    public Func<RedirectContext<DevelopmentAuthenticationOptions>, Task> OnRedirectToAccessDenied { get; set; } = context =>
    {
        if (IsAjaxRequest(context.Request))
        {
            context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
            context.Response.StatusCode = 403;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
    
    private static bool IsAjaxRequest(HttpRequest request)
    {
        return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
            string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
    }
    
    /// <summary>
    /// Implements the interface method by invoking the related delegate method.
    /// </summary>
    /// <param name="context">Contains information about the event</param>
    public virtual Task RedirectToAccessDenied(RedirectContext<DevelopmentAuthenticationOptions> context) => OnRedirectToAccessDenied(context);
}
