using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    /// <summary>
    /// Generic innterface that contain all the functions to One Entry of Entity Context 
    /// </summary>
    /// <typeparam name="T">Type of Table (Entity Class)</typeparam>
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        T Add(T entity);//changed from void to T
        void AddRange(IEnumerable<T> entities);
        void Edit(T entity);
        void Delete(T entity);
        void Save();
        int Count();
        T Find(params object[] KeyValues);
        T FindSingle(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate);
        List<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        List<T> Get(int StartIndex, int Count, string Sorting, out int CountRecords);

        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to specified key selector and element selector functions.
        //
        // Parameters:
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        // Type parameters:
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains values of type TElement
        //     selected from the input sequence.
        //
        IDictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Expression<Func<T, bool>> where = null);


        /// <summary>
        /// return list displaytext and value to select html async
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        //Task<List<Options>> GetOptions(Expression<Func<T, Options>> keySelector,Expression<Func<T, bool>> where=null, int Count = 0, Expression<Func<T, string>> orderby = null);
        Task<List<Options>> GetOptions(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null, int count = 0, Expression<Func<T, string>> orderby = null, Expression<Func<T, int>> orderbyint = null);
        /// <summary>
        /// return list displaytext and value to select html async
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        List<Options> GetOptionsNoAsync(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null);

        IEnumerable<TResult> GetSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select);
        bool Any(Expression<Func<T, bool>> func);
        IQueryable<T> Where(Expression<Func<T, bool>> func);
        IQueryable<T> WhereAndInclude<TProperty>(Expression<Func<T, bool>> func, Expression<Func<T, TProperty>> path);
        Task<int> SaveAsync(WindowsPrincipal NTUser, bool isSystem = false, List<History> histories = null, IDictionary<string, string> Fields = null, string UserName = "");
        Task<int> SaveChangesAsync();
        Task<T> FindAsync(params object[] KeyValues);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<TResult>> GetSelectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> func);
    }
}
