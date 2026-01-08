using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BasicEntity<int>
    {
        private readonly AppDbContext context;
        public GenericRepository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity != null)
                context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

        }

        public IEnumerable<T> GetAll()
        => context.Set<T>().AsNoTracking().ToList();


        public IEnumerable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        => context.Set<T>().AsNoTracking().ToList();

        public async Task<IReadOnlyList<T>> GetAllAsync()
          => await context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        => await context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>().Where(x => x.Id == id);
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var entity_value = await context.Set<T>().FindAsync(id);
            if (entity_value != null)
            {
                context.Update(entity_value);
                await context.SaveChangesAsync();
            }
        }
    }
}
