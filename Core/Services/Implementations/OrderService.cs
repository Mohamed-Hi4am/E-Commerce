using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
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

            var address = mapper.Map<ShippingAddress>(request.ShipToAddress);

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

            // 4. Sub Total --> item.price * item.quantity

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            var exsitingOrder = await orderRepo.
                GetAsync(new OrderWithPaymentIntentSepcifications(basket.PaymentIntentId!));

            if (exsitingOrder is not null)
                orderRepo.Delete(exsitingOrder);

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 5. Create Order

            var order = new Order(userEmail, address, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId!);

            // Save at DB

            await orderRepo.AddAsync(order);

            await unitOfWork.SaveChangesAsync();

            // Map from Order -- > OrderResult & Return

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                                        .GetAsync(new OrderWithIncludeSpecifications(id))
                                        ?? throw new OrderNotFoundException(id);

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                                         .GetAllAsync(new OrderWithIncludeSpecifications(email));

            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), item.Quantity, product.Price);
    }
}