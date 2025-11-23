using AutoMapper;
using Domain.Contracts;
using Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper) : IServiceManager
    {
        private readonly Lazy<IProductService> _productService =
            new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));

        public IProductService ProductService => _productService.Value;
    }
}
