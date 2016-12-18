using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.TableViews
{
    public class ReleaseView
    {
        #region
        //public ReleaseView(Release r)
        //{
        //    this.ReleaseName = r.Name;
        //    this.AccountName = r.Account != null ? r.Account.Name : String.Empty;
        //    this.ReleaseID = r.ReleaseID;
        //    this.PrepFPName = r.Account != null ? r.Account.PrepFPName : String.Empty;
        //    PREP.DAL.Models.Employee SPNameEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 : null;
        //    this.SPNameEmployee = SPNameEmployeeTmp != null ? SPNameEmployeeTmp.FirstName + " " + SPNameEmployeeTmp.LastName : null;
        //    PREP.DAL.Models.Employee ProgramMeEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 7) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1 : null;
        //    this.ProgramMeEmployee = ProgramMeEmployeeTmp != null ? ProgramMeEmployeeTmp.FirstName + " " + ProgramMeEmployeeTmp.LastName : null;
        //    this.ProductionStartDate = r.ReleaseMilestones.Any(m => m.MilestoneID == 12) ? String.Format("{0:d-MMM-yyyy}", r.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault().MilestoneDate.ToString()) : null;
        //    this.PrepReviewMode = "Full";
        //   // this.PrepFPName = r.ReleaseStakeholders.
        //}
        #endregion
        [Display(Name = "Release ID")]
        public int ReleaseID { get; set; }
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "PREP FP Name")]
        public string PrepFPName { get; set; }
        [Display(Name = "Release Name")]
        public string ReleaseName { get; set; }
        public string SPNameEmployee { get; set; }
        [Display(Name = "PREP Review Mode")]
        public string PrepReviewMode { get; set; }
        public string ProgramMeEmployee { get; set; }
        [Display(Name = "Production Start Date")]

        public string ProductionStartDate { get; set; }
        public bool IsExistCheckList
        {
            get
            {
                using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                {
                    return db.IsExistCheckList(ReleaseID);
                }
            }
        }
    }
}
