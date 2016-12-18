using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseCharacteristic")]
    public class ReleaseCharacteristic
    {
        #region Properties
        [Column(Order = 0),DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReleaseCharacteristicID { get; set; }

        [Key,Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key,Column(Order = 2)]
        public int CharacteristicID { get; set; }

        public bool IsChecked { get; set; }
        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("CharacteristicID")]
        public virtual Characteristic Characteristic { get; set; }
        #endregion
    }
}
