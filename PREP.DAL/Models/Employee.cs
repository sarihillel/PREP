using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Employee")]
  //  [MetadataType(typeof(Employee_Validation))]
    public class Employee
    {
        #region Properties
        [Key]
        public int EmployeeID { get; set; }
        public int MDMCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName  { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region relationships
         public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Publication> Publications { get; set; }
        
        #endregion
    }

  
    //public partial class Employee_Validation
    //{
    //    [Required]
    //    public int? MDMCode { get; set; }
    //    [DisplayColumn("Tryyyyy")]
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }

    //}


}
