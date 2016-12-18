using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Models
{
    [Table("SubAreaScore")]
    public class SubAreaScore
    {

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int SubAreaScoreID { get; set; }
        [Key, Column(Order = 1)]
        public int AreaScoreID { get; set; }
        [Key, Column(Order = 2)]
        public int SubAreaID { get; set; }
        public double Score { get; set; }
        [NotMapped]
        public double? LastScore { get; set; }
        #endregion

        #region Relationship
        [ForeignKey("SubAreaID")]
        public virtual SubArea SubArea { get; set; }
        [ForeignKey("AreaScoreID")]
        public virtual AreaScore AreaScore { get; set; }

        #endregion
    }
}