using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Models
{
    [Table("SubAreaScoreTemp")]
    public class SubAreaScoreTemp
    {
        #region Properties
        [Key]
        public int SubAreaScoreTempID { get; set; }
        public int AreaScoreTempID { get; set; }
        public int SubAreaID { get; set; }
        public double Score { get; set; }
        #endregion

        #region relationships
        [ForeignKey("AreaScoreTempID")]
        public virtual AreaScoreTemp AreaScoreTemp { get; set; }
        #endregion
    }
}
