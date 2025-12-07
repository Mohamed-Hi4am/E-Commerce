using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // Get All
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
        // Get All overload
        public Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications);
        // Get by Id
        Task<TEntity?> GetAsync(TKey id);
        // Get by Id overload
        public Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> specifications);

        // Count items
        public Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications);

        // Create
        Task AddAsync(TEntity entity);
        // Update
        void Update(TEntity entity); // Because "Add" has an asynchronous version but "Update" and "Delete" don't
        // Delete
        void Delete(TEntity entity);
    }
}
