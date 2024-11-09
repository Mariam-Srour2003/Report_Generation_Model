using Infrastructure.Repository.IBase;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repository.Base
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        private DbSet<T> _set;
        protected Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        public async Task<T> GetById(long id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _set.ToListAsync();
        }


        public async Task Add(T entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entity)
        {
            await _set.AddRangeAsync(entity);
        }
        public void RemoveRange(List<T> entities)
        {
            _set.RemoveRange(entities);
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }


    }
}