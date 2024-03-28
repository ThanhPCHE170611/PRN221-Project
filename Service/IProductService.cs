using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProductService
    {
        public List<Product> GetProducts();
        public Product AddProduct(Product product);
        public Product GetProductByID(int? productId);
        public Product UpdateProduct(Product product);
        public Product DeleteProduct(int? productId);
    }
}
