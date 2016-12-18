using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Characteristic")]
    public class Characteristic
    {
        #region Properties
        [Key]
        public int CharacteristicID { get; set; }
        public string Name { get; set; }
        public ParameterType Type { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ToolTip { get; set; }
        #endregion

        #region relationships
        public virtual ICollection<ReleaseCharacteristic> ReleaseCharacteristics { get; set; }
        public virtual ICollection<QuestionCharacteristic> QuestionCharacteristics { get; set; }
        #endregion
    }
}
