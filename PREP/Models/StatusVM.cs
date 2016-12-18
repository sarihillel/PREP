using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class StatusVM
    {
        public int ReleaseID { get; set; }
        public int CPID { get; set; }
        public bool IsDraft { get; set; }
        public List<AreaScoreVM> AreaScores { get; set; }
        public ReleaseGeneralDetails Details { get; set; }
        public StatusTextVM StatusText { get; set; }
        public List<ReleaseStakeholderVM> ReleaseStakeholder { get; set; }


    }
}