using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseAreaOwnerRepository : GenericRepository<PREPContext, ReleaseAreaOwner>, IReleaseAreaOwnerRepository
    {
        public List<ReleaseAreaOwner> AddAreaOwner()
        {
            List<ReleaseAreaOwner> ReleaseAreaOwner = new List<ReleaseAreaOwner> { };
            using (IAreaRepository dbAreaOwner = new AreaRepository())
            {
                //get all family Product who's active
                IEnumerable<Area> AreaOwners = dbAreaOwner.Get().Where(a=>a.IsDeleted==false);  //m => m.IsVisible == true

                //in save needs get releaseid and add it
                AreaOwners.ToList().ForEach(ao => ReleaseAreaOwner.Add(new ReleaseAreaOwner() { AreaID = ao.AreaID, Area = ao }));

            }
            return ReleaseAreaOwner;

        }
    }
}
