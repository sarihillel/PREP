using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.TableViews
{
    class QuestionAreaView
    {
        public int QuestionID { get; set; }
        public string TableName { get; set; }
        public int ParameterID { get; set; }
        public ParameterType ParameterType { get; set; }
        public AdminValue AdminValue { get; set; }
        public AdminValue UserValue { get; set; }
    }
}
