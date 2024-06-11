using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.Models
{
    /// <summary>
    /// Класс для представления команды сниппета
    /// </summary>
    public class AddOrUpdateSnippetDto
    {
        /// <summary>
        /// Тема сниппета
        /// </summary>
        public string Theme { get; set; }

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
