using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{




    #region RiskLevels enum
    /*Note: 
        on change/add Record or DisplayName Please Change in \PREP\scripts\checklist\checklist.js
         1. at enums.RiskLevelID (Values)
         2: at enums.RiskLevelIDNames (DisplayName)
     */
    public enum RiskLevels
    {
        [Display(Name = "Initiated")]
        Initiated = 0,//0
        [Display(Name = "NA")]
        NA = 1,//0
        [Display(Name = "On Hold")]
        OnHold = 2,//0
        [Display(Name = "None (Closed)")]
        NoneClosed = 3,//100
        [Display(Name = "None (As Planned)")]
        NoneAsPlanned = 4,//100
        [Display(Name = "Low")]
        Low = 5,//90
        [Display(Name = "Medium")]
        Med = 6,//50
        [Display(Name = "High")]
        High = 7,//0
        [Display(Name = "Show Stopper")]
        ShowStopper = 8//0
    }

    #endregion

    [Table("RiskLevel")]
    public class RiskLevel
    {
        #region Properties

        [Key, Column(Order = 0)]
        public RiskLevels RiskLevelID { get; set; }
        public int Value { get; set; }

        #endregion

        #region relationships

        public virtual ICollection<ReleaseChecklistAnswer> ReleaseChecklistAnswers { get; set; }

        #endregion
    }

}
