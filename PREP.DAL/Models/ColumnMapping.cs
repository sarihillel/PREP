using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ColumnMapping")]
    public class ColumnMapping
    {
        #region Properties
        [Key]
        public int ColumnMappingID { get; set; }
        public int TableID { get; set; }
        public string ColumnName { get; set; }
        public string ColumnExcelName { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region relationships
        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }
        #endregion
    }
}
