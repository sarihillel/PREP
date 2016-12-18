using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("History")]
    public class History
    {
        #region Properties
        [Key]
        public int HistoryID { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int ItemID { get; set; }
        public int TableID { get; set; }   
        public int ActivityLogID { get; set; }
        public int? ReleaseID { get; set; }
        #endregion

        #region Consractors
        public History(){ }
        public History(string FieldName, string OldValue, string NewValue, int itemID, int tableID,int? releaseID,int activityLogID)
        {
            this.ActivityLogID = activityLogID;
            this.ItemID = itemID;
            this.TableID = tableID;
            this.FieldName = FieldName;
            this.OldValue = OldValue;
            this.NewValue = NewValue;
            this.ReleaseID = releaseID;
        }
        #endregion

        #region relationships
        [ForeignKey("ActivityLogID")]
        public virtual ActivityLog ActivityLog { get; set; }
        [ForeignKey("ReleaseID")]
        public virtual Release Release { get; set; }
        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }
        #endregion
    }
}
