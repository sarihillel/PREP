using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("VendorAreas")]
    public class VendorAreas
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int VendorAreasID { get; set; }

        [Key, Column(Order = 1)]
        public int VendorID { get; set; }
        [Key, Column(Order = 2)]
        public int AreaID { get; set; }
        public bool IsChecked { get; set; }
        #endregion

        #region relationships
       // [ForeignKey("VendorID")]
       // public virtual ReleaseVendor Vendor { get; set; }
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }
        #endregion
    }
}