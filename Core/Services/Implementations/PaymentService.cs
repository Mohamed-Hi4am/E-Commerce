using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.Dtos.BasketModule;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Domain.Entities.ProductModule.Product;

namespace Services.Implementations
{
    public class PaymentService(IConfiguration configuration,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPaymentService
    {
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Configure Stripe API Key Using The Secret Key From "appsettings"
            StripeConfiguration.ApiKey = configuration.GetSection("StripeSettings")["SecretKey"];

            // Retrieve the customer's basket from Basket repository
            var basket = await basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);

            // Validate the items real price from the DB instead of relying on the Frontend
            foreach(var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
            }

            // Get The Basket's Delivery Method, Then Retrieve its Shipping Price From DB to add it to The Total.
            if (!basket.DeliveryMethodId.HasValue)
                throw new Exception("No Delivery Method Was Selected");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value) ?? throw new DeliveryMethodException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Price;

            // Calc Total: [(item.price) * (item. Quantity)] + basket.shippingPrice (in cents)
            var amount = (long) (basket.Items.Sum(i => i.Price * i.Quantity) + basket.ShippingPrice) * 100;

            // Create the Stripe's Payment Intent Service
            var stripeService = new PaymentIntentService();

            // Create Or Update Payment Intent
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId)) // Create
            {
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };

                var paymentIntent = await stripeService.CreateAsync(createOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };

                await stripeService.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await basketRepository.CreateOrUpdateBasketAsync(basket);

            return mapper.Map<BasketDTO>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string json, string header)
        {
            // FETCH THE SECRET:
            var endpointSecret = configuration.GetSection("StripeSettings")["EndPointSecret"];

            // VERIFY AND CONSTRUCT THE STRIPE EVENT:
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                header,
                endpointSecret,
                throwOnApiVersionMismatch: false
            );

            // EXTRACT THE DATA AND CAST TO PAYMENT INTENT:
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            // ROUTE THE EVENT:
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                {
                    await UpdatePaymentIntentSucceeded(paymentIntent!.Id);
                    break;
                }
                case EventTypes.PaymentIntentPaymentFailed:
                {
                    await UpdatePaymentIntentFailed(paymentIntent!.Id);
                    break;
                }
                default:
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
                }
            }
        }

        private async Task UpdatePaymentIntentFailed(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            var order = await orderRepo.GetAsync(new
            OrderWithPaymentIntentSepcifications(paymentIntentId))
            ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;

            orderRepo.Update(order);

            await unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentIntentSucceeded(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            var order = await orderRepo.GetAsync(new
            OrderWithPaymentIntentSepcifications(paymentIntentId))
            ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;

            orderRepo.Update(order);

            await unitOfWork.SaveChangesAsync();
        }
    }
}