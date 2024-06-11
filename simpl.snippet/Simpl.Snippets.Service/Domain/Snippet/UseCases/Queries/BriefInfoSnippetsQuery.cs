using FluentValidation;
using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Snippet.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries
{
    /// <summary>
    /// Коллекция сниппетов для получения краткой информации
    /// </summary>
    public class BriefInfoSnippetsQuery : IRequest<IEnumerable<BriefInfoSnippetResponse>>, ISnippetDirectionCommand
    {
        /// <summary>
        /// Направление сниппета
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Уровень сниппета
        /// </summary>
        public Level? Level { get; set; }

        /// <summary>
        /// Идентификатор автора сниппета
        /// </summary>
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Начальная дата создания сниппета
        /// </summary>
        public DateTimeOffset? CreatedDateStart { get; set; }

        /// <summary>
        /// Конечная дата создания сниппета
        /// </summary>
        public DateTimeOffset? CreatedDateEnd { get; set; }

        /// <summary>
        /// Начальная дата изменения сниппета
        /// </summary>
        public DateTimeOffset? ModifiedDateStart { get; set; }

        /// <summary>
        /// Конечная дата изменения сниппета
        /// </summary>
        public DateTimeOffset? ModifiedDateEnd { get; set; }
    }

    public class BriefInfoSnippetsQueryHandler : IRequestHandler<BriefInfoSnippetsQuery, IEnumerable<BriefInfoSnippetResponse>>
    {
        private ISnippetRepository Repository { get; }
        private IValidator<BriefInfoSnippetsQuery> Validator { get; }

        public BriefInfoSnippetsQueryHandler(ISnippetRepository snippetRepository, IValidator<BriefInfoSnippetsQuery> validator)
        {
            Repository = snippetRepository;
            Validator = validator;
        }

        public async Task<IEnumerable<BriefInfoSnippetResponse>> Handle(BriefInfoSnippetsQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await Validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var result = await Repository.GetFilteredAsync(request, cancellationToken);

            return result;
        }
    }
}
