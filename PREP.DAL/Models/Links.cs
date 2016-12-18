using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    public enum LinkType
    {
        UsefulLink = 1,
        WeeklyReport = 2

    }
    [Table("Links")]
    public class Links
    {
        #region Properties
        [Key]
        public int LinksID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public LinkType Type { get; set; }
        #endregion
        #region relationships
        //  public virtual ICollection<ReleaseUseFullLinks> ReleaseFamilyProducts { get; set; }
        #endregion
    }
}
