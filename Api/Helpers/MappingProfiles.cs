using System.Linq;
using Api.Dtos;
using AutoMapper;
using Infrastructure.Models;


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

            CreateMap<FirmOrder, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<FirmOrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

           CreateMap<Infrastructure.Identity.Models.Address, AddressDto>().ReverseMap();

            CreateMap <AddressDto, FirmAddress>().ReverseMap(); ;

            CreateMap<OrderDto, FirmOrder>();
        }
    }
}