using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseChecklistAnswerRepository : IGenericRepository<ReleaseChecklistAnswer>
    {

        IQueryable<ReleaseChecklistAnswerView> GetByFiltering(int ReleaseID,  string Sorting = null, string Filtering = null);
        IEnumerable<AreaScore> GetStatus(int ReleaseId);
       bool IsExistCheckList(int? releaseId);
         Task<int> EditReleaseChecklist(List<ReleaseChecklistAnswerView> Checklist, WindowsPrincipal user);

    }
}
