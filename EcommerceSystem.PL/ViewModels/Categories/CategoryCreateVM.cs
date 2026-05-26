using System.ComponentModel.DataAnnotations;


namespace EcommerceSystem.PL
{
    
    public class CategoryCreateVM
    {
        /*---------------------------------------------------*/
        public int Id { get; set; }

        [Display(Name= "Category Name")]
        public string Name { get; set; }
        /*---------------------------------------------------*/
        
    }
}
