using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{

    [Table("AreaScore")]
    public class AreaScore
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Column(Order = 0)]
        public int AreaScoreID { get; set; }
        [Index("IX_Release_CP_Area", 1, IsUnique = true)]
        public int ReleaseID { get; set; }
        [Index("IX_Release_CP_Area", 2, IsUnique = true)]
        public int CPID { get; set; }
        [Index("IX_Release_CP_Area", 3, IsUnique = true)]
        public int AreaID { get; set; }
        public double Score { get; set; }
        [NotMapped]
        public double? LastScore { get; set; }
        #endregion

        #region Relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("CPID")]
        public virtual CP CP { get; set; }
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        public virtual ICollection<SubAreaScore> SubAreaScores { get; set; }
        #endregion
    }
}
