using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.Domain.Authorization.Abstract
{
    /// <summary>
    /// Интерфейс команды направления сниппета
    /// </summary>
    public interface ISnippetDirectionCommand
    {
        /// <summary>
        /// Направление
        /// </summary>
        Direction Direction { get; set; }
    }
}

