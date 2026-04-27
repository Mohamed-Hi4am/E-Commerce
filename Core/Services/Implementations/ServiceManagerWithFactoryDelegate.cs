using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDelegate(
        Func<IAuthenticationService> authenticationFactory,
        Func<IProductService> productFactory,
        Func<IBasketService> basketFactory,
        Func<IOrderService> orderFactory,
        Func<IPaymentService> paymentFactory,
        Func<ICacheService> cacheFactory) : IServiceManager
    {
        public IAuthenticationService AuthenticationService => authenticationFactory.Invoke();

        public IProductService ProductService => productFactory.Invoke();

        public IBasketService BasketService => basketFactory.Invoke();

        public IOrderService OrderService => orderFactory.Invoke();

        public IPaymentService PaymentService => paymentFactory.Invoke();

        public ICacheService CacheService => cacheFactory.Invoke();
    }
}