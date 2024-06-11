using Simpl.Snippets.Service.Domain.CodeRunner.Models;

namespace Simpl.Snippets.Service.Domain.CodeRunner.Abstract
{
    public interface ICodeRunnerFactory
    {
        /// <summary>
        /// Выполнить снипет кода
        /// </summary>
        /// <param name="snippetLanguage">язык кода сниппета</param>
        /// <param name="codeSnippet">Снипет</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат выполнения</returns>
        public Task<string> RunCodeSnippetAsync(SnippetLanguage snippetLanguage, string codeSnippet, CancellationToken cancellationToken = default);
    }
}
