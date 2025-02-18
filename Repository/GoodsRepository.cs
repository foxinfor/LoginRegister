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
            var localEntity = _applicationDbContext.Goods.Local.FirstOrDefault(x => x.Id == entity.Id);
            if (localEntity != null)
            {
                _applicationDbContext.Entry(localEntity).State = EntityState.Detached;
            }

            _applicationDbContext.Goods.Remove(entity);
            _applicationDbContext.SaveChanges();
        }

        public Goods Get(int id)
        {
            return _applicationDbContext.Goods.Find(id);
        }

        public IEnumerable<Goods> GetAll()
        {
            return _applicationDbContext.Goods.AsNoTracking().ToList();
        }

        public void Update(Goods entity)
        {
            _applicationDbContext.Goods.Update(entity);
            _applicationDbContext.SaveChanges();
        }
    }
}