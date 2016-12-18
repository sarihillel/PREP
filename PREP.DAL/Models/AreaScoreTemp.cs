using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Models
{

    [Table("AreaScoreTemp")]
    public class AreaScoreTemp
    {
        #region Properties
        [Key]
        public int AreaScoreTempID { get; set; }      
        public int PublicationID { get; set; }
        public int AreaID { get; set; }
        public double Score { get; set; }
        #endregion

        #region relationships
        [ForeignKey("PublicationID")]
        public virtual PublicationTemp publicationTemp { get; set; }
        public virtual ICollection<SubAreaScoreTemp> SubAreaScoreTemps { get; set; }
        #endregion

    }



}
