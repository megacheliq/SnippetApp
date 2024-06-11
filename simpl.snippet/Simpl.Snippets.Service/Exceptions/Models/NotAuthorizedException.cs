using Simpl.Snippets.Service.Exceptions.Abstract;
using static Simpl.Snippets.Service.Exceptions.Models.ExceptionConstants;

namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Исключение "Не авторизован в системе"
    /// </summary>
    public class NotAuthorizedException : AbstractHttpException
    {
        public override int StatusCode => 401;

        public NotAuthorizedException() : base(NOT_AUTHORIZED_MESSAGE.Message)
        {
        }

        public NotAuthorizedException(string message) : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
