using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IMilestoneRepository: IGenericRepository<Milestone>
    {
        IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);

        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        IEnumerable<Options> GetOrderTable();
    }
   
}
