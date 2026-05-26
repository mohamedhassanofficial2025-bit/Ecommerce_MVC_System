using System.ComponentModel.DataAnnotations;

namespace EcommerceSystem.PL
{
    public class CategoryProductsReadVM
    {
        public int ProdId { get; set; }
        public string ProdTitle { get; set; }
   
        public string ProdDescription { get; set; }

        public string ? imgUrl { get; set; }
        public decimal ProdPrice { get; set; }
        public int ProdCount { get; set; }
        public string? ProdCategory { get; set; }
    }
}
