using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.DataAccess.Repository.IRepository
{
   public interface IRepository<T>where T :class
    {
        T Get(int Id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>
            filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy=null,
            string includeproperties=null
            );
        T FirstOrDefault(
            Expression<Func<T,bool>>
            filter=null,
            string includeproperties=null
            );
        void Add(T entity);
        void Remove(int Id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
