using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseMilestoneRepository : IGenericRepository<ReleaseMilestone>
    {
        List<ReleaseMilestone> AddReleaseMilestone();

        /// <summary>
        /// return MilstoneDate who's  IsVisible By Milestone Name
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="MilestoneName"></param>
        /// <returns></returns>
        DateTime? GetMilestoneDate(int ReleaseID, string MilestoneName);
      
    }
}
