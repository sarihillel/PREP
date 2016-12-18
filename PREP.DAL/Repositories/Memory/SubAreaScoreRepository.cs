using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;

namespace PREP.DAL.Repositories.Memory
{
    public class SubAreaScoreRepository : GenericRepository<PREPContext, SubAreaScore>, ISubAreaScoreRepository
    {


    }
}
