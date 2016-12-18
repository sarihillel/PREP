using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PREP.DAL.Models;
using System.Linq.Expressions;

namespace PREP.Models
{
    public class Releases
    {
       
        public Releases(Release r)
        {
            this.ReleaseName = r.Name;
            this.AccountName = r.Account.Name;
            this.ReleaseID = r.ReleaseID;
            this.PrepFPName = r.Account.PrepFPName;
            PREP.DAL.Models.Employee SPNameEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 : null;
            this.SPNameEmployee = SPNameEmployeeTmp != null ? SPNameEmployeeTmp.FirstName + " " + SPNameEmployeeTmp.LastName : null;
            PREP.DAL.Models.Employee ProgramMeEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 7) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1 : null;
            this.ProgramMeEmployee = ProgramMeEmployeeTmp != null ? ProgramMeEmployeeTmp.FirstName + " " + ProgramMeEmployeeTmp.LastName : null;
            this.ProductionStartDate = r.ReleaseMilestones.Any(m => m.MilestoneID == 12) ? String.Format("{0:d-MMM-yyyy}", r.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault().MilestoneDate) : null;
             this.PrepReviewMode = "Full";
            this.CheckListLnk = "@Html.ActionLink('Checklist >>' ,'' , new { id = "+ReleaseID.ToString()+" })";
            //  this.PrepFPName=r.ReleaseStakeholders.
        }
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
        public string CheckListLnk { get; set; }
    }
}