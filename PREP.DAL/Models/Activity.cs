using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{

    public enum ActivityType
    {
        Create = 1,
        Edit = 2
            
    }

    [Table("Activity")]
    public class Activity
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public ActivityType ActivityID { get; set; }
        public string Name { get; set; }
        #endregion

        #region relationships
        public virtual IEnumerable<ActivityLog> ActivityLogs { get; set; }
        #endregion
    }
}
