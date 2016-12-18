using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IPublicationTempRepository : IGenericRepository<PublicationTemp>
    {
        Task UpdateScoresByPublicationID(int PublicationID, string UserName);
    }

}
