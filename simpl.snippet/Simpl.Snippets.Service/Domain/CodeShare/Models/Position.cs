namespace Simpl.Snippets.Service.Domain.CodeShare.Models
{
    /// <summary>
    /// Позиция курсора
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Номер строки
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Колонка
        /// </summary>
        public int Column { get; set; }
    }
}