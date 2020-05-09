# Perfectphase.Azure.AppService.EasyAuth

Azure App Service Authentication (EasyAuth) middleware for ASP.NET CORE that implements support for Authenticate, Challenge, Forbidden and SignOut. 

For the moment this project only supports AzureAd, not Google, twitter etc.

## What is `EasyAuth`?

Azure `App Service` has a feature to turn on Authentication on top of your application code. This is useful if you don't want to handle the nitty gritty of auth. It's meant to be a quick and easy way to put an authentication layer above an application hosted on an app service. More details can be found here https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization.

There is a how to get started tutorial [here](https://www.benday.com/2018/05/17/walkthrough-part-2-configure-app-service-authentication-for-your-azure-web-app/).

# Why this project

This library adds middleware that reads the `X-MS-CLIENT-PRINCIPAL` header and extracts the claims and creates a ClaimsPrincipal that can be used by the ASP.NET CORE identity system.

```csharp
services.AddAuthentication(AzureAdDefaults.AuthenticationScheme)
    .AddEasyAuthAzureAd());
```

The middleware also supports the Challenge, Forbidden and SignOut handlers so you will be redirected to the correct endpoints supported by EasyAuth if you chose not to have EasyAuth force the user to authenticate for you.

# Development

Because EasyAuth does all the hard work of dealing with OAuth2/OpenIdConnect for you, this cause an issue on your local development box, as the magic `X-MS-CLIENT-PRINCIPAL` header will not be available.  To make local development a bit easier you can use the `Development` auth handler.

```csharp
if (_environment.IsDevelopment())
{
    // Dummy auth for dev machine where Easy Auth isn't available
    services.AddAuthentication(DevelopmentAuthenticationDefaults.AuthenticationScheme)
        .AddDevelopment(o =>
        {
            o.Claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "Bloggs"));
            o.Claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "Fred"));
        });
}
else
{
    // App Service Easy Auth using Azure AD
    services.AddAuthentication(AzureAdDefaults.AuthenticationScheme)
        .AddEasyAuthAzureAd();
}
```

This middleware adds some default claims for you but you can add extra or remove existing ones if you want.

# App Roles
If you've defined custom appRoles in your Enterprise app's manifest in Azure AD, you need to override the claim type used to map to the roles as below

```csharp
services.AddAuthentication(AzureAdDefaults.AuthenticationScheme)
.AddEasyAuthAzureAd(o =>
{
    // Override the default role type returned from EasyAuth
    o.RoleClaimType = "roles";
});
```

# Selectively restricting access

The middleware just creates a standard `ClaimsPrincipal` and `ClaimsIdentity` which gets assigned to the `Request.User` object, so all the standard ASP.NET CORE methods work such as the `[Authorize]` attribute or conventions for razor pages, e.g.

```csharp
// Add authentication to selected pages
services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizePage("/RestrictedPage");
        options.Conventions.AuthorizePage("/RequiresRole", "RequireRole3");  //Use the policy defined below
    });

// Create a role based policy for our demo pages
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireRole3",
            policy => policy.RequireRole("role3"));
});

```