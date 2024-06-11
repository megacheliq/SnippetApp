using Simpl.Snippets.Service.DataAccess.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Simpl.Snippets.Service.Domain.Authorization.Models
{
    /// <summary>
    /// Класс, представляющий аутентифицированного пользователя.
    /// </summary>
    public class AuthUser : ClaimsIdentity
    {
        private const string RolesClaimName = "roles";

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// ФИО пользователя.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Адрес электронной почты пользователя.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Должность пользователя.
        /// </summary>
        public string Position { get; }

        /// <summary>
        /// Направление пользователя.
        /// </summary>
        public Direction? UserDirection { get; }

        /// <summary>
        /// Предпочитаемое имя пользователя (например, логин).
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Роли пользователя.
        /// </summary>
        public IReadOnlySet<string> Roles { get; }

        /// <summary>
        /// Является ли пользователь администратором.
        /// </summary>
        public bool IsAdmin { get; }


        public AuthUser(ClaimsPrincipal claimsPrincipal, AuthOptions options)
            : base(claimsPrincipal.Claims)
        {
            Id = new Guid(claimsPrincipal.FindFirstValue("user_id"));
            FullName = Name;
            Position = "Разраб";
            UserName = claimsPrincipal.FindFirstValue("preferred_username");
            Email = claimsPrincipal.FindFirstValue("email");
            UserDirection = Direction.Backend;

            var roles = new HashSet<string>();

            FillRealmRole(claimsPrincipal, roles);
            Roles = roles;

            IsAdmin = roles.Contains(options.AdminRole);
        }

        private void FillRealmRole(ClaimsPrincipal claimsPrincipal, HashSet<string> roles)
        {
            var realmRoleNode = claimsPrincipal.FindFirstValue("realm_access");

            if (string.IsNullOrEmpty(realmRoleNode))
                return;

            using var resourceAccessDocument = JsonDocument.Parse(realmRoleNode);
            var realmRoles = resourceAccessDocument.RootElement
               .GetProperty(RolesClaimName)
               .EnumerateArray()
               .Select(x => x.GetString())
               .Where(x => !string.IsNullOrEmpty(x));
            roles.UnionWith(realmRoles);
        }
    }
}
