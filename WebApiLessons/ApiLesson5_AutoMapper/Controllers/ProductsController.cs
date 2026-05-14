using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;
using ApiLesson5_Shared.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiLesson5_AutoMapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductStorage _productStorage;
        private readonly IMapper _mapper;

        public ProductsController(IProductStorage productStorage, IMapper mapper)
        {
            _productStorage = productStorage ?? throw new ArgumentNullException(nameof(productStorage));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var products = _productStorage.GetAllProducts();

            var productDtos = _mapper.Map<List<ProductDto>>(products);


            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var product = _productStorage.GetProductById(id);
            if (product == null)
                return NotFound();

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpPost]
        public ActionResult<ProductDto> CreateProduct(ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);
            var createdProduct = _productStorage.AddProduct(product);

            var productDto = _mapper.Map<ProductDto>(createdProduct);
            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDto> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            var product = _mapper.Map<Product>(productUpdateDto);

            var updatedProduct = _productStorage.Update(id, product);

            return Ok(_mapper.Map<ProductDto>(updatedProduct));
        }
    }
}
