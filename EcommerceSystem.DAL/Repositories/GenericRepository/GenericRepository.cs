using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /*------------------------------------------------------------------*/
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        /*------------------------------------------------------------------*/
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        /*------------------------------------------------------------------*/

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }


        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            _context.Add(entity);
        }
    }
}
