using DAL.Models;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories.Generic
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> Get(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? take = null,
            string includeProperties = "",
            params Expression<Func<T, object>>[] thenIncludeProperties);
        public PaginatedDataModel<T> GetPage(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T Find(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        bool IsExists<TValue>(string key, TValue value);
        public IEnumerable<T> ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters);
        TResult ExecuteScalarSqlFunction<TResult>(string sqlFunctionQuery, params object[] parameters);
        TResult ExecuteScalarRawSql<TResult>(string sqlQuery, params object[] parameters);
        public IEnumerable<T> Get(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
        public T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
