using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegister.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessageRepository _messageRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(MessageRepository messageRepository, UserManager<ApplicationUser> userManager)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var messages = await _messageRepository.GetMessagesByRecipientEmailAsync(user.Email);
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