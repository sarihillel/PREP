using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    public enum AdminValue
    {
        YES = 1,
        NO = 2
    }


    [Table("QuestionCharacteristic")]
    public class QuestionCharacteristic 
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int QuestionCharacteristicID { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionID { get; set; }
        [Key, Column(Order = 2)]
        public int CharacteristicID { get; set; }
        public string Comments { get; set; }
        public AdminValue AdminValue { get; set; }

        #endregion

        #region relationships

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }
        [ForeignKey("CharacteristicID")]
        public virtual Characteristic Characteristic { get; set; }

        #endregion
    }
}