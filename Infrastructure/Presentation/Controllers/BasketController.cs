using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using Shared.Dtos.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController(IServiceManager serviceManager) : ApiControllerBase
    {
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        [HttpGet("{id}")] // GET: BaseUrl/api/Basket/Basket01
        public async Task<ActionResult<BasketDTO>> Get(string id)
            => Ok(await serviceManager.BasketService.GetBasketAsync(id));

        [HttpPost] // POST: BaseUrl/api/Basket
        public async Task<ActionResult<BasketDTO>> Update(BasketDTO basketDTO)
            => Ok(await serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDTO));

        [HttpDelete("{id}")] // Delete: BaseUrl/api/Basket
        public async Task<ActionResult> Delete(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent(); // 204
        }
    }
}
