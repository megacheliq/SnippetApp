namespace Simpl.Snippets.Service.Domain.CodeShare.Models
{
    /// <summary>
    /// Выделение пользователя
    /// </summary>
    public class Selection
    {
        /// <summary>
        /// Начало выделения
        /// </summary>
        public Position Start { get; set; }

        /// <summary>
        /// Конец выделения
        /// </summary>
        public Position End { get; set; }
    }
}