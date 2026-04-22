using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDelegate(
        Func<IAuthenticationService> authenticationService,
        Func<IProductService> productService,
        Func<IBasketService> basketService,
        Func<IOrderService> orderService,
        Func<IPaymentService> paymentService) : IServiceManager
    {
        public IAuthenticationService AuthenticationService => authenticationService.Invoke();

        public IProductService ProductService => productService.Invoke();

        public IBasketService BasketService => basketService.Invoke();

        public IOrderService OrderService => orderService.Invoke();

        public IPaymentService PaymentService => paymentService.Invoke();
    }
}