using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class ReleaseFamilyProducts
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ReleaseFamilyProductID { get; set; }
        public bool IsChecked { get; set; }
        public string ToolTip { get; set; }
        public List<ReleaseProducts> Products { get; set; }
    }

   
}
