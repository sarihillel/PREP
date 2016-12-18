using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PREP.DAL.Models
{
    [Table("SubArea")]
    public class SubArea
    {
        #region Properties
        [Key]
        public int SubAreaID { get; set; }
        public string Name { get; set; }
        public int AreaID { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        #endregion



        #region relationships
        [ForeignKey("AreaID")]
        public virtual Area Area { get; set; }
        public virtual ICollection<Question> QuestionRepositorys { get; set; }
        public virtual ICollection<SubAreaScore> SubAreaScores { get; set; }

        #endregion
    }
}
