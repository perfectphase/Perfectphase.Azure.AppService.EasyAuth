using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Perfectphase.Azure.AppService.EasyAuth.AzureAd;
using Perfectphase.Azure.AppService.EasyAuth.Development;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyAuthExtensions
    {
        // AzureAd
        public static AuthenticationBuilder AddEasyAuthAzureAd(this AuthenticationBuilder builder)
            => builder.AddEasyAuthAzureAd(AzureAdDefaults.AuthenticationScheme);

        public static AuthenticationBuilder AddEasyAuthAzureAd(this AuthenticationBuilder builder, string authenticationScheme)
            => builder.AddEasyAuthAzureAd(authenticationScheme, configureOptions: null);

        public static AuthenticationBuilder AddEasyAuthAzureAd(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
            => builder.AddEasyAuthAzureAd(AzureAdDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddEasyAuthAzureAd(this AuthenticationBuilder builder, string authenticationScheme, Action<AzureAdOptions> configureOptions)
            => builder.AddEasyAuthAzureAd(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddEasyAuthAzureAd(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<AzureAdOptions>, PostConfigureEasyAuthAzureAdOption>());
            builder.Services.AddOptions<AzureAdOptions>(authenticationScheme);
            return builder.AddScheme<AzureAdOptions, AzureAdHandler>(authenticationScheme, displayName, configureOptions);
        }



        // Development
        public static AuthenticationBuilder AddDevelopment(this AuthenticationBuilder builder) 
            => builder.AddDevelopment(DevelopmentAuthenticationDefaults.AuthenticationScheme);

        public static AuthenticationBuilder AddDevelopment(this AuthenticationBuilder builder, string authenticationScheme)
            => builder.AddDevelopment(authenticationScheme, configureOptions: null);

        public static AuthenticationBuilder AddDevelopment(this AuthenticationBuilder builder, Action<DevelopmentAuthenticationOptions> configureOptions)
            => builder.AddDevelopment(DevelopmentAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddDevelopment(this AuthenticationBuilder builder, string authenticationScheme, Action<DevelopmentAuthenticationOptions> configureOptions)
            => builder.AddDevelopment(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddDevelopment(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<DevelopmentAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<DevelopmentAuthenticationOptions>, PostConfigureDevelopmentAuthenticationOption>());
            builder.Services.AddOptions<DevelopmentAuthenticationOptions>(authenticationScheme);
            return builder.AddScheme<DevelopmentAuthenticationOptions, DevelopmentAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }


    }
}
