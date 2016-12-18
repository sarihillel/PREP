using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public enum LinkType
    {
        UsefulLink = 1,
        WeeklyReport = 2

    }
    public class Links
    {
        public int LinksID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public LinkType Type { get; set; }
    }
}