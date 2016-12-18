using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;

namespace PREP.DAL.Functions.Extensions
{
    public static class IQueryableExtensions
    {

        /// <summary>
        /// Gets string of order by (for example 'Name Asc') , find column in DBSet/query and does Lamda Expression order by to the data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="IncludeSortNotMapped">if true sorting  NotMappedAttribute Propertues also </param>
        /// <returns></returns>
        public static IQueryable<T> GetOrderByQuery<T>(this IQueryable<T> source, string orderBy, bool IncludeSortNotMapped = false)
        {
            string methodName = "OrderBy";
            string[] orderByArray = orderBy.Split();
            orderBy = orderByArray[0];
            if (orderByArray.Count() > 1)
                if (orderByArray[1].ToLower() == "desc")
                    methodName = "OrderByDescending";
            var sourceType = typeof(T);
            var property = sourceType.GetProperty(orderBy);
            if (property == null || !IncludeSortNotMapped && property.GetCustomAttributes(typeof(NotMappedAttribute), false).Count() > 0)
                property = sourceType.GetProperties().FirstOrDefault();
            var attrcolumn = property.GetCustomAttributes(typeof(ColumnAttribute), false).Cast<ColumnAttribute>().FirstOrDefault();
            property = attrcolumn == null ? property : sourceType.GetProperty(attrcolumn.Name);
            var parameterExpression = Expression.Parameter(sourceType, "x");
            var getPropertyExpression = Expression.MakeMemberAccess(parameterExpression, property);
            var orderByExpression = Expression.Lambda(getPropertyExpression, parameterExpression);
            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                                                   new[] { sourceType, property.PropertyType }, source.Expression,
                                                   orderByExpression);

            return source.Provider.CreateQuery<T>(resultExpression);
        }

        public static IQueryable<T> GetOrderByQuery<T>(this IQueryable<T> source, Expression<Func<T, Object>> ExpressionOrder, string orderBy)
        {
            string[] orderByArray = orderBy.Split();
            orderBy = orderByArray[0];
            if (orderByArray.Count() > 1 && orderByArray[1].ToLower() == "desc")
               return  source.OrderByDescending(ExpressionOrder);
            else
                return source.OrderBy(ExpressionOrder);
        }

        public static IQueryable<T> GetOrderByQuery<T>(this IQueryable<T> source, Expression<Func<T, Object>> ExpressionOrder, bool isDesc)
        {
           if(isDesc == true)
                return source.OrderByDescending(ExpressionOrder);
            else
                return source.OrderBy(ExpressionOrder);
        }

        /// <summary>
        /// return list by StartIndex and Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="StartIndex"></param>
        /// <param name="Count"></param>
        /// <param name="CountRecords"></param>
        /// <returns></returns>
        public static IQueryable<T> Get<T>(this IQueryable<T> source, int StartIndex, int Count, out int CountRecords)
        {
            CountRecords = source.Count();
            return Count > 0 && CountRecords>0 
                       ? source.Skip(StartIndex).Take(Count) //Paging
                       : source; //No paging

        }

        /// <summary>
        /// return  list of the next CountRecords available order  that is not exsits in  table
        /// </summary>
        /// <param name="orderquery">get iquerarable of column of order in table. for examlpe: DbSet.Select(q => new OrderTableView() { Order = q.QuestionOrder });</param>
        /// <param name="CountRecords">CountRecords to return , Default is 10 records</param>
        /// <returns></returns>
        public static IEnumerable<Options> GetAvailableOrder(this IQueryable<OrderTableView> orderquery, int CountRecords = 10)
        {
            List<Options> ListAvailable = new List<Options>();
            //last Min available order
            int LastMinOrder = 1;
            try
            {
                for (int i = 0; i < CountRecords; i++)
                {
                    //find if exsits the next number
                    int Order = orderquery.Where(q => q.Order == LastMinOrder).Select(e => e.Order).FirstOrDefault();
                    //if exsits find the next available min order in table
                    if (Order != 0)
                    {
                        var query = (from q in orderquery
                                     join q1 in orderquery on q.Order + 1 equals q1.Order into qqq
                                     from questionleft in qqq.DefaultIfEmpty()
                                     where q.Order >= LastMinOrder && questionleft == null
                                     select q.Order + 1);
                        Order = query.Min();
                        LastMinOrder = Order;
                    }
                    ListAvailable.Add(new Options() { Value = LastMinOrder, DisplayText = LastMinOrder.ToString() });
                    //add record to the next order
                    LastMinOrder++;
                }

            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListAvailable;
        }


    }
}
