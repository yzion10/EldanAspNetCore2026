using ApiLesson5_Shared.Domain;

namespace ApiLesson5_Shared.Repositories
{
    public interface IProductStorage
    {
        Product AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
    }
}