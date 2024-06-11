namespace Simpl.Snippets.Service.Domain.CodeShare.Models
{
    /// <summary>
    /// Модель хранящая в себе информацию о пользователе в сессии
    /// </summary>
    public class MessageObject
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Цвет выделения, курсора
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Позиция курсора, выделение
        /// </summary>
        public Selection Selection { get; set; }
    }
}