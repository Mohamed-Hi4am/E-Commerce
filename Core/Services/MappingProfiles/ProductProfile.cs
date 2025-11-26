using AutoMapper;
using Domain.Entities.ProductModule;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandResultDto>();

            CreateMap<Product, ProductResultDto>()
            .ForMember(d => d.BrandName, options => options.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.TypeName, options => options.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, options => options.MapFrom<PictureUrlResolver>());

            CreateMap<ProductType, TypeResultDto>();
        }
    }
}
