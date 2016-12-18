using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class ReleaseAreaOwners
    {
        public int ReleaseAreaOwnerID { get; set; }

        public int ReleaseID { get; set; }

        public int AreaID { get; set; }
        public string AreaName { get; set; }
      
        public bool IsChecked { get; set; }
       
        public Responsibility Resposibility { get; set; }
        public int? AmdocsFocalPoint1ID { get; set; }
        public int? AmdocsFocalPoint2ID { get; set; }
        public string CustomerFocalPoint1 { get; set; }
        public string CustomerFocalPoint2 { get; set; }
        public string AmdocsFocalPoint1Name { get; set; }
        public string AmdocsFocalPoint2Name { get; set; }
        public Employee AmdocsFocalPoint1 { get; set; }
        public Employee AmdocsFocalPoint2 { get; set; }
        public string ToolTip { get; set; }
    }
}