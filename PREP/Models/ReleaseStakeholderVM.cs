using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PREP.DAL.Models;

namespace PREP.Models
{
    public class ReleaseStakeholderVM
    {  
        public int ReleaseStakeholderID { get; set; }
        public int ReleaseID { get; set; }
        public int StakeholderID { get; set; }
        //public string Employee1FirstName { get; set; }
        //public string Employee1LastName { get; set; }
        //public string Employee2FirstName { get; set; }
        //public string Employee2LastName { get; set; }
        public string FullNameEmployee1 { get; set; }
        public string FullNameEmployee2 { get; set; }
        public string StakeholderName { get; set; }
        public int? EmployeeID1 { get; set; }
        public int? EmployeeID2 { get; set; }
        public bool IsAddUser { get; set; }
        public bool IsDeleteUser { get; set; }
        public string ToolTip { get; set; }
        //public virtual Stakeholder Stakeholder { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Employee Employee2 { get; set; }

    }
}