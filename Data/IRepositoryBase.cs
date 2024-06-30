using GuitarShop.Data.DataAccess;
using System.Linq.Expressions;

namespace University_of_free_state_booking_Facilities.Data
{
    public interface IRepositoryBase<T>
    {
        T GetById(int id);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAllSorted(string sortBy);
        IEnumerable<T> GetWithOptions(QueryOptions<T> options);
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
      
        void Save();
    }
}
