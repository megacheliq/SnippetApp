using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Exceptions.Abstract
{
    /// <summary>
    /// Базовый интерфейс исключения
    /// </summary>
    public interface IHttpException
    {
        /// <summary>
        /// Код состояния HTTP
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Получить сообщение об исключении
        /// </summary>
        /// <returns>Сообщение об исключении</returns>
        ExceptionMessageDto GetMessage();
    }
}
