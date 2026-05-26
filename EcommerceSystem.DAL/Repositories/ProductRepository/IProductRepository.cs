using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL.Repositories.ProductRepository
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        public IEnumerable<Product> GetAllWithCategory();
        public Product? GetByIdWithCategory(int id);

    }
}
