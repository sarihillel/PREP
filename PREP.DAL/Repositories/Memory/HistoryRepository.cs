using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Functions;
using System.Security.Principal;
using System.Globalization;
using System.Data.Entity;
using System.Configuration;
//using System.Data.Entity;

namespace PREP.DAL.Repositories.Memory
{
    public class HistoryRepository : GenericRepository<PREPContext, History>, IHistoryRepository
    {
        public IEnumerable<History> GetHistoryReleaseCheckListId(int releaseCheckListId)
        {
            int tableId = StaticResources.GetTableID(ConfigurationManager.AppSettings["ReleaseChecklistAnswer"]);
            List<string> requiredFields = new List<string>()
            {
                "AreaID",
                "SubAreaID",
                "RiskLevelID",
                "QuestionText",
                "ActualComplation",
                "HandlingStartDate",
                "Responsibility",
                "QuestionOwner",
            };
            var query = DbSet.AsNoTracking()
             .Where(a => a.ItemID == releaseCheckListId && a.TableID == tableId && requiredFields.Contains(a.FieldName) && a.ActivityLog.ActivityID == ActivityType.Edit)
              .Include(a => a.ActivityLog)
              .Include(a => a.ActivityLog.Employee);
            return query.ToList();
        }
        //public List<int> getx(int releaseid)
        //{
        //    var query = DbSet.AsNoTracking()
        //        .Include(h => h.ActivityLog)
        //        .Where(h => h.TableID == 26 && h.FieldName == "RiskLevelId" && h.ReleaseID == releaseid && h.ActivityLog.Date == (
        //        DbSet
        //        .Include(x => x.ActivityLog).Take(1).OrderBy(x=>x.ActivityLog.Date).

        //        )).FirstOrDefault();
        //}
        public History GetLastActivityLogUpdate(int? ReleaseID = null, ActivityType? activityType = null)
        {
            var Query = DbSet
                              .Include(e => e.ActivityLog)
                              .Include(a => a.ActivityLog.Employee)
                            .Include(a => a.Table);

            if (activityType != null && activityType.Value == ActivityType.Create)
            {
                if (ReleaseID != null)
                    Query = Query.Where(a =>
                       a.Table.Name == typeof(Release).Name && a.ReleaseID == ReleaseID && a.ActivityLog.ActivityID == activityType.Value);
                Query = Query.OrderBy(a => a.ActivityLog.Date);
            }
            else {
                if (ReleaseID != null)
                    Query = Query.Where(a =>
                       (a.Table.Name == typeof(Release).Name ||
                       a.Table.Name == typeof(ReleaseMilestone).Name ||
                       a.Table.Name == typeof(ReleaseCPReviewModeQ).Name ||
                       a.Table.Name == typeof(ReleaseCPReviewMode).Name ||
                       a.Table.Name == typeof(ReleaseAreaOwner).Name ||
                       a.Table.Name == typeof(ReleaseFamilyProduct).Name ||
                       a.Table.Name == typeof(ReleaseStakeholder).Name ||
                       a.Table.Name == typeof(ReleaseProduct).Name ||
                           a.Table.Name == typeof(ReleaseCharacteristic).Name)
                         && a.ReleaseID == ReleaseID);
                if (activityType != null)
                    Query = Query.Where(a => a.ActivityLog.ActivityID == activityType.Value);
                Query = Query.OrderByDescending(a => a.ActivityLog.Date);
            }
            return Query.FirstOrDefault();
        }

