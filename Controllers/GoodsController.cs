using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoginRegister.Controllers
{
    public class GoodsController : Controller
    {
        private readonly GoodsRepository _goodsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public GoodsController(GoodsRepository goodsRepository, UserManager<ApplicationUser> userManager, CategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _goodsRepository = goodsRepository;
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
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
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
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var goods = _goodsRepository.Get(id);
            if (goods == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", goods.CategoryId);
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Goods model)
        {
            if (ModelState.IsValid)
            {
                var existingGood = _goodsRepository.Get(model.Id);

                if (existingGood == null)
                {
                    return NotFound();
                }

                existingGood.Name = model.Name;
                existingGood.Count = model.Count;
                existingGood.Color = model.Color; 
                existingGood.Size = model.Size; 
                existingGood.Gender = model.Gender;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", model.CategoryId); 
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

        public async Task<IActionResult> Details(int id)
        {
            var goods = await _goodsRepository.GetAsync(id);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

    }
}