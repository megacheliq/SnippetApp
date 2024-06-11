namespace Simpl.Snippets.Service.Domain.Snippet.Models
{
    /// <summary>
    /// Класс, представляющий объект с идентификатором и названием перечисления
    /// </summary>
    public class ItemDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }
}
