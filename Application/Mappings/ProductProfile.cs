using AutoMapper;
using CrudMvc.Application.DTO;
using CrudMvc.Domain.Models;

namespace CrudMvc.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => "/images/" + src.ImageFileName));

            CreateMap<CreateProductDTO, Product>();
        }
    }
}
