using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LoginRegister.Controllers
{
    public class GoodsController : Controller
    {
        private readonly GoodsRepository _goodsRepository;
        private readonly MailRepository _mailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public GoodsController(GoodsRepository goodsRepository, MailRepository mailRepository, UserManager<ApplicationUser> userManager,CategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _goodsRepository = goodsRepository;
            _mailRepository = mailRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var goodsList = _goodsRepository.GetAll();
            var categoriesList = _categoryRepository.GetAll();
            ViewBag.Categories = categoriesList;
            return View(goodsList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Goods model)
        {
            if (ModelState.IsValid)
            {
                _goodsRepository.Add(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var goods = _goodsRepository.Get(id);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Goods model)
        {
            if (ModelState.IsValid)
            {
                var existingGood = _goodsRepository.Get(model.Id);

                if (existingGood.Count == 0 && model.Count > 0)
                {
                    var messages = await _context.Messages
                        .Where(m => m.GoodId == model.Id)
                        .ToListAsync();


                    foreach (var message in messages)
                    {
                        var mail = new Mail
                        {
                            Email = message.RecipientEmail,
                            Context = $"Уведомление о появлении {existingGood.Name} в наличии",
                            CreatedAt = DateTime.UtcNow 
                        };

                        await _mailRepository.AddMailAsync(mail);
                    }
                }

                existingGood.Name = model.Name;
                existingGood.Count = model.Count;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var goods = _goodsRepository.Get(id);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var goods = _goodsRepository.Get(id);
            if (goods != null)
            {
                _goodsRepository.Delete(goods);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}