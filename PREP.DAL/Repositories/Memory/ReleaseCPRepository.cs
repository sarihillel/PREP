using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using System.Data.Entity.SqlServer;
using PREP.DAL.Functions;
using System.Linq.Expressions;
using PREP.DAL.TableViews;
using System.Security.Principal;
using System.Globalization;
using System.Configuration;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseCPRepository : GenericRepository<PREPContext, ReleaseCP>, IReleaseCPRepository
    {
        /// <summary>
        /// select Query to ReleaseCPView
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private IQueryable<ReleaseCPView> SelectReleaseCPView(Expression<Func<ReleaseCP, bool>> Expression)
        {
            var Query = DbSet
                         .Include(p => p.Employee.MDMCode)
                         .Include(p => p.CP)
                         .Include(p => p.Release)
                         .Include(p => p.Release.Account);

            if (Expression != null)
                Query = Query.Where(Expression);

            return from TableCp in Query
                   select new ReleaseCPView
                   {
                       ReleaseCPID = TableCp.ReleaseCPID,
                       CPID = TableCp.CPID,
                       CPName = TableCp.CP.Name,
                       ReleaseID = TableCp.ReleaseID,
                       ReleaseName = TableCp.Release.Name,
                       PublicationID = TableCp.PublicationID,
                       AccountName = TableCp.Release.Account.Name,
                       PublicationMailDate = TableCp.PublicationMailDate,
                       PublicationMailLink = TableCp.PublicationMail != null ? TableCp.PublicationMail : "#",
                       //PublicationMailLinkText = "Downlowd",
                       PublicationCount = TableCp.PublicationCount,
                       SendByID = TableCp.SendByID == null ? null : (int?)TableCp.Employee.MDMCode,
                       SendByName = TableCp.SendByName,
                       ExceptionIndicator = TableCp.ExceptionIndicator ?? false,
                       ExceptionDate = TableCp.ExceptionDate,
                       ExceptionRemarks = TableCp.ExceptionRemarks,
                       DelayReason = TableCp.DelayReason,
                       DelayReasonHistory = TableCp.DelayReasonHistory,
                       Comments = TableCp.Comments,
                       IsDeleted = TableCp.IsDeleted,

                   };
        }


        /// <summary>
        /// Return Field ReleaseCPView By Keys
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <param name="CPID"></param>
        /// <returns></returns>
        public ReleaseCPView GetField(int ReleaseID, int CPID)
        {
            return SelectReleaseCPView(c => c.ReleaseID == ReleaseID && c.CPID == CPID).FirstOrDefault();
        }
        public ReleaseCPView GetField(int ReleaseCPID)
        {
            return SelectReleaseCPView(c => c.ReleaseCPID == ReleaseCPID).FirstOrDefault();
        }
        /// <summary>
        /// Reururn List of ReleaseCPView By Params
        /// </summary>
        /// <param name="StartIndex">Index Row To Return From List</param>
        /// <param name="Count">Top Records To show</param>
        /// <param name="Sorting">Column To Sort, String Name Column and 'Asc' or 'Desc', For Example: "AccountName desc"</param>
        /// <param name="Filtering">String To Filter of List</param>
        /// <param name="CountRecords">Return Count Recrode who's Found</param>
        /// <returns></returns>
        public IEnumerable<ReleaseCPView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            Filtering = Filtering ?? "";
            List<ReleaseCPView> queryList = null;

            try
            {
                Expression<Func<ReleaseCP, bool>> Expression = null;
                //if (Filtering != "")
                //{

                //    Expression = TableCp =>
                //                TableCp.CP.Name.Contains(Filtering)
                //               || TableCp.Release.Name.Contains(Filtering)
                //               || TableCp.Release.Account.Name.Contains(Filtering)
                //               || TableCp.Publication.PublicationMail.Contains(Filtering)
                //               || TableCp.ExceptionRemarks.Contains(Filtering)
                //               //|| TableCp.DelayDay.Contains(Filtering)
                //               || TableCp.Comments.Contains(Filtering);

                //    if (Filtering.IsNumeric())
                //    {
                //        Expression = Expression.Or(TableCp =>  TableCp.ReleaseID.ToString().Contains(Filtering) || TableCp.CPID.ToString().Contains(Filtering));
                //    }
                //    if (Filtering.IsNumeric() || CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.ToList().Exists(m => m.Contains(Filtering)) )
                //    {
                //        Expression = Expression.Or(TableCp =>
                //                         //|| ((SqlFunctions.DateName("mm", TableCp.ExceptionDate).ToString() + " " + SqlFunctions.DateName("dd", TableCp.ExceptionDate) + " ," + SqlFunctions.DateName("yyyy", TableCp.ExceptionDate)).ToString()).Contains(Filtering)
                //                         ((SqlFunctions.DateName("m", TableCp.Publication.PublicationMailDate).Substring(0, 3) + " " + SqlFunctions.DateName("dd", TableCp.Publication.PublicationMailDate) + "," + SqlFunctions.DateName("yyyy", TableCp.Publication.PublicationMailDate).Substring(2, 2)).ToString()).Contains(Filtering)
                //                        || ((SqlFunctions.DateName("m", TableCp.ExceptionDate).Substring(0, 3) + " " + SqlFunctions.DateName("dd", TableCp.ExceptionDate) + "," + SqlFunctions.DateName("yyyy", TableCp.ExceptionDate).Substring(2, 2)).ToString()).Contains(Filtering));
                //    }
                //    if ("true".Contains(Filtering))
                //    {
                //        Expression = Expression.Or(TableCp => (TableCp.ExceptionIndicator??false));

                //    }
                //    if ("false".Contains(Filtering))
                //    {
                //        Expression = Expression.Or(TableCp =>  !(TableCp.ExceptionIndicator ?? false));

                //    }
                //}


                IQueryable<ReleaseCPView> query = SelectReleaseCPView(Expression);
                Expression<Func<ReleaseCPView, bool>> ExpressionFilter = null;

                //Calculate Field Order and Filter
                string[] CalculatedFeild = { "PlannedDate", "DelayDays" };
                if (CalculatedFeild.Contains(Sorting.Split(' ')[0]) || Filtering != "")
                {
                    queryList = query.ToList();
                    query = queryList.AsQueryable();
                    //var sss = query.Where(CPView => CPView.PublicationMailDate == null ? false : ((DateTime)CPView.PublicationMailDate).ToString("MMM d,yyyy hh:mm").Contains(Filtering)).ToList();
                    if (Filtering != "")
                    {
                        ExpressionFilter = (CPView =>
                                  (CPView.CPName == null ? false : CPView.CPName.Contains(Filtering))
                                  || (CPView.ReleaseName == null ? false : CPView.ReleaseName.Contains(Filtering))
                                  || (CPView.AccountName == null ? false : CPView.AccountName.Contains(Filtering))
                                  || (CPView.PublicationMailLink == null ? false : CPView.PublicationMailLink.Contains(Filtering))
                                  || (CPView.PublicationCount.ToString().Contains(Filtering))
                                  || (CPView.SendByName == null ? false : CPView.SendByName.Contains(Filtering))
                                  || (CPView.ExceptionRemarks == null ? false : CPView.ExceptionRemarks.Contains(Filtering))
                                  || (CPView.Comments == null ? false : CPView.Comments.Contains(Filtering))
                                   || (CPView.DelayReason == null ? false : CPView.DelayReason.Contains(Filtering))
                                   || (CPView.DelayReasonHistory == null ? false : CPView.DelayReasonHistory.Contains(Filtering))
                                  );

                        if (Filtering.IsNumeric())
                        {
                            ExpressionFilter = ExpressionFilter.Or(CPView => CPView.ReleaseID.ToString().Contains(Filtering) || CPView.CPID.ToString().Contains(Filtering)
                           || (CPView.DelayDays == null ? false : CPView.DelayDays.Value.ToString().Contains(Filtering))


                            );
                        }
                        if (Filtering.IsNumeric() || Filtering.Split(' ')[0].Length >= 3 && CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.ToList().Exists(m => m.Contains(Filtering.Split(' ')[0].Substring(0, 3))))
                        {
                            ExpressionFilter = ExpressionFilter.Or(CPView =>
                            (CPView.ExceptionDate == null ? false : ((DateTime)CPView.ExceptionDate).ToString("MMM d,yyyy").Contains(Filtering))
                            || (CPView.PublicationMailDate == null ? false : ((DateTime)CPView.PublicationMailDate).ToString("MMM d,yyyy hh:mm").Contains(Filtering))
                            || (CPView.PlannedDate == null ? false : CPView.PlannedDate.Value.ToString("MMM d,yyyy").Contains(Filtering))
                            );
                            // //|| ((SqlFunctions.DateName("mm", TableCp.ExceptionDate).ToString() + " " + SqlFunctions.DateName("dd", TableCp.ExceptionDate) + " ," + SqlFunctions.DateName("yyyy", TableCp.ExceptionDate)).ToString()).Contains(Filtering)
                            // ((SqlFunctions.DateName("m", TableCp.Publication.PublicationMailDate).Substring(0, 3) + " " + SqlFunctions.DateName("dd", TableCp.Publication.PublicationMailDate) + "," + SqlFunctions.DateName("yyyy", TableCp.Publication.PublicationMailDate).Substring(2, 2)).ToString()).Contains(Filtering)
                            //|| ((SqlFunctions.DateName("m", TableCp.ExceptionDate).Substring(0, 3) + " " + SqlFunctions.DateName("dd", TableCp.ExceptionDate) + "," + SqlFunctions.DateName("yyyy", TableCp.ExceptionDate).Substring(2, 2)).ToString()).Contains(Filtering));
                        }
                        if ("true".Contains(Filtering))
                        {
                            ExpressionFilter = ExpressionFilter.Or(CPView => (CPView.ExceptionIndicator == true));

                        }
                        if ("false".Contains(Filtering))
                        {
                            ExpressionFilter = ExpressionFilter.Or(CPView => !(CPView.ExceptionIndicator == true));

                        }

                        query = query.Where(ExpressionFilter);
                    }

                }
                query = query.GetOrderByQuery(Sorting, true);
                query = query.Get(StartIndex, Count, out CountRecords);
                queryList = query.ToList();
            }
            catch (Exception ex)
            {
                CountRecords = 0;
                queryList = null;
                Errors.Write(ex);
            }
            return queryList;
        }
        public ReleaseCP GetReleaseCPId(int ReleseCPID)
        {
            ReleaseCP releaseCP = DbSet.AsNoTracking().Where(r => r.ReleaseCPID == ReleseCPID)
                        .Include(p => p.Publication).FirstOrDefault();
            return releaseCP;
        }
        public ReleaseCP getReleaseCPByCPAndRelease(int releaseId, int cpId)
        {
            return cpId == -1 ? null :
                DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId && r.CPID == cpId)
                    .Include(rcp => rcp.CP)
                    .Include(rcp => rcp.Release).Include(rcp => rcp.Release.Account).FirstOrDefault();
        }
        public ReleaseCPView GetReleaseCPViewById(int ReleseCPID)
        {
            ReleaseCP releaseCP = DbSet.AsNoTracking().Where(r => r.ReleaseCPID == ReleseCPID)
                        //  .Include(p => p.Publication)
                        .Include(p => p.CP)
                         .Include(p => p.Release)
                         .Include(p => p.Release.Account).FirstOrDefault();

            return new ReleaseCPView(releaseCP, releaseCP.Release.Account.Name);
            //{ReleaseName=releaseCP.Release.Name,
            //    ReleaseCPID = releaseCP.ReleaseCPID,
            //    CPID = releaseCP.CPID,
            //    CPName = releaseCP.CP.Name,
            //    // CP = releaseCP.CPID,
            //    //  PublicationID = releaseCP.PublicationID,
            //    ReleaseID = releaseCP.ReleaseID,
            //    //  Release = releaseCP.ReleaseID,
            //    AccountName = releaseCP.Release.Account.Name,
            //    //PlannedDate = TableCp.PlannedDate ,
            //     PublicationMailDate = releaseCP.Publication != null ? releaseCP.Publication.PublicationMailDate : null,
            //    //  PublicationMailLink = releaseCP.Publication.PublicationMail,
            //    ExceptionIndicator = releaseCP.ExceptionIndicator ?? true,
            //    ExceptionDate = releaseCP.ExceptionDate,
            //    ExceptionRemarks = releaseCP.ExceptionRemarks,
            //    DelayReason = releaseCP.DelayReason,
            //    DelayReasonHistory = releaseCP.DelayReasonHistory,
            //   PublicationCount=releaseCP.PublicationCount,//
            //   // DelayDays = TableCp.DelayDay,
            //    Comments = releaseCP.Comments,
            //    IsDeleted = releaseCP.IsDeleted
            // };
        }
        public ReleaseCP getReleseCP(int ReleseCPID)
        {
            return DbSet.AsNoTracking().Where(r => r.ReleaseCPID == ReleseCPID).FirstOrDefault();
        }
        //public ReleaseCP getReleseCPDataForView(int ReleseCPID)
        //{
        //    using (IReleaseCPRepository db = new ReleaseCPRepository())
        //    {
        //        ReleaseCP ReleaseCPUpdated = db.FindSingle(m => m.ReleaseCPID == ReleaseCPView.ReleaseCPID).
        //        return DbSet.AsNoTracking().Where(r => r.ReleaseCPID == ReleseCPID).FirstOrDefault().Include(r=>r.Publication);

        //}
        //public int Edit(ReleaseCPView ReleaseCPView)
        //{
        //    int Count = 0;
        //    using (IReleaseCPRepository db = new ReleaseCPRepository())
        //    {
        //        ReleaseCP ReleaseCPUpdated = db.FindSingle(m => m.ReleaseCPID == ReleaseCPView.ReleaseCPID);
        //        ReleaseCPUpdated.PublicationID = ReleaseCPView.PublicationID == 0 ? null : ReleaseCPView.PublicationID;
        //        ReleaseCPUpdated.ExceptionIndicator = ReleaseCPView.ExceptionIndicator;
        //        ReleaseCPUpdated.ExceptionDate = ReleaseCPView.ExceptionDate;
        //        ReleaseCPUpdated.ExceptionRemarks = ReleaseCPView.ExceptionRemarks;
        //        ReleaseCPUpdated.DelayReason = ReleaseCPView.DelayReason;
        //        ReleaseCPUpdated.DelayReasonHistory = ReleaseCPView.DelayReasonHistory;
        //        ReleaseCPUpdated.Comments = ReleaseCPView.Comments;
        //        ReleaseCPUpdated.IsVisible = ReleaseCPView.IsVisible;
        //        db.Edit(ReleaseCPUpdated);

        //        Count += await db.SaveAsync(User);
        //    }
        //    //if  ReleaseCP edit and PublicationID not null edit PublicationMailDate
        //    //if (Count>0 && (ReleaseCPView.PublicationID ?? 0) > 0 )
        //    //{
        //    //    using (IPublicationRepository db = new PublicationRepository())
        //    //    {
        //    //        Publication Pub = db.Find(ReleaseCPView.PublicationID);
        //    //        Pub.PublicationMailDate = ReleaseCPView.PublicationMailDate;
        //    //        db.Edit(Pub);
        //    //        Count += await db.SaveAsync(User); ;
        //    //    }
        //    //}
        //   return Count;
        //}
        public async Task<int> Edit(ReleaseCPView ReleaseCPView, WindowsPrincipal User)
        {
            //DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId)
            //  .Include(e => e.ReleaseMilestones)
            int Count = 0;
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                try
                {
                    ReleaseCP ReleaseCPUpdated = db.GetReleaseCPId(ReleaseCPView.ReleaseCPID);
                    ReleaseCPUpdated.PublicationID = ReleaseCPView.PublicationID == 0 ? null : ReleaseCPView.PublicationID;
                    if (ReleaseCPUpdated.Publication != null)
                        ReleaseCPUpdated.Publication.PublicationMailDate = ReleaseCPView.PublicationMailDate;
                    ReleaseCPUpdated.ExceptionIndicator = ReleaseCPView.ExceptionIndicator;
                    ReleaseCPUpdated.ExceptionDate = ReleaseCPView.ExceptionDate;
                    ReleaseCPUpdated.ExceptionRemarks = ReleaseCPView.ExceptionRemarks;
                    ReleaseCPUpdated.DelayReason = ReleaseCPView.DelayReason;
                    ReleaseCPUpdated.DelayReasonHistory = ReleaseCPView.DelayReasonHistory;
                    ReleaseCPUpdated.Comments = ReleaseCPView.Comments;
                    ReleaseCPUpdated.IsDeleted = ReleaseCPView.IsDeleted;
                    db.Edit(ReleaseCPUpdated);
                }
                catch (Exception ex) { var y = ex; }
                Count += await db.SaveAsync(User);
            }
            return Count;
        }
        public async Task<int> Add(ReleaseCPView ReleaseCPView, WindowsPrincipal User)
        {
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                ReleaseCP ReleaseCP = new ReleaseCP
                {
                    CPID = ReleaseCPView.CPID,
                    ReleaseID = ReleaseCPView.ReleaseID,
                    ExceptionIndicator = ReleaseCPView.ExceptionIndicator,
                    ExceptionDate = ReleaseCPView.ExceptionDate,
                    ExceptionRemarks = ReleaseCPView.ExceptionRemarks,
                    DelayReason = ReleaseCPView.DelayReason,
                    DelayReasonHistory = ReleaseCPView.DelayReasonHistory,
                    Comments = ReleaseCPView.Comments,
                    IsDeleted = ReleaseCPView.IsDeleted
                };
                db.Add(ReleaseCP);
                return await db.SaveAsync(User);
            }
        }
        public ReleaseCPView GetUpdatedField(int ReleaseID, int CPID)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ReleaseCPView> GetReleaseCPByFiltering(Expression<Func<ReleaseCP, bool>> Expression)
        {
            return SelectReleaseCPView(Expression).ToList();
        }
        public async Task updatePublicationDetails(int ReleaseID, int CPID, string FileName, Employee emp)
        {
            try
            {
                int count = 0;
                var publishCP = DbSet.Where(rcp => rcp.ReleaseID == ReleaseID & rcp.CPID == CPID).FirstOrDefault();

                if (publishCP != null)
                {
                    publishCP.PublicationMailDate = DateTime.Now;
                    publishCP.PublicationMail = FileName;
                    publishCP.SendByID = emp.EmployeeID;
                    publishCP.SendByName = emp.FirstName + " " + emp.LastName;
                    publishCP.PublicationCount++;
                    count += await SaveAsync(null, false, null, null, emp.UserName);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
