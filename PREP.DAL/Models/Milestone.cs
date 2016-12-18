using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Milestone")]
    public class Milestone
    {
        #region Properties
        [Key]
        public int MilestoneID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<ReleaseMilestone> ReleaseMilestones { get; set; }
        public virtual ICollection<CP> CPs { get; set; }
        [InverseProperty("Milestone")]
        public virtual ICollection<Question> QuestionMilestones { get; set; }
        [InverseProperty("PreviousMilestone")]
        public virtual ICollection<Question> QuestionPreviousMilestones { get; set; }
        #endregion
    }
}
