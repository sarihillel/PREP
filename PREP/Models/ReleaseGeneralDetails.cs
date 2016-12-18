using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PREP.DAL.Repositories.Memory;

namespace PREP.Models
{
    public class ReleaseGeneralDetails
    {
        public ReleaseGeneralDetails(Release release)
        {
            ReleaseID = release.ReleaseID != 0 ? release.ReleaseID : new Nullable<Int32>();
            AccountID = release.Account != null ? release.Account.AccountID : 0;
            AccountName = release.Account != null ? release.Account.Name : "";
            Name = release.Name;
            Size = release.Size;
            LOB = release.LOB;
            ReleaseCP rcp = release.ReleaseCPs !=null && release.ReleaseCPs.Count()>0 ? release.ReleaseCPs.OrderByDescending(i => i.PublicationMailDate).First():null;
            CP cp = rcp == null || rcp.PublicationMailDate == null ? null : rcp.CP;
            LastCP = cp != null  ? cp.Name : null;
            LastCPDate = rcp !=null && rcp.PublicationMailDate != null ? rcp.PublicationMailDate : null;
            Accounts = new AccountRepository().Get(a => a.IsDeleted == false).ToList();
            DismissIndicator = release.DismissIndicator;
        }

        public ReleaseGeneralDetails()
        {

        }
        #region Properties
        [DisplayName("IsDissmissed")]
        [ReadOnly(true)]
        public bool IsFullTrack { get; set; }
        
        [DisplayName("Release ID:")]
        [ReadOnly(true)]
        [Editable(false, AllowInitialValue = false)]
        public int? ReleaseID { get; set; }
        [ScaffoldColumn(false)]
        public int AccountID { get; set; }
        [DisplayName("Account Name:")]
        public string AccountName { get; set; }
        [MaxLength(50)]
        [DisplayName("Release Name:")]
        [Required(ErrorMessage = "Value must be specified")]
        public string Name { get; set; }
        [DisplayName("Release Size:")]
        [Required(ErrorMessage = "Value must be specified")]
        public double Size { get; set; }
        public string LOB { get; set; }
        [ReadOnly(true)]
        [DisplayName("Last CP:")]
        [Editable(false, AllowInitialValue = false)]
        public string LastCP { get; set; }
        [ReadOnly(true)]
        [DisplayName("Last CP Date:")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Editable(false, AllowInitialValue = false)]
        public DateTime? LastCPDate { get; set; }
        [ReadOnly(true)]
        [DisplayName("Created Date:")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Editable(false, AllowInitialValue = false)]
        public DateTime CreatedDate { get; set; }
        [ReadOnly(true)]
        [DisplayName("Created By:")]
        [Editable(false, AllowInitialValue = false)]
        public string CreatedBy { get; set; }
        [ReadOnly(true)]
        [DisplayName("Modified Date:")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [Editable(false, AllowInitialValue = false)]
        public DateTime? ModifiedDate { get; set; }
        [ReadOnly(true)]
        [DisplayName("Modified By:")]
        [Editable(false, AllowInitialValue = false)]
        public string ModifiedBy { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        [DisplayName("IsDissmissed")]
        [ReadOnly(true)]
        public string DismissIndicator { get; set; }
        #endregion
    }
}