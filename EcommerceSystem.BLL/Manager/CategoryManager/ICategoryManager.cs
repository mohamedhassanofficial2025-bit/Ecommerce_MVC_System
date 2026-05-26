using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.BLL
{
    public interface ICategoryManager
    {
        /*---------------------------------------------------*/
        List<CategoryReadDTO> GetAllCategories();
        /*---------------------------------------------------*/
        CategoryReadDTO GetCategoryById(int id);
        /*---------------------------------------------------*/
        void CreateCategory(CategoryCreateDTO categoryCreateVM);
        /*---------------------------------------------------*/
        void UpdateCategory(CategoryEditDTO categoryEditVM);
        /*---------------------------------------------------*/
        void DeleteCategory(int id);
        /*---------------------------------------------------*/

    }
}
