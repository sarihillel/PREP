using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models
{
    public class ReleaseTabs
    {
        public ReleaseGeneralDetails GeneralDetails { get; set; }
        public ReleaseAndFamilyProducts ProductsInScope { get; set; }
        public IEnumerable<ReleaseCharacteristicVM> ReleaseCharacteristic { get; set; }
        public IEnumerable<ReleaseCPReviewMode> CheckPointReviewMode { get; set; }
        public IEnumerable<ReleaseMilestoneVM> ReleaseMilestones { get; set; }
        public IEnumerable<ReleaseStakeholderVM> ReleaseStakeholders { get; set; }
        public IEnumerable<ReleaseAreaOwners> AreaOwners { get; set; }
        public Vendor VendorFocalPoints { get; set; }
    }


 
}