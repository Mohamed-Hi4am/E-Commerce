using Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentSepcifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithPaymentIntentSepcifications(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}