using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        // SaveChangesAsync()
        Task<int> SaveChangesAsync();

        // Signature for a method that returns an object/instance from a class that implements IGenericRepository
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    }
}
