using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseCPReviewMode")]
    public class ReleaseCPReviewMode
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Column(Order = 0)]
        public int ReleaseCPReviewModeID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]

        public int CPReviewModeID { get; set; }
        public bool IsChecked { get; set; } // that Column Name??? 
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("CPReviewModeID")]
        public virtual CPReviewMode CPReviewMode { get; set; }
        #endregion
    }
}
