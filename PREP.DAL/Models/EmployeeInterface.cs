using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("EmployeeInterface")]
    public class EmployeeInterface
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None), Column(Order = 0)]
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region relationships
       
        
        #endregion
    }
}

