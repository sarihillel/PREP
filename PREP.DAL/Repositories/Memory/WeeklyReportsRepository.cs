using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class WeeklyReportsRepository : GenericRepository<PREPContext, WeeklyReports>, IWeeklyReportsRepository
    {
       
    }
}
