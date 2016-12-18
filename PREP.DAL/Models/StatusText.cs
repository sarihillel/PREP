using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("StatusText")]
    public class StatusText
    {
        #region Properties
        [Key]
        public int StatusTextID { get; set; }
        public int ReleaseID { get; set; }
        public string HighLightText { get; set; }
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        #endregion
    }
}
