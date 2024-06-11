using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries
{
    /// <summary>
    /// Запрос на получение случайного сниппета
    /// </summary>
    public class RandomSnippetQuery : IRequest<RandomSnippetResponse>, ISnippetDirectionCommand
    {
        /// <summary>
        /// Направление сниппета
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Уровень сниппета
        /// </summary>
        public Level Level { get; set; }
    }
   
    public class RandomSnippetQueryHandler : IRequestHandler<RandomSnippetQuery, RandomSnippetResponse>
    {
        private ISnippetRepository Repository { get; }

        public RandomSnippetQueryHandler(ISnippetRepository repository)
        {
            Repository = repository;
        }

        public async Task<RandomSnippetResponse> Handle(RandomSnippetQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await Repository.GetRandomAsync(request, cancellationToken);          

            return result ?? throw new NoDataFoundException("Не найдено ниодного снипета для данного направления и уровня");
        }
    }

}
