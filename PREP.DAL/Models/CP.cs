using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("CP")]
    public class CP
    {
        #region Properties
        [Key]
        [DisplayName("CP ID:")]
        public int CPID { get; set; }
        [DisplayName("Name:")]
        public string Name { get; set; }
        [DisplayName("Order:")]
        public int Order { get; set; }
        [DisplayName("Effective Date:")]
        public DateTime? EffectiveDate { get; set; }
        [DisplayName("Expiration Date:")]
        public DateTime? ExpirationDate { get; set; }
        [DisplayName("Milestone ID:")]
        public int? MilestoneID { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        #region relationships
        [ForeignKey("MilestoneID")]
        public virtual Milestone Milestone { get; set; }
        public virtual ICollection<ReleaseCP> ReleaseCPs { get; set; }
        #endregion

    }
}
