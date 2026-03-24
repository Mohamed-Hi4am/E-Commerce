using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ApiControllerBase
    {
        // Create Order
        [HttpPost] // POST: BaseURL/api/Orders
        public async Task<ActionResult<OrderResult>> Create(OrderRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrderAsync(request, email!);

            return Ok(order);
        }

        // Get Orders
        [HttpGet] // GET: BaseURL/api/Orders
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.OrderService.GetOrdersByEmailAsync(email!);
            return Ok(orders);
        }

        // Get Order By ID
        [HttpGet("{id}")] // GET: BaseURL/api/Orders/{id}
        public async Task<ActionResult<OrderResult>> GetOrder(Guid id)
        {
            var order = await serviceManager.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [AllowAnonymous]
        [HttpGet("DeliveryMethods")] // GET: BaseURL/api/Orders/DeliveryMethods
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethods()
        {
            var methods = await serviceManager.OrderService.GetDeliveryMethodsAsync();

            return Ok(methods);
        }
    }
}
