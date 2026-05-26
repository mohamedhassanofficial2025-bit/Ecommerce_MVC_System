using EcommerceSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.BLL
{
    public class CategoryManager : ICategoryManager
    {
        /*---------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /*---------------------------------------------------*/

        public List<CategoryReadDTO> GetAllCategories()
        {
            var Categories = _unitOfWork.CategoryRepository.GetAllWithProducts();
            return   Categories.Select(c => new CategoryReadDTO()
            {
                Id = c.Id,
                Name = c.Name,
                NoProductTypes = c.Products.Count(),
                Products= c.Products.ToList()
            }).ToList();
        }
        /*---------------------------------------------------*/

        public CategoryReadDTO GetCategoryById(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetByIdWithProducts(id);
            if (category == null)
                return null;
            return new CategoryReadDTO()
            {
                Id = category.Id,
                Name = category.Name,
                NoProductTypes = category.Products.Count(),
                Products = category.Products.ToList()
            };
        }
        /*---------------------------------------------------*/

        public void CreateCategory(CategoryCreateDTO categoryCreateVM)
        {
            Category category = new Category()
            {
                Name = categoryCreateVM.Name,
            };
            _unitOfWork.CategoryRepository.Insert(category);
            _unitOfWork.save();
        }
        /*---------------------------------------------------*/

        public void UpdateCategory(CategoryEditDTO categoryEditVM)
        {
            var category = _unitOfWork.CategoryRepository.GetByIdWithProducts(categoryEditVM.Id);
            if (category == null)
                return;
            category.Name = categoryEditVM.Name;
            _unitOfWork.save();  
        }
        /*---------------------------------------------------*/

        public void DeleteCategory(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
                return;
            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.save();
        }


    }
}
