using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class SubAreaRepository : GenericRepository<PREPContext, SubArea>, ISubAreaRepository
    {
        public IEnumerable<SubArea> getSubAreaByAreaId(int areaId)
        {
            return DbSet.AsNoTracking().Where(a => a.AreaID == areaId).ToList();
        }
        private IQueryable<Object> SelectSubAreaView(Expression<Func<SubArea, bool>> Expression, string Sorting)
        {
            var Query = DbSet.Include(s => s.Area);

            if (Expression != null)
                Query = Query.Where(Expression);
            if (Sorting != null)
                if (Sorting.Split()[0] == "AreaID")
                {
                    Expression<Func<SubArea, Object>> ExpressionOrderBy = (TableSubArea => TableSubArea.Area.Name);
                    Query = Query.GetOrderByQuery(ExpressionOrderBy, Sorting);
                }
                else Query = Query.GetOrderByQuery(Sorting);

            return Query.Select(s =>
                    new
                    {
                       SubAreaID = s.SubAreaID,
                        Name = s.Name,
                        AreaID = s.AreaID,
                        Order=s.Order,
                        IsDeleted=s.IsDeleted,
                        AreaName=s.Area.Name
                    });
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListSubArea = null;
            CountRecords = 0;
            Expression<Func<SubArea, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableSubArea => TableSubArea.Area.Name.Contains(Filtering) || 
                                        TableSubArea.SubAreaID.ToString().Contains(Filtering) ||
                                        TableSubArea.Name.Contains(Filtering) ||
                                        (TableSubArea.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                                        TableSubArea.Order.ToString().Contains(Filtering)
                                        );

                var query = SelectSubAreaView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListSubArea = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListSubArea;

        }
        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Options> GetOrderTable(int AreaID)
        {
            return DbSet.Where(c => c.AreaID == AreaID).Select(prop => new OrderTableView() { Order = prop.Order }).GetAvailableOrder();
        }
    }
}
