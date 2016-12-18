using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{

    public enum Mode
    {
        ADD,
        EDIT,
        VIEW
    }
  
    public class CurrentRelease 
    {
        public int ReleaseId;
        public int CurrentTabIndex { get; set; }
        public string CurrentAction { get; set; }
        public Release Release { get; set; }
        public Mode Mode { get; set; }
        public ReleaseTabs  Tabs { get; set; }
    }
}