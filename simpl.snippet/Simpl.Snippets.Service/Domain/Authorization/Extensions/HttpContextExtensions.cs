using Simpl.Snippets.Service.Domain.Authorization.Models;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Extensions
{
    /// <summary>
    /// Расширения для HttpContext
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Получить модель аутентифицированного пользователя
        /// </summary>
        /// <param name="httpContext">Контекст HTTP-запроса</param>
        /// <returns>Объект AuthUser</returns>
        public static AuthUser GetAuthUser(this HttpContext httpContext)
        {
            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            return httpContext.User.Identities.SingleOrDefault(i => i.GetType() == typeof(AuthUser)) as AuthUser
                ?? throw new ForbiddenException();
        }
    }
}
