using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            // 1. Shipping Address

            var address = mapper.Map<ShippingAddress>(request.ShippingAddress);

            // 2. OrderItems => BasketId --> BasketItems --> OrderItems

            var basket = await basketRepository.GetBasketAsync(request.BasketId)
                         ?? throw new BasketNotFoundException (request.BasketId);

            // Get Selected Items at Basket from Product Repo

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id)
                              ?? throw new ProductNotFoundException(item.Id);

                orderItems.Add(CreateOrderItem(item, product));
            }

            // 3. Delivery Method

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                                .GetAsync(request.DeliveryMethodId)
                                ?? throw new DeliveryMethodException(request.DeliveryMethodId);

            // 4. Sub Total -- > item.price + item.quantity

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 5. Create Order

            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal);

            // Save at DB

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);

            await unitOfWork.SaveChangesAsync();

            // Map from Order -- > OrderResult & Return

            return mapper.Map<OrderResult>(order);
        }

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);
    }
}
