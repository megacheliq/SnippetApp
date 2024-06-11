using Simpl.Snippets.Service.Exceptions.Abstract;

namespace Simpl.Snippets.Service.Exceptions.Models
{
    /// <summary>
    /// Исключение "Неверный запрос"
    /// </summary>
    public class BadRequestException : AbstractHttpException
    {

        public override int StatusCode => 400;

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
