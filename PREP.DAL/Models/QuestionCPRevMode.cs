using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("QuestionCPRevMode")]
    public class QuestionCPRevMode
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionCPRevModeID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int? CPReviewModeID { get; set; }
        public string Comments { get; set; }
        public AdminValue AdminValue { get; set; }

        #endregion

        #region relationships

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("CPReviewModeID")]
        public virtual CPReviewMode CPReviewMode { get; set; }

        #endregion
    }
}