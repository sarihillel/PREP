using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace PREP.DAL.Models
{
    [Table("ReleaseCP")]
    public class ReleaseCP
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseCPID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int CPID { get; set; }
        public int? PublicationID { get; set; }
        public bool? ExceptionIndicator { get; set; }

        public DateTime? ExceptionDate { get; set; }
        public string ExceptionRemarks { get; set; }
        public string DelayReason { get; set; }
        public string DelayReasonHistory { get; set; }
        public string Comments { get; set; }
        public int PublicationCount { get; set; }
        public string PublicationMail { get; set; }
        public DateTime? PublicationMailDate { get; set; }
        public int? SendByID { get; set; } 
        public string SendByName { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region relationships
        [ForeignKey("SendByID")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("CPID")]
        public virtual CP CP { get; set; }
        [ForeignKey("PublicationID")]
        public virtual Publication Publication { get; set; }
        #endregion
    }
}
