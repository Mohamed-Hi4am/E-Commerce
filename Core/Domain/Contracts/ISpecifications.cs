using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // The "Where" clause. 
        public Expression<Func<TEntity, bool>>? Criteria { get; }

        // The "Include" list (Joins). 
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        public Expression<Func<TEntity, object>>? OrderBy { get; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; }

    }
}
