using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Publication")]
    public class Publication
    {
        #region Properties
        [Key]
        public int PublicationID { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public int SentByID { get; set; } //???
        public string SendByName { get; set; }
        public string PublicationMail { get; set; }
        public DateTime? PublicationMailDate { get; set; }
        public DateTime? Date { get; set; }
        public bool IsDeleated { get; set; }
        #endregion

        #region relationships
        [ForeignKey("SentByID")]
        public virtual Employee Employee { get; set; }
        public virtual ICollection<ReleaseCP> ReleaseCPs { get; set; }
       
        #endregion
    }
}
