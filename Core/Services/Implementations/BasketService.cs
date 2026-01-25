using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);

            var createdOrUpdatedBasket = await basketRepository.CreateOrUpdateBasketAsync(customerBasket);

            return createdOrUpdatedBasket is null ?
                throw new Exception("Can't Create Or Update Basket NOW : (")
                : mapper.Map<BasketDTO> (createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
            => await basketRepository.DeleteBasketAsync(id); 

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);

            return basket is null ? throw new BasketNotFoundException(id) : mapper.Map<BasketDTO>(basket);
        }
    }
}
