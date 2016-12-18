using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class ReleaseMilstoneCP_Footer
    {
        public ReleaseMilstoneCP_Footer()
        {

        }
        public string MilestoneDate { get; set; }
        public string MilestoneName { get; set; }
        public string CheckPointDate { get; set; }
        public int? CheckPointId { get; set; }
        public string CPName { get; set; }
        public bool IsCreated { get; set; }

        private string FormatDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return String.Empty;
            }
            else
            {
                DateTime tepmDate2 = dateTime ?? DateTime.Now;
                return String.Format("{0:d-MMM-yyyy}", tepmDate2);
                //return tepmDate2.ToString("dd-mmm-yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
                // return tepmDate2.ToString("dd-mmm");
            }
        }
        public ReleaseMilstoneCP_Footer(ReleaseMilestone rm, List<int> cpids, ICollection<ReleaseCP> releaseCps, string accountName)
        {
            this.MilestoneName = rm.Milestone.Name;
            this.MilestoneDate = FormatDateTime(rm.MilestoneDate);
            ReleaseCPView releaseCP = null;
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                releaseCP = new ReleaseCPView((!rm.Milestone.CPs.Any(cp => cp.MilestoneID == rm.MilestoneID && cpids.Contains(cp.CPID)))? null :
                    releaseCps.Where(r => r.ReleaseID == rm.ReleaseID && 
                r.CPID == rm.Milestone.CPs.Where(cp => cp.MilestoneID == rm.MilestoneID && cpids.Contains(cp.CPID)).FirstOrDefault().CPID).FirstOrDefault(), accountName);
            }
            IsCreated = releaseCP != null;
            this.CheckPointDate = this.IsCreated ? FormatDateTime(releaseCP.PlannedDate) : String.Empty;
            this.CheckPointId = this.IsCreated ? releaseCP.ReleaseCPID : -1;
            this.CPName = this.IsCreated ? releaseCP.CPName : String.Empty;
        }
    }
}