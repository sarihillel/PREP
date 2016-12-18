using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class PublicationRepository : GenericRepository<PREPContext, Publication>, IPublicationRepository
    {
        private IQueryable<Object> SelectPublicationView(Expression<Func<Publication, bool>> Expression, string Sorting)
        {
            var Query = from a in DbSet
                        select a;


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return from TablePublication in Query
                   select new
                   {
                       PublicationID = TablePublication.PublicationID,
                       Name = TablePublication.Name,
                       SentByID = TablePublication.SentByID,
                       SendByName = TablePublication.SendByName,
                       PublicationMail = TablePublication.PublicationMail,
                       PublicationMailDate = TablePublication.PublicationMailDate,
                       Date= TablePublication.Date,
                       IsDeleated= TablePublication.IsDeleated,
                       MDMCode = TablePublication.Employee.MDMCode
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListPublication = null;
            CountRecords = 0;
            Expression<Func<Publication, bool>> ExpressionFilter = null;
            try
            {

                if ((Filtering ?? "") != "")
                    ExpressionFilter = (TablePublication => TablePublication.SendByName.Contains(Filtering) ||
                              TablePublication.PublicationID.ToString().Contains(Filtering) ||
                              //TablePublication.ToString().Contains(Filtering) ||
                              TablePublication.PublicationMail.Contains(Filtering) ||
                              (TablePublication.IsDeleated == true ? "true" : "false").Contains(Filtering) ||
                              TablePublication.Name.Contains(Filtering));

                var query = SelectPublicationView(ExpressionFilter, Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListPublication = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListPublication;

        }


        
       

    }
}
