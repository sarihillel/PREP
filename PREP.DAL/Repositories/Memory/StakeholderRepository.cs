using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class StakeholderRepository : GenericRepository<PREPContext, Stakeholder>, IStakeholderRepository
    {
        private IQueryable<Object> SelectStakeholderView(Expression<Func<Stakeholder, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TableStakeholder in Query
                   select new
                   {
                       StakeholderID = TableStakeholder.StakeholderID,
                       Name = TableStakeholder.Name,
                       Order = TableStakeholder.Order,
                       ToolTip = TableStakeholder.ToolTip,
                       Type = TableStakeholder.Type,
                       IsDeleted = TableStakeholder.IsDeleted,
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListStakeholder = null;
            CountRecords = 0;
            Expression<Func<Stakeholder, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableStakeholder => TableStakeholder.ToolTip.Contains(Filtering) ||
                              TableStakeholder.StakeholderID.ToString().Contains(Filtering) ||
                              TableStakeholder.Order.ToString().Contains(Filtering) ||
                              (TableStakeholder.Type.ToString() == "1" ? "Add" : "Remove").Contains(Filtering)||
                              (TableStakeholder.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                              TableStakeholder.Name.Contains(Filtering));

                var query = SelectStakeholderView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListStakeholder = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListStakeholder;

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
