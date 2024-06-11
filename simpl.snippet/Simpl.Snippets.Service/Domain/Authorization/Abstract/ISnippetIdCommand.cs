namespace Simpl.Snippets.Service.Domain.Authorization.Abstract
{
    /// <summary>
    /// Интерфейс команды идентификатора сниппета
    /// </summary>
    public interface ISnippetIdCommand
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        string Id { get; set; }
    }
}
