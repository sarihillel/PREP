using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class StatusAreaTextVM  
    {
        public int StatusAreaTextID { get; set; }
        public int AreaID { get; set; }
        public int releaseID { get; set; }
        public string Text { get; set; }

    }
}