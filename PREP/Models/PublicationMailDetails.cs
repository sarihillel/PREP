using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PREP.Models
{
    public class PublicationMailDetails
    {
        public int ReleaseID { get; set; }
        public int CPID { get; set; }
      //  public string CPName { get; set; }
        public string FromEmail { get; set; }
        public string FromPSW{ get; set; }
        public string To { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string beforeMessage { get; set; }
        [AllowHtml]
        public string afterMessage { get; set; }
        public string imagePath { get; set; }
        //public string base64string { get; set; }
    }
}