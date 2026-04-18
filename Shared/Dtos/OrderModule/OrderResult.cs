using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderModule
{
    public record OrderResult
    {
        public Guid Id { get; init; }
        
        public string UserEmail { get; init; } = string.Empty;
        
        public AddressDto ShippingAddress { get; init; }
        
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();

        public string Status { get; init; }

        public string DeliveryMethod { get; init; }

        public int? DeliveryMethodId { get; init; }
        
        public decimal Subtotal { get; init; }

        // Payment --- > Required For Later
        public string PaymentIntentId { get; init; } = string.Empty;

        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.Now;
        
        // Derived attribute
        public decimal Total { get; init; }
    }
}
