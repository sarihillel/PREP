using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
     //   IEnumerable<ActivityLog> GetActivityLogByReleaseCheckListId(int releaseCheckListId);
      //  List<HistoryView> GetList(int? ReleaseID, DateTime? StartDate, DateTime? EndDate, int TableId, int EmployeeId);

        /// <summary>
        /// return Max Updated/Created record of tActivityLog
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
       // ActivityLog GetLastActivityLogUpdate(int? ReleaseID = null, ActivityType? activityType = null);
    }

}
