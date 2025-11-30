using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ProductModule;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            // 1. Retrieve all brands --> UnitOfWork
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            
            // 2. Mapping from ProductBrand to BrandResultDto
            var brandsResult = _mapper.Map<IEnumerable<BrandResultDto>>(brands);

            // 3. Return
            return brandsResult;
        }

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecParams parameters)
        {
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithBrandAndTypeSpecifications(parameters));

            var productsResult = _mapper.Map<IEnumerable<ProductResultDto>>(products);
            
            var pageSize = productsResult.Count();

            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(new ProductCountSpecifications(parameters));

            return new PaginatedResult<ProductResultDto>(parameters.PageIndex, pageSize,
            totalCount, productsResult);

        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var typesResult = _mapper.Map<IEnumerable<TypeResultDto>>(types);

            return typesResult;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(new ProductWithBrandAndTypeSpecifications(id));

            var productResult = _mapper.Map<ProductResultDto>(product);

            return productResult;
        }
    }
}
