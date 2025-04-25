using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Perfectphase.Azure.AppService.EasyAuth.Development;

public class DevelopmentAuthenticationHandler : AuthenticationHandler<DevelopmentAuthenticationOptions>
{
    public DevelopmentAuthenticationHandler(IOptionsMonitor<DevelopmentAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
    }

    /// <summary>
    /// The handler calls methods on the events which give the application control at certain points where processing is occurring.
    /// If it is not provided a default instance is supplied which does nothing when the methods are called.
    /// </summary>
    protected new DevelopmentAuthenticationEvents Events
    {
        get => (DevelopmentAuthenticationEvents)base.Events;
        set => base.Events = value;
    }

    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new DevelopmentAuthenticationEvents());

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(Options.Claims, Scheme.Name, Options.NameClaimType, Options.RoleClaimType);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        Logger.LogTrace("Constructed the Claims Principal successfully.");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
    
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        var accessDeniedUri = Options.AccessDeniedPath;
        var redirectContext = new RedirectContext<DevelopmentAuthenticationOptions>(Context, Scheme, Options, properties, BuildRedirectUri(accessDeniedUri));
        await Events.RedirectToAccessDenied(redirectContext);
    }
    
}
