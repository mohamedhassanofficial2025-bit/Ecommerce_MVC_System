using System.ComponentModel.DataAnnotations;

namespace EcommerceSystem.PL
{
    public class ProductReadVM
    {
        /*---------------------------------------------------*/

        public int ProdId { get; set; }
        public string ProdTitle { get; set; }
        public string ProdDescription { get; set; }
        public decimal ProdPrice { get; set; }
        public int ProdCount { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public string? ImageURL { get; set; }
        public string ProdCategory { get; set; }
        public int CategoryId { get; set; }
        /*---------------------------------------------------*/

    }
}
