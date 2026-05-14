using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;

namespace ApiLesson5_Shared.Repositories
{
    public interface IProductStorage
    {
        Product AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        Product? Update(int id, Product updatedProduct);
    }
}