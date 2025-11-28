using Domain.Contracts;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        #region Criteria
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        
        // Constructor: used when we want to filter by some "Where" clause
        protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        #endregion

        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

        // A helper method to easily add Includes to the list property
        protected void AddIncludes(Expression<Func<TEntity, object>> includeExpressions)
        {
            IncludeExpressions.Add(includeExpressions);
        }
        #endregion

        #region Sort
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        protected void SetOrderBy(Expression<Func<TEntity, object>> orderByExpression)
            => OrderBy = orderByExpression;
        // This method will store something like this: P => NameAsc

        protected void SetOrderByDescending(Expression<Func<TEntity, object>>
        orderByDescExpression)
            => OrderByDescending = orderByDescExpression;
        // This method will store something like this: P => NameDesc 
        #endregion
    }
}
