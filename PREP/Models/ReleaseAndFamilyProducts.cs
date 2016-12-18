using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class ReleaseAndFamilyProducts
    {
        public IEnumerable<ReleaseFamilyProducts> FamilyProducts { get; set; }
        public Release Release { get; set; }
    }
}