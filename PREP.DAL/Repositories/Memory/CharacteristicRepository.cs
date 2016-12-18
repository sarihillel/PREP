using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class CharacteristicRepository : GenericRepository<PREPContext, Characteristic>, ICharacteristicRepository
    {

        /// <summary>
        /// Get all Family Products with their related Products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Characteristic> GetAllFamilyProducts()
        {
            IEnumerable<Characteristic> CharacteristicRelease = DbSet.Where(f => f.IsDeleted == false).OrderBy(b => b.Order).ToList();
            // FamilyProductRelease.ToList().ForEach(p => p.Products.Where(e=> e.IsVisible == true).OrderBy(e => e.Order).ToList());
            return CharacteristicRelease;
        }

        /// <summary>
        /// select Query to FamilyProductView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectCharacteristicView(Expression<Func<Characteristic, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TableCharacteristic in Query
                   select new
                   {
                       CharacteristicID = TableCharacteristic.CharacteristicID,
                       Name = TableCharacteristic.Name,
                       Order = TableCharacteristic.Order,
                       ToolTip = TableCharacteristic.ToolTip,
                       Type = TableCharacteristic.Type,
                       IsDeleted = TableCharacteristic.IsDeleted,
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListFamilyProduct = null;
            CountRecords = 0;
            Expression<Func<Characteristic, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableCharacteristic => TableCharacteristic.ToolTip.Contains(Filtering) ||
                              TableCharacteristic.CharacteristicID.ToString().Contains(Filtering) ||
                              TableCharacteristic.Order.ToString().Contains(Filtering) ||
                              (TableCharacteristic.Type == ParameterType.Add ? "Add" : "Remove").Contains(Filtering) ||
                              (TableCharacteristic.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                              TableCharacteristic.Name.Contains(Filtering));

                var query = SelectCharacteristicView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListFamilyProduct = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListFamilyProduct;

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
