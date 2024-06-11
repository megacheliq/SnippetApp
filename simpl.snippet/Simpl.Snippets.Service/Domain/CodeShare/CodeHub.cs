using Simpl.Snippets.Service.Domain.CodeShare.Abstract;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Simpl.Snippets.Service.Domain.CodeShare.Models;

namespace Simpl.Snippets.Service.Domain.CodeShare
{
    public class CodeHub : Hub
    {
        private readonly IRedisService _redisService;

        public CodeHub(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task JoinGroup(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await SendPreviousMessages(sessionId);
        }

        public async Task LeaveGroup(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }

        public async Task SendToGroup(string sessionId, string message)
        {
            await _redisService.SetMessageAsync(sessionId, message);
            await Clients.Group(sessionId).SendAsync("Receive", message);
        }

        public async Task SendSelectionToGroup(string sessionId, string username, MessageObject messageObject)
        {
            var messageJson = JsonConvert.SerializeObject(messageObject);
            await _redisService.SetMessageAsync($"{sessionId}_{username}", messageJson);
            await Clients.Group(sessionId).SendAsync("ReceiveSelection", messageObject);
        }

        private async Task SendPreviousMessages(string sessionId)
        {
            var message = await _redisService.GetMessageAsync(sessionId);
            if (!string.IsNullOrEmpty(message))
            {
                await SendToGroup(sessionId, message);
            }
        }
    }
}
