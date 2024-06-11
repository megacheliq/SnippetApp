using Simpl.Snippets.Service.Exceptions.Abstract;
using System.Runtime.Serialization;
using static Simpl.Snippets.Service.Exceptions.Models.ExceptionConstants;

namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Исключение "Нет данных, удовлетворяющих запросу"
    /// </summary>
    public class NoDataFoundException : AbstractHttpException
    {
        public override int StatusCode => 404;

        public NoDataFoundException() : base(NOT_FOUND_MESSAGE.Message)
        {
        }

        public NoDataFoundException(string message) : base(message)
        {
        }

        protected NoDataFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NoDataFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
