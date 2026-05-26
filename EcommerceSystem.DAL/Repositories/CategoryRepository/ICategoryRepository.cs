
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        public IEnumerable<Category> GetAllWithProducts();
        public Category? GetByIdWithProducts(int id);
    }
}
