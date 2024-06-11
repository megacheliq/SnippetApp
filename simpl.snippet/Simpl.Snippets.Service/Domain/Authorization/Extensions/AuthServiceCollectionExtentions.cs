using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Simpl.Snippets.Service.Domain.Authorization.Models;
using Simpl.Snippets.Service.Domain.Authorization.Services;
using System.Net;

namespace Simpl.Snippets.Service.Domain.Authorization.Extensions
{
    /// <summary>
    /// Расширения для конфигурации авторизации Keycloak
    /// </summary>
    public static class AuthServiceCollectionExtentions
    {
        /// <summary>
        /// Добавляет конфигурацию авторизации Keycloak в сервисы
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="logger">Логгер</param>
        /// <returns>Коллекция сервисов с добавленной конфигурацией авторизации</returns>
        public static IServiceCollection AddKeycloakAuthorization(
            this IServiceCollection services,
            IConfiguration configuration,
            ILogger logger)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var authOptionsSection = configuration.GetSection("AuthorizationOptions");
            services.Configure<AuthOptions>(authOptionsSection);
            var authOptions = authOptionsSection.Get<AuthOptions>();

            // Добавление HttpContextAccessor в сервисы
            services.AddHttpContextAccessor();

            services.AddSingleton<IAuthorizationHandler, SnippetUserAuthorizationHandler>();

            // Конфигурация аутентификации
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // Настройка параметров валидации токена
                    options.Authority = "http://keycloak:8080/realms/simpl/";
                    options.Audience = authOptions.Audience;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(5),
                        NameClaimType = "preferred_username"
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();

                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var exception = context.Exception;

                            logger.LogError(exception, "Exception occured");

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var user = new AuthUser(context.Principal, authOptions);
                            context.Principal.AddIdentity(user);

                            return Task.CompletedTask;
                        },
                    };
                });

            return services;
        }
    }
}
