using PREP.DAL.TableViews;
using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;

namespace PREP.DAL.Repositories.Memory
{

    public class HistoryEntry
    {
        public DbEntityEntry Entry { get; set; }
        public List<History> Histories { get; set; }
    }

    /// <summary>
    /// Generic class that contain all the functions to One Entry Type of Entity Context 
    /// </summary>
    /// <param name="C">
    /// Context
    /// </param>
    /// <param name="T">
    /// Type of Table 
    /// </param>
    public abstract class GenericRepository<C, T> : IGenericRepository<T> where T : class where C : DbContext, new()
    {
        #region Properties
        private delegate History GetHistoryFields(DbEntityEntry entry, string propertyName, int activityLogID, int tableId, int itemId, int? releaseId);

        private C _entities = new C();
        protected C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }
        protected DbSet<T> DbSet { get; set; }
        #endregion
        

        #region Constructors 
        public GenericRepository()
        {
            try
            {
                DbSet = _entities.Set<T>();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        #endregion

        #region Private Methods


        /// <summary>
        /// get List of Entries of One  Activity Type(Create/Edit)  and Save them in Hisory and ActivityLog Tables 
        /// </summary>
        /// <param name="NTUser"></param>
        /// <param name="addedEntries"></param>
        /// <param name="ActivityType"></param>
        /// <returns></returns>
        private async Task SaveToLogByActivityType(int NTUser, List<DbEntityEntry> Entries, ActivityType Activity, bool isSystem = false, IDictionary<string, string> Fields = null, List<History> HistoriesList = null)
        {
            if (Entries == null || Entries.Count() <= 0)
                return;

            try
            {

                ActivityLog activityLog = new ActivityLog(NTUser, Activity);
                using (IActivityLogRepository db = new ActivityLogRepository())
                {
                    db.Add(activityLog);
                    await db.SaveChangesAsync();
                }

                if (HistoriesList == null)
                {
                    GetHistoryFields GetHistoryFields;
                    //if Create only get current all fields to history else get oly Updated fields in entry
                    if (Activity == ActivityType.Create)
                        GetHistoryFields = GetFields;
                    else
                        GetHistoryFields = GetUpdatedFields;
                    HistoriesList = new List<History>();
                    foreach (var item in Entries)
                    {
                        HistoriesList.AddRange(GetHistoryList(item, GetHistoryFields, activityLog.ActivityLogID, Fields));
                    }
                    // Entries.ForEach(e => 
                }
                else
                {
                    HistoriesList.ForEach(h => h.ActivityLogID = activityLog.ActivityLogID);
                }

                if (HistoriesList != null && HistoriesList.Count() > 0)
                {
                    using (PREPContext db = new PREPContext())
                    {
                        using (var transactionScope = new TransactionScope())
                        {
                            db.BulkInsert(HistoriesList);
                            db.SaveChanges();
                            transactionScope.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex.Message);
            }
        }


        //    private async Task<bool> UpdateEntityHistoryAsync(Log Log, WindowsPrincipal NTUser, ActivityType Activity, bool isSystem, DbEntityEntry entry, int i, GetHistoryFields HistoryDelegate, IDictionary<string, string> Fields = null)
        //{
        //    OutputDebug.Write("SaveToLogByActivityType Start");

        //    List<History> Histories = HistoryDelegate(entry, Fields);
        //    int key, releaseId;
        //    OutputDebug.Write("SaveToLogByActivityType Histories ");

        //    int.TryParse(GetEntityID(entry).ToString(), out key);
        //    if (await Log.UpdateLogs(entry.Entity, key, GetReleaseID(entry), Histories, Activity, isSystem ? ConfigurationManager.AppSettings["SystemUserNtnet"] : NTUser.Identity.Name) == false) return false;
        //    OutputDebug.Write("SaveToLogByActivityType UpdateLogs ");

        //    return true;
        //}


        /// <summary>
        /// Get Entity Table and  Return  All Fields  to HistoryList 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private History GetFields(DbEntityEntry entry, string propertyName, int activityLogID, int tableID, int itemId, int? releaseId)
        {
            var newValue = entry.CurrentValues.GetValue<object>(propertyName) != null ?
                    entry.CurrentValues.GetValue<object>(propertyName).ToString() : string.Empty;
            if (newValue != null)
            {
                return new History(propertyName, string.Empty, newValue, itemId, tableID, releaseId, activityLogID);
            }
            return null;
        }
        /// <summary>
        /// Get Entity Table and find which fields changed and return them to HistoryList
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private History GetUpdatedFields(DbEntityEntry entry, string propertyName, int activityLogID, int tableID, int itemId, int? releaseId)
        {
            var DatabaseValue = entry.GetDatabaseValues().GetValue<object>(propertyName);
            var CurrentValue = entry.CurrentValues.GetValue<object>(propertyName);
            var originalValue = (DatabaseValue != null ?
                            (DatabaseValue.GetType().IsEnum == false ?
                            DatabaseValue.ToString() : ((int)DatabaseValue).ToString()) :
                            string.Empty);
            var newValue = CurrentValue != null ?
               (CurrentValue.GetType().IsEnum == false ? CurrentValue.ToString() : ((int)CurrentValue).ToString())
               : string.Empty;
            if (originalValue != newValue)
            {
                return new History(propertyName, originalValue, newValue, itemId, tableID, releaseId, activityLogID);
            }
            return null;
        }
        private List<History> GetHistoryList(DbEntityEntry entry, GetHistoryFields getHistoryFields, int activityLogId, IDictionary<string, string> Fields = null)
        {
            List<History> histories = new List<History>();
            int tableId, itemId;
            int? releaseId;
            tableId = GetTableID(entry.Entity);
            itemId = GetEntityID(entry);
            releaseId = GetReleaseID(entry);
            try
            {
                foreach (var propertyName in entry.OriginalValues.PropertyNames)
                {
                    if (Fields == null || Fields.Count == 0 || Fields.ContainsKey(propertyName))
                    {
                        History history = getHistoryFields(entry, propertyName, activityLogId, tableId, itemId, releaseId);
                        if (history != null)
                            histories.Add(history);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return histories;
        }


        private int GetTableID(object entry)
        {
            int result;
            result= StaticResources.GetTableID(entry.GetType().Name);
            if (result == 0 && entry.GetType().BaseType!=null)
                 return StaticResources.GetTableID(entry.GetType().BaseType.Name);
            return result;
        }


       
        /// <summary>
        /// gets primary keys
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private int? GetReleaseID(DbEntityEntry entry)
        {
            if (entry.GetDatabaseValues().PropertyNames.Contains("ReleaseID") == false)
                return null;
            else
            {
                DbPropertyEntry releaseIDProperty = entry.Property("ReleaseID");
                return Convert.ToInt32(releaseIDProperty.OriginalValue);
            }
        }
        /// <summary>
        /// gets primary keys
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private int GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)_entities).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return (int)objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        private int GetEntityID(DbEntityEntry entry)
        {
            //var x= entry.CurrentValues.PropertyNames.First();
            var x = entry.Entity.GetType().GetProperties().FirstOrDefault(
                    p => p.CustomAttributes.Count() > 0 && p.CustomAttributes.Any(
                        attr => attr.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute)
                    && attr.ConstructorArguments.Count() > 0 && attr.ConstructorArguments.Any(ca => ca.Value.ToString() == "1"))
                    );
            if (x != null)
                return (int)entry.Property(x.Name.ToString()).CurrentValue;
            else return GetPrimaryKeyValue(entry);
            // entry.CurrentValues.
            //  var objectStateEntry = ((IObjectContextAdapter)_entities).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            //  return objectStateEntry.    
            // return x;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// add entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual T Add(T entity)//changed from void to T
        {
            try
            {
                return DbSet.Add(entity);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return null;
        }
        /// <summary>
        /// AddRange entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void AddRange(IEnumerable<T> entities)
        {
            try
            {
                DbSet.AddRange(entities);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        /// <summary>
        /// Attach entity to Db for edit
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(T entity)
        {
            try
            {
                DbSet.Attach(entity);
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// delete entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void Delete(T entity)
        {
            try
            {
                DbSet.Remove(entity);

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// Save all changes on DB
        /// </summary>
        public virtual void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        public virtual int Count()
        {
            int Cnt = 0;
            try
            {
                Cnt = DbSet.Count();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return Cnt;
        }
        public void Dispose()
        {
            try
            {
                Context.Dispose();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        /// <summary>
        /// Find one instance By key/s of the table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find(params object[] KeyValues)
        {
            T item = null;
            try
            {
                item = DbSet.Find(KeyValues);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }

        /// <summary>
        /// First Or Default conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FindSingle(Expression<Func<T, bool>> predicate)
        {
            T item = null;
            try
            {
                item = DbSet.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }
        /// <summary>
        /// Find by conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = null;
            try
            {
                items = DbSet.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        /// <summary>
        /// get all list of table
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetAll()
        {
            List<T> items = null;
            try
            {
                items = DbSet.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        /// <summary>
        /// get list of table according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<T> Get(int StartIndex, int Count, string Sorting, out int CountRecords)
        {
            CountRecords = this.Count();
            var query = Sorting != null ? DbSet.GetOrderByQuery(Sorting) : DbSet;
            return Count > 0 && CountRecords > 0
                       ? query.Skip(StartIndex).Take(Count).ToList() //Paging
                       : query.ToList(); //No paging
        }

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
        public IDictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Expression<Func<T, bool>> where = null)
        {
            IQueryable<T> query = DbSet;
            if (where != null)
                query = query.Where(where);
            return query.ToDictionary(keySelector, elementSelector);
        }


        /// <summary>
        /// return list displaytext and value to select html async
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public async Task<List<Options>> GetOptions(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null, int count = 0, Expression<Func<T, string>> orderby = null, Expression<Func<T, int>> orderbyint = null)
        {
            IQueryable<T> query = DbSet;
            if (where != null)
                query = query.Where(where);
            if (count > 0 && orderby != null) query = query.OrderBy(orderby).Take(count);
            if (count > 0 && orderbyint != null) query = query.OrderBy(orderbyint).Take(count);
            return await query.Select(keySelector).ToListAsync();
        }
        /// <summary>
        /// return list displaytext and value to select html
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public List<Options> GetOptionsNoAsync(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null)
        {
            if (where != null)
                return DbSet.Where(where).Select(keySelector).ToList();
            return DbSet.Select(keySelector).ToList();
        }

        public virtual IEnumerable<TResult> GetSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select)
        {
            IEnumerable<TResult> items = null;
            try
            {
                items = DbSet.Where(predicate).Select(select).ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public virtual bool Any(Expression<Func<T, bool>> func)
        {
            bool result = false;
            try
            {
                result = DbSet.Any(func);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return result;
        }
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> func)
        {
            IQueryable<T> items = null;
            try
            {
                items = DbSet.Where(func);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        /// <summary>
        /// get list of table include relation-ship according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual IQueryable<T> WhereAndInclude<TProperty>(Expression<Func<T, bool>> func, Expression<Func<T, TProperty>> path)
        {
            IQueryable<T> items = null;
            try
            {
                items = DbSet.Where(func).Include(path);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        #endregion

        #region Public Async Methods
        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public async virtual Task<int> SaveAsync(WindowsPrincipal NTUser, bool isSystem = false, List<History> histories = null, IDictionary<string, string> Fields = null, string UserName="")
        {
            int employeeID;
            using (IEmployeeRepository db = new EmployeeRepository())
            {
                employeeID = db.GetEmployee(NTUser != null ? NTUser.Identity.Name : UserName != "" ? UserName : ConfigurationManager.AppSettings["SystemUserNtnet"]);
            }
            int count = 0;
            try
            {
                var EditedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                var AddedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                var DeletedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

                //must be before do save to compare current values and db values 
                // List<DbEntityEntry>  EditedEntriesCopy= EditedEntries.ConvertAll(ent=> new DbEntityEntry(ent))
                await SaveToLogByActivityType(employeeID, EditedEntries, ActivityType.Edit, isSystem, Fields, histories);
                count += await SaveChangesAsync();
                //must be after do save to get key from database
                await SaveToLogByActivityType(employeeID, AddedEntries, ActivityType.Create);
                //    await saveadded(employeeID, AddedEntries);
                Debug.WriteLine("after ########################################################################");

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                count = -1;
            }
            return count;
        }

        //public Task SaveToLogByActivityTypeTask(WindowsPrincipal NTUser, List<DbEntityEntry> entries, bool isSystem)
        //{
        //    try
        //    {
        //        return Task.Run(() =>
        //                       SaveToLogByActivityType(NTUser, entries,
        //        ActivityType.Edit, isSystem)
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        string s = ex.Message;

        //    }
        //    return Task.Run(() =>
        //         SaveToLogByActivityType(NTUser, Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList(),
        //         ActivityType.Edit, isSystem)
        //         );
        //}

        //public Task saveadded(int NTUser, List<DbEntityEntry> entries)
        //{
        //    try
        //    {
        //        return Task.Run(() =>
        //                       SaveToLogByActivityType(NTUser, entries,
        //        ActivityType.Create)
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        string s = ex.Message;

        //    }
        //    return Task.Run(() =>
        //         SaveToLogByActivityType(NTUser, Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList(),
        //         ActivityType.Create)
        //         );
        //}

        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            int count = 0;
            try
            {
                count = await _entities.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                count = -1;
                Errors.Write(ex);
            }
            return count;
        }

        /// <summary>
        /// Find one instance By key/s of the table Async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(params object[] KeyValues)
        {
            T item = null;
            try
            {
                item = await DbSet.FindAsync(KeyValues);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }
        /// <summary>
        /// Find by conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        /// <summary>
        /// get all list of table
        /// </summary>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }

        public async virtual Task<IEnumerable<TResult>> GetSelectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select)
        {
            IEnumerable<TResult> items = null;
            try
            {
                items = await DbSet.Where(predicate).Select(select).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }

        /// <summary>
        /// get list of table according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> func)
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.Where(func).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        #endregion
    }

}
