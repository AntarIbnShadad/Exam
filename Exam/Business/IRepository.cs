using System.Linq.Expressions;

namespace Exam.Business
{
    public interface IRepository
    {
        IQueryable<T> Get<T>() where T : class;
        T GetById<T>(object id) where T : class;
        T GetBy<T>(Expression<Func<T, bool>> predicate) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(object id, Action<T> updateFn) where T : class;
        void Delete<T>(T entity) where T : class;
        void Save();
    }
}
