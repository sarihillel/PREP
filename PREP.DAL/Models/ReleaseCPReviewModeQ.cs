using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseCPReviewModeQ")]
    public class ReleaseCPReviewModeQ
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Column(Order = 0)]
        public int ReleaseCPReviewModeQID { get; set; }

        [Key,Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key,Column(Order = 2)]

        public int? CPReviewModeQID { get; set; }
        public bool IsFullTrack { get; set; } // that Column Name??? 
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("CPReviewModeQID")]
        public virtual CPReviewModeQ CPReviewModeQ { get; set; }
        #endregion
    }
}
