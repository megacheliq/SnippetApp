using Simpl.Snippets.Service.Exceptions.Abstract;

namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Исключение "Ошибка в серверной логике"
    /// </summary>
    public class BusinessLogicException : AbstractHttpException
    {
        public override int StatusCode => 500;

        public BusinessLogicException(string message) : base(message)
        {

        }

        public BusinessLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
