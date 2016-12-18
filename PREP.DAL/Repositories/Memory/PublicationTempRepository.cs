using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class PublicationTempRepository : GenericRepository<PREPContext, PublicationTemp>, IPublicationTempRepository
    {
        public async Task UpdateScoresByPublicationID(int PublicationID, string UserName)
        {
            try
            {
                int count = 0;
                PublicationTemp currentPub = DbSet.Where(p => p.PublicationID == PublicationID)
                                                    .Include(p => p.AreaScoreTemps)
                                                    .Include(p => p.AreaScoreTemps.Select(a => a.SubAreaScoreTemps)).FirstOrDefault();
                if (currentPub != null)
                {
                    using (IAreaScoreRepository db = new AreaScoreRepository())
                    {
                        // save area scores
                        currentPub.AreaScoreTemps.ToList().ForEach(a =>
                        {
                            AreaScore currentAreaScore = db.Where(s => s.ReleaseID == currentPub.ReleaseID && s.CPID == currentPub.CPID && s.AreaID == a.AreaID).FirstOrDefault();
                            if (currentAreaScore != null)
                            {
                                currentAreaScore.Score = a.Score;
                            }
                            else
                            {
                                db.Add(new AreaScore() { AreaID = a.AreaID, ReleaseID = currentPub.ReleaseID, CPID = currentPub.CPID, Score = a.Score });
                                Task.Run(async () => { count += await db.SaveAsync(null, false, null, null, UserName); }).Wait();
                                currentAreaScore = db.Where(s => s.AreaID == a.AreaID && s.ReleaseID == currentPub.ReleaseID && s.CPID == currentPub.CPID).FirstOrDefault();
                            }

                            // save sub area scores
                            using (ISubAreaScoreRepository dbSub = new SubAreaScoreRepository())
                            {
                                a.SubAreaScoreTemps.ToList().ForEach(s =>
                                {
                                    SubAreaScore currentSub = dbSub.Where(sub => sub.SubAreaID == s.SubAreaID && sub.AreaScoreID == currentAreaScore.AreaScoreID).FirstOrDefault();
                                    if (currentSub != null)
                                    {
                                        currentSub.Score = s.Score;
                                    }
                                    else
                                    {
                                        dbSub.Add(new SubAreaScore() { SubAreaID = s.SubAreaID, AreaScoreID = currentAreaScore.AreaScoreID, Score = s.Score });
                                    }
                                });
                                Task.Run(async () => { count += await dbSub.SaveAsync(null, false, null, null, UserName); }).Wait();                                
                            }
                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
