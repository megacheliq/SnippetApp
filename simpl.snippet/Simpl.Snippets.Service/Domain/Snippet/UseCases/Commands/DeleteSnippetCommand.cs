using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands
{
    /// <summary>
    /// Команда удаления сниппета
    /// </summary>
    public class DeleteSnippetCommand : IRequest, ISnippetIdCommand
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        public string Id { get; set; }
    }

    public class DeleteSnippetCommandHandler : IRequestHandler<DeleteSnippetCommand>
    {
        private ISnippetRepository Repository { get; }
        private ILogger<DeleteSnippetCommandHandler> Logger { get; }

        public DeleteSnippetCommandHandler(ISnippetRepository repository, ILogger<DeleteSnippetCommandHandler> logger)
        {
            Repository = repository;
            Logger = logger;
        }

        /// <summary>
        /// Обработать команду удаления сниппета
        /// </summary>
        /// <param name="request">Команда удаления сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public async Task Handle(DeleteSnippetCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await Repository.DeleteAsync(request.Id, cancellationToken);

            Logger.LogInformation($"The snippet with id {request.Id} was deleted");
        }
    }
}
