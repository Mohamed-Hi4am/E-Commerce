global using ShippingAddress = Domain.Entities.OrderModule.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderModule
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, ShippingAddress shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get; set; } = string.Empty;
        
        public Address ShippingAddress { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }

        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        
        public DeliveryMethod DeliveryMethod { get; set; }
        
        public int? DeliveryMethodId { get; set; }
        
        // SubTotal = OrderItem.price * OrderItem.Quantity
        // Total = SubTotal + Shipping Price
        // (Derived Attribute) ==> DTO Or Mapping Profile
        public decimal Subtotal { get; set; }

        // Payment --- > Later
        
        public string PaymentIntentId { get; set; }
        
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    }
}