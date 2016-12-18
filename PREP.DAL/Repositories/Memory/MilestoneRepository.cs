using PREP.DAL.Functions;
using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PREP.DAL.Functions.Extensions;
using System.Linq.Expressions;
using System.Data.Entity;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class MilestoneRepository : GenericRepository<PREPContext, Milestone>, IMilestoneRepository
    {
 

        /// <summary>
        /// select Query to MilestoneView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectMilestoneView(Expression<Func<Milestone, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TableMilestone in Query
                   select new
                   {
                       MilestoneID = TableMilestone.MilestoneID,
                       Name = TableMilestone.Name,
                       Order = TableMilestone.Order,
                       ToolTip = TableMilestone.ToolTip,
                       IsDeleted = TableMilestone.IsDeleted,
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListMilestone = null;
            CountRecords = 0;
            Expression<Func<Milestone, bool>> ExpressionFilter = null;
            try
            {


                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableMilestone => TableMilestone.ToolTip.Contains(Filtering) ||
                              TableMilestone.MilestoneID.ToString().Contains(Filtering) ||
                              TableMilestone.Order.ToString().Contains(Filtering) ||
                              TableMilestone.Name.Contains(Filtering));

                var query = SelectMilestoneView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListMilestone = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListMilestone;

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
