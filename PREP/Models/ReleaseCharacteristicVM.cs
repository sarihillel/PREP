using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PREP.DAL.Models;

namespace PREP.Models
{
    public class ReleaseCharacteristicVM
    {
        public int ReleaseID { get; set; }
        public int CharacteristicID { get; set; }     
        public string CharacteristicName { get; set; }
        public string ToolTip { get; set; }
        public bool IsChecked { get; set; }
    }
}