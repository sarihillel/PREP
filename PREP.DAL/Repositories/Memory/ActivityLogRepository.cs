using PREP.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Collections;
//using System.Data.Objects;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Security.Principal;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;
using System.Linq.Expressions;
using PREP.DAL.Functions.Extensions;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Reflection;
using PREP.DAL.Repositories;

namespace PREP.DAL.Repositories.Memory
{

    public class ActivityLogRepository : GenericRepository<PREPContext, ActivityLog>, IActivityLogRepository
    {
    }
}
