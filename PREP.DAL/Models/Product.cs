using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Product")]
    public class Product
    {
        #region Properties
        [Key]
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int? FamilyProductID { get; set; }
        public ParameterType Type { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        [ForeignKey("FamilyProductID")]
        public virtual FamilyProduct FamilyProduct { get; set; }
        public virtual ICollection<ReleaseProduct> ReleaseProducts { get; set; }
        public virtual ICollection<QuestionProduct> QuestionProducts { get; set; }
        #endregion
    }
}
