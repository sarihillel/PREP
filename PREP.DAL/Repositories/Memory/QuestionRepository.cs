using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using System.Data.Entity.SqlServer;
using PREP.DAL.Functions;
using PREP.DAL.TableViews;
using System.Security.Principal;

namespace PREP.DAL.Repositories.Memory
{

    public class QuestionRepository : GenericRepository<PREPContext, Question>, IQuestionRepository
    {
        /// <summary>
        /// select Query to QuestionView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<Object> SelectQuestionView(Expression<Func<Question, bool>> Expression, string Sorting = null, string Filtering = null)
        {

            string AreaFocalPointName = "Area Focal Point";
            var Query = DbSet
                          .Include(p => p.SubArea.Area)
                          .Include(p => p.Milestone)
                          .Include(p => p.Stakeholder)
                          .Include(P => P.PreviousMilestone);

            if (Expression != null)
                Query = Query.Where(Expression);
            else if ((Filtering ?? "") != "")
            {
                Filtering = Filtering.Trim();
                Query = Query.Where(TableQuestion => TableQuestion.Milestone.Name.Contains(Filtering) ||
                            TableQuestion.QuestionCode.Contains(Filtering) ||
                            TableQuestion.QuestionText.Contains(Filtering) ||
                            TableQuestion.Order.ToString().Contains(Filtering) ||
                            TableQuestion.QuestionInfo.Contains(Filtering) ||
                            TableQuestion.SubArea.Area.Name.Contains(Filtering) ||
                            TableQuestion.SubArea.Name.Contains(Filtering) ||
                            TableQuestion.Milestone.Name.Contains(Filtering) ||
                            TableQuestion.PreviousMilestone.Name.Contains(Filtering) ||
                            TableQuestion.RatioBetweenMilestones.ToString().Contains(Filtering) ||
                            TableQuestion.UpdatesLog.Contains(Filtering) ||
                            (TableQuestion.IsFocalPoint == true ? AreaFocalPointName : TableQuestion.Stakeholder.Name).Contains(Filtering)
                         );
            }

            if (Sorting != null)
            {
                string Sort = Sorting.Split()[0];
                Expression<Func<Question, Object>> ExpressionOrderBy = null; ;
                switch (Sort)
                {
                    case "MilestoneID":
                        ExpressionOrderBy = (TableQuestion => TableQuestion.Milestone.Name);
                        break;
                    case "PreviousMilestoneID":
                        ExpressionOrderBy = (TableQuestion => TableQuestion.PreviousMilestone.Name);
                        break;
                    case "SubAreaID":
                        ExpressionOrderBy = (TableQuestion => TableQuestion.SubArea.Name);
                        break;
                    case "AreaID":
                        ExpressionOrderBy = (TableQuestion => TableQuestion.SubArea.Area.Name);
                        break;
                    case "QuestionOwnerID":
                        ExpressionOrderBy = (TableQuestion => (TableQuestion.IsFocalPoint == true ? AreaFocalPointName : TableQuestion.Stakeholder.Name));
                        break;
                }
                if (ExpressionOrderBy != null)
                    Query = Query.GetOrderByQuery(ExpressionOrderBy, Sorting);
                else
                    Query = Query.GetOrderByQuery(Sorting);


            }
            return from TableQuestion in Query
                   select new
                   {
                       QuestionID = TableQuestion.QuestionID,
                       QuestionCode = TableQuestion.QuestionCode,
                       QuestionText = TableQuestion.QuestionText,
                       Order = TableQuestion.Order,
                       MilestoneID = TableQuestion.MilestoneID,
                       PreviousMilestoneID = TableQuestion.PreviousMilestoneID,
                       RatioBetweenMilestones = TableQuestion.RatioBetweenMilestones,
                       QuestionInfo = TableQuestion.QuestionInfo,
                       SubAreaID = TableQuestion.SubAreaID,
                       AreaID = TableQuestion.SubArea.Area.AreaID,
                       QuestionOwnerID = TableQuestion.QuestionOwnerID,
                       IsFocalPoint = TableQuestion.IsFocalPoint,
                       AreaFocalPointName = AreaFocalPointName,
                       UpdatesLog = TableQuestion.UpdatesLog
                   };
        }
        public IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {

            //using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            //{
            //  var a=  db.GetByFiltering(19, ChecklistFilter.ALLQuestions, null);
            //}

            IEnumerable<Object> ListQuestion = null;
            CountRecords = 0;
            try
            {

                var query = SelectQuestionView(null, Sorting, Filtering);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListQuestion = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListQuestion;

        }

        public Object GetField(int QuestionID)
        {
            return SelectQuestionView(c => c.QuestionID == QuestionID).FirstOrDefault();
        }
        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Options> GetOrderTable()
        {
            return DbSet.Select(q => new OrderTableView() { Order = q.Order }).GetAvailableOrder();
        }

        #region QuestionClasification
        /// <summary>
        /// return Table By Type of Entity
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        public async Task<List<Options>> GetParametersOptions(string TableName,bool isDeleted=true)
        {
            return await QuestionClasifications.GetParametersOptions(TableName,isDeleted);
        }
   

        /// <summary>
        /// Return List View  of all QuestionsClassification List 
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="StartIndex"></param>
        /// <param name="Count"></param>
        /// <param name="Sorting"></param>
        /// <param name="Filtering"></param>
        /// <param name="CountRecords"></param>
        /// <returns></returns>
        public IEnumerable<QuestionClasifications> GetQuestionClasificationByFiltering(int QuestionID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<QuestionClasifications> ListQuestionClasification = null;
            CountRecords = 0;
            try
            {
                Expression<Func<QuestionClasifications, bool>> ExpressionFilter = p => p.QuestionID == QuestionID;


                if ((Filtering ?? "") != "")
                {
                    ExpressionFilter = ExpressionFilter.And(

                            tbl => (
                            tbl.ParameterName.Contains(Filtering) ||
                            tbl.TableName.Contains(Filtering) ||
                            (tbl.AdminValue == AdminValue.YES ? "YES" : "NO").Contains(Filtering) ||
                            (tbl.ParameterType == ParameterType.Add ? "Add" : "Remove").Contains(Filtering)
                            )
                        );
                }
                Sorting = Sorting.Replace("TableID", "TableName");
                var query = QuestionClasifications.SelectQuestionClasificationsView(Context, ExpressionFilter);
                query = query.GetOrderByQuery(Sorting);

                //string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListQuestionClasification = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListQuestionClasification;
        }

        /// <summary>
        /// Add new Record By Get QuestionClasifications Object
        /// return if sucsses or not 
        /// </summary>
        /// <param name="Record"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        public async Task<int> AddQuestionClasificationAndSaveAsync(QuestionClasifications Record, WindowsPrincipal NTUser)
        {
            return await QuestionClasifications.AddSaveAsync(Record, NTUser);
        }
        /// <summary>
        /// edit Record By Get QuestionClasifications Object
        /// return if sucsses or not 
        /// </summary>
        /// <param name="Record"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        public async Task<int> EditQuestionClasificationAndSaveAsync(QuestionClasifications Record, WindowsPrincipal NTUser)
        {
            return await QuestionClasifications.EditSaveAsync(Record, NTUser);
        }
        /// <summary>
        /// Delete Record By Get QuestionClasifications Object
        /// return if sucsses or not 
        /// </summary>
        /// <param name="Record"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        public async Task<int> DeleteQuestionClasificationAndSaveAsync(QuestionClasifications Record, WindowsPrincipal NTUser)
        {
            return await QuestionClasifications.DeleteSaveAsync(Record, NTUser);
        }
        public async Task<int> DeleteQuestionAndSaveAsync(Question Record, WindowsPrincipal NTUser)//new realy deleted
        {
            Delete(Record);
            //Record.IsDeleted = true;
            // Edit(Record);
            return await SaveAsync(NTUser);
        }

        /// <summary>
        /// get QuestionClasification Field By QuestionID and RecordID
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="RecordID"></param>
        /// <returns></returns>
        public QuestionClasifications GetQuestionClasificationField(int QuestionID, int ParameterID, string TableName)
        {
            QuestionClasifications Record = null;
            try
            {
                Record = QuestionClasifications.SelectQuestionClasificationsView(Context, p => p.QuestionID == QuestionID && p.ParameterID == ParameterID && p.TableName == TableName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return Record;
        }
        #endregion
        //public DateTime? HandlingStartDatecalculation(int? QuestionID, int? ReleaseID)
        //{
        //    Release Release;
        //    var Question = DbSet.Where(x => x.QuestionID == QuestionID).Include(x => x.Milestone).Include(x => x.PreviousMilestone).First();
        //    using (IReleaseRepository db = new ReleaseRepository())
        //    {
        //        Release = db.Where(a => a.ReleaseID == ReleaseID).Include(r => r.ReleaseMilestones).First();
        //    }
        //    return HandlingStartDatecalculation(Release, Question.Milestone.MilestoneID, Question.PreviousMilestone.MilestoneID, Question.RatioBetweenMilestones);
        //}

        public DateTime? HandlingStartDatecalculation(Release Release, int milestoneID, int? prevMilestoneID, double ratio)
        {
            DateTime MilestoneDate;

            var PrevMilestone = prevMilestoneID == null ? null : Release.ReleaseMilestones.FirstOrDefault(m => m.MilestoneID == prevMilestoneID);
            var Milestone = milestoneID == 0 ? null : Release.ReleaseMilestones.FirstOrDefault(m => m.MilestoneID == milestoneID);

            if (Milestone == null || Milestone.MilestoneDate == null)
                return null;
            else
                MilestoneDate = (DateTime)Milestone.MilestoneDate;
            if (PrevMilestone == null || PrevMilestone.MilestoneDate == null)
                return MilestoneDate.AddDays(-21);
            DateTime PrevMilestoneDate = (DateTime)PrevMilestone.MilestoneDate;
            double MilestomeDaysDiff = (MilestoneDate - PrevMilestoneDate).TotalDays;
            //Errors.Write("MilstoneDate=" + MilestoneDate.ToString() + " PrevMilestoneDate="
            //    + PrevMilestoneDate.ToString() + " MilestomeDaysDiff=" + MilestomeDaysDiff.ToString() + " ratio=" + ratio.ToString() + " add days=" + Math.Round(MilestomeDaysDiff * ratio / 100).ToString() + " handlingStartDate=" + PrevMilestoneDate.AddDays(Math.Round(MilestomeDaysDiff * ratio / 100)).ToString());
            return PrevMilestoneDate.AddDays(Math.Round(MilestomeDaysDiff * ratio / 100));
        }
        public bool IsQuestionHasParameters(int QuestionId)
        {
            int count = 0;
            using (IQuestionAreaRepository db = new QuestionAreaRepository())
            {
                count += db.Where(a => a.QuestionID == QuestionId).Count();
            }
            using (IQuestionCharacteristicRepository db = new QuestionCharacteristicRepository())
            {
                count += db.Where(a => a.QuestionID == QuestionId).Count();
            }
            using (IQuestionFamilyProductRepository db = new QuestionFamilyProductRepository())
            {
                count += db.Where(a => a.QuestionID == QuestionId).Count();
            }
            using (IQuestionProductRepository db = new QuestionProductRepository())
            {
                count += db.Where(a => a.QuestionID == QuestionId).Count();
            }
            using (IQuestionStakeholderRepository db = new QuestionStakeholderRepository())
            {
                count += db.Where(a => a.QuestionID == QuestionId).Count();
            }
            return count > 0;
        }
        #region Initiate Questions
        public async Task SetInitiateQuestions(Release CurrentRelease, List<int> ReleaseQuestion, WindowsPrincipal User)
        {

            try
            {
                List<ReleaseChecklistAnswer> ReleaseChecklistAnswers = new List<ReleaseChecklistAnswer>();

                if (ReleaseQuestion != null && ReleaseQuestion.Count() > 0)
                {
                    var Questions = DbSet.Where(q => ReleaseQuestion.Contains(q.QuestionID))
                          .Include(p => p.SubArea.Area)
                          .Include(p => p.Milestone)
                          .Include(p => p.Stakeholder)
                          .Include(P => P.PreviousMilestone)
                          .Select(q => new
                          {
                              QuestionID = q.QuestionID,
                              QuestionText = q.QuestionText,
                              QuestionInfo = q.QuestionInfo,
                              Order = q.Order,
                              PreviousMilestoneID = q.PreviousMilestoneID,
                              MilestoneID = q.MilestoneID,
                              QuestionOwnerID = q.QuestionOwnerID,
                              SubArea = q.SubArea,
                              RatioBetweenMilestones = q.RatioBetweenMilestones
                          }).ToList();

                    Questions.ForEach(q =>
                    {
                        var newCheclistAnswer = new ReleaseChecklistAnswer()
                        {
                            QuestionID = q.QuestionID,
                            ReleaseID = CurrentRelease.ReleaseID,
                            QuestionText = q.QuestionText,
                            QuestionInfo = q.QuestionInfo,
                            SubAreaID = q.SubArea.SubAreaID,
                            AreaID = q.SubArea.AreaID,
                            Responsibility = CurrentRelease.ReleaseAreaOwners.FirstOrDefault(a => a.AreaID == q.SubArea.AreaID).Resposibility
                        };

                        //var PrevMilestone = CurrentRelease.ReleaseMilestones.First(m => m.MilestoneID == q.PreviousMilestone.MilestoneID);
                        //var Milestone = CurrentRelease.ReleaseMilestones.First(m => m.MilestoneID == q.Milestone.MilestoneID);
                        //DateTime PrevMilestoneDate = PrevMilestone != null && PrevMilestone.MilestoneDate != null ? (DateTime)PrevMilestone.MilestoneDate : new DateTime(1753,1,1);
                        //DateTime MilestoneDate = Milestone != null && Milestone.MilestoneDate != null ? (DateTime)Milestone.MilestoneDate : new DateTime(1753, 1, 1);


                        //// Calculate HandlingStartDate & RiskLevel just if Milastones Dates Exist.
                        //if (!DateTime.Equals(PrevMilestoneDate, new DateTime(1753, 1, 1)) && !DateTime.Equals(MilestoneDate, new DateTime(1753, 1, 1)))
                        //{
                        //    double MilestomeDaysDiff = (MilestoneDate - PrevMilestoneDate).TotalDays;
                        //    DateTime HandlingStartDate = PrevMilestoneDate.AddDays(MilestomeDaysDiff * q.RatioBetweenMilestones);
                        //    newCheclistAnswer.HandlingStartDate = HandlingStartDate;
                        //    newCheclistAnswer.RiskLevelID = HandlingStartDate > DateTime.Today ? RiskLevels.Initiated : RiskLevels.High;
                        //}
                        //else
                        //{
                        //    newCheclistAnswer.HandlingStartDate = null;
                        //    newCheclistAnswer.RiskLevelID = RiskLevels.Initiated;
                        //}
                        newCheclistAnswer.HandlingStartDate = HandlingStartDatecalculation(CurrentRelease, q.MilestoneID, q.PreviousMilestoneID, q.RatioBetweenMilestones);
                        newCheclistAnswer.RiskLevelID = newCheclistAnswer.HandlingStartDate > DateTime.Today || newCheclistAnswer.HandlingStartDate == null ? RiskLevels.Initiated : RiskLevels.High;
                        // Calculate QuestionOwner According to Question -> QuestionOwner value : AmdocsFocalPoint / Stakeholder 
                        int? OwnerID;

                        if (q.QuestionOwnerID == null) //Area Focal Point
                        {
                            var AreaOwner = CurrentRelease.ReleaseAreaOwners.First(a => a.AreaID == q.SubArea.AreaID);
                            OwnerID = AreaOwner != null ? AreaOwner.AmdocsFocalPoint1ID ?? null : null;
                        }
                        else
                        {
                            var StakeholderOwner = CurrentRelease.ReleaseStakeholders.FirstOrDefault(s => s.StakeholderID == q.QuestionOwnerID);
                            OwnerID = StakeholderOwner != null ? StakeholderOwner.EmployeeID1 ?? null : null;
                        }
                        newCheclistAnswer.QuestionOwnerID = OwnerID;

                        ReleaseChecklistAnswers.Add(newCheclistAnswer);

                    });

                }

                CurrentRelease.ReleaseChecklistAnswers = ReleaseChecklistAnswers;

                using (IReleaseRepository db = new ReleaseRepository())
                {
                    await db.EditReleaseChecklistAnswer(CurrentRelease, User);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        #endregion
    }
}
