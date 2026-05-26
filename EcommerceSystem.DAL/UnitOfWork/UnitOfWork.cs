using EcommerceSystem.DAL.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        /*------------------------------------------------------------------*/
        private readonly AppDbContext _appDbContext;
        /*------------------------------------------------------------------*/

        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        /*------------------------------------------------------------------*/
        public UnitOfWork
            (
                IProductRepository productRepository,
                ICategoryRepository categoryRepository,
                AppDbContext appDbContext
            )
        {
            _appDbContext = appDbContext;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
        }

        /*------------------------------------------------------------------*/

        public void save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
