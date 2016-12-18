using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("QuestionFamilyProduct")]
    public class QuestionFamilyProduct
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionFamilyProductID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int FamilyProductID { get; set; }
        public string Comments { get; set; }
        public AdminValue AdminValue { get; set; }
        #endregion

        #region relationships
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("FamilyProductID")]
        public virtual FamilyProduct FamilyProduct { get; set; }
        #endregion      
    }
}