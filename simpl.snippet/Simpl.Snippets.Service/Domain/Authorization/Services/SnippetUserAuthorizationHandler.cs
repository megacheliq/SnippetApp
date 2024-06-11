using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Domain.Authorization.Models;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Services
{
    /// <summary>
    /// Обработчик авторизации пользователя сниппетов
    /// </summary>
    public class SnippetUserAuthorizationHandler : AuthorizationHandler<SnippetUserRequirement>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        private AuthOptions Options { get; }

        public SnippetUserAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IOptions<AuthOptions> options)
        {
            HttpContextAccessor = httpContextAccessor;
            Options = options.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SnippetUserRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
                throw new NotAuthorizedException();

            var user = HttpContextAccessor.HttpContext
                .GetAuthUser();

            if (user.Roles.Contains(Options.UserRole))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
