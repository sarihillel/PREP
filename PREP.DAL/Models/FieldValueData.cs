using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("FieldValueData")]
    public class FieldValueData
    {
        #region Properties
        [Key]
        public int FieldValueDataID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<Table> Tables { get; set; }
        #endregion
    }
}