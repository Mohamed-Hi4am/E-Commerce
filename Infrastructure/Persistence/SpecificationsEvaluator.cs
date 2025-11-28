using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, TKey> (IQueryable<TEntity> inputQuery,
        ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            // start with DbSet<TEntity>
            // ex: Products DbSet
            var query = inputQuery;

            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);
            // Becomes something like: Prodcuts.Where(P => P.Id == 5)

            query = specifications.IncludeExpressions.Aggregate(query, (currentQuery, includeExpression) =>
                                                                currentQuery.Include(includeExpression));
            //like: Prodcuts.Where(P => P.Id == 5).Include(P => P.ProductType).Include(P => P.ProductBrand)

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            else if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            return query;
        }
    }
}
