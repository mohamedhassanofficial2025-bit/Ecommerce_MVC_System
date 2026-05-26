using EcommerceSystem.DAL;
using EcommerceSystem.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.BLL
{
    public class ProductManager : IProductManager
    {
        /*---------------------------------------------------*/
        private readonly IUnitOfWork _unitOfWork;

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /*---------------------------------------------------*/

        public GeneralResult<List<ProductReadDTO>> GetAllProducts()
        {
            var Products = _unitOfWork.ProductRepository.GetAllWithCategory();
            var data= Products.Select(p => new ProductReadDTO()
            {
                ProdId = p.Id,
                ProdTitle = p.Title,
                ProdDescription = p.Description,
                ProdCount = p.Count,
                ProdPrice = p.Price,
                ImageURL = p.ImageURL,
                ExpiryDate = p.ExpiryDate,
                ProdCategory = p.Category.Name
            }).ToList();
            return GeneralResult<List<ProductReadDTO>>.IsSucces("Success",data);
        }
        /*---------------------------------------------------*/
        public GeneralResult<ProductReadDTO> GetProductById(int id)
        {
            var p= _unitOfWork.ProductRepository.GetByIdWithCategory(id);
            var product= new ProductReadDTO()
            {
                ProdId = p.Id,
                ProdTitle = p.Title,
                ProdDescription = p.Description,
                ProdCount = p.Count,
                ExpiryDate = p.ExpiryDate,
                ProdPrice = p.Price,
                ImageURL = p.ImageURL,
                CategoryId= p.CategoryId,
                ProdCategory = p.Category.Name
            };
            return GeneralResult<ProductReadDTO>.IsSucces("Success",product);
        }
        /*---------------------------------------------------*/
        public GeneralResult<ProductEditDTO> UpdateProduct(ProductEditDTO productEditDto)
        {
            /*---------------------------------------------------*/

            var product = _unitOfWork.ProductRepository.GetByIdWithCategory(productEditDto.ProdId);
            product.Title = productEditDto.ProdTitle;
            product.Description = productEditDto.ProdDescription;
            product.Price = productEditDto.ProdPrice;
            product.ExpiryDate = productEditDto.ExpiryDate;
            product.Count = productEditDto.ProdCount;
            product.CategoryId = productEditDto.CategoryId;
            product.ImageURL = productEditDto.ImageURL;
            _unitOfWork.save();
            return GeneralResult<ProductEditDTO>.IsSucces("Success", productEditDto);
        }

        /*---------------------------------------------------*/

        public GeneralResult<ProductCreateDTO> CreateProduct(ProductCreateDTO productCreateDto)
        {
            /*---------------------------------------------------*/
           
            /*---------------------------------------------------*/
            //-->code
            var product = new Product()
            {
                Title = productCreateDto.ProdTitle,
                Description = productCreateDto.ProdDescription,
                Price = productCreateDto.ProdPrice,
                ExpiryDate = productCreateDto.ExpiryDate,
                Count = productCreateDto.ProdCount,
                CategoryId = productCreateDto.CategoryId,
                ImageURL= productCreateDto.ImageURL
            };
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.save();
            return GeneralResult<ProductCreateDTO>.IsSucces("Success", productCreateDto);
        }

        /*---------------------------------------------------*/
        public GeneralResult DeleteProduct(int id)
        {
            var Product = _unitOfWork.ProductRepository.GetById(id);    
            _unitOfWork.ProductRepository.Delete(Product!);
            _unitOfWork.save();
            return GeneralResult.IsSucces("Deleted");
        }

        /*---------------------------------------------------*/
        public List<CategoryReadDTO>? GetCategoryList()
        {
            var categoryList = _unitOfWork
                .CategoryRepository.GetAll().ToList();
            return categoryList.Select(c => new CategoryReadDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}
