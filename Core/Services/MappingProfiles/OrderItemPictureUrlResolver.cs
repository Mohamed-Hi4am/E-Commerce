using AutoMapper;
using AutoMapper.Execution;
using Domain.Entities.OrderModule;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            // https://localhost:7081+ images/products/ItalianChickenMarinade.png

            if (string.IsNullOrWhiteSpace(source.Product.PictureUrl))
                return string.Empty;

            // BaseUrl + source.PictureUrl
            // https://localhost:7081+ images/products/ItalianChickenMarinade.png

            return $"{configuration["BaseUrl"]} {source.Product.PictureUrl}";
        }
    }
}
