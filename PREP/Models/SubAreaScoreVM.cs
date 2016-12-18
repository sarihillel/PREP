using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class SubAreaScoreVM
    {
        public int SubAreaID { get; set; }
        public string Name { get; set; }
        public ScoreIMG ScoreStatus { get; set; }
        public double Score { get; set; }
        public Trand Trand { get; set; }
    }

}