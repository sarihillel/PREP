using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class CPRepository : GenericRepository<PREPContext, CP>, ICPRepository
    {
        /// <summary>
        /// select Query to CPView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectCPView(Expression<Func<CP, bool>> Expression,string Sorting)
        {
            var Query = DbSet
                          .Include(p => p.Milestone);

            if (Expression != null)
                Query = Query.Where(Expression);
            if (Sorting != null)
                if (Sorting.Split()[0]== "MilestoneID")
        {
                    Expression<Func<CP, Object>> ExpressionOrderBy = (TableCP => TableCP.Milestone.Name);
                    Query = Query.GetOrderByQuery(ExpressionOrderBy, Sorting);

                }
                else Query= Query.GetOrderByQuery(Sorting);

            return from TableCp in Query
                             select new
                             {
                                 CPID = TableCp.CPID,
                                 Name = TableCp.Name,
                                 Order = TableCp.Order,
                                 MilestoneID = TableCp.MilestoneID,
                                 MilestoneName =TableCp.Milestone.Name,
                                 EffectiveDate = TableCp.EffectiveDate,
                                 ExpirationDate = TableCp.ExpirationDate,
                                 IsDeleted= TableCp.IsDeleted,
                             };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListCP = null;
            CountRecords = 0;
            Expression<Func<CP, bool>> ExpressionFilter = null;
            try
            {


                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableCp => TableCp.Milestone.Name.Contains(Filtering) ||
                              TableCp.CPID.ToString().Contains(Filtering) ||
                              ((SqlFunctions.DateName("mm", TableCp.ExpirationDate).Substring(0, 3).ToString() + " " + SqlFunctions.DateName("dd", TableCp.ExpirationDate) + " ," + SqlFunctions.DateName("yyyy", TableCp.ExpirationDate)).ToString()).Contains(Filtering) ||
                               ((SqlFunctions.DateName("mm", TableCp.EffectiveDate).Substring(0, 3).ToString() + " " + SqlFunctions.DateName("dd", TableCp.EffectiveDate) + " ," + SqlFunctions.DateName("yyyy", TableCp.EffectiveDate)).ToString()).Contains(Filtering) ||
                              TableCp.Order.ToString().Contains(Filtering) ||
                              (TableCp.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                              TableCp.Name.Contains(Filtering));

                var query = SelectCPView(ExpressionFilter,Sorting);
                string sqlstring = query.ToString();
                query= query.Get(StartIndex, Count, out CountRecords);
                ListCP = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListCP;

        }

        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Options> GetOrderTable()
        {
            return DbSet.Select(prop => new OrderTableView() { Order = prop.Order }).GetAvailableOrder();
        }
    }
}
