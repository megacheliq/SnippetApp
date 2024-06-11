using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Snippet.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries
{
    /// <summary>
    /// Запрос на получение информации о сниппете по его идентификатору
    /// </summary>
    public class SnippetInfoQuery : IRequest<FullSnippetInfoResponse>, ISnippetIdCommand
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        public string Id { get; set; }
    }

    public class SnippetInfoQueryHandler : IRequestHandler<SnippetInfoQuery, FullSnippetInfoResponse>
    {
        private ISnippetRepository Repository { get; }
       
        public SnippetInfoQueryHandler(ISnippetRepository reposiroty)
        {
            Repository = reposiroty;
        }

        /// <summary>
        /// Обработать запрос на получение информации о сниппете по его идентификатору.
        /// </summary>
        /// <param name="request">Запрос на получение информации о сниппете.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public async Task<FullSnippetInfoResponse> Handle(SnippetInfoQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await Repository.GetByIdOrDefaultAsync(request.Id, cancellationToken);
        }
    }
}
