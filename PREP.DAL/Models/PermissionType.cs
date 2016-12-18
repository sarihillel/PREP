using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("PermissionType")]
    public class PermissionType
    {
        #region Properties
        [Key]
        public int PermissionTypeID { get; set; }
        public string Name { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<ReleasePermissionType> ReleaseFamilyProducts { get; set; }
        #endregion
    }
}