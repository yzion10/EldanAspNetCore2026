using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;
using Riok.Mapperly.Abstractions;

namespace ApiLesson5_Mapperly.Mappings
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial ProductDto ToDto(Product product);
        public partial List<ProductDto> ToDtos(List<Product> products);
    }
}
