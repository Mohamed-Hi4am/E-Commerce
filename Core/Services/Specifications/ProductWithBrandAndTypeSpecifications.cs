using Domain.Entities;
using Domain.Entities.ProductModule;
using Microsoft.IdentityModel.Tokens;
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
        public ProductWithBrandAndTypeSpecifications() : base(null)
        {
            AddIncludes(P => P.ProductBrand);
            AddIncludes(P => P.ProductType);
        }

        // Scenario 2: we want to return a specific product by Id
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            AddIncludes(P => P.ProductBrand);
            AddIncludes(P => P.ProductType);
        }
    }
}
