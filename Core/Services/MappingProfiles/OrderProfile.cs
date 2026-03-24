global using ShippingAddress = Domain.Entities.OrderModule.Address;
global using UserAddress = Domain.Entities.IdentityModule.Address;
using AutoMapper;
using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Microsoft.Extensions.Options;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<ShippingAddress, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
            .ForMember(D => D.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<Order, OrderResult>()
            .ForMember(d => d.PaymentStatus, o => o.MapFrom(s => s.PaymentStatus))
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.Subtotal + s.DeliveryMethod.Price));

            CreateMap<DeliveryMethod, DeliveryMethodResult>();
        }

    }

}
