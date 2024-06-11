using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.Models
{
    /// <summary>
    /// Класс для представления полной информации о сниппете
    /// </summary>
    public class FullSnippetInfoResponse
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тема сниппета
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Идентификатор автора
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Имя автора
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Направление сниппета
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Уровень сниппета
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        /// Код сниппета
        /// </summary>
        public string CodeSnippet { get; set; }

        /// <summary>
        /// Основной вопрос
        /// </summary>
        public string MainQuestion { get; set; }

        /// <summary>
        /// Ответ на основной вопрос
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// Список дополнительных вопросов
        /// </summary>
        public List<string> AdditionalQuestions { get; set; }
    }
}
