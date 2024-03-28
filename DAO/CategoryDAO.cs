using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;

        public CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoryDAO();
                }
                return instance;
            }
        }

        public List<Category> GetCategories()
        {
            List<Category> category;
            try
            {
                var context = new PRN221_ProjectContext();
                category = context.Categories
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public Category GetCategoryAreaByID(int id)
        {
            Category category = null;
            try
            {
                var db = new PRN221_ProjectContext();
                category = db.Categories.SingleOrDefault(u => u.CategoryId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return category;
        }

        public bool AddCategory(Category category)
        {
            try
            {
                bool existingCategory = GetCategories()
                    .Any(a => a.CategoryCode.ToLower().Equals(category.CategoryCode.ToLower()));

                if (existingCategory == false)
                {
                    category.Status = 1;

                    using (var db = new PRN221_ProjectContext())
                    {
                        db.Categories.Add(category);
                        db.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    throw new Exception("Category already exists!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Add Category: {ex.Message}", ex);
                return false;
            }
        }

        public bool UpdateCategory(Category category)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    var existing = db.Categories.SingleOrDefault(x => x.CategoryId == category.CategoryId);
                    if (existing != null)
                    {
                        existing.Name = category.Name;

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Category not found for updating.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCategory: {ex.Message}", ex);
                return false;
            }
        }

        public bool BanCategoryStatus(int id)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    Category category1 = db.Categories.SingleOrDefault(c => c.CategoryId == id);

                    if (category1 != null)
                    {
                        bool product = db.Products.Any(u => u.CategoryId == id);
                        if (!product)
                        {
                            category1.Status = (category1.Status == 0) ? 1 : 0;

                            db.Entry(category1).State = EntityState.Modified;
                            db.SaveChanges();
                            Console.WriteLine("Category status updated successfully!");
                            return true;
                        } else
                        {
                            throw new Exception("This category also have products, can not ban!");
                        }
                    }
                    else
                    {
                        throw new Exception("Category does not exist!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in BanCategory: {ex.Message}");
                return false;
            }
        }

        public List<Category> LoadCategories()
        {
            List<Category> loadArea;
            try
            {
                var myStoreDB = new PRN221_ProjectContext();
                loadArea = myStoreDB.Categories
               .Where(area => area.CategoryId != null)
               .Select(area => new Category
               {
                   CategoryId = area.CategoryId,
                   Name = area.Name
               })
               .Distinct()
               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return loadArea;
        }
    }
}
