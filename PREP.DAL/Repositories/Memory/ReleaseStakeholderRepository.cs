using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseStakeholderRepository : GenericRepository<PREPContext, ReleaseStakeholder>, IReleaseStakeholderRepository
    {
        public List<ReleaseStakeholder> AddStakeHolder()
        {
            List<ReleaseStakeholder> ReleaseStakeholder = new List<ReleaseStakeholder> { };
            using (IStakeholderRepository dbStakeholder = new StakeholderRepository())
            {
                //get all family Product who's active
                IEnumerable<Stakeholder> Stakeholders = dbStakeholder.Get().Where(s=>s.IsDeleted==false); //m => m.IsVisible == true

                //in save needs get releaseid and add it
                Stakeholders.ToList().ForEach(sh => ReleaseStakeholder.Add(new ReleaseStakeholder() { StakeholderID = sh.StakeholderID, Stakeholder = sh }));
              
            }
            return ReleaseStakeholder;

        }
       

    }
}
