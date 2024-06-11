using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Simpl.Snippets.Service.Domain.CodeShare.Abstract;
using Simpl.Snippets.Service.Domain.CodeShare.Models;
using Microsoft.AspNetCore.Authorization;

namespace Simpl.Snippets.Service.Domain.CodeShare
{
    /// <summary>
    /// Контроллер, обрабатывающий запросы по обмену кодом
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CodeShareController : ControllerBase
    {
        private readonly IRedisService _redisService;

        public CodeShareController(IRedisService redisService)
        {
            _redisService = redisService;
        }

        /// <summary>
        /// Создание сессии
        /// </summary>
        /// <returns>id сессии</returns>
        [HttpPost]
        [Route("CreateSession")]
        public async Task<IActionResult> CreateSession()
        {
            var sessionId = await _redisService.CreateSessionAsync();
            return Ok(new { sessionId });
        }

        /// <summary>
        /// Проверка, существует ли сообщение
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsMessageExists")]
        [AllowAnonymous]
        public async Task<IActionResult> IsMessageExists(string key)
        {
            return Ok(new { sessionExist = await _redisService.MessageExistsAsync(key) });
        }

        /// <summary>
        /// Регистрация пользователя в сессии
        /// </summary>
        /// <param name="sessionId">id сессии</param>
        /// <param name="user">Имя пользователя</param>
        /// <param name="color">Цвет выделение, курсора</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterUserOnSession")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserOnSession(string sessionId, string user, string color)
        {
            var key = $"{sessionId}_{user}";

            if (await _redisService.MessageExistsAsync(key))
            {
                return Conflict(new { registratedUser = false });
            }

            var messageObject = new MessageObject
            {
                Username = user,
                Color = color,
                Selection = new Selection 
                { 
                    Start = new Position 
                    { 
                        Column = 0, 
                        LineNumber = 0 
                    }, 
                    End = new Position 
                    { 
                        Column = 0,
                        LineNumber = 0
                    }
                }
            };

            var messageJson = JsonConvert.SerializeObject(messageObject);
            var ttl = await _redisService.GetKeyTTLAsync(sessionId);

            await _redisService.SetMessageAsync(key, messageJson);
            return Ok(new { registratedUser = await _redisService.MessageExistsAsync(key), key, ttl });
        }

        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteMessage")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMessage(string key)
        {
            await _redisService.DeleteMessageAsync(key);
            return Ok();
        }

        /// <summary>
        /// Получение сообщения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Сообщение</returns>
        [HttpGet]
        [Route("GetMessage")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMessage(string key)
        {
            var message = await _redisService.GetMessageAsync(key);
            return Ok(new { message });
        }
    }
}