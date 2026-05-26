using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public class Product
    {
        /*---------------------------------------------------*/
        // Product Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string? ImageURL { get; set; }
        public DateOnly ExpiryDate { get; set; } = new DateOnly();
        /*---------------------------------------------------*/
        //Relation With Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        /*------------------------------------------------------------------*/
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /*------------------------------------------------------------------*/
    }
}
