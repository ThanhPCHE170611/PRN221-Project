using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category GetCategoryById(int id);
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool BanCategoryStatus(int id);
        List<Category> LoadCategories();
    }
}
