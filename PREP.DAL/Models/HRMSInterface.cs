using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("HRMSInterface")]
    public class HRMSInterface
    {
        #region Properties
        [Key]
        public int HRMSInterfaceFieldsID { get; set; }
        public string Name { get; set; }
        public string value { get; set; }
        #endregion
    }
}
