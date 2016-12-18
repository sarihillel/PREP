using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public enum ScoreIMG
    {
        like,
        flag,
        stop,
        blank
    }
    public enum Trand
    {
        up,
        down,
        nochange,
        none
    }
    public class AreaScoreVM
    {
        public int AreaID { get; set; }
        public int ReleaseID { get; set; }
        public string Name { get; set; }
        public ScoreIMG ScoreStatus { get; set; }
        public double Score { get; set; }

        public Trand Trand { get; set; }
        public string StatusAreaText { get; set; }
        public List<SubAreaScoreVM> SubAreaScors { get; set; }
        
    }
}