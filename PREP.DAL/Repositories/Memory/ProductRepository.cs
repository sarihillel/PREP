using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class ProductRepository : GenericRepository<PREPContext, Product>, IProductRepository
    {
        /// <summary>
        /// Get All Products with their related Releases 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> ReleaseProduct = DbSet.Include(p => p.ReleaseProducts).ToList();
            return ReleaseProduct;
        }

        /// <summary>
        /// select Query to ProductView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectProductView(Expression<Func<Product, bool>> Expression, string Sorting)
        {
           
            var Query = DbSet.Include(p => p.FamilyProduct);

            if (Expression != null)
                Query = Query.Where(Expression);
            if (Sorting != null)
                if (Sorting.Split()[0] == "FamilyProductID")
                {
                    Expression<Func<Product, Object>> ExpressionOrderBy = (TableProduct => TableProduct.FamilyProduct.Name);
                    Query = Query.GetOrderByQuery(ExpressionOrderBy, Sorting);

                }
                else Query = Query.GetOrderByQuery(Sorting);

            return Query.Select(p =>
                    new
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Order = p.Order,
                        FamilyProductID = p.FamilyProductID,
                        FamilyProductName = p.FamilyProduct.Name,
                        Type = p.Type,
                        ToolTip = p.ToolTip,
                        IsDeleted = p.IsDeleted
                    });
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListProduct = null;
            CountRecords = 0;
            Expression<Func<Product, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TableProduct => TableProduct.FamilyProduct.Name.Contains(Filtering) ||
                              TableProduct.ProductID.ToString().Contains(Filtering) ||
                              TableProduct.Name.Contains(Filtering) ||
                              TableProduct.ToolTip.Contains(Filtering) ||
                              TableProduct.Order.ToString().Contains(Filtering) ||
                              (TableProduct.Type == ParameterType.Add ? "Add" : "Remove").Contains(Filtering)||
                              (TableProduct.IsDeleted ? "true" : "false").Contains(Filtering));

                var query = SelectProductView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListProduct = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListProduct;

        }
      
        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Options> GetOrderTable(int FamilyProductID)
        {
            return DbSet.Where(c=>c.FamilyProductID== FamilyProductID).Select(prop => new OrderTableView() { Order = prop.Order }).GetAvailableOrder();
        }

    }
}
