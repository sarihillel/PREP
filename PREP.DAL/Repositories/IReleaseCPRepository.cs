using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseCPRepository : IGenericRepository<ReleaseCP>
    {
        ReleaseCP getReleseCP(int ReleseCPID);
        ReleaseCPView GetUpdatedField(int ReleaseID, int CPID);
        ReleaseCPView GetReleaseCPViewById(int ReleseCPID);
        ReleaseCP getReleaseCPByCPAndRelease(int releaseId, int cpId);
        /// <summary>
        /// Return Field ReleaseCPView By Keys
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="CPID"></param>
        /// <returns></returns>
        ReleaseCPView GetField(int ReleaseID, int CPID);
        /// <summary>
        /// Return Field ReleaseCPView By ReleseCPID
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="CPID"></param>
        /// <returns></returns>
        ReleaseCPView GetField(int ReleseCPID);
        /// <summary>
        /// Reururn List of ReleaseCPView By Params
        /// </summary>
        /// <param name="StartIndex">Index Row To Return From List</param>
        /// <param name="Count">Top Records To show</param>
        /// <param name="Sorting">Column To Sort, String Name Column and 'Asc' or 'Desc', For Example: "AccountName desc"</param>
        /// <param name="Filtering">String To Filter of List</param>
        /// <param name="CountRecords">Return Count Recrode who's Found</param>
        /// <returns></returns>
        IEnumerable<ReleaseCPView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        Task<int> Edit(ReleaseCPView ReleaseCPView, WindowsPrincipal User);
        //int Edit(ReleaseCPView ReleaseCPView);
        Task<int> Add(ReleaseCPView ReleaseCPView, WindowsPrincipal User);
        ReleaseCP GetReleaseCPId(int ReleseCPID);
        IEnumerable<ReleaseCPView> GetReleaseCPByFiltering(Expression<Func<ReleaseCP, bool>> Expression);

         Task updatePublicationDetails(int ReleaseID, int CPID, string FileName, Employee  emp);
    }
}
