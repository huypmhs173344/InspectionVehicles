using System.Collections.Generic;

namespace Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllIncluding(string[] includeProperties);
        T GetById(int id);
        T GetByIdIncluding(int id, string[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
