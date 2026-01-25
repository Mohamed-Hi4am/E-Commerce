using Domain.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        // Get Basket By Id
        public Task<CustomerBasket?> GetBasketAsync(string id);
        
        // Delete Basket
        public Task<bool> DeleteBasketAsync(string id);
        
        // Create Or Update Basket
        public Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);
    }

}
