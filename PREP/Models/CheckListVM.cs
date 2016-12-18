using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace PREP.Models
{
    public class CheckListVM
    {
        public string Responsibility { get; set; }
        public string RiskLevelList { get; set; }
        public Int32 UserID { get; set; }
        public Int32 ReleaseID { get; set; }
      //  public IEnumerable<Area> AreaList { get; set; }
    


    }
}