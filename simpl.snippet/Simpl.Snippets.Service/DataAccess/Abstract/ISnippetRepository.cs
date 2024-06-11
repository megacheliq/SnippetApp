using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries;

namespace Simpl.Snippets.Service.DataAccess.Abstract
{
    /// <summary>
    /// Интерфейс репозитория для работы со сниппетами
    /// </summary>
    public interface ISnippetRepository
    {
        /// <summary>
        /// Получить краткую информацию о снипетах
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Коллекция краткой информации о сниппетах</returns>
        Task<List<BriefInfoSnippetResponse>> GetFilteredAsync(BriefInfoSnippetsQuery request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить полную информацию о сниппете по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Полная информация о сниппете</returns>
        Task<FullSnippetInfoResponse> GetByIdOrDefaultAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить случайный сниппет 
        /// <param name="request">Запрос на получение случайного сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Случайный сниппет.</returns>
        Task<RandomSnippetResponse> GetRandomAsync(RandomSnippetQuery request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создать новый сниппет
        /// </summary>
        /// <param name="snippet">Cниппет</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task CreateAsync(TSnippet snippet, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновить существующий сниппет
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="dto">Модель для обновления</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task UpdateAsync(string id, AddOrUpdateSnippetDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить сниппет по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    }
}
