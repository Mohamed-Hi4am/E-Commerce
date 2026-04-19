using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderModule
{
    public record DeliveryMethodResult
    {
        public int Id { get; set; }
        public string ShortName { get; init; }
        public string Description { get; init; }
        public string DeliveryTime { get; init; } // Within 3 Days
        public decimal Cost { get; init; }
    }
}
