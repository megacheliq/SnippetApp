using MediatR.Pipeline;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Services
{
    /// <summary>
    /// Препроцессор авторизации на основе направления сниппета
    /// </summary>
    /// <typeparam name="T">Тип команды с направлением сниппета</typeparam>
    public class AuthPreProcessorByDirection<T> : IRequestPreProcessor<T> where T : class, ISnippetDirectionCommand
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public AuthPreProcessorByDirection(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Task Process(T request, CancellationToken cancellationToken)
        {
            var user = HttpContextAccessor.HttpContext
                .GetAuthUser();

            if (user.IsAdmin || user.UserDirection == request.Direction)
                return Task.CompletedTask;

            throw new NotAuthorizedException();
        }
    }
}
