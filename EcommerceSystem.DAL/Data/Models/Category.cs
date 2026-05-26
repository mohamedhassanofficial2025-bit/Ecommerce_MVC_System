using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public class Category
    {
        /*---------------------------------------------------*/
        public int Id { get; set; }
        public string Name { get; set; }
        /*---------------------------------------------------*/
        // Relation With Products
        public ICollection<Product> Products { get; set; } =
            new HashSet<Product>();
        /*------------------------------------------------------------------*/
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /*------------------------------------------------------------------*/
    }
}
