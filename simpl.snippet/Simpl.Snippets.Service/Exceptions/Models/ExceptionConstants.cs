namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Класс, содержащий константы сообщений об исключениях
    /// </summary>
    public static class ExceptionConstants
    {        
        public static readonly ExceptionMessageDto FORBIDDEN_MESSAGE = new() { Message = "У вас отсутствуют права доступа для выполнения этой операции. Обратитесь к администратору" };
        public static readonly ExceptionMessageDto NOT_FOUND_MESSAGE = new() { Message = "Запрашиваемая вами страница либо объект не найдены" };
        public static readonly ExceptionMessageDto NOT_AUTHORIZED_MESSAGE = new() { Message = "У вас отсутствуют права доступа для выполнения этой операции. Обратитесь к администратору" };

        /// <summary>
        /// Получить сообщение об ошибке "Метод не поддерживается"
        /// </summary>
        /// <param name="methodName">Название метода</param>
        /// <returns>Сообщение</returns>
        public static ExceptionMessageDto GetMethodNotAllowedMessage(string methodName)
            => new() { Message = $"Метод {methodName} не поддерживается в данном контроллере" };
    }
}
