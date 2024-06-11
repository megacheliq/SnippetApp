using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Simpl.Snippets.Service.Domain.Authorization.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Extensions
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Применить глобальную политику аутентификации
        /// </summary>
        public static void RequireGlobalAuthentication(this MvcOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var globalPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new SnippetUserRequirement())
                .Build();

            options.Filters.Add(new AuthorizeFilter(globalPolicy));
        }
    }
}
