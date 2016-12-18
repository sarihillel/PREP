using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("UseFullLinks")]
    public class UseFullLinks
    {
        #region Properties
        [Key]
        public int UseFullLinksID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<ReleaseUseFullLinks> ReleaseFamilyProducts { get; set; }
        #endregion
    }
}