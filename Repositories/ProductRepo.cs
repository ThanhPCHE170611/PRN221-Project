using BusinessObject.Models;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDAO productDAO;

        public ProductRepo()
        {
            productDAO = new ProductDAO();
        }
        public List<Product> GetProducts() => productDAO.GetProducts();
        public Product AddProduct(Product product) => productDAO.AddProduct(product);
        public Product GetProductByID(int? productId) => productDAO.GetProductByID(productId);
        public Product UpdateProduct(Product product) => productDAO.UpdateProduct(product);
        public Product DeleteProduct(int? product) => productDAO.DeleteProduct(product);
    }
}
