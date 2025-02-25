using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegister.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessageRepository _messageRepository;

        public MessagesController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetAllMessagesAsync(); 
            return View(messages); 
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.RecipientEmail) || message.GoodId <= 0)
            {
                return BadRequest("Некорректные данные.");
            }

            message.GoodUrl = Url.Action("Details", "Goods", new { id = message.GoodId }, Request.Scheme);

            await _messageRepository.AddMessageAsync(message);
            return Ok();
        }
    }
}