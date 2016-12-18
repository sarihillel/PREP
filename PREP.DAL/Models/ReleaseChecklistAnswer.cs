using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    /*Note: 
       on change/add Record or DisplayName Please Change in \PREP\scripts\checklist\checklist.js at enums.Responsibility (Values)
    */
    public enum Responsibility
    {
        Amdocs = 0,
        Customer = 1,
    }

    [Table("ReleaseChecklistAnswer")]
    public class ReleaseChecklistAnswer
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseChecklistAnswerID { get; set; }
        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionInfo { get; set; }
        public int AreaID { get; set; }
        public int SubAreaID { get; set; }
        public int? QuestionOwnerID { get; set; }
        public DateTime? HandlingStartDate { get; set; }
        [DefaultValue(0)]
        public int ActualComplation { get; set; }
        public RiskLevels RiskLevelID { get; set; }
        public string ExtrenalFocalPoint { get; set; }
        public Responsibility Responsibility { get; set; }
        public string Comments { get; set; }
        public DateTime? LastAutomaticUpdateDate { get; set; }
        public bool IsEdited { get; set; }
        public int? Log { get; set; } //int??
        public int? AsPlannedCounter { get; set; } //int??


        #endregion

        #region relationships
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }

        [ForeignKey("SubAreaID")]
        public virtual SubArea SubArea { get; set; }
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("RiskLevelID")]
        public virtual RiskLevel RiskLevel { get; set; }
        [ForeignKey("QuestionOwnerID")]
        public virtual Employee QuestionOwner { get; set; }
        #endregion
    }
}