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
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;
using System.Configuration;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseRepository : GenericRepository<PREPContext, Release>, IReleaseRepository
    {
        #region Public Methods
      

        public async Task<int> EditReleaseChecklistAnswer(Release newRelease, WindowsPrincipal user)
        {
            int count = 0;
            try
            {
                var dbRelease = DbSet.Where(r => r.ReleaseID == newRelease.ReleaseID)
                                                   .Include(e => e.ReleaseChecklistAnswers).FirstOrDefault();
                if (dbRelease.ReleaseChecklistAnswers.Count > 0)
                {
                    List<ReleaseChecklistAnswerArcive> arciveQuestion = dbRelease.ReleaseChecklistAnswers.ToList().Select(s =>
                        new ReleaseChecklistAnswerArcive()
                        {
                            ReleaseID = s.ReleaseID,
                            QuestionID = s.QuestionID,
                            QuestionText = s.QuestionText,
                            SubAreaID = s.SubAreaID,
                            QuestionOwnerID = s.QuestionOwnerID,
                            ActualComplation = s.ActualComplation,
                            AsPlannedCounter = s.AsPlannedCounter,
                            Comments = s.Comments,
                            ExtrenalFocalPoint = s.ExtrenalFocalPoint,
                            HandlingStartDate = s.HandlingStartDate,
                            IsEdited = s.IsEdited,
                            LastAutomaticUpdateDate = s.LastAutomaticUpdateDate,
                            Log = s.Log,
                            QuestionInfo = s.QuestionInfo,
                            Responsibility = s.Responsibility,
                            RiskLevelID = s.RiskLevelID,
                            AreaID = s.AreaID
                        }).ToList();

                    //using (PREPContext db = new PREPContext())
                    //{
                    //    using (var transactionScope = new TransactionScope())
                    //    {
                    //        db.BulkInsert(arciveQuestion);
                    //        count += await db.SaveAsync(user);
                    //        transactionScope.Complete();
                    //    }
                    //}
                    Context.BulkInsert(arciveQuestion);
                    dbRelease.ReleaseChecklistAnswers.Clear();
                    count += await this.SaveAsync(user);
                }

                List<ReleaseChecklistAnswer> tempReleaseChecklistAnswer = new List<ReleaseChecklistAnswer>();
                foreach (ReleaseChecklistAnswer newAnswer in newRelease.ReleaseChecklistAnswers)
                {
                    var dbObject = dbRelease.ReleaseChecklistAnswers.SingleOrDefault(s => s.ReleaseChecklistAnswerID == newAnswer.ReleaseChecklistAnswerID && newAnswer.ReleaseChecklistAnswerID != 0);
                    if (dbObject != null)
                        Context.Entry<ReleaseChecklistAnswer>(dbObject).CurrentValues.SetValues(newAnswer);
                    else
                       // tempReleaseChecklistAnswer.Add(newAnswer);
                        dbRelease.ReleaseChecklistAnswers.Add(newAnswer);
                    
                }
              //  dbRelease.ReleaseChecklistAnswers.add(tempReleaseChecklistAnswer);
                count += await this.SaveAsync(user);
                //using (PREPContext db = new PREPContext())
                //{
                //    using (var transactionScope = new TransactionScope())
                //    {
                //        db.BulkInsert(tempReleaseChecklistAnswer);
                //        count += await db.SaveAsync(user);
                //        transactionScope.Complete();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                count = -1;

            }
            return count;
        }

        public IEnumerable<ReleaseView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {

            IEnumerable<ReleaseView> ListReleases = null;
            CountRecords = 0;
            try
            {
                Expression<Func<ReleaseView, bool>> Expression = null;
                if (!string.IsNullOrEmpty(Filtering))
                    Expression = (c => c.ReleaseName.Contains(Filtering)
                    || c.AccountName.Contains(Filtering)
                   || SqlFunctions.StringConvert((double)c.ReleaseID).Contains(Filtering)
                    || c.PrepFPName.Contains(Filtering)
                    || c.PrepReviewMode.Contains(Filtering)
                    || c.ProductionStartDate.Contains(Filtering)
                    || c.ProgramMeEmployee.Contains(Filtering)
                    || c.SPNameEmployee.Contains(Filtering));
                var query = SelectReleaseIDBySearchView(Expression);

                query = query.GetOrderByQuery(Sorting);
                string sqlstring = query.ToString();
                query = query.Get(StartIndex, Count, out CountRecords);
                ListReleases = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListReleases;

        }
        public Release GetSingle(int? ReleaseID)
        {
            return DbSet.FirstOrDefault(prop => prop.ReleaseID == ReleaseID);
        }
        /// <summary>
        /// get all release and relationships to release tab
        /// </summary>
        /// <param name="releaseId"></param>
        /// <returns></returns>
        /// 
        public async Task<int> AddRelease(Release NewRelease, WindowsPrincipal user)
        {
            int Count = 0;
            try
            {
                DbSet.Add(NewRelease);
                Count += await this.SaveAsync(user);

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                Count = -1;
            }
            return Count;
        }

        public Release GetReleseAndRelationships(int releaseId)
        {
            Release releaseDetails = null;
            try
            {
                var query = DbSet.AsNoTracking()
                            .Where(r => r.ReleaseID == releaseId)
                            .Include(t => t.ReleaseFamilyProducts)
                            .Include(e => e.ReleaseFamilyProducts.Select(f => f.FamilyProduct))
                            .Include(e => e.ReleaseProducts.Select(p => p.Product))
                            .Include(e => e.ReleaseCharacteristics)
                            .Include(e => e.ReleaseCharacteristics.Select(c => c.Characteristic))
                            .Include(e => e.ReleaseCPs)
                            .Include(e => e.ReleaseCPs.Select(rcp => rcp.CP))
                            .Include(e => e.ReleaseChecklistAnswers)
                            .Include(e => e.ReleaseMilestones.Select(rm => rm.Milestone))
                            .Include(e => e.Account)
                            .Include(e => e.ReleaseStakeholders.Select(rs => rs.Stakeholder))
                            .Include(e => e.ReleaseStakeholders.Select(rs => rs.Employee1))
                            .Include(e => e.ReleaseStakeholders.Select(rs => rs.Employee2))
                            .Include(e => e.ReleaseAreaOwners)
                            .Include(e => e.ReleaseAreaOwners.Select(ra => ra.Area))
                            .Include(e => e.ReleaseAreaOwners.Select(ra => ra.AmdocsFocalPoint1))
                            .Include(e => e.ReleaseAreaOwners.Select(ra => ra.AmdocsFocalPoint2))
                            .Include(e => e.ReleaseCPReviewModeQs)
                            .Include(e => e.ReleaseChecklistAnswers)
                            .Include(e => e.ReleaseVendors)
                            .Include(e => e.ReleaseVendors.Select(v => v.Vendor.VendorAreass));

                 releaseDetails = query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return releaseDetails;
        }

        public Release GetReleaseCPData(int releaseId)
        {
            return DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId)
            .Include(e => e.Account)
            .Include(e => e.ReleaseMilestones.Select(x => x.Milestone))
            .Include(e => e.ReleaseCPs)
            .Include(e => e.ReleaseCPs.Select(x => x.CP)).FirstOrDefault();
        }
        public Release GetReleseForStatus(int releaseId)
        {
            return DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId)
            .Include(e => e.Account)
            .FirstOrDefault();
        }
        public Release GetReleaseCPDataAndMilstones(int releaseId)
        {
            return DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId)
            .Include(e => e.Account)
            .Include(e => e.ReleaseMilestones)
            .Include(e => e.ReleaseMilestones.Select(x => x.Milestone))
            .Include(e => e.ReleaseCPs)
            .Include(e => e.ReleaseCPs.Select(x => x.CP)).FirstOrDefault();
        }


        /// <summary>
        /// add new  release and relationships to release tab
        /// Note: in relationships who's added: releaseid is 0, after add release in database remember update releaseid 
        /// </summary>
        /// <param name="releaseId"></param>
        /// <returns></returns>
        public Release GetNewReleseAndRelationships()
        {
            Release Release = new Release { };

            using (IReleaseMilestoneRepository db = new ReleaseMilestoneRepository())
            {
                Release.ReleaseMilestones = db.AddReleaseMilestone();
            }
            using (IReleaseFamilyProductRepository db = new ReleaseFamilyProductRepository())
            {
                Release.ReleaseFamilyProducts = db.AddFamilyProduct();
            }
            using (IReleaseProductRepository db = new ReleaseProductRepository())
            {
                Release.ReleaseProducts = db.AddProduct();
            }
            using (IReleaseStakeholderRepository db = new ReleaseStakeholderRepository())
            {
                Release.ReleaseStakeholders = db.AddStakeHolder();
            }
            using (IReleaseAreaOwnerRepository db = new ReleaseAreaOwnerRepository())
            {
                Release.ReleaseAreaOwners = db.AddAreaOwner();
            }
            using (IReleaseCharacteristicRepository db = new ReleaseCharacteristicRepository())
            {
                Release.ReleaseCharacteristics = db.AddCharacteristic();
            }
            return Release;
        }



        /// <summary>
        /// Load all the Products and indicates his related Products
        /// </summary>
        /// <param name="Release"></param>
        /// <returns></returns>
        public List<object> SetAllReleaseProducts(int ReleaseID)
        {
            List<ReleaseFamilyProduct> AllFamilyProducts = new List<ReleaseFamilyProduct>();
            List<ReleaseProduct> AllProducts = new List<ReleaseProduct>();
            foreach (var f in new FamilyProductRepository().GetAllFamilyProducts().ToList())
            {
                bool IsExist = f.ReleaseFamilyProducts.Any(rf => rf.ReleaseID == ReleaseID);
                ReleaseFamilyProduct newfamily = new ReleaseFamilyProduct()
                {
                    FamilyProductID = f.FamilyProductID,
                    ReleaseID = ReleaseID,
                    ReleaseFamilyProductID = IsExist ? f.ReleaseFamilyProducts.Where(rf => rf.ReleaseID == ReleaseID).First().ReleaseFamilyProductID : 0,
                    IsChecked = IsExist ? f.ReleaseFamilyProducts.Where(rf => rf.ReleaseID == ReleaseID).First().IsChecked : false,
                    FamilyProduct = f

                };

                List<ReleaseProduct> newproduct = newfamily.FamilyProduct.Products.Select(p =>
                         new ReleaseProduct
                         {
                             ProductID = p.ProductID,
                             ReleaseID = ReleaseID,
                             ReleaseProductID = p.ReleaseProducts.Any(rp => rp.ReleaseID == ReleaseID) ? p.ReleaseProducts.Where(rp => rp.ReleaseID == ReleaseID).First().ReleaseProductID : 0,
                             IsChecked = p.ReleaseProducts.Any(rp => rp.ReleaseID == ReleaseID) ? p.ReleaseProducts.Where(rp => rp.ReleaseID == ReleaseID).First().IsChecked : false,
                             Product = p,
                             Name = p.Name
                         }).ToList();
                AllFamilyProducts.Add(newfamily);
                AllProducts.AddRange(newproduct);

            }

            List<object> ReleaseProduct = new List<object>();
            ReleaseProduct.Add(AllFamilyProducts);
            ReleaseProduct.Add(AllProducts);
            return ReleaseProduct;
        }
        public IEnumerable<ReleaseView> GetReleaseJoinAccount()
        {
            var Query = DbSet.AsNoTracking()
                .Include(r => r.Account)
                    .Include(r => r.ReleaseStakeholders)
                    .Include(r => r.ReleaseStakeholders.Select(rsh => rsh.Stakeholder))
                    .Include(r => r.ReleaseStakeholders.Select(rsh => rsh.Employee1))
                    .Include(r => r.ReleaseMilestones).OrderBy(r => r.ReleaseID)
                    .Select(r => new ReleaseView()
                    {
                        ReleaseName = r.Name,
                        AccountName = r.Account != null ? r.Account.Name : String.Empty,
                        ReleaseID = r.ReleaseID,
                        PrepFPName = r.Account != null ? r.Account.PrepFPName : String.Empty,
                        PrepReviewMode = "Full",

                        //  PREP.DAL.Models.Employee SPNameEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 : null;
                        SPNameEmployee =
                            r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) &&
                            r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 != null ?
                            r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1.FirstName + " " + r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1.LastName : String.Empty,
                                    ProgramMeEmployee =
                            r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 7) &&
                            r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1 != null ?
                            r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1.FirstName + " " + r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1.LastName : String.Empty,
                        ProductionStartDate = r.ReleaseMilestones.Any(m => m.MilestoneID == 12) ?
                       (SqlFunctions.StringConvert((double)SqlFunctions.DatePart("MM", r.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault().MilestoneDate)).Trim() + "-" + SqlFunctions.DateName("dd", r.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault().MilestoneDate).Trim() + "-" + SqlFunctions.DateName("yyyy", r.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault().MilestoneDate).Trim())
                   : String.Empty

                    }

                    );
            return Query;
        }


        private Boolean createExpressionFilterForRelease(String text, Release releas)
        {
            //if (releas == null)
            //    return false;
            var employeeTemp = releas.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault();
            var employee = releas.ReleaseStakeholders != null ? employeeTemp != null ? employeeTemp.Employee1 : null : null;
            var programMeEmployeeTemp = releas.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault();
            var programMeEmployee = releas.ReleaseStakeholders != null ? programMeEmployeeTemp != null ? programMeEmployeeTemp.Employee1 : null : null;
            var ProductionStartDate = releas.ReleaseMilestones != null ? releas.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault() : null;


            return (releas != null
                && releas.Name.Contains(text)
                || releas.Account != null && releas.Account.Name != null && releas.Account.Name.Contains(text) || SqlFunctions.StringConvert((double)releas.ReleaseID).Contains(text) || releas.Account.PrepFPName.Contains(text))
                || (employee != null && (employee.FirstName ?? "" + " " + employee.LastName ?? "").Contains(text) ||
                (programMeEmployee != null && (programMeEmployee.FirstName ?? "" + " " + programMeEmployee.LastName ?? "").Contains(text)));
        }

        private object getReleaseOrderByExpression(string field, Release release)
        {
            object ExpressionOrderBy = release.ReleaseID;
            switch (field)
            {
                case "ReleaseName":
                    ExpressionOrderBy = release.Name;
                    break;
                case "AccountName":
                    ExpressionOrderBy = release.Account.Name;
                    break;
                case "PrepFPName":
                    ExpressionOrderBy = release.Account.PrepFPName;
                    break;
                case "SPNameEmployee":
                    var employeeTemp = release.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault();
                    var employee = release.ReleaseStakeholders != null ? employeeTemp != null ? employeeTemp.Employee1 : null : null;
                    ExpressionOrderBy = employee != null ? employee.FirstName + " " + employee.LastName : null;
                    break;
                case "ProgramMeEmployee":
                    var programMeEmployeeTemp = release.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault();
                    var programMeEmployee = release.ReleaseStakeholders != null ? programMeEmployeeTemp != null ? programMeEmployeeTemp.Employee1 : null : null;
                    ExpressionOrderBy = programMeEmployee != null ? programMeEmployee.FirstName + " " + programMeEmployee.LastName : null;
                    break;
                case "ProductionStartDate":
                    var ProductionStartDate = release.ReleaseMilestones != null ? release.ReleaseMilestones.Where(m => m.MilestoneID == 12).FirstOrDefault() : null;
                    ExpressionOrderBy = ProductionStartDate != null ? ProductionStartDate.MilestoneDate : null;
                    break;
            }
            return ExpressionOrderBy;
        }


        public Expression<Func<ReleaseView, bool>> getSearchReleaseCondition(string text)
        {
            return (r => r.AccountName.Contains(text) || r.ReleaseName.Contains(text) || SqlFunctions.StringConvert((double)r.ReleaseID).Contains(text) || r.PrepFPName.Contains(text) || r.SPNameEmployee.Contains(text)
           || r.ProgramMeEmployee.Contains(text) || r.ProductionStartDate.Contains(text) || r.PrepReviewMode.Contains(text));
        }

        private IQueryable<ReleaseView> SelectReleaseIDBySearchView(Expression<Func<ReleaseView, bool>> Expression = null)
        {
            IQueryable<ReleaseView> Query;

            Query = DbSet.AsNoTracking()
                .Include(r => r.Account)
                    .Include(r => r.ReleaseStakeholders)
                    .Include(r => r.ReleaseStakeholders.Select(rsh => rsh.Stakeholder))
                    .Include(r => r.ReleaseStakeholders.Select(rsh => rsh.Employee1))
                    .Include(r => r.ReleaseMilestones.Select(rm => rm.Milestone))
                    .Select(r => new ReleaseView()
                    {
                        ReleaseName = r.Name,
                        AccountName = r.Account != null ? r.Account.Name : String.Empty,
                        ReleaseID = r.ReleaseID,
                        PrepFPName = r.Account != null ? r.Account.PrepFPName : String.Empty,
                        PrepReviewMode = "Full",

                        //  PREP.DAL.Models.Employee SPNameEmployeeTmp = r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) ? r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 : null;
                        SPNameEmployee =
                r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 6) &&
                r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1 != null ?
                r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1.FirstName + " " + r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 6).FirstOrDefault().Employee1.LastName : String.Empty,
                        ProgramMeEmployee =
                r.ReleaseStakeholders.Any(rsh => rsh.StakeholderID == 7) &&
                r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1 != null ?
                r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1.FirstName + " " + r.ReleaseStakeholders.Where(rsh => rsh.StakeholderID == 7).FirstOrDefault().Employee1.LastName : String.Empty,
                        ProductionStartDate = r.ReleaseMilestones.Any(m => m.Milestone.Name == "Production") ?
                       (SqlFunctions.DateName("dd", r.ReleaseMilestones.Where(m => m.Milestone.Name == "Production").FirstOrDefault().MilestoneDate)) + "-" + SqlFunctions.DateName("mm", r.ReleaseMilestones.Where(m => m.Milestone.Name == "Production").FirstOrDefault().MilestoneDate).Substring(0, 3) + "-" + SqlFunctions.DateName("yyyy", r.ReleaseMilestones.Where(m => m.Milestone.Name == "Production").FirstOrDefault().MilestoneDate)
                   // ((SqlFunctions.DateName("mm", TableCp.EffectiveDate).Substring(0, 3).ToString() + " " + SqlFunctions.DateName("dd", TableCp.EffectiveDate) + " ," + SqlFunctions.DateName("yyyy", TableCp.EffectiveDate)).ToString())
                   : String.Empty

                    }

                    );
            if (Expression != null)
                Query = Query.Where(Expression);
            return Query;

        }
   
        private IQueryable<ReleseNameView> SelectReleaseNameView(Expression<Func<ReleseNameView, bool>> Expression = null)
        {
            IQueryable<ReleseNameView> Query;

            Query = DbSet
                    .Select(Tbl => new ReleseNameView()
                    {
                        Name = Tbl.Name
                    }
                    );
            Query = Query.Union(DbSet
                   .Select(Tbl => new ReleseNameView()
                   {
                       Name = SqlFunctions.StringConvert((double)Tbl.ReleaseID)
                   }
                   ));
            Query = Query.Union(DbSet
                   .Select(Tbl => new ReleseNameView()
                   {
                       Name = Tbl.Account.PrepFPName
                   }
                   ));

            Query = Query.Union(DbSet
                    .Include(r => r.Account)
                   .Select(Tbl => new ReleseNameView()
                   {
                       Name = Tbl.Account.Name
                   }
                   ));

            Query = Query.Union(
                        Context.Set<ReleaseStakeholder>()
                        .Include(rsh => rsh.Employee1)
                        .Where(r => r.Stakeholder.StakeholderID == 6 || r.Stakeholder.StakeholderID == 7)
                         .Select(Tbl => new ReleseNameView()
                         {
                             Name = (Tbl.Employee1.FirstName ?? "" + " " + Tbl.Employee1.LastName ?? "")
                         }
                   ));
            Query = Query.Union(
                        Context.Set<ReleaseMilestone>()
                        .Include(Tbl => Tbl.Milestone)
                        .Where(m => m.MilestoneID == 12)
                         .Select(Tbl => new ReleseNameView()
                         {
                             //   Name = Tbl.MilestoneDate == null ? "" : Tbl.MilestoneDate.ToString()
                             Name = Tbl.MilestoneDate == null ? "" : (SqlFunctions.StringConvert((double)SqlFunctions.DatePart("MM", Tbl.MilestoneDate)).Trim() + "/" + SqlFunctions.DateName("dd", Tbl.MilestoneDate).Trim() + "/" + SqlFunctions.DateName("yyyy", Tbl.MilestoneDate).Trim())
                         }
                   ));
            if (Expression != null)
                Query = Query.Where(Expression);
            return Query;

        }

        public IEnumerable<ReleseNameView> GetReleaseJoinAccountBySearch(String text)
        {
            IEnumerable<ReleseNameView> ReleseNameView = null;
            try
            {
                Int32 Count = 15;
                text = text.Trim();
                var query = SelectReleaseNameView(t => t.Name.Contains(text));
                query.OrderBy(t => t.Name);
                query = query.Take(Count);
                ReleseNameView = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return ReleseNameView;

        }

        public IEnumerable<object> GetReleaseStakeHolders(int ReleaseID)

        {
            var query = from r in DbSet
                        where (r.ReleaseID == ReleaseID)
                        join rs in Context.ReleaseStakeHolders on r.ReleaseID equals rs.ReleaseID into rsu
                        from m in rsu.DefaultIfEmpty()
                        join s in Context.Stakeholder on m.StakeholderID equals s.StakeholderID
                        into u
                        from um in u.DefaultIfEmpty()
                        join e in Context.Employee on m.EmployeeID1 equals e.EmployeeID
                       into e2
                        from e3 in e2.DefaultIfEmpty()
                        select new
                        {
                            StakeHolder = um.Name,
                        };
            return query.ToList();
        }
        public IEnumerable<object> GetReleaseStakeHoldersEmployee(int ReleaseID, int StakHolderID)

        {
            var query = from r in DbSet
                        where (r.ReleaseID == ReleaseID)
                        join rs in Context.ReleaseStakeHolders on r.ReleaseID equals rs.ReleaseID into rsu
                        from m in rsu.DefaultIfEmpty()
                        join s in Context.Stakeholder on m.StakeholderID equals s.StakeholderID
                        into u
                        where (m.StakeholderID == StakHolderID)
                        join e in Context.Employee on m.EmployeeID1 equals e.EmployeeID
                        into e2
                        from e3 in e2.DefaultIfEmpty()
                        select ((e3.FirstName ?? "") + " " + (e3.LastName ?? "")).Trim();
            return query.ToList();
        }

        public object GetReleaseProductionStartDateMS(int ReleaseID)
        {
            var query = from r in DbSet.Where(r => r.ReleaseID == ReleaseID)
                        join p in Context.ReleaseMilestones on r.ReleaseID equals p.ReleaseID into rm
                        from x in rm.DefaultIfEmpty()
                        join m in Context.Milestones on x.MilestoneID equals m.MilestoneID into mil
                        where (x.MilestoneID == 12)
                        from mn in mil.DefaultIfEmpty()
                        select x.MilestoneDate;



            return query.FirstOrDefault();
        }

        public List<Options> GetReleaseOptions()
        {
            List<Options> Options;
            using (IReleaseRepository db = new ReleaseRepository())
            {
                Options = db.GetOptionsNoAsync(o => new Options() { DisplayText = o.Name, Value = o.ReleaseID });
            }
            return Options;
        }
        public Release getReleaseForInitiateCheckList(int releaseId)
        {
            return DbSet.AsNoTracking().Where(r => r.ReleaseID == releaseId)
                .Include(r => r.ReleaseAreaOwners)
.Include(r => r.ReleaseProducts)
.Include(r => r.ReleaseFamilyProducts)
.Include(r => r.ReleaseCharacteristics)
.Include(r => r.ReleaseAreaOwners)
.Include(r => r.ReleaseStakeholders)
.Include(r => r.ReleaseMilestones)
.Include(r => r.ReleaseChecklistAnswers).FirstOrDefault();
        }

        /// <summary>
        /// Get mail adresses of  ReleaseArea and ReleaseStakeholders employees
        /// </summary>
        /// <param name="ReleaseID"></param>
        /// <returns></returns>
        public IEnumerable<string> GetEmployeesMailAddress(int ReleaseID, bool isDraft)
        {
            using (IReleaseRepository db = new ReleaseRepository())
            {
                List<string> MailAddresses = new List<string>();
                var CurrentRelease = db.Where(r => r.ReleaseID == ReleaseID)
                                            .Include(ra => ra.ReleaseAreaOwners.Select(a => a.AmdocsFocalPoint1))
                                            .Include(rs => rs.ReleaseStakeholders.Select(s => s.Stakeholder))
                                            .Include(rs => rs.ReleaseStakeholders.Select(s => s.Employee1))
                                            .Include(rs => rs.ReleaseStakeholders.Select(s => s.Employee2)).First();

                MailAddresses.AddRange(CurrentRelease.ReleaseAreaOwners.Where(a => a.AmdocsFocalPoint1 != null).Select(a => a.AmdocsFocalPoint1.Email).ToList());
                if (!isDraft)
                {
                    MailAddresses.AddRange(CurrentRelease.ReleaseStakeholders.Where(a => a.Employee1 != null).Select(a => a.Employee1.Email).ToList()
                 );
                    MailAddresses.AddRange(CurrentRelease.ReleaseStakeholders.Where(a => a.Employee2 != null).Select(a => a.Employee2.Email).ToList());
                }
                else
                {
                    MailAddresses.AddRange(CurrentRelease.ReleaseStakeholders.Where(a => (a.Stakeholder.Name != ConfigurationManager.AppSettings["ServicePartnerStakeholder"].ToString() &&
                  a.Stakeholder.Name != ConfigurationManager.AppSettings["CBEStakeholder"].ToString()) && a.Employee1 != null).Select(a => a.Employee1.Email).ToList());
                    MailAddresses.AddRange(CurrentRelease.ReleaseStakeholders.Where(a => (a.Stakeholder.Name != ConfigurationManager.AppSettings["ServicePartnerStakeholder"].ToString() &&
                 a.Stakeholder.Name != ConfigurationManager.AppSettings["CBEStakeholder"].ToString()) && a.Employee2 != null).Select(a => a.Employee2.Email).ToList());
                }
            
                //MailAddresses.AddRange(CurrentRelease.ReleaseAreaOwners.Where(a => a.AmdocsFocalPoint1 != null).Select(a => a.AmdocsFocalPoint1.Email).ToList());
                //MailAddresses.AddRange(CurrentRelease.ReleaseStakeholders.Where(a => a.Employee1 != null).Select(a => a.Employee1.Email).ToList());


                return MailAddresses.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
            }
        }
        #endregion
    }
}



