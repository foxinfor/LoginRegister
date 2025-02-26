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
    }
}