using System.Linq;
using Api.Dtos;
using AutoMapper;
using Infrastructure.Models;
using Infrastructure.OrderAggregate;

namespace Api.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
             .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            CreateMap<FirmProduct, FirmProductToReturnDto>()
           .ForMember(d => d.PictureUrl, o => o.MapFrom<FirmProductUrlResolver>());

            CreateMap<Infrastructure.Identity.Models.Address, AddressDto>().ReverseMap();

            CreateMap <AddressDto, FirmAddress>();

            CreateMap<OrderDto, Order>();
        }
    }
}