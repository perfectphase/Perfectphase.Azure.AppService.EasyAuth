using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Perfectphase.Azure.AppService.EasyAuth.AzureAd;


public class AzureAdEvents
{
    public Func<RedirectContext<AzureAdOptions>, Task> OnRedirectToLogin { get; set; } = context =>
    {
        if (IsAjaxRequest(context.Request))
        {
            context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
            context.Response.StatusCode = 401;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    public Func<RedirectContext<AzureAdOptions>, Task> OnRedirectToAccessDenied { get; set; } = context =>
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

    public Func<RedirectContext<AzureAdOptions>, Task> OnRedirectToLogout { get; set; } = context =>
    {
        if (IsAjaxRequest(context.Request))
        {
            context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    public Func<RedirectContext<AzureAdOptions>, Task> OnRedirectToReturnUrl { get; set; } = context =>
    {
        if (IsAjaxRequest(context.Request))
        {
            context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
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
    public virtual Task RedirectToLogout(RedirectContext<AzureAdOptions> context) => OnRedirectToLogout(context);

    /// <summary>
    /// Implements the interface method by invoking the related delegate method.
    /// </summary>
    /// <param name="context">Contains information about the event</param>
    public virtual Task RedirectToLogin(RedirectContext<AzureAdOptions> context) => OnRedirectToLogin(context);

    /// <summary>
    /// Implements the interface method by invoking the related delegate method.
    /// </summary>
    /// <param name="context">Contains information about the event</param>
    public virtual Task RedirectToReturnUrl(RedirectContext<AzureAdOptions> context) => OnRedirectToReturnUrl(context);

    /// <summary>
    /// Implements the interface method by invoking the related delegate method.
    /// </summary>
    /// <param name="context">Contains information about the event</param>
    public virtual Task RedirectToAccessDenied(RedirectContext<AzureAdOptions> context) => OnRedirectToAccessDenied(context);
}
