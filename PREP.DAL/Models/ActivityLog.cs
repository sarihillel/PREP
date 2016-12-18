using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace PREP.DAL.Models
{
    [Table("ActivityLog")]
    public class ActivityLog
    {
        public ActivityLog(int NTUser, ActivityType activityType)
        {         
            this.EmployeeID = NTUser;
            this.ActivityID = activityType;
            this.Date = DateTime.Now;
        }
        public ActivityLog() { }
        #region Properties
        [Key]
        public int ActivityLogID { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeID { get; set; }
        public ActivityType ActivityID { get; set; }
       
        #endregion

        #region relationships
        
        public virtual ICollection<History> Histories { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("ActivityID")]
        public virtual Activity Activity { get; set; }

       
        #endregion
    }
}
