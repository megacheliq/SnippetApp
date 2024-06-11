using MediatR.Pipeline;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Services
{
    /// <summary>
    /// Препроцессор авторизации на основе идентификатора сниппета
    /// </summary>
    /// <typeparam name="T">Тип команды с идентификатором сниппета</typeparam>
    public class AuthPreProcessorById<T> : IRequestPreProcessor<T> where T : class, ISnippetIdCommand
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        private ISnippetRepository Repository { get; }

        public AuthPreProcessorById(IHttpContextAccessor httpContextAccessor, ISnippetRepository repository)
        {
            HttpContextAccessor = httpContextAccessor;
            Repository = repository;
        }

        public async Task Process(T request, CancellationToken cancellationToken)
        {
            var user = HttpContextAccessor.HttpContext
                .GetAuthUser();

            if (user.IsAdmin)
                return;

            var snippetInfo = await Repository.GetByIdOrDefaultAsync(request.Id, cancellationToken);

            if (user.Id != snippetInfo.AuthorId)
                throw new NotAuthorizedException();
        }
    }
}
