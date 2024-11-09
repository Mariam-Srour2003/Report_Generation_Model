using System;

namespace Infrastructure.Repository.IBase
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(long id);
        Task<IEnumerable<T>> GetAll();

        Task Add(T entity);
        Task AddRangeAsync(List<T> entity);
        void Delete(T entity);
        void RemoveRange(List<T> entities);
        void Update(T entity);

    }
}