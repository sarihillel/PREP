using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class FamilyProductRepository : GenericRepository<PREPContext, FamilyProduct>, IFamilyProductRepository
    {
        
        /// <summary>
        /// Get all Family Products with their related Products
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FamilyProduct> GetAllFamilyProducts()
        {
            IEnumerable<FamilyProduct> FamilyProductRelease = DbSet.Where(f => f.IsDeleted == false).OrderBy(b => b.Order)
                .Include(fp => fp.ReleaseFamilyProducts)
                .Include(fp => fp.Products.Select(p => p.ReleaseProducts)).ToList();
            // FamilyProductRelease.ToList().ForEach(p => p.Products.Where(e=> e.IsVisible == true).OrderBy(e => e.Order).ToList());
            foreach (var item in FamilyProductRelease)
            {
               item.Products = item.Products.Where(p => p.IsDeleted == false).ToList();
                item.Products = item.Products.OrderBy(p => p.Order).ToList();
            }


            return FamilyProductRelease;
        }

        /// <summary>
        /// select Query to FamilyProductView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectFamilyProductView(Expression<Func<FamilyProduct, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;
                          

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TableFamilyProduct in Query
                   select new
                   {
                       FamilyProductID = TableFamilyProduct.FamilyProductID,
                       Name = TableFamilyProduct.Name,
                       Order = TableFamilyProduct.Order,
                       ToolTip = TableFamilyProduct.ToolTip,
                       Type = TableFamilyProduct.Type,
                       IsDeleted = TableFamilyProduct.IsDeleted,
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListFamilyProduct = null;
            CountRecords = 0;
            Expression<Func<FamilyProduct, bool>> ExpressionFilter = null;
            try
            {
               
                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableFamilyProduct => TableFamilyProduct.ToolTip.Contains(Filtering) ||
                              TableFamilyProduct.FamilyProductID.ToString().Contains(Filtering) ||
                              TableFamilyProduct.Order.ToString().Contains(Filtering) ||
                              (TableFamilyProduct.Type== ParameterType.Add ? "Add" : "Remove").Contains(Filtering)||
                              (TableFamilyProduct.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                              TableFamilyProduct.Name.Contains(Filtering));

                var query = SelectFamilyProductView(ExpressionFilter, Sorting);
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
