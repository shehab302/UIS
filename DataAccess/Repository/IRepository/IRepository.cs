using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public IQueryable<T> GetAll(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);
        public IQueryable<T> GetOne(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);
        void Add(T category);
        void AddRange(IEnumerable<T> entity);
        void Edit(T category);
        void Delete(T category);
        void Commit();

    }
}
