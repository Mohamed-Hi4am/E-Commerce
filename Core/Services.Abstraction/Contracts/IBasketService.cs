using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IBasketService
    {
        public Task<BasketDTO> GetBasketAsync(string id);

        public Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket);

        public Task<bool> DeleteBasketAsync(string id);

    }
}
