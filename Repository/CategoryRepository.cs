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

        public async Task<List<Category>> GetCategoriesWithHierarchyAsync()
        {
            var categories = await _applicationDbContext.Categories
                .AsNoTracking()
                .ToListAsync();

            var categoryDict = categories.ToDictionary(c => c.Id);

            foreach (var category in categories)
            {
                if (category.SubCategories == null)
                {
                    category.SubCategories = new List<Category>();
                }

                if (category.ParentId.HasValue)
                {
                    if (!categoryDict[category.ParentId.Value].SubCategories.Any())
                    {
                        categoryDict[category.ParentId.Value].SubCategories = new List<Category>();
                    }

                    categoryDict[category.ParentId.Value].SubCategories.Add(category);
                }
            }

            return categories.ToList();
        }
    }
}