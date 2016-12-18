using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("OurVision")]
    public class OurVision
    {
        #region Properties
        [Key]
        public int OurVisionID { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        #endregion

        #region relationships
        //public virtual ICollection<TableName> TableNames { get; set; }
        #endregion
    }
}