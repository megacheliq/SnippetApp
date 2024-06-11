namespace Simpl.Snippets.Service.Domain.CodeRunner.Abstract
{
    public interface ICodeRunnerStrategy
    {
        /// <summary>
        /// Выполнить снипет кода
        /// </summary>
        /// <param name="snippetCode">Снипет</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Результат выполнения</returns>
        public Task<string> RunSnippetCodeAsync(string snippetCode, CancellationToken cancellationToken = default);
    }
}
