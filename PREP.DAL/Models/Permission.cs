using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Permission")]
    public class Permission
    {
        #region Properties
        [Key]
        public int PermissionID { get; set; }
        public string PType { get; set; }
        public string DLName { get; set; }
        public bool IsActive { get; set; }

        public int TableID { get; set; }
        #endregion

        #region relationships
        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }
        #endregion
    }
}
