using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
    }
}
