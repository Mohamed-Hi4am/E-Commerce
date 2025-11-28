using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        #region Get All Products
        [HttpGet] // GET: BaseUrl/api/Products
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts(ProductSortingOptions sort)
            => Ok(await serviceManager.ProductService.GetAllProductsAsync(sort));
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
        [HttpGet("{id:int}")] // GET: BaseUrl/api/Products/id
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
            => Ok(await serviceManager.ProductService.GetProductByIdAsync(id));
        #endregion
    }
}
