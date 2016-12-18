using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseVendor")]
    public class ReleaseVendor
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseVendorID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int VendorID { get; set; }

        public string Name { get; set; }
        public int? FamilyProductID { get; set; }
        public bool IsFullTrack { get; set; } // that Column Name??? 
        #endregion   

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }

        [ForeignKey("FamilyProductID")]
        public virtual FamilyProduct FamilyProduct { get; set; }
       // public virtual ICollection<VendorAreas> VendorAreas { get; set; }

        #endregion
    }
}
