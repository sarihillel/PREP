using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("SystemConfig")]
    public class SystemConfig
    {
        #region Properties
        [Key]
        public int SystemConfigID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<ReleaseSystemConfig> ReleaseFamilyProducts { get; set; }
        #endregion
    }
}