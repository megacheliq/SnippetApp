namespace Simpl.Snippets.Service.Domain.CodeShare.Abstract
{
    public interface IRedisService
    {
        /// <summary>
        /// Получить сообщение
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Сообщение</returns>
        Task<string> GetMessageAsync(string key);

        /// <summary>
        /// Установить сообщение
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Сообщение</param>
        /// <param name="expiry">Время жизни</param>
        /// <returns></returns>
        Task SetMessageAsync(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Создание сессии
        /// </summary>
        /// <returns>id сессии</returns>
        Task<string> CreateSessionAsync();

        /// <summary>
        /// Проверка существует ли сообщение
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        Task<bool> MessageExistsAsync(string key);

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        Task DeleteMessageAsync(string key);

        /// <summary>
        /// Получить время жизни ключа
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Время жизни</returns>
        Task<TimeSpan?> GetKeyTTLAsync(string key);
    }
}