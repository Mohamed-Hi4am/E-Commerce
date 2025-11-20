using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }

        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
           => (IGenericRepository <TEntity, TKey>)
           _repositories.GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, TKey>(_dbContext));

    }
}
