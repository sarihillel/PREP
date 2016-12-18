using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        //Dictionary<int, string> getKeyValue();
        IEnumerable<Options> GetOrderTable();
    }

}
