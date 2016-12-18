using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PREP.DAL.Models;

namespace PREP.Models
{
    public class ScoreIndicator
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public int ScoreLevel { get; set; }
        public ScoreIMG ScoreIMG { get; set; }
        

    }
}
