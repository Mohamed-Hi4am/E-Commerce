using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
       
        public string PictureUrl { get; set; } = null!;
       
        public decimal Price { get; set; }

        // 1-M between ProductType and Product

        // Navigation property for ProductType
        public ProductType ProductType { get; set; }

        // FK for ProductType
        public int TypeId { get; set; }

        
        // 1-M between ProductBrand and Product

        // Navigation property for ProductBrand
        public ProductBrand ProductBrand { get; set; }

        // FK for ProductBrand
        public int BrandId { get; set; }


    }
}
