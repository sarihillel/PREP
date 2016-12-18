using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Vendor")]
    public class Vendor
    {
        #region Properties
        [Key]
        public int VendorID { get; set; }
        public string Name { get; set; }
        #endregion
       
        #region relationships
        public virtual ICollection<ReleaseVendor> ReleaseVendors { get; set; }
        public virtual ICollection<VendorAreas> VendorAreass { get; set; }
        #endregion
    }
}
