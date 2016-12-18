using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace PREP.DAL.Repositories.Memory
{
    public class AccountRepository : GenericRepository<PREPContext, Account>, IAccountRepository
    {
        private IQueryable<Object> SelectAccountView(Expression<Func<Account, bool>> Expression, string Sorting)
        {
            using (EmployeeRepository db = new EmployeeRepository())
            {

                var Query = from a in DbSet
                                    .Include(p=>p.Employee)
                            select a;


                if (Expression != null)
                    Query = Query.Where(Expression);

                if (Sorting != null)
                    Query = Query.GetOrderByQuery(Sorting);
                return from TableAccount in Query
                       select new
                       {
                           AccountID = TableAccount.AccountID,
                           Name = TableAccount.Name,
                           PrepFPName = TableAccount.PrepFPName,
                           PrepFPEmpID = TableAccount.PrepFPEmpID,
                           IsDeleted = TableAccount.IsDeleted,
                           MDMCode= (TableAccount.Employee==null? null: (int?)TableAccount.Employee.MDMCode)
                       };
            }
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<Object> ListAccount = null;
            CountRecords = 0;
            Expression<Func<Account, bool>> ExpressionFilter = null;
            try
            {
                using (EmployeeRepository db = new EmployeeRepository())
                {
                    if ((Filtering ?? "") != "")
                        ExpressionFilter = (TableAccount =>
                                  TableAccount.AccountID.ToString().Contains(Filtering) ||
                                  TableAccount.PrepFPName.ToString().Contains(Filtering) ||
                                  TableAccount.Name.ToString().Contains(Filtering) ||
                                  (TableAccount.IsDeleted == true ? "true" : "false").Contains(Filtering) ||
                                  TableAccount.Employee.MDMCode.ToString().Contains(Filtering));
                                  //db.GetMDMCodeByEmpId(TableAccount.PrepFPEmpID).ToString().Contains(Filtering));

                    var query = SelectAccountView(ExpressionFilter, Sorting);
                   
                    string sqlstring = query.ToString();
                    CountRecords = query.Count();


                    query = Count > 0 ? query.Skip(StartIndex).Take(Count) : query;
                    ListAccount = query.ToList();
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListAccount;

        }
    }
}
