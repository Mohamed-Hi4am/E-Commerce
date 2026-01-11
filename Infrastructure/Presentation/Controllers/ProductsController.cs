using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ProductsController(IServiceManager serviceManager) : ApiControllerBase
    {
        #region Get All Products
        [HttpGet] // GET: BaseUrl/api/Products
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecParams parameters)
            => Ok(await serviceManager.ProductService.GetAllProductsAsync(parameters));
        #endregion

        #region Get All Brands
        [HttpGet("Brands")] // GET: BaseUrl/api/Products/Brands
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
            => Ok(await serviceManager.ProductService.GetAllBrandsAsync());
        #endregion

        #region Get All Types
        [HttpGet("Types")] // GET: BaseUrl/api/Products/Types
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
            => Ok(await serviceManager.ProductService.GetAllTypesAsync());
        #endregion

        #region Get Product By Id
        [ProducesResponseType(typeof(ProductResultDto), StatusCodes.Status200OK)]
        [HttpGet("{id:int}")] // GET: BaseUrl/api/Products/id
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
            => Ok(await serviceManager.ProductService.GetProductByIdAsync(id));
        #endregion
    }
}
