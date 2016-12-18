using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Models
{
    [Table("PublicationTemp")]
    public class PublicationTemp
    {
        [Key]
        public int PublicationID { get; set; }
        public int ReleaseID { get; set; }
        public int CPID { get; set; }
        public DateTime PublicationDate { get; set; }

        #region relationships
        public virtual ICollection<AreaScoreTemp> AreaScoreTemps { get; set; }
        #endregion

    }
}
