using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Question")]
    public class Question
    {
        #region Properties

        [Key]
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionCode { get; set; }
        public string QuestionInfo { get; set; }
        public int SubAreaID { get; set; }
        public int MilestoneID { get; set; }
        public int? PreviousMilestoneID { get; set; }
        public double RatioBetweenMilestones { get; set; }
        public int? QuestionOwnerID { get; set; }
        public string UpdatesLog { get; set; }
        public int Order { get; set; }
       // public string PRRFPName { get; set; }
        //public string Status { get; set; }
        public bool? IsFocalPoint { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        #region relationships

        [ForeignKey("MilestoneID")]
        public virtual Milestone Milestone { get; set; }
        [ForeignKey("PreviousMilestoneID")]
        public virtual Milestone PreviousMilestone { get; set; }
        [ForeignKey("QuestionOwnerID")]
        public virtual Stakeholder Stakeholder { get; set; }
        [ForeignKey("SubAreaID")]
        public virtual SubArea SubArea { get; set; }
        public virtual ICollection<QuestionArea> QuestionAreas { get; set; }
        public virtual ICollection<QuestionCPRevMode> QuestionCPRevModes { get; set; }
        public virtual ICollection<QuestionCPRevModeQ> QuestionCPRevModeQs { get; set; }
        public virtual ICollection<QuestionProduct> QuestionProducts { get; set; }
        public virtual ICollection<QuestionCharacteristic> QuestionCharacteristics { get; set; }
        public virtual ICollection<QuestionStakeholder> QuestionStakeholders { get; set; }
        public virtual ICollection<QuestionFamilyProduct> QuestionFamilyProducts { get; set; }

        #endregion
    }
}
