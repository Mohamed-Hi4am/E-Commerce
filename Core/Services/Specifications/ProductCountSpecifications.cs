using Domain.Entities.ProductModule;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductCountSpecifications(ProductSpecParams parameters) : base(produt
            => (!parameters.TypeId.HasValue || produt.TypeId == parameters.TypeId.Value)
            && (!parameters.BrandId.HasValue || produt.BrandId == parameters.BrandId.Value))
        {

        }
    }
}
