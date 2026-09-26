using Microsoft.AspNetCore.Mvc;
using Mercado.Filters;
using Mercado.Services;
using Mercado.ViewModels;

namespace Mercado.Controllers
{
    [RequireLogin]
    public class ChatBotController : Controller
    {
        private readonly IInventoryChatService _chatService;

        public ChatBotController(IInventoryChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequestViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Please type a question first");

            var reply = await _chatService.AskAboutInventoryAsync(request.Message);
            return Json(new ChatResponseViewModel { Reply = reply });
        }
    }
}