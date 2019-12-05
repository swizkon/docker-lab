using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SayHelloApp
{
    public static class OpenIdConnectConfiguration
    {
        public static void AddOpenIdConnectAuthentication(this IServiceCollection services, IHostingEnvironment env,
            IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var openIdConnectSettings = new OpenIdConnectSettings();
            configuration.GetSection("Security:OpenIdConnectSettings").Bind(openIdConnectSettings);

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.Authority = openIdConnectSettings.Authority;
            //        options.ClientId = openIdConnectSettings.ClientId;
            //        options.RequireHttpsMetadata = openIdConnectSettings.RequireHttpsMetadata;

            //        options.ResponseMode = "form_post";
            //        options.ResponseType = "code";
                    // options.ResponseType = "token id_token";

            //        options.GetClaimsFromUserInfoEndpoint = openIdConnectSettings.GetClaimsFromUserInfoEndpoint;

                    //options.ClientId = "implicit";

                    // options.GetClaimsFromUserInfoEndpoint = openIdConnectSettings.GetClaimsFromUserInfoEndpoint;
            //        foreach (var scope in openIdConnectSettings.Scopes.Split(' '))
            //        {
            //            options.Scope.Add(scope);
            //        }

            //        options.SaveTokens = openIdConnectSettings.SaveTokens;
            //    });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = openIdConnectSettings.Authority;
                    options.ClientId = openIdConnectSettings.ClientId;

                    options.RequireHttpsMetadata = false;

                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    options.SaveTokens = true;
                });
        }
    }
}