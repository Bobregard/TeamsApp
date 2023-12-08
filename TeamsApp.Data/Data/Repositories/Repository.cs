using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;

namespace TeamsApp.Data.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _db;
        public Repository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.Run(() => _db.Set<T>());
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() => _db.Set<T>().Where(expression).FirstOrDefaultAsync());
        }

        public async Task AddAsync(T entity)
        {
            await Task.Run(() => _db.Set<T>().Add(entity));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => _db.Set<T>().Update(entity));
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _db.Set<T>().Remove(entity));
        }
    }
}
