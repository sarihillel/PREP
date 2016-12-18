using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseChecklistAnswerArcive")]
    public class ReleaseChecklistAnswerArcive
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseChecklistAnswerArciveID { get; set; }
        public int ReleaseID { get; set; }
        public int QuestionID { get; set; }
        public int AreaID { get; set; }
        public int SubAreaID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionInfo { get; set; }
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
    


    }
}