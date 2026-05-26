
using System.ComponentModel.DataAnnotations;

namespace EcommerceSystem.PL
{
    public class ProductCreateVM
    {
        /*---------------------------------------------------*/
        //Send From User

        
        public int ProdId { get; set; }
        [Required(ErrorMessage = "Title field is required")]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string ProdTitle { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string ProdDescription { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100000")]
        public decimal ProdPrice { get; set; }

        [Required(ErrorMessage = "Count is required")]
        [Range(0, 10000, ErrorMessage = "Count must be between 0 and 10000")]
        public int ProdCount { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }
        public string ProdCategory { get; set; }
        public DateOnly ExpiryDate { get; set; }
        /*---------------------------------------------------*/
        //Send To User
        public List<CategoryReadVM>? Categories { get; set; }
    }
}
