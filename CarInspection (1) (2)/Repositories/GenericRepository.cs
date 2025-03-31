using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly CarInspectionDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CarInspectionDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAllIncluding(string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking().ToList();
        }

        public virtual T GetByIdIncluding(int id, string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
