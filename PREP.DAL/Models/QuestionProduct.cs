using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("QuestionProduct")]
    public class QuestionProduct
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionProductID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int ProductID { get; set; }
        public AdminValue AdminValue { get; set; }
        public string Comments { get; set; }
        #endregion

        #region relationships
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
        #endregion
    }
}