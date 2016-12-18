using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    public class CalculatedField
    {
        #region Properties
        [Key]
        public int CalculatedFieldID { get; set; }
        public string Name { get; set; }
        public int? TableID { get; set; }
        public string Formula { get; set; }
        public string ExcelFormula { get; set; }
        public string ExcelColumnName { get; set; }
        public int? Type { get; set; } //??
        public bool IsActive { get; set; }
        #endregion

        #region relationships
        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }
        #endregion
    }
}
