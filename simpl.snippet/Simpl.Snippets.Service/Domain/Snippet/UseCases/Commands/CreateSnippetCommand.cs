using FluentValidation;
using MediatR;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Abstract;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Domain.Snippet.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands
{
    /// <summary>
    /// Команда создания нового сниппета
    /// </summary>
    public class CreateSnippetCommand : IRequest, ISnippetDirectionCommand
    {
        /// <summary>
        /// Направление сниппета
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Данные сниппета
        /// </summary>
        public AddOrUpdateSnippetDto CommandDto { get; set; }
    }

    public class CreateSnippetCommandHandler : IRequestHandler<CreateSnippetCommand>
    {
        public CreateSnippetCommandHandler(ISnippetRepository snippetService, IValidator<CreateSnippetCommand> validator, IHttpContextAccessor httpContextAccessor, ILogger<CreateSnippetCommandHandler> logger)
        {
            SnippetRepository = snippetService;
            Validator = validator;
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
        }

        private ISnippetRepository SnippetRepository { get; }
        private IValidator<CreateSnippetCommand> Validator { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private ILogger<CreateSnippetCommandHandler> Logger { get; }

        public async Task Handle(CreateSnippetCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await Validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

            var user = HttpContextAccessor.HttpContext
                .GetAuthUser();

            var snippet = new TSnippet
            {
                AuthorId = user.Id,
                AuthorName = user.FullName,
                Theme = request.CommandDto.Theme,
                Direction = request.Direction,
                Level = request.CommandDto.Level,
                CodeSnippet = request.CommandDto.CodeSnippet,
                MainQuestion = request.CommandDto.MainQuestion,
                Solution = request.CommandDto.Solution,
                AdditionalQuestions = request.CommandDto.AdditionalQuestions,
                CreatedDate = DateTimeOffset.UtcNow,
                ModifiedDate = DateTimeOffset.UtcNow
            };

            await SnippetRepository.CreateAsync(snippet, cancellationToken);

            Logger.LogInformation($"The snippet with id {snippet.Id} was inserted");
        }
    }
}
