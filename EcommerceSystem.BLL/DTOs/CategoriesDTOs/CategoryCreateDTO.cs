using System.ComponentModel.DataAnnotations;


namespace EcommerceSystem.BLL
{
    
    public class CategoryCreateDTO
    {
        /*---------------------------------------------------*/
        public int Id { get; set; }

        [Display(Name= "Category Name")]
        public string Name { get; set; }
        /*---------------------------------------------------*/
        
    }
}
