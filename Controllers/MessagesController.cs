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
        private readonly GoodsRepository goodsRepository;

        public MessagesController(MessageRepository messageRepository, UserManager<ApplicationUser> userManager, GoodsRepository goodsRepository)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
            this.goodsRepository = goodsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var messages = await _messageRepository.GetMessagesByRecipientEmailAsync(user.Email);


            var goodIds = messages.Select(m => m.GoodId).Distinct().ToList();
            var goods = await goodsRepository.GetGoodsByIdsAsync(goodIds);

            var model = new Tuple<IEnumerable<Message>, IEnumerable<Goods>>(messages, goods);
            return View(model);
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
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _messageRepository.GetAsync(id);
            if (message != null)
            {
                await _messageRepository.DeleteAsync(message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}