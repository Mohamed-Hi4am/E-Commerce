using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IPaymentService
    {
        public Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId);

        public Task UpdateOrderPaymentStatusAsync(string json, string header);
    }
}