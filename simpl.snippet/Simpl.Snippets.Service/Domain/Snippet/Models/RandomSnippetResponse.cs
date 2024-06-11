namespace Simpl.Snippets.Service.Domain.Snippet.Models;

/// <summary>
/// Класс для представления случайного сниппета
/// </summary>
public class RandomSnippetResponse
{
    /// <summary>
    /// Имя автора
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// Код сниппета
    /// </summary>
    public string CodeSnippet { get; set; }

    /// <summary>
    /// Основной вопрос
    /// </summary>
    public string MainQuestion { get; set; }

    /// <summary>
    /// Список дополнительных вопросов
    /// </summary>
    public List<string> AdditionalQuestions { get; set; }
}
