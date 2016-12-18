using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("HRMSFields")]
    public class HRMSFields
    {
        #region Properties
        [Key]
        public int HRMSFieldsID { get; set; }
        public string Name { get; set; }
        public string value { get; set; }
        public string ParameterName { get; set; }
        #endregion
    }
}
