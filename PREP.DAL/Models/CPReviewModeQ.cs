using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("CPReviewModeQ")]
    public class CPReviewModeQ
    {
        #region Properties
        [Key]
        public int CPReviewModeQID { get; set; }
        public string Name { get; set; }
       // public bool IsDeleted { get; set; }


        #endregion

        #region relationships
        public virtual ICollection<ReleaseCPReviewModeQ> ReleaseCPReviewModeQs { get; set; }
        public virtual ICollection<QuestionCPRevModeQ> QuestionCPRevModeQs { get; set; }
        #endregion

    }
}
