using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("QuestionStakeholder")]
    public class QuestionStakeholder
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionStakeholderID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int StakeholderID { get; set; }
        public string Comments { get; set; }
        public AdminValue AdminValue { get; set; }
        #endregion

        #region relationships
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("StakeholderID")]
        public virtual Stakeholder Stakeholder { get; set; }
        #endregion      
    }
}