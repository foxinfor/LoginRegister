using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Repository
{
    public class GoodsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly CategoryRepository _categoryRepository;

        public GoodsRepository(ApplicationDbContext applicationDbContext, CategoryRepository categoryRepository)
        {
            _applicationDbContext = applicationDbContext;
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Goods entity)
        {
            await _applicationDbContext.Goods.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Goods entity)
        {
            _applicationDbContext.Goods.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Goods> GetAsync(int id)
        {
            return await _applicationDbContext.Goods
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Goods>> GetAllAsync()
        {
            return await _applicationDbContext.Goods
                .Include(g => g.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(Goods entity)
        {
            _applicationDbContext.Goods.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Goods>> GetByCategoryWithSubcategoriesAsync(int categoryId)
        {
            var subCategories = await _categoryRepository.GetSubCategoriesAsync(categoryId);
            var subcategoryIds = subCategories.Select(c => c.Id).ToList();

            return await _applicationDbContext.Goods
                .Where(g => subcategoryIds.Contains(g.CategoryId))
                .ToListAsync();
        }

        public async Task<List<Goods>> GetGoodsByIdsAsync(IEnumerable<int> goodIds)
        {
            return await _applicationDbContext.Goods
                .Where(g => goodIds.Contains(g.Id))
                .ToListAsync();
        }
    }
}