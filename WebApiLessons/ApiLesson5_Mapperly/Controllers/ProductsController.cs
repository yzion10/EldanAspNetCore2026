using ApiLesson5_Mapperly.Mappings;
using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;
using ApiLesson5_Shared.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson5_Mapperly.Controllers
{
    // שימוש ב mapperly

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductStorage _productStorage;
        private readonly ProductMapper _productMapper;

        public ProductsController(IProductStorage productStorage, ProductMapper productMapper)
        {
            _productStorage = productStorage ?? throw new ArgumentNullException(nameof(productStorage));
            _productMapper = productMapper ?? throw new ArgumentNullException(nameof(productMapper));
        }

        [HttpGet]
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var products = _productStorage.GetAllProducts();

            var productsToReturn = _productMapper.ToDtos(products);

            return Ok(productsToReturn);
        }

    }
}