        private List<HistoryView> SelectHistoryView(Expression<Func<History, bool>> Expression)
        {
            int accountId, releaseCpId;
            using (ITableRepository db = new TableRepository())
            {
                accountId = db.GetTableIdByTableName("Account");
                releaseCpId = db.GetTableIdByTableName("ReleaseCP");
            }
            var Query = DbSet.AsNoTracking()
            .Include(a => a.ActivityLog)
            .Include(a => a.Release)
            .Include(a => a.Release.Account)
            .Include(a => a.ActivityLog.Employee)
            .Include(a => a.Release.ReleaseCPs.Select(s => s.CP))
            .Include(a => a.Table);
           // .Include(a=>a.Release.ReleaseMilestones.Select(s=>s.Milestone))
           // .Include(a => a.Release.ReleaseProducts.Select(s => s.Product))
           // .Include(a => a.Release.ReleaseFamilyProducts.Select(s => s.FamilyProduct))
           // .Include(a => a.Release.ReleaseStakeholders.Select(s => s.Stakeholder))
           // .Include(a => a.Release.ReleaseAreaOwners.Select(s => s.Area));


            if (Expression != null)
                Query = Query.Where(Expression);

            List<HistoryView> HistoryView = new List<TableViews.HistoryView>();
            foreach (var item in Query)
            {
                HistoryView historyView = new HistoryView();
                historyView.ReleaseId = item.TableID != accountId && item.Release != null ? (int?)item.Release.ReleaseID : null;
                historyView.ReleaseName = item.TableID != accountId && item.Release != null ? item.Release.Name : null;
                historyView.AccountId = item.Release != null ? (int?)item.Release.AccountID : null;
                historyView.AccountName = item.Release != null && item.Release.Account != null ? item.Release.Account.Name : "";
                historyView.Id = item.ActivityLogID;
                historyView.CpId = item.TableID == releaseCpId && item.Release != null && /*TableHistory.Release.ReleaseCPs!=null&&*/ item.Release.ReleaseCPs.FirstOrDefault(a => a.ReleaseCPID == item.ItemID) != null ? (int?)item.Release.ReleaseCPs.FirstOrDefault(a => a.ReleaseCPID == item.ItemID).CPID : null;//only if exists
                historyView.CpName = item.TableID == releaseCpId && item.Release != null/*&& TableHistory.Release.ReleaseCPs != null */&& item.Release.ReleaseCPs.FirstOrDefault(a => a.ReleaseCPID == item.ItemID) != null && item.Release.ReleaseCPs.FirstOrDefault(a => a.ReleaseCPID == item.ItemID).CP != null ? item.Release.ReleaseCPs.FirstOrDefault(a => a.ReleaseCPID == item.ItemID).CP.Name : "";
                if (item.FieldName == "AmdocsFocalPoint2ID" || item.FieldName == "AmdocsFocalPoint1ID" || item.FieldName == "EmployeeID1" || item.FieldName == "EmployeeID2")
                {
                    using (IEmployeeRepository db = new EmployeeRepository())
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(item.OldValue))
                            {
                                string name = db.GetFullNameByEmpId(Convert.ToInt32(item.OldValue));
                                historyView.OldValue = name != null ? name : "";
                            }
                            else
                            {
                                historyView.OldValue = "";
                            }
                            if (!string.IsNullOrEmpty(item.NewValue))
                            {
                                string name = db.GetFullNameByEmpId(Convert.ToInt32(item.NewValue));
                                historyView.NewValue = name != null ? name : "";
                            }
                            else
                            {
                                historyView.NewValue = "";
                            }
                        }
                        catch
                        {
                            historyView.NewValue = "";
                        }
                    }
                }
                else
                {
                    historyView.OldValue = item != null ? item.OldValue : "";
                    historyView.NewValue = item != null ? item.NewValue : "";
                }
                historyView.ModifiedById = item.ActivityLog != null && item.ActivityLog.Employee != null ? (int?)item.ActivityLog.Employee.MDMCode : null;
                historyView.ModifiedByName = item.ActivityLog != null && item.ActivityLog.Employee != null ? item.ActivityLog.Employee.FirstName + " " + item.ActivityLog.Employee.LastName : "";
                historyView.ModifiedDate = item.ActivityLog != null && item.ActivityLog.Date != null ? item.ActivityLog.Date.ToString() : "";
                historyView.TableName = item.Table != null ? item.Table.Name : "";
                if (item.Table.ParameterRelashionID != 0)//
                {
                    string fieldName = null;

                    string tableName = StaticResources.GetTableName(item.Table.ParameterRelashionID);
                    fieldName = GetParameterTableName(tableName, item.ItemID);
                    historyView.FieldName = fieldName;
                }
                else
                {
                    historyView.FieldName = item != null ? item.FieldName : "";
                }
                HistoryView.Add(historyView);
            }
            return HistoryView.ToList();
        }

        public string GetParameterTableName(string tableName, int ReleaseParameterID)
        {
            if (tableName == "Milestone")
            {
                using (IReleaseMilestoneRepository dbr = new ReleaseMilestoneRepository())
                {
                    var ReleaseMilestone = dbr.Where(a => a.ReleaseMilestoneID == ReleaseParameterID).Include(a => a.Milestone).FirstOrDefault();
                    return ReleaseMilestone != null && ReleaseMilestone.Milestone != null ? ReleaseMilestone.Milestone.Name : "";
                }
            }
            else if (tableName == "Product")
            {
                using (IReleaseProductRepository dbr = new ReleaseProductRepository())
                {
                    var ReleaseProduct = dbr.Where(a => a.ReleaseProductID == ReleaseParameterID).Include(a => a.Product).FirstOrDefault();
                    return ReleaseProduct != null && ReleaseProduct.Product != null ? ReleaseProduct.Product.Name : "";
                }
            }
            else if (tableName == "FamilyProduct")
            {
                using (IReleaseFamilyProductRepository dbr = new ReleaseFamilyProductRepository())
                {
                    var ReleaseFamilyProduct = dbr.Where(a => a.ReleaseFamilyProductID == ReleaseParameterID).Include(a => a.FamilyProduct).FirstOrDefault();
                    return ReleaseFamilyProduct != null && ReleaseFamilyProduct.FamilyProduct != null ? ReleaseFamilyProduct.FamilyProduct.Name : "";
                }
            }
            else if (tableName == "Stakeholder")
            {
                using (IReleaseStakeholderRepository dbr = new ReleaseStakeholderRepository())
                {
                    var ReleaseStakeholder = dbr.Where(a => a.ReleaseStakeholderID == ReleaseParameterID).Include(a => a.Stakeholder).FirstOrDefault();
                    return ReleaseStakeholder != null && ReleaseStakeholder.Stakeholder != null ? ReleaseStakeholder.Stakeholder.Name : "";
                }
            }
            else if (tableName == "Area")
            {
                using (IReleaseAreaOwnerRepository dbr = new ReleaseAreaOwnerRepository())
                {
                    var ReleaseAreaOwner = dbr.Where(a => a.ReleaseAreaOwnerID == ReleaseParameterID).Include(a => a.Area).FirstOrDefault();
                    return ReleaseAreaOwner != null && ReleaseAreaOwner.Area != null ? ReleaseAreaOwner.Area.Name : "";
                }
            }
            return null;
        }

        public List<HistoryView> GetList(int? ReleaseID, DateTime? StartDate, DateTime? EndDate, int TableId, int ModifiedById)
        {
            int releaseMilestoneTableId, releaseAreaOwnersTableId, releaseProductTableId, releaseFamilyProductTableId, releaseStakeHolderTableId, accountTableId, releaseTableId, releaseCpTableId;
            using (ITableRepository db = new TableRepository())
            {
                releaseMilestoneTableId = db.GetTableIdByTableName("ReleaseMilestone");
                releaseAreaOwnersTableId = db.GetTableIdByTableName("ReleaseAreaOwner");
                releaseProductTableId = db.GetTableIdByTableName("ReleaseProduct");
                releaseFamilyProductTableId = db.GetTableIdByTableName("ReleaseFamilyProduct");
                releaseStakeHolderTableId = db.GetTableIdByTableName("ReleaseStakeholder");
                accountTableId = db.GetTableIdByTableName("Account");
                releaseTableId = db.GetTableIdByTableName("Release");
                releaseCpTableId = db.GetTableIdByTableName("ReleaseCP");
            }
            Expression<Func<History, bool>> Expression;
            using (ITableRepository db = new TableRepository())
            {
                EndDate = EndDate.Value.AddDays(1);
                //TODO check if activitylog exists
                Expression = c => c.ActivityLog.Date >= StartDate;
                Expression = Expression.And(c => c.ActivityLog.Date < EndDate);
                if (ModifiedById != 0)
                    Expression = Expression.And(c => c.ActivityLog.EmployeeID == ModifiedById);
                if (TableId == releaseTableId)
                    Expression = Expression.And(c =>
                    c.TableID == releaseTableId ||
                    c.TableID == releaseMilestoneTableId ||
                    c.TableID == releaseAreaOwnersTableId ||
                    c.TableID == releaseProductTableId ||
                    c.TableID == releaseFamilyProductTableId ||
                    c.TableID == releaseStakeHolderTableId
                    );
                else if (TableId != 0)//one table
                    Expression = Expression.And(c => c.TableID == TableId);
                else//all tables
                {
                    Expression = Expression.And(c =>
                    c.TableID == accountTableId ||
                    c.TableID == releaseTableId ||
                    c.TableID == releaseCpTableId ||
                    c.TableID == releaseMilestoneTableId ||
                    c.TableID == releaseAreaOwnersTableId ||
                    c.TableID == releaseProductTableId ||
                    c.TableID == releaseFamilyProductTableId ||
                    c.TableID == releaseStakeHolderTableId
                    );
                }
                if (ReleaseID != 0)//not all releases
                    Expression = Expression.And(c => c.ReleaseID == ReleaseID);
            }
            List<HistoryView> historyView = SelectHistoryView(Expression).ToList();
            return historyView;
        }

    }
}
