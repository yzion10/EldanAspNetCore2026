using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;
using AutoMapper;

namespace ApiLesson5_AutoMapper.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Create a mapping configuration between the Product domain model and the ProductDto
            CreateMap<Product, ProductDto>();
        }
    }
}
