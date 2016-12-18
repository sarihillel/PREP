using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseMilestone")]
    public class ReleaseMilestone
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseMilestoneID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int? MilestoneID { get; set; }

        public DateTime? MilestoneDate { get; set; }
        #endregion   

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("MilestoneID")]
        public virtual Milestone Milestone { get; set; }
        #endregion   
    }
}