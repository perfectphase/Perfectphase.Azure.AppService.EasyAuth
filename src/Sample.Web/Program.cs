using Perfectphase.Azure.AppService.EasyAuth.AzureAd;
using Perfectphase.Azure.AppService.EasyAuth.Development;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Dummy auth for dev machine where Easy Auth isn't available
    builder.Services.AddAuthentication(DevelopmentAuthenticationDefaults.AuthenticationScheme)
        .AddDevelopment(o =>
        {
            o.RoleClaimType = "roles";
            o.Claims.Add(new Claim("roles", "role1"));
            o.Claims.Add(new Claim("roles", "role2"));
        });
}
else
{
    // App Service Easy Auth using Azure AD
    builder.Services.AddAuthentication(AzureAdDefaults.AuthenticationScheme)
        .AddEasyAuthAzureAd(o =>
        {
            // Override the default role type returned from EasyAuth
            o.RoleClaimType = "roles";
        });
}

// Add authentication to selected pages
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizePage("/RestrictedPage");
        options.Conventions.AuthorizePage("/RequiresRole", "RequireRole3");  //Use the policy defined below
    });

// Create a role based policy for our demo pages
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireRole3",
         policy => policy.RequireRole("role3"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
