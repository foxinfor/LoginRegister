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

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.RecipientEmail))
            {
                return BadRequest("Некорректные данные.");
            }

            await _messageRepository.AddMessageAsync(message);
            return Ok();
        }
    }
}