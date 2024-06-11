using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries
{
    public class SnippetAuthorQuery : IRequest<IEnumerable<AuthorDto>>, ISnippetDirectionCommand
    {
        /// <summary>
        /// Направление
        /// </summary>
        public Direction Direction { get; set; }
    }

    public class SnippetAuthorQueryHandler : IRequestHandler<SnippetAuthorQuery, IEnumerable<AuthorDto>>
    {
        private IAuthorRepository Repository { get; }

        public SnippetAuthorQueryHandler(IAuthorRepository repository)
        {
            Repository = repository;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(SnippetAuthorQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await Repository.GetAll(request.Direction, cancellationToken);

            return result;
        }
    }
}
