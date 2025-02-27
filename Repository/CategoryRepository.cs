using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(Category entity)
        {
            await _applicationDbContext.Categories.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category entity)
        {
            _applicationDbContext.Categories.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await _applicationDbContext.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _applicationDbContext.Categories
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _applicationDbContext.Categories.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllWithoutSame(int id)
        {
            return await _applicationDbContext.Categories
                .Include(c => c.SubCategories).Where(c => c.Id != id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Category>> GetSubCategoriesAsync(int categoryId)
        {
            var category = await _applicationDbContext.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            var subCategories = new List<Category>();

            if (category != null)
            {
                subCategories.Add(category);
                foreach (var subCategory in category.SubCategories)
                {
                    subCategories.AddRange(await GetSubCategoriesAsync(subCategory.Id));
                }
            }

            return subCategories;
        }
    }
}