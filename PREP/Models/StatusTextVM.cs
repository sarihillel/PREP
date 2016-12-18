using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class StatusTextVM
    {
        #region Properties
        public int StatusTextID { get; set; }
        public int ReleaseID { get; set; }
        public string HighLightText { get; set; }
        #endregion

        //#region relationships
        //public virtual Release Release { get; set; }
        //#endregion
    }
}