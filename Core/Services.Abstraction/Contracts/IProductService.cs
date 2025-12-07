using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IProductService
    {
        // Get all products
        public Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecParams parameters);
        // Get product by Id
        public Task<ProductResultDto> GetProductByIdAsync(int id);
        // Get all brands
        public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        // Get all types
        public Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

    }
}
