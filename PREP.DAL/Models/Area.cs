using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Area")]
    public class Area
    {
        #region Properties
        [Key]
        public int AreaID { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }
        public int? Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<QuestionArea> QuestionAreas { get; set; }
        public virtual ICollection<SubArea> SubAreas { get; set; }
        public virtual ICollection<AreaScore> AreaSources { get; set; }
        public virtual ICollection<StatusAreaText> StatusAreaTexts { get; set; }
        public virtual ICollection<ReleaseAreaOwner> ReleaseAreaOwners { get; set; }
        public virtual ICollection<VendorAreas> VendorAreass { get; set; }
        #endregion
    }
}
