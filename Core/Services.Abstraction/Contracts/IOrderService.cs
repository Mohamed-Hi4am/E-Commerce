using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IOrderService
    {
        // Get OrderById ===> OrderResult(Guid id)
        public Task<OrderResult> GetOrderByIdAsync(Guid id);
        
        // Get Orders For User By Email ===> IEnumerable<OrderResult>(string email)
        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email);
        
        // Create Order ===> OrderResult(OrderResuest(BasketId & ShippingAddress), userEmail)
        public Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail);
        
        // Get All DeliveryMethods ===> IEnumarable<DeliveryResult>
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();
    }
}
