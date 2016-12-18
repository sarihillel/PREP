using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("StatusAreaText")]
    public class StatusAreaText
    {
        #region Properties
        [Column(Order = 0)]
        public int StatusAreaTextID { get; set; }
        [Key,Column(Order =1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int AreaID { get; set; }
        public string AreaText { get; set; }
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        #endregion
    }
}
