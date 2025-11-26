using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        => asNoTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync()
                        : await _dbContext.Set<TEntity>().ToListAsync();

        // GetAll that takes a specification
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationsEvaluator.GetQuery<TEntity, TKey>(_dbContext.Set<TEntity>(), specifications).ToListAsync();

        public async Task<TEntity?> GetAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);

        // Get that takes a specification
        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationsEvaluator.GetQuery<TEntity, TKey>(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

    }
}
