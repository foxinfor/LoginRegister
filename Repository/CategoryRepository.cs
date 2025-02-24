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

        public void Add(Category entity)
        {
            _applicationDbContext.Categories.Add(entity);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(Category entity)
        {
            _applicationDbContext.Categories.Remove(entity);
            _applicationDbContext.SaveChanges();
        }

        public Category Get(int id)
        {
            return _applicationDbContext.Categories
                .Include(c => c.SubCategories) 
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _applicationDbContext.Categories
                .Include(c => c.SubCategories) 
                .AsNoTracking()
                .ToList();
        }

        public void Update(Category entity)
        {
            _applicationDbContext.Categories.Update(entity);
            _applicationDbContext.SaveChanges();
        }
    }
}