using Microsoft.AspNetCore.Authorization;

namespace Simpl.Snippets.Service.Domain.Authorization.Models
{
    /// <summary>
    /// Требование наличия роли пользователя приложения снипетов 
    /// </summary>
    /// <remarks>
    /// В данном случае нет дополнительных свойств или методов, так как требование авторизации представляет только маркерный интерфейс
    /// </remarks>
    public class SnippetUserRequirement : IAuthorizationRequirement
    {
    }
}
