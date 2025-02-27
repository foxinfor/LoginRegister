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

        public GoodsController(GoodsRepository goodsRepository, UserManager<ApplicationUser> userManager, CategoryRepository categoryRepository)
        {
            _goodsRepository = goodsRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var goodsList = await _goodsRepository.GetAllAsync();
            var categoriesList = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categoriesList;
            return View(goodsList);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Goods model)
        {
            if (ModelState.IsValid)
            {
                await _goodsRepository.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var goods = await _goodsRepository.GetAsync(id);
            if (goods == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", goods.CategoryId);
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Goods model)
        {
            if (ModelState.IsValid)
            {
                var existingGood = await _goodsRepository.GetAsync(model.Id);
                if (existingGood == null)
                {
                    return NotFound();
                }

                existingGood.Name = model.Name;
                existingGood.Count = model.Count;
                existingGood.Color = model.Color;
                existingGood.Size = model.Size;
                existingGood.CategoryId = model.CategoryId;
                existingGood.Gender = model.Gender;
                existingGood.Price = model.Price;

                await _goodsRepository.UpdateAsync(existingGood); 
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var goods = await _goodsRepository.GetAsync(id);
            if (goods == null)
            {
                return NotFound();
            }
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goods = await _goodsRepository.GetAsync(id);
            if (goods != null)
            {
                await _goodsRepository.DeleteAsync(goods);
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

        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            IEnumerable<Goods> goodsList;

            if (categoryId == 0) 
            {
                goodsList = await _goodsRepository.GetAllAsync();
            }
            else
            {
                goodsList = await _goodsRepository.GetByCategoryWithSubcategoriesAsync(categoryId);
            }

            return PartialView("GoodsList", goodsList);
        }
    }
}