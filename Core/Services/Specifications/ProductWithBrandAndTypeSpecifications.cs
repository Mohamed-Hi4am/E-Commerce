using Domain.Entities;
using Domain.Entities.ProductModule;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications: BaseSpecifications<Product, int>
    {
        // Scenario 1: we want to return all products with no filters
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams parameters) :
            base(product =>
                    (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value) &&
                    (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value)
            )
        {
            AddIncludes(P => P.ProductBrand);
            AddIncludes(P => P.ProductType);

            switch (parameters.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    SetOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    SetOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    SetOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    SetOrderByDescending(P => P.Price);
                    break;
                default:
                    SetOrderBy(P => P.Name);
                    // We made the default to sort by name asc, we can change it if we want
                    break;
            }

            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }

        // Scenario 2: we want to return a specific product by Id
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            AddIncludes(P => P.ProductBrand);
            AddIncludes(P => P.ProductType);
        }
    }
}
