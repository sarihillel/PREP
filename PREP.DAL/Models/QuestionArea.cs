using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("QuestionArea")]
    public class QuestionArea
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionAreaID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int AreaID { get; set; }
        public string Comments { get; set; }
        public AdminValue AdminValue { get; set; }
        #endregion


        #region relationships
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        #endregion
    }
}