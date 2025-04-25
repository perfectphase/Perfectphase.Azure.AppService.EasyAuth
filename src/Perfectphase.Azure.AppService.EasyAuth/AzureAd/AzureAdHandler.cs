using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Perfectphase.Azure.AppService.EasyAuth.Shared;
using Claim = System.Security.Claims.Claim;

namespace Perfectphase.Azure.AppService.EasyAuth.AzureAd;

public class AzureAdHandler : SignOutAuthenticationHandler<AzureAdOptions> //AuthenticationHandler<AzureAdOptions>
{
    public AzureAdHandler(IOptionsMonitor<AzureAdOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
    }

    /// <summary>
    /// The handler calls methods on the events which give the application control at certain points where processing is occurring.
    /// If it is not provided a default instance is supplied which does nothing when the methods are called.
    /// </summary>
    protected new AzureAdEvents Events
    {
        get => (AzureAdEvents)base.Events;
        set => base.Events = value;
    }

    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new AzureAdEvents());

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {

        if (Context.Request.Headers.ContainsKey(KnownEasyAuthHeaders.PrincipalIdpHeaderName) &&
            Context.Request.Headers[KnownEasyAuthHeaders.PrincipalIdpHeaderName] == "aad")
        {
            var (claims, nameType, roleType) = Map(Context.Request.Headers);
            if (claims != null)
            {
                var identity = new ClaimsIdentity(claims, Scheme.Name, Options.NameClaimType ?? nameType, Options.RoleClaimType ?? roleType);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                Logger.LogTrace("Constructed the Claims Principal successfully.");
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            
            return Task.FromResult(AuthenticateResult.Fail("Failed to constructed the Claims Principal successfully."));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        var redirectUri = properties.RedirectUri;
        if (string.IsNullOrEmpty(redirectUri))
        {
            redirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
        }

        var loginUri = Options.LoginPath + QueryString.Create(Options.LoginReturnUrlParameter, redirectUri);
        var redirectContext = new RedirectContext<AzureAdOptions>(Context, Scheme, Options, properties, BuildRedirectUri(loginUri));
        await Events.RedirectToLogin(redirectContext);
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        var returnUrl = properties.RedirectUri;
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = OriginalPathBase + OriginalPath + Request.QueryString;
        }
        var accessDeniedUri = Options.AccessDeniedPath + QueryString.Create(Options.LoginReturnUrlParameter, returnUrl);
        var redirectContext = new RedirectContext<AzureAdOptions>(Context, Scheme, Options, properties, BuildRedirectUri(accessDeniedUri));
        await Events.RedirectToAccessDenied(redirectContext);
    }

    protected override async Task HandleSignOutAsync(AuthenticationProperties properties)
    {
        var returnUrl = properties.RedirectUri;
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = OriginalPathBase + OriginalPath + Request.QueryString;
        }
        var accessDeniedUri = Options.LogoutPath + QueryString.Create(Options.LogoutReturnUriParameter, returnUrl);
        var redirectContext = new RedirectContext<AzureAdOptions>(Context, Scheme, Options, properties, BuildRedirectUri(accessDeniedUri));
        await Events.RedirectToLogout(redirectContext);
    }

    public (IList<Claim>? claims, string? nameType, string? roleType) Map(IHeaderDictionary headers)
    {
        if (!string.IsNullOrEmpty(headers[KnownEasyAuthHeaders.PrincipalObjectHeader]))
        {
            var claims = new List<Claim>();
            Logger.LogTrace($"Building claims from payload in {KnownEasyAuthHeaders.PrincipalObjectHeader} header.");

            try
            {
                var headerValue = headers[KnownEasyAuthHeaders.PrincipalObjectHeader].First();
                if (headerValue is null)
                {
                    Logger.LogWarning($"{KnownEasyAuthHeaders.PrincipalObjectHeader} header was null.");
                    return (null, null, null);
                }

                var payload = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue));
                var data = JsonSerializer.Deserialize<EasyAuthPrincipalModel>(payload);
                if (data == null)
                {
                    Logger.LogWarning($"Deserialization of {KnownEasyAuthHeaders.PrincipalObjectHeader} header failed.");
                    return (null, null, null);
                }

                foreach (var claimsModel in data.Claims)
                {
                    var claimType = claimsModel.Type;
                    var ignoreClaimType = Options.IgnoreClaimTypes.Any(c =>
                        c.Equals(claimType, StringComparison.OrdinalIgnoreCase));

                    if (!ignoreClaimType)
                    {
                        claims.Add(new Claim(claimType, claimsModel.Value));
                    }
                    else
                    {
                        Logger.LogTrace("Ignoring ClaimType: {ClaimType}", claimType);
                    }
                }

                return (claims, data.NameClaimType, data.RoleClaimType);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Building claims from header failed");
                return (null, null, null);
            }
        }

        Logger.LogWarning($"{KnownEasyAuthHeaders.PrincipalObjectHeader} header was not present or in the expected format.");
        return (null, null, null);
    }


}
