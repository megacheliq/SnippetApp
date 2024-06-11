using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.DataAccess.Abstract
{
    public interface IAuthorRepository
    {
        /// <summary>
        /// Получить всех авторов по направлению
        /// </summary>
        /// <param name="direction">Направление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<IEnumerable<AuthorDto>> GetAll(Direction direction, CancellationToken cancellationToken = default);
    }
}
