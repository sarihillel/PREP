using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    public enum ParameterType
    {
        Remove = 0,
        Add = 1
    }

    //Hrms Properties
    [Table("Account")]
    public class Account
    {
        #region Properties
        [Key]
        public int AccountID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string PrepFPName { get; set; }
        public int? PrepFPEmpID { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        #region relationships
        [ForeignKey("PrepFPEmpID")]
        public virtual Employee Employee { get; set; }
        public virtual IEnumerable<Release> Releases { get; set; }
        #endregion
    }





}
