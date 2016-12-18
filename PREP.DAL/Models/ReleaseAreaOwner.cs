using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseAreaOwner")]
    public class ReleaseAreaOwner
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity),Column(Order =0)]
        public int ReleaseAreaOwnerID { get; set; }

        [Key,Column(Order =1)]
        public int ReleaseID { get; set; }
        [Key,Column(Order = 2)]
        public int AreaID { get; set; }

        public bool IsChecked { get; set; }
        public Responsibility Resposibility { get; set; }
        public int? AmdocsFocalPoint1ID { get; set; }
        public int? AmdocsFocalPoint2ID { get; set; }
        public string CustomerFocalpoint1 { get; set; }
        public string CustomerFocalPoint2 { get; set; }
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        [ForeignKey("AmdocsFocalPoint1ID")]
        public virtual Employee AmdocsFocalPoint1 { get; set; }
        [ForeignKey("AmdocsFocalPoint2ID")]
        public virtual Employee AmdocsFocalPoint2 { get; set; }
    #endregion

}
}
