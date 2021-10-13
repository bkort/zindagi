using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Zindagi.Domain.UserAggregate.DomainEvents;

namespace Zindagi.Infra.Auth
{
    public static class DependencyInjection
    {
        public static void AddAuthProvider(this IServiceCollection services, IConfiguration config)
        {
            var domain = config["AuthProvider:Domain"];
            var clientId = config["AuthProvider:ClientId"];
            var clientSecret = config["AuthProvider:ClientSecret"];

            if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                throw new MissingFieldException("AuthProvider config missing, please check.");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options => options.Cookie.Name = "zg.auth")
                .AddOpenIdConnect(AuthConstants.AuthenticationScheme, options =>
                {
                    options.Authority = $"https://{domain}";
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;

                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.CorrelationCookie.Name = "zg.correlation.";
                    options.NonceCookie.Name = "zg.nonce.";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    options.SaveTokens = true;

                    options.CallbackPath = AuthConstants.CallbackPath;

                    options.ClaimsIssuer = AuthConstants.ClaimsIssuer;

                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var builder = services.BuildServiceProvider();
                            var mediator = builder.GetService<IMediator>();
                            if (mediator != null && context.Principal != null)
                                await mediator.Publish(new UserLoggedIn(context.Principal));
                        },
                        OnRedirectToIdentityProviderForSignOut = context =>
                        {
                            var logoutUri = $"https://{config["AuthProvider:Domain"]}/v2/logout?client_id={config["AuthProvider:ClientId"]}";

                            var postLogoutUri = context.Properties.RedirectUri;
                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/", StringComparison.Ordinal))
                                {
                                    var request = context.Request;
                                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
