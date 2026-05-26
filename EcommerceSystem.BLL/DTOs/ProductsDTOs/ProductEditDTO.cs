
using System.ComponentModel.DataAnnotations;


namespace EcommerceSystem.BLL
{
    public class ProductEditDTO
    {
        /*---------------------------------------------------*/
        //Send From User


        public int ProdId { get; set; }
        public string ProdTitle { get; set; }
        public string ProdDescription { get; set; }
        public decimal ProdPrice { get; set; }
        public int ProdCount { get; set; }

        public string? ProdName { get; set; }
        public int CategoryId { get; set; }
        public string ProdCategory { get; set; }
        public string? ImageURL { get; set; }
        public DateOnly ExpiryDate { get; set; }
        /*---------------------------------------------------*/
        //Send To User
        public List<CategoryReadDTO>? Categories { get; set; }
    }
}
