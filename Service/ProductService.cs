using BusinessObject.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        public ProductService()
        {
            _productRepo = new ProductRepo();
        }
        public List<Product> GetProducts() => _productRepo.GetProducts();
        public Product AddProduct(Product product) => _productRepo.AddProduct(product);
        public Product GetProductByID(int? productId) => _productRepo.GetProductByID(productId);
        public Product UpdateProduct(Product product) => _productRepo.UpdateProduct(product);
        public Product DeleteProduct(int? productId) => _productRepo.DeleteProduct(productId);
    }
}
