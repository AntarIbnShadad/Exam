using Exam.Data;
using System.Linq.Expressions;

namespace Exam.Business
{
    public class Repository: IRepository
    {
        private AppDbContext _db;
        public Repository(AppDbContext db) 
        {
            _db = db;
        }
        public IQueryable<T> Get<T>() where T : class
        {
            return _db.Set<T>();
        }

        public T GetById<T>(object id) where T : class
        {
            return _db.Set<T>().Find(id);
        }
        public T GetBy<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _db.Set<T>().FirstOrDefault(predicate);
        }
        public void Add<T>(T entity) where T : class
        {
            _db.Set<T>().Add(entity);
            Save();
        }

        public void Update<T>(object id, Action<T> updateFn) where T : class
        {
            var dbSet = _db.Set<T>();
            var entity = dbSet.Find(id);
            if (entity == null)
            {
                throw new Exception($"{typeof(T).Name} with ID {id} not found.");
            }
            updateFn(entity);
            Save();
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = _db.Set<T>();
            var entity = dbSet.FirstOrDefault(predicate);
            if (entity == null)
            {
                throw new Exception($"{typeof(T).Name} not found for the given condition.");
            }

            dbSet.Remove(entity);
            Save();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
