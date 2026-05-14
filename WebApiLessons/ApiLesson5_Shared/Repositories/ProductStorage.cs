using ApiLesson5_Shared.Domain;

namespace ApiLesson5_Shared.Repositories
{
    public class ProductStorage : IProductStorage
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Description = "Description of Product 1", Price = 10.99m, Stock = 100, Notes = "Notes for Product 1" },
            new Product { Id = 2, Name = "Product 2", Description = "Description of Product 2", Price = 20.99m, Stock = 50, Notes = "Notes for Product 2" },
            new Product { Id = 3, Name = "Product 3", Description = "Description of Product 3", Price = 30.99m, Stock = 25, Notes = "Notes for Product 3" }
        };

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1; // Auto-increment ID
            _products.Add(product);
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductById(id);

            if (product != null)
                _products.Remove(product);
        }
    }
}
