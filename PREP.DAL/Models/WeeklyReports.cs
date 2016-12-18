using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("WeeklyReports")]
    public class WeeklyReports
    {
        #region Properties
        [Key]
        public int WeeklyReportsID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<ReleaseWeeklyReports> ReleaseFamilyProducts { get; set; }
        #endregion
    }
}