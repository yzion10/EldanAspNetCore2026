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

            CreateMap<ProductCreateDto, Product>();

            CreateMap<ProductUpdateDto, Product>();

            // Create a mapping configuration between the Feature domain model and the FeatureDto
            CreateMap<Feature, FeatureDto>().
                ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => $"{src.Id} - {src.Name}")); // Custom mapping for the Description property in FeatureDto
        }
    }
}
