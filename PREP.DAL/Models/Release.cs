using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Release")]
    public class Release
    {
        #region Properties
        [Key]
        public int ReleaseID { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }
        public int AccountID { get; set; }
        [Required]
        public double Size { get; set; }
        public string LOB { get; set; }
        //public string Division { get; set; }
        //public int? HRMSID { get; set; }
        public string Drop { get; set; }
        public string ProjectType { get; set; }
        public bool? StatusConnectInd { get; set; }
        public string HRMSReleaseStatus { get; set; }
        public ReleaseStatus Status { get; set; }
        public int? Probability { get; set; }
        public decimal PlannedDCUTMM { get; set; } //??
        
        public string Organization { get; set; }
        public string CurrentStage { get; set; }
        public DateTime? CurrentStageDate { get; set; }
        public DateTime? ScopingOriginalStartDate { get; set; } //??
        public DateTime? ATOriginalStartDate { get; set; }//??
        public DateTime? ProductionOriginalStartDate { get; set; } //??
        public string KickoffComments { get; set; }
        public DateTime? PRKickoffPlannedDate { get; set; }
        public string DismissIndicator { get; set; }
        public string DismissReason { get; set; }
        public string DismissApprovedBy { get; set; }
        public DateTime? DismissApprovalDate { get; set; }
        public bool NewSystemIndicator { get; set; }
        public string Owner { get; set; }
        public string AdditionalProducts { get; set; }
        public bool IsDeleted { get; set; }
        public string SDRMode { get; set; }
        public string DelayReason { get; set; }
        public string DelayReasonHistory { get; set; }
      
        
        #endregion

        #region relationships
        [ForeignKey("AccountID")]
        public virtual Account Account { get; set; }

        public virtual ICollection<ReleaseCP> ReleaseCPs { get; set; }
        public virtual ICollection<ReleaseMilestone> ReleaseMilestones { get; set; }
        public virtual ICollection<ReleaseCPReviewModeQ> ReleaseCPReviewModeQs { get; set; }
        public virtual ICollection<ReleaseCPReviewMode> ReleaseCPReviewModes { get; set; }
        public virtual ICollection<ReleaseAreaOwner> ReleaseAreaOwners { get; set; }
        public virtual ICollection<ReleaseFamilyProduct> ReleaseFamilyProducts { get; set; }
        public virtual ICollection<ReleaseStakeholder> ReleaseStakeholders { get; set; }
        public virtual ICollection<ReleaseChecklistAnswer> ReleaseChecklistAnswers { get; set; }
        public virtual ICollection<StatusText> StatusTexts { get; set; }
        public virtual ICollection<StatusAreaText> StatusAreaTexts { get; set; }
        public virtual ICollection<ReleaseProduct> ReleaseProducts { get; set; }
        public virtual ICollection<ReleaseVendor> ReleaseVendors { get; set; }
        public virtual ICollection<ReleaseCharacteristic> ReleaseCharacteristics { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<AreaScore> AreaScores { get; set; }
        #endregion 
    }
    #region enums
    public enum ReleaseStatus
    {
        Active = 0,
        Potential = 1,
        OnHold = 2,
        Cancelled = 3,
        Closed = 4
    }
    //public enum HRMSReleaseStatus
    //{
    //    Development = 0,
    //    Potential = 1,
    //    Production = 2,
    //    History = 3,
    //    Canceled = 4,
    //    OnHold = 5,
    //    Future = 6
    //}
    #endregion
}
