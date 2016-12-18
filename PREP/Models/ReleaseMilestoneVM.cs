using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PREP.DAL.Models;

namespace PREP.Models
{
    public class ReleaseMilestoneVM
    {
        public int ReleaseID { get; set; }
        public int? MilestoneID { get; set; }
        [Display( Name ="Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:dd-MMM-yyyy}")]
        //   [Required(ErrorMessage = "Milestone Date is Requierd")]
        public DateTime? MilestoneDate { get; set; }
        public string MilestoneName { get; set; }
        public string ToolTip { get; set; }
       // public virtual Milestone Milestone { get; set; }

    }
}