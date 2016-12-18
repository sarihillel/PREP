using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface ISubAreaRepository : IGenericRepository<SubArea>
    {
        IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
  
        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        IEnumerable<Options> GetOrderTable(int AreaID);
        IEnumerable<SubArea> getSubAreaByAreaId(int areaId);
    }

}
