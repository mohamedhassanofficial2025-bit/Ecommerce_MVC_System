

using EcommerceSystem.DAL;

namespace EcommerceSystem.PL
{
    public class CategoryReadVM
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
