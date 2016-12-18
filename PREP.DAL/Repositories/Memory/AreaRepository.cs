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
    public class AreaRepository : GenericRepository<PREPContext, Area>, IAreaRepository
    {
        private IQueryable<Object> SelectAreaView(Expression<Func<Area, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TableArea in Query
                   select new
                   {
                      
                       AreaID = TableArea.AreaID,

                       Name = TableArea.Name,
                       Order = TableArea.Order,
                       ToolTip = TableArea.ToolTip,
                       Type = TableArea.Type,
                       IsDeleted = TableArea.IsDeleted,
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListArea = null;
            CountRecords = 0;
            Expression<Func<Area, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableArea => TableArea.ToolTip.Contains(Filtering) ||
                              TableArea.AreaID.ToString().Contains(Filtering) ||
                              TableArea.Order.ToString().Contains(Filtering) ||
                              (TableArea.Type.ToString()=="1"? "Add" : "Remove").Contains(Filtering)||
                              (TableArea.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                              TableArea.Name.Contains(Filtering));

                var query = SelectAreaView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                CountRecords = query.Count();//
                query = Count > 0 ? query.Skip(StartIndex).Take(Count) : query;//
                ListArea = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListArea;

        }
        public IEnumerable<Options> GetOrderTable()
        {
            return DbSet.Select(prop => new OrderTableView() { Order = (int)prop.Order }).GetAvailableOrder();
        }
        //public Dictionary<int , string> getKeyValue()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    DbSet.ToList().ForEach(a => dic.Add(a.AreaID, a.Name));
        //    return dic;
        //}
    }
}
