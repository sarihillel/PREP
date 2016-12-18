using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IHistoryRepository : IGenericRepository<History>
    {
        IEnumerable<History> GetHistoryReleaseCheckListId(int releaseCheckListId);
        History GetLastActivityLogUpdate(int? ReleaseID = null, ActivityType? activityType = null);
        List<HistoryView> GetList(int? ReleaseID, DateTime? StartDate, DateTime? EndDate, int TableId, int ModifiedById);
    }

}
