using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseFamilyProduct")]
    public class ReleaseFamilyProduct
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseFamilyProductID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int FamilyProductID { get; set; }

        public bool IsChecked { get; set; }
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("FamilyProductID")]
        public virtual FamilyProduct FamilyProduct { get; set; }

        #endregion
    }
}
