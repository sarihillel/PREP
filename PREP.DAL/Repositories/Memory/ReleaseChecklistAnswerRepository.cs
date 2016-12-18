using PREP.DAL.Functions;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseChecklistAnswerRepository : GenericRepository<PREPContext, ReleaseChecklistAnswer>, IReleaseChecklistAnswerRepository
    {
        #region ViewStatus
        public IEnumerable<AreaScore> GetStatus(int ReleaseId)
        {
            Release currentRelease;
            using (IReleaseRepository db = new ReleaseRepository())
            {
                currentRelease = db.Where(r => r.ReleaseID == ReleaseId)
                                    .Include(r => r.ReleaseAreaOwners)
                                    .Include(r => r.ReleaseAreaOwners.Select(a => a.Area.SubAreas)).FirstOrDefault();
            }

            List<RiskLevels> RiskArr = new List<RiskLevels>() { RiskLevels.OnHold, RiskLevels.Initiated, RiskLevels.NA };
            IEnumerable<ReleaseChecklistAnswer> RelevantQuestions = DbSet.Where(q => q.ReleaseID == ReleaseId && !RiskArr.Contains(q.RiskLevelID))
                                                                            .Include(q => q.Question)
                                                                            .Include(q => q.SubArea)
                                                                            .Include(q => q.RiskLevel).ToList();
            List<AreaScore> AreaScores = new List<AreaScore>();
            currentRelease.ReleaseAreaOwners.OrderBy(r => r.Area.Order).ToList().ForEach(a =>
            {
                var AreaQuestions = RelevantQuestions.Where(q => q.SubArea.AreaID == a.AreaID).ToList();
                if (AreaQuestions.Count > 0)
                {
                    AreaScore Area = new AreaScore()
                    {
                        Area = a.Area,
                        Score = ScoreCalculation(AreaQuestions),
                        LastScore = GetLastScore(ReleaseId, a.AreaID),
                        SubAreaScores = new List<SubAreaScore>()
                    };
                    a.Area.SubAreas.OrderBy(s => s.Order).ToList().ForEach(s =>
                    {
                        var SubAreaQuestions = RelevantQuestions.Where(q => q.SubAreaID == s.SubAreaID).ToList();
                        SubAreaScore SubScore = new SubAreaScore()
                        {
                            SubArea = s,
                            Score = SubAreaQuestions.Count > 0 ? ScoreCalculation(SubAreaQuestions) : -1,
                            LastScore = GetLastScore(ReleaseId, s.AreaID, s.SubAreaID)
                        };
                        Area.SubAreaScores.Add(SubScore);
                    });
                    AreaScores.Add(Area);
                }
            });
            return AreaScores;

        }


        private double ScoreCalculation(List<ReleaseChecklistAnswer> Questions)
        {
            double Score = 0;
            int Reducation = 0;
            IDictionary<RiskLevels, int> RiskLevelDic;
            RiskLevelDic = Context.Set<RiskLevel>().ToDictionary(c => c.RiskLevelID, c => c.Value);

            var QuestionLevel = Questions.GroupBy(p => p.RiskLevelID).ToDictionary(g => g.Key, g => g.Count());

            QuestionLevel.ToList().ForEach(q => { Score += RiskLevelDic[q.Key] * q.Value; });
            Score = Score / Questions.Count;

            if (QuestionLevel.ContainsKey(RiskLevels.ShowStopper))
                Reducation = 30 + 6 * (QuestionLevel[RiskLevels.ShowStopper] - 1) + 2 * (QuestionLevel.ContainsKey(RiskLevels.High) ? QuestionLevel[RiskLevels.High] : 0);
            else if (QuestionLevel.ContainsKey(RiskLevels.High))
                Reducation = 10 + 2 * (QuestionLevel[RiskLevels.High] - 1);

            Score = Score * (100 - Reducation) / 100;
            return Score > 0 ? Score : 0;
        }
        #endregion
        private double? GetLastScore(int ReleaseID, int AreaID = 0, int? SubAreaID = 0)
        {
            double? LastScore = null;
            var LastPublish = Context.Set<ReleaseCP>().Where(r => r.ReleaseID == ReleaseID && r.PublicationMailDate != null).OrderByDescending(q => q.PublicationMailDate).FirstOrDefault();
            if (LastPublish != null)
            {
                AreaScore CurrentAreaScore = Context.Set<AreaScore>().Where(a => a.ReleaseID == ReleaseID && a.CPID == LastPublish.CPID && a.AreaID == AreaID).Include(a => a.SubAreaScores).FirstOrDefault();
                if (CurrentAreaScore != null)
                    if (SubAreaID == 0)
                        LastScore = CurrentAreaScore.Score;
                    else
                    {
                        SubAreaScore CurrentSubAreaScore = CurrentAreaScore.SubAreaScores.Where(s => s.SubAreaID == SubAreaID).FirstOrDefault();
                        LastScore = CurrentSubAreaScore != null ? CurrentSubAreaScore.Score : 0;
                    }
            }
            return LastScore;
        }

        public async Task<int> EditReleaseChecklist(List<ReleaseChecklistAnswerView> Checklist, WindowsPrincipal user)
        {
            int count = 0;
            try
            {
                foreach (var newObject in Checklist)
                {
                    var dbObject = DbSet.SingleOrDefault(s => s.ReleaseChecklistAnswerID == newObject.ReleaseChecklistAnswerID);
                    if (dbObject != null)
                    {
                        // Update ReleaseChecklistAnswerView that are in the  collection

                        //for securitiy not recomended
                        //Context.Entry<ReleaseChecklistAnswer>(dbObject).CurrentValues.SetValues(newObject);

                        dbObject.HandlingStartDate = newObject.HandlingStartDate;
                        dbObject.Comments = newObject.Comments;
                        //dbObject.SubAreaID = newObject.SubAreaID;
                        //dbObject.AreaID = newObject.AreaID;
                        dbObject.Responsibility = newObject.Responsibility;
                        //dbObject.QuestionOwnerID = newObject.QuestionOwnerID;
                        dbObject.ActualComplation = newObject.ActualComplation;
                        dbObject.RiskLevelID = newObject.RiskLevelID;

                    }

                }
                count += await this.SaveAsync(user);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                count = -1;
            }
            return count;
        }

        /// <summary>
        /// select Query to ReleaseChecklistAnswerView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<ReleaseChecklistAnswerView> SelectReleaseChecklistAnswerView(Expression<Func<ReleaseChecklistAnswerView, bool>> Expression = null, string Sorting = null)
        {
            int ReleaseChecklistAnswerTableID = StaticResources.GetTableID(typeof(ReleaseChecklistAnswer).Name);
            //select
            IQueryable<ReleaseChecklistAnswerView> Query =
                                    from Tbl in DbSet
                                    .Include(p => p.Release)//
                                    .Include(p => p.SubArea)//
                                    .Include(p => p.SubArea.Area)//
                                    .Include(p => p.QuestionOwner)//
                                    .Include(p => p.Question.Stakeholder)//
                                    .Include(p => p.Question.Milestone)//
                                    join activity in (
                                                 Context.Set<History>()
                                                .Include(a => a.ActivityLog)
                                                .Where(a => a.TableID == ReleaseChecklistAnswerTableID)// && a.Employee.UserName=="System")
                                                .GroupBy(c => c.ItemID)
                                                .Select(g => new { Key = g.Key, MaxDate = g.Max(x => x.ActivityLog.Date) })

                                    ) on Tbl.ReleaseChecklistAnswerID equals activity.Key into GroupActivity
                                    from activityLastDate in GroupActivity.DefaultIfEmpty()
                                    select new ReleaseChecklistAnswerView
                                    {
                                        ReleaseChecklistAnswerID = Tbl.ReleaseChecklistAnswerID,
                                        ReleaseID = Tbl.ReleaseID,
                                        ReleaseName = Tbl.Release.Name,
                                        QuestionID = Tbl.QuestionID,
                                        QuestionCode = Tbl.Question.QuestionCode,
                                        SubAreaID = Tbl.SubArea.SubAreaID,
                                        SubAreaName = Tbl.SubArea.Name,
                                        AreaID = Tbl.AreaID,
                                        AreaName = Tbl.Area.Name,
                                        QuestionMilestoneID = Tbl.Question.MilestoneID,
                                        QuestionMilestoneName = Tbl.Question.Milestone.Name,
                                        QuestionText = Tbl.QuestionText,
                                        QuestionInfo = Tbl.QuestionInfo,
                                        QuestionOwnerID = Tbl.QuestionOwnerID,
                                        QuestionOwnerName = Tbl.QuestionOwner == null ? (Tbl.Question.QuestionOwnerID==null?Tbl.Area.Name:Tbl.Question.Stakeholder.Name) : ((Tbl.QuestionOwner.FirstName ?? "") + " " + (Tbl.QuestionOwner.LastName ?? "")),
                                        
                                        
                                        //: Tbl.Release.ReleaseAreaOwners.Where(a=>a.AreaID==Tbl.SubArea.AreaID).FirstOrDefault().AmdocsFocalPoint1.FirstName+" "+ Tbl.Release.ReleaseAreaOwners.Where(a => a.AreaID == Tbl.SubArea.AreaID).FirstOrDefault().AmdocsFocalPoint1.LastName,
                                        HandlingStartDate = Tbl.HandlingStartDate,
                                        ActualComplation = Tbl.ActualComplation,
                                        RiskLevelID = Tbl.RiskLevelID,
                                        ExtrenalFocalPoint = Tbl.ExtrenalFocalPoint,
                                        Responsibility = Tbl.Responsibility,
                                        Comments = Tbl.Comments,
                                        QuestionOrder = Tbl.Question.Order,
                                        IsEdited = Tbl.IsEdited,
                                        LastAutomaticUpdateDate = Tbl.LastAutomaticUpdateDate,
                                        //LastModifiedDate = activityLastDate.MaxDate
                                        LastModifiedDate=DateTime.MaxValue
                                    };

            //filter
            if (Expression != null)
                Query = Query.Where(Expression);
            //Sorting
            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);

            return Query;
        }
        //private void SetSubAreaList(ReleaseChecklistAnswerView rclv)
        //{
        //    using (ISubAreaRepository db = new SubAreaRepository())
        //    {
        //        rclv.SubAreaList = db.getSubAreaByAreaId(rclv.AreaID);
        //    }
        //}
        public IQueryable<ReleaseChecklistAnswerView> GetByFiltering(int ReleaseID, string Sorting = null, string Filtering = null)
        {
            //  IEnumerable<ReleaseChecklistAnswerView> ListReleaseChecklistAnswerView = null;
            IQueryable<ReleaseChecklistAnswerView> query=null;
            try
            {
                Expression<Func<ReleaseChecklistAnswerView, bool>> Expression = p => p.ReleaseID == ReleaseID;

                query = SelectReleaseChecklistAnswerView(Expression, Sorting);
                string sqlstring = query.ToString();

               // ListReleaseChecklistAnswerView = query.ToList();
                //  ListReleaseChecklistAnswerView.ToList().ForEach(x => SetSubAreaList(x));
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return query;

        }

        public bool IsExistCheckList(int? releaseId)
        {
            if (releaseId == null)
                releaseId = -1;
            return DbSet.AsNoTracking().Where(chl => chl.ReleaseID == releaseId).Any();
        }
    }
}
