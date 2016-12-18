using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("CPReviewMode")]
    public class CPReviewMode
    {
        #region Properties
        [Key]
        public int CPReviewModeID { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public int? Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<ReleaseCPReviewMode> ReleaseCPReviewModes { get; set; }
        public virtual ICollection<QuestionCPRevMode> QuestionCPRevModes { get; set; }
        #endregion

    }
}
