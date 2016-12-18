using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("FamilyProduct")]
    public class FamilyProduct
    {
        #region Properties
        [Key]
        public int FamilyProductID { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<ReleaseFamilyProduct> ReleaseFamilyProducts { get; set; }
        public virtual ICollection<ReleaseVendor> ReleaseVendors { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<QuestionFamilyProduct> QuestionFamilyProducts { get; set; }
        #endregion
    }
}
