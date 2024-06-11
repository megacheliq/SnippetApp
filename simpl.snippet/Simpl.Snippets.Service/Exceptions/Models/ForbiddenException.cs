using Simpl.Snippets.Service.Exceptions.Abstract;
using static Simpl.Snippets.Service.Exceptions.Models.ExceptionConstants;

namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Исключение "Нет доступа к компоненту приложения"
    /// </summary>
    public class ForbiddenException : AbstractHttpException
    {
        public override int StatusCode => 403;

        public ForbiddenException() : base(FORBIDDEN_MESSAGE.Message)
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
