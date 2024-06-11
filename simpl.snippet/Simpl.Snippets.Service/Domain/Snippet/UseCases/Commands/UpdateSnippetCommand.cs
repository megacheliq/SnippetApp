using FluentValidation;
using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Domain.Snippet.Validators;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands
{
    /// <summary>
    /// Команда обновления сниппета
    /// </summary>
    public class UpdateSnippetCommand : IRequest, ISnippetIdCommand
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Данные сниппета.
        /// </summary>
        public AddOrUpdateSnippetDto Dto { get; set; }
    }

    public class UpdateSnippetCommandHandler : IRequestHandler<UpdateSnippetCommand>
    {
        private UpdateSnippetCommandValidator Validator { get; }
        private ISnippetRepository Repository { get; }
        private ILogger<UpdateSnippetCommandHandler> Logger { get; }

        public UpdateSnippetCommandHandler(ISnippetRepository repository, UpdateSnippetCommandValidator validator, ILogger<UpdateSnippetCommandHandler> logger)
        {
            Validator = validator;
            Repository = repository;
            Logger = logger;
        }

        public async Task Handle(UpdateSnippetCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await Validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            await Repository.UpdateAsync(request.Id, request.Dto, cancellationToken);

            Logger.LogInformation($"The snippet with id {request.Id} was updated");
        }
    }
}
