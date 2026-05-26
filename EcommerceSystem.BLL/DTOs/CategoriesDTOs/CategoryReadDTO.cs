

using EcommerceSystem.DAL;

namespace EcommerceSystem.BLL
{
    public class CategoryReadDTO
    {
        /*---------------------------------------------------*/
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoProductTypes { get; set; }
        /*---------------------------------------------------*/
        // Relation With Products
        public List<Product> Products { get; set; } =
            new List<Product>();

    }
}
