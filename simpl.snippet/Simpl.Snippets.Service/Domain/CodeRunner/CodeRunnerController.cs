using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.CodeRunner.Models;
using Simpl.Snippets.Service.Domain.CodeRunner.UseCases;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries;
using Simpl.Snippets.Service.Exceptions.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Simpl.Snippets.Service.Domain.CodeRunner
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeRunnerController : ControllerBase
    {
        private IMediator Mediator { get; }

        public CodeRunnerController(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Запустить на выполнение код и получить его консольный вывод
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Консольный вывод</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("RunCode")]
        public Task<CodeOutputResponse> RunCodeAsync(RunCodeCommand command, CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Выполнить код снипета по Id
        /// </summary>
        /// <param name="snippetId">Id снипета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpPost("RunSnippet/{snippetId}")]
        public async Task<CodeOutputResponse> RunSnippetAsync(string snippetId, CancellationToken cancellationToken = default)
        {
            var snippetQuery = new SnippetInfoQuery { Id = snippetId };
            var snippet = await Mediator.Send(snippetQuery, cancellationToken);

            var runCodeCommand = new RunCodeCommand
            {
                SnippetCode = snippet.CodeSnippet,
                Language = GetLanguageByDirection(snippet.Direction)
            };
            var codeOutput = await Mediator.Send(runCodeCommand, cancellationToken);

            return codeOutput;
        }

        private static SnippetLanguage GetLanguageByDirection(Direction direction) => direction switch
        {
            Direction.Backend => SnippetLanguage.CSharp,
            _ => throw new NotSupportedException()
        };
    }
}
