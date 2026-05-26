using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public IEnumerable<Category> GetAllWithProducts()
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }

        public Category? GetByIdWithProducts(int id)
        {
            return _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        }
    }
}
