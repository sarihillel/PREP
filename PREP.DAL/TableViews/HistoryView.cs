using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.TableViews
{
    public class HistoryView
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int? ReleaseId { get; set; }
        public string ReleaseName { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public int? CpId { get; set; }
        public string CpName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int? ModifiedById { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
    }
}
