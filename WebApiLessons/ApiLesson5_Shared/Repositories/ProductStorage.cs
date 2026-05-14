using ApiLesson5_Shared.Domain;
using ApiLesson5_Shared.Dto;

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
        private object _lock = new object();

        public List<Product> GetAllProducts()
        {
            lock (_lock)
                return _products.Select(Clone).ToList();
        }

        public Product? GetProductById(int id)
        {
            lock (_lock)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                return product != null ? Clone(product) : null;
            }
        }

        public Product AddProduct(Product product)
        {
            lock (_lock)
            {
                var productToAdd = Clone(product);

                productToAdd.Id = _products.Max(p => p.Id) + 1; // Auto-increment ID
                productToAdd.Notes += $" (Added on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})"; // Add note with timestamp
                _products.Add(productToAdd);

                return Clone(productToAdd);
            }
        }

        public void DeleteProduct(int id)
        {
            lock (_lock)
            {
                var product = GetProductById(id);

                if (product != null)
                    _products.Remove(product);
            }
        }

        public static Product Clone(Product product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Notes = product.Notes
            };
        }
    }
}
