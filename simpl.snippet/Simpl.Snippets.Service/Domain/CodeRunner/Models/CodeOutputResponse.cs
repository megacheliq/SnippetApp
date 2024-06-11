namespace Simpl.Snippets.Service.Domain.CodeRunner.Models
{
    /// <summary>
    /// Результат выполнения кода
    /// </summary>
    public record CodeOutputResponse
    {
        /// <summary>
        /// Консольный вывод
        /// </summary>
        public string Output { get; init; }
    }
}
