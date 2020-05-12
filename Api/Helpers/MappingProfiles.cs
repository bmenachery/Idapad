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
        }
    }
}