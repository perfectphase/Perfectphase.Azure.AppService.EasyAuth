using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Perfectphase.Azure.AppService.EasyAuth;
using Perfectphase.Azure.AppService.EasyAuth.AzureAd;
using Perfectphase.Azure.AppService.EasyAuth.Development;

namespace Sample.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                // Dummy auth for dev machine where Easy Auth isn't available
                services.AddAuthentication(DevelopmentAuthenticationDefaults.AuthenticationScheme)
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
                services.AddAuthentication(AzureAdDefaults.AuthenticationScheme)
                    .AddEasyAuthAzureAd(o =>
                    {
                        // Override the default role type returned from EasyAuth
                        o.RoleClaimType = "roles";
                    });
            }
            
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
