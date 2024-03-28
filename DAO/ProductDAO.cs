using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;

        public ProductDAO() { }

        public static ProductDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductDAO();
                }
                return Instance;
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> listProduct = null;
            try
            {
                var dbContext = new PRN221_ProjectContext();
                listProduct = dbContext.Products
                    .Include(c => c.Area)
                    .Include(c => c.Category)
                    .Include(c => c.StockOutDetails)
                    .Include(c => c.LotDetails)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listProduct;
        }
        
        public Product AddProduct(Product product)
        {
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                Product old = myStoreDB.Products
                    .SingleOrDefault(p => p.ProductCode.ToLower().Equals(product.ProductCode.ToLower()));

                if(old == null)
                {
                    product.Quantity = 0;
                    product.Status = 1;
                    myStoreDB.Products.Add(product);
                    myStoreDB.SaveChanges();
                } else
                {
                    Console.WriteLine("Can not create new Product");    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public Product GetProductByID(int? id)
        {
            Product product = null;
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                product = myStoreDB.Products
                            .AsNoTracking()
                            .Include(c => c.Area)
                            .Include(c => c.Category)
                            .Include(c => c.StockOutDetails)
                            .Include(c => c.LotDetails)
                            .SingleOrDefault(p => p.ProductId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public Product UpdateProduct(Product product)
        {
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                bool existingPartner = GetProducts()
                        .Where(p => p.ProductId != product.ProductId)
                        .Any(p => p.ProductCode.ToLower().Equals(product.ProductCode.ToLower()));

                if (!existingPartner)
                {
                    myStoreDB.Products.Update(product);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("Product code already exists!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }

        public Product DeleteProduct(int? productId)
        {
            Product product = GetProductByID(productId);
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                myStoreDB.Products.Remove(product);
                myStoreDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return product;
        }
    }
}
