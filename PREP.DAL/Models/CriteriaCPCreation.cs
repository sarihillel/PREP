using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("CriteriaCPCreation")]
    public class CriteriaCPCreation
    {
        #region Properties
        [Key]
        public int CriteriaCPCreationID { get; set; }
        public int? VersionID { get; set; }
        public int? Criteria { get; set; }
        public int? EmployeeID { get; set; }
        public string Status { get; set; }
        #endregion

        #region relationships
        //  public virtual ICollection<Class> Classes { get; set; }
        #endregion
    }
}