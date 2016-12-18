using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseMilestoneRepository : GenericRepository<PREPContext, ReleaseMilestone>, IReleaseMilestoneRepository
    {
        public List<ReleaseMilestone> AddReleaseMilestone()
        {
            List<ReleaseMilestone> ReleaseMilestones = new List<ReleaseMilestone> { };
            using (IMilestoneRepository dbMilestone = new MilestoneRepository())
            {
                //get all milestone who's active
                IEnumerable<Milestone> Milestones = dbMilestone.Get().Where(m=>m.IsDeleted == false); //m => m.IsVisible == true

                //in save needs get releaseid and add it
                //Milestones.ToList().ForEach(m=>Context.ReleaseMilestones.Attach(new ReleaseMilestone() { ReleaseMilestoneID = m.MilestoneID, Milestone = m }));
                Milestones.ToList().ForEach(m => ReleaseMilestones.Add(new ReleaseMilestone() {  MilestoneID = m.MilestoneID, Milestone =m }));
               
            }
            return ReleaseMilestones;

        }

        /// <summary>
        /// return MilstoneDate who's  IsVisible By Milestone Name
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="MilestoneName"></param>
        /// <returns></returns>
        public DateTime? GetMilestoneDate(int ReleaseID, string MilestoneName)
        {
            return DbSet.Include(m => m.Milestone)
               .Where(m => m.Milestone.IsDeleted == false
                       && m.ReleaseID == ReleaseID
                       && m.Milestone.Name.ToLower().Trim() == MilestoneName.ToLower().Trim()
                       && m.MilestoneDate != null)
               .Select(m => m.MilestoneDate)
               .FirstOrDefault();
        }
    }
}
