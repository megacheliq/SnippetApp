using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Simpl.Snippets.Service.DataAccess.Models
{
    /// <summary>
    /// Класс SnippetCollection представляет коллекцию сниппетов (кодовых фрагментов)
    /// </summary>
    public class TSnippet
    {
        /// <summary>
        /// Идентификатор сниппета
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Тема сниппета
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Идентификатор автора сниппета
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Имя автора сниппета
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Направление разработки, к которому относится сниппет
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Direction Direction { get; set; }

        /// <summary>
        /// Уровень сложности сниппета
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public Level Level { get; set; }

        /// <summary>
        /// Текст сниппета (код)
        /// </summary>
        public string CodeSnippet { get; set; }

        /// <summary>
        /// Основной вопрос к коду снипета
        /// </summary>
        public string MainQuestion { get; set; }

        /// <summary>
        /// Дополнительные вопросы по теме снипеты
        /// </summary>
        public List<string> AdditionalQuestions { get; set; }

        /// <summary>
        /// Ответ на основной вопрос
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// Дата и время создания сниппета
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Дата и время последнего изменения сниппета
        /// </summary>
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
