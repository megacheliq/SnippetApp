using MediatR;
using Microsoft.AspNetCore.Mvc;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Domain.Snippet.Extensions;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Snippet
{
    /// <summary>
    /// Контроллер, обрабатывающий все запросы, связанные со сниппетами кода
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SnippetController : ControllerBase
    {
        private static readonly ICollection<ItemDto> Levels = EnumExtensions.GetDescriptions<Level>().ToList();
        private static readonly ICollection<ItemDto> Directions = EnumExtensions.GetDescriptions<Direction>().ToList();

        private IMediator Mediator { get; }

        public SnippetController(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Поиск по сниппетам кода на основе различных критериев
        /// </summary>
        /// <param name="command">Объект, содержащий критерии поиска</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Информация о сниппетах</returns>
        [HttpGet("GetAll")]
        public async Task<IEnumerable<BriefInfoSnippetResponse>> GetBriefInfoSnippetsAsync(
            [FromQuery] BriefInfoSnippetsQuery command,
            CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Получение полной информации о сниппете по его идентификатор
        /// </summary>
        /// <param name="id">Идентификатор сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Полная информация о сниппете</returns>
        [HttpGet("Get/{id}")]
        public async Task<FullSnippetInfoResponse> Get(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            var command = new SnippetInfoQuery { Id = id };
            return await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Получение случайного сниппета на основе направления и уровня
        /// </summary>
        /// <param name="command">Объект, содержащий информацию о направлении и уровне</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Случайный сниппет</returns>
        [HttpGet("GetRandom")]
        public async Task<RandomSnippetResponse> Get(
            [FromQuery] RandomSnippetQuery command,
            CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Получить описания всех элементов перечисления "Направление"
        /// </summary>
        /// <returns>Коллекция пар (идентификатор-описание)</returns>
        [HttpGet("GetDirections")]
        public IEnumerable<ItemDto> GetDirections()
        {
            var user = HttpContext.GetAuthUser();

            if (user.IsAdmin)
                return Directions;

            if (user.UserDirection == null)
                throw new NotAuthorizedException();

            return Directions.Where(x => (Direction)x.Id == user.UserDirection);
            
        }

        /// <summary>
        /// Получить описания всех элементов перечисления "Уровень"
        /// </summary>
        /// <returns>Коллекция пар(идентификатор-описание)</returns>
        [HttpGet("GetLevels")]
        public IEnumerable<ItemDto> GetLevels() => Levels;

        /// <summary>
        /// Получить авторов снипетов по направлению
        /// </summary>
        /// <param name="direction">Направление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Коллекция авторов</returns>
        [HttpGet("GetAuthors/{direction}")]
        public async Task<IEnumerable<AuthorDto>> GetSnippetAuthorsAsync(Direction direction, CancellationToken cancellationToken = default)
        {
            var command = new SnippetAuthorQuery { Direction = direction };

            var result = await Mediator.Send(command, cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Добавить новый сниппет
        /// </summary>
        /// <param name="command">Объект, содержащий информацию о создаваемом сниппете</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("Create")]
        public async Task Post(CreateSnippetCommand command, CancellationToken cancellationToken = default)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Обновить существующий сниппет по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="request">Объект, содержащий информацию о обновляемом сниппете</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPut("Update/{id}")]
        public async Task Put(string id, AddOrUpdateSnippetDto request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var command = new UpdateSnippetCommand { Id = id, Dto = request };

            await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Удалить сниппет по его идентификатор
        /// </summary>
        /// <param name="id">Идентификатор сниппета</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("Delete/{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            var command = new DeleteSnippetCommand { Id = id };
            await Mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Распарсить снипет из текстового файла по шаблону
        /// </summary>
        /// <param name="file">Текстовый файл</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Модель со снипетом. Если файл не соответствует шаблону - будет пустая.</returns>
        [HttpPost("ParseFromTextFile")]
        public async Task<AddOrUpdateSnippetDto> ParseSnippetFromTextFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = file.OpenReadStream();
            var command = new ParseSnippetFromTextFileCommand { TextFile = stream };

            var result = await Mediator.Send(command, cancellationToken);

            return result;
        }
    }
}
