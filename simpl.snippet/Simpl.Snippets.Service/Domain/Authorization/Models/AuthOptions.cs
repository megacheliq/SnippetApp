namespace Simpl.Snippets.Service.Domain.Authorization.Models
{
    /// <summary>
    /// Класс, представляющий параметры аутентификации
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Адрес проверки токена
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Название клиента
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Роль администратора
        /// </summary>
        public string AdminRole { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string UserRole { get; set; }
    }
}
