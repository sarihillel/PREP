using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("Table")]
    public class Table
    {
        #region Properties
        [Key]
        public int TableID { get; set; }
        public string Name { get; set; }
        public int ParameterRelashionID { get; set; }
        #endregion

        #region relationships
        public virtual ICollection <Permission> Permissions { get; set; }
        public virtual ICollection<ColumnMapping> ColumnMappings { get; set; }
        public virtual ICollection<CalculatedField> CalculatedFields { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        #endregion
    }
}
