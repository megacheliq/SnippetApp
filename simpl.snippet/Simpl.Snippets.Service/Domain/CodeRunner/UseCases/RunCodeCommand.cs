using MediatR;
using Simpl.Snippets.Service.Domain.CodeRunner.Abstract;
using Simpl.Snippets.Service.Domain.CodeRunner.Models;

namespace Simpl.Snippets.Service.Domain.CodeRunner.UseCases
{
    public class RunCodeCommand : IRequest<CodeOutputResponse>
    {
        /// <summary>
        /// Язык снипета
        /// </summary>
        public SnippetLanguage Language { get; set; }

        /// <summary>
        /// Код снипета
        /// </summary>
        public string SnippetCode { get; set; }
    }

    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, CodeOutputResponse>
    {
        private ICodeRunnerFactory CodeRunnerFactory { get; }

        public RunCodeCommandHandler(ICodeRunnerFactory codeRunnerFactory)
        {
            CodeRunnerFactory = codeRunnerFactory;
        }

        public async Task<CodeOutputResponse> Handle(RunCodeCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var consoleOutput = await CodeRunnerFactory.RunCodeSnippetAsync(request.Language, request.SnippetCode, cancellationToken);
            var result = new CodeOutputResponse { Output = consoleOutput };

            return result;
        }
    }
}
