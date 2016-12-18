using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("ReleaseStakeholder")]
    public class ReleaseStakeholder
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ReleaseStakeholderID { get; set; }

        [Key, Column(Order = 1)]
        public int ReleaseID { get; set; }
        [Key, Column(Order = 2)]
        public int StakeholderID { get; set; }

        public int? EmployeeID1 { get; set; }
        public int? EmployeeID2 { get; set; }

        #endregion

        #region relationships
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }

        [ForeignKey("StakeholderID")]
        public virtual Stakeholder Stakeholder { get; set; }

        [ForeignKey("EmployeeID1")]
        public virtual Employee Employee1 { get; set; }

        [ForeignKey("EmployeeID2")]
        public virtual Employee Employee2 { get; set; }

        #endregion

    }
}
