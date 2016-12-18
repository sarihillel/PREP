using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Stakeholder")]
    public class Stakeholder
    {
        #region Properties
        [Key]
        public int StakeholderID { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<ReleaseStakeholder> ReleaseStakeholders { get; set; }
        public virtual ICollection<QuestionStakeholder> QuestionStakeHolders { get; set; }
        #endregion
    }
}
