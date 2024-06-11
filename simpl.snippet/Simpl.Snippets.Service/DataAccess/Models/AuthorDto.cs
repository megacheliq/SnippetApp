namespace Simpl.Snippets.Service.DataAccess.Models
{
    public record AuthorDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Name { get; set; }
    }
}
