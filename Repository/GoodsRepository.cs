using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Repository
{
    public class GoodsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GoodsRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Add(Goods entity)
        {
            _applicationDbContext.Goods.Add(entity);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(Goods entity)
        {
            _applicationDbContext.Goods.Remove(entity);
            _applicationDbContext.SaveChanges();
        }

        public Goods Get(int id)
        {
            return _applicationDbContext.Goods.Find(id);
        }

        public IEnumerable<Goods> GetAll()
        {
            return _applicationDbContext.Goods
                .Include(g => g.Category)
                .AsNoTracking()
                .ToList();
        }

        public void Update(Goods entity)
        {
            _applicationDbContext.Goods.Update(entity);
            _applicationDbContext.SaveChanges();
        }

        public async Task<Goods> GetAsync(int id)
        {
            return await _applicationDbContext.Goods.FindAsync(id);
        }
    }
}