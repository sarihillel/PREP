using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseRepository : IGenericRepository<Release>
    {
        Release getReleaseForInitiateCheckList(int releaseId);
        IEnumerable<ReleaseView> GetReleaseJoinAccount();
        Task<int> EditReleaseChecklistAnswer(Release newRelease, WindowsPrincipal user);
        Release GetSingle(int? ReleaseID);
        Release GetReleseAndRelationships(int releaseId);
        Release GetReleaseCPData(int releaseId);
        Release GetReleseForStatus(int releaseId);
        Release GetReleaseCPDataAndMilstones(int releaseId);
        Release GetNewReleseAndRelationships();
        List<object> SetAllReleaseProducts(int ReleaseID);
        //   IEnumerable<Release> GetReleaseJoinAccount();
        IEnumerable<ReleseNameView> GetReleaseJoinAccountBySearch(String text);
        IEnumerable<object> GetReleaseStakeHolders(int ReleaseID);
        IEnumerable<object> GetReleaseStakeHoldersEmployee(int ReleaseID, int StakHolderID);
        object GetReleaseProductionStartDateMS(int ReleaseID);
        Task<int> AddRelease(Release NewRelease, WindowsPrincipal user);
        List<Options> GetReleaseOptions();
        IEnumerable<ReleaseView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        IEnumerable<string> GetEmployeesMailAddress(int ReleaseID, bool isDraft);
    }
}

