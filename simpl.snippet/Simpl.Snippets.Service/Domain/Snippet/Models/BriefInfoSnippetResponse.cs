namespace Simpl.Snippets.Service.Domain.Snippet.Models;

/// <summary>
/// Класс для представления краткой информации о сниппетах кода
/// </summary>
public class BriefInfoSnippetResponse
{
    /// <summary>
    /// Идентификатор сниппета
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Идентификатор автора
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Имя автора
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// Тема сниппета
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// Дата создания сниппета
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Дата изменения сниппета
    /// </summary>
    public DateTimeOffset ModifiedDate { get; set; }
}
