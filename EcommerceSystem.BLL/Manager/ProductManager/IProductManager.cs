using EcommerceSystem.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.BLL
{
    public interface IProductManager
    {
        /*---------------------------------------------------*/
        public GeneralResult<List<ProductReadDTO>> GetAllProducts();
        /*---------------------------------------------------*/
        public GeneralResult<ProductReadDTO> GetProductById(int id);
        /*---------------------------------------------------*/
        public GeneralResult<ProductCreateDTO> CreateProduct(ProductCreateDTO productCreateVM);
        /*---------------------------------------------------*/
        public GeneralResult<ProductEditDTO> UpdateProduct(ProductEditDTO productEditVM);
        /*---------------------------------------------------*/
        public GeneralResult DeleteProduct(int id);
        /*---------------------------------------------------*/
        public List<CategoryReadDTO>? GetCategoryList();
        /*---------------------------------------------------*/
    }
}
