using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.TableViews
{
    [ComplexType]
    public class Options
    {
        public string DisplayText { get; set; }
        //public string HelpTextValue { get; set; }
        public int Value { get; set; }
    }
}
