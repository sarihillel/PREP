using EntityFramework.BulkInsert.Extensions;
using PREP.DAL;
using PREP.DAL.Functions;
using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.DAL.TableViews;
using PREP.Functions;
using PREP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace PREP.Controllers
{
  //  [Authorize(Roles = @"NTNET\ITPrepTest")]
    public class ReleasesController : BaseController
    {
        #region Private Properties
        public CurrentRelease CurrentRelease
        {
            get
            {
                if (!(Session["currentRelease"] is CurrentRelease))
                    return null;
                return (CurrentRelease)Session["currentRelease"];

            }
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// reset Current Release And Mode ,Sets CurrentTabIndex to 0
        /// </summary>
        /// <param name="ReleaseId"></param>
        /// <param name="Mode">Add Edit or View </param>
        void SetCurrentRelease(int ReleaseId = 0, Mode Mode = Mode.ADD, int TabIndex = 0)
        {
            CurrentRelease currentRelease = new CurrentRelease();
            using (ReleaseRepository db = new ReleaseRepository())
            {
                currentRelease.ReleaseId = ReleaseId;
                currentRelease.Mode = Mode;
                if (Mode == Mode.ADD)
                {
                    currentRelease.Release = db.GetNewReleseAndRelationships();
                }
                else
                {
                    currentRelease.Release = db.GetReleseAndRelationships(currentRelease.ReleaseId);
                }
                Session["CurrentRelease"] = currentRelease;
                currentRelease.Tabs = new ReleaseTabs
                {
                    GeneralDetails = GetReleaseGeneralDetails(),
                    ProductsInScope = GetProductsInScope(),
                    ReleaseCharacteristic = GetReleaseCharacteristic(),
                    ReleaseMilestones = GetReleaseMilestoneVM(),
                    ReleaseStakeholders = GetReleaseStakeholder(),
                    AreaOwners = GetReleaseAreaOwners(),
                    CheckPointReviewMode = new List<ReleaseCPReviewMode>() { },
                };
                currentRelease.CurrentTabIndex = TabIndex;
            };
        }
        private bool IsExistCheckList(int releaseID)
        {
            using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            {
                return db.IsExistCheckList(releaseID);
            }
        }
        /// <summary>
        /// find if ExistCheckList in Details.js beffore initiate
        /// </summary>
        public JsonResult ExistCheckListAnswers(int releaseID)
        {
            return Json(new { Result = IsExistCheckList(releaseID) });
        }
        #endregion

        #region Releselist
        // fill the view model-Releases

        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            List<ReleaseView> Releases = new List<ReleaseView>();
            int CountRecord = 0;

            using (IReleaseRepository Db = new ReleaseRepository())
            {
                Releases = Db.GetByFiltering(param.iDisplayStart, param.iDisplayLength, param.SortBy, param.sSearch, out CountRecord).ToList();
            }


            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = CountRecord,
                iTotalDisplayRecords = CountRecord,
                aaData = Releases
            }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Index(String textSearch = null)
        {
            ViewBag.textSearch = textSearch;
            return View();
        }
        #endregion

        #region Header
        public PartialViewResult _ReleaseHeader(int ReleaseID, ReleaseGeneralDetails GeneralDetails)
        {
            using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            {
                ViewBag.IsExistCheckList = db.IsExistCheckList(ReleaseID);
            }
            return PartialView(GeneralDetails);
        }
        #endregion

        #region footer
        public PartialViewResult _ReleaseFooter(int releaseId)
        {
            Release releaseFooter;
            //if after Save
            if (CurrentRelease == null || CurrentRelease.ReleaseId != releaseId)
            {
                if (releaseId == 0)
                    return PartialView();
                else
                {
                    using (IReleaseRepository db = new ReleaseRepository())
                    {
                        releaseFooter = db.GetReleaseCPData(releaseId);
                    }
                }
            }
            else
            {
                releaseFooter = CurrentRelease.Release;
            }
            if (releaseFooter != null && releaseId != 0)
            {
                foreach (var rm in releaseFooter.ReleaseMilestones)
                {
                    ViewData[rm.Milestone.Name] = String.Format("{0:d-MMM-yyyy}", rm.MilestoneDate);
                }
                ReleaseCPView rcpview;
                foreach (var rcp in releaseFooter.ReleaseCPs)
                {
                    rcpview = new ReleaseCPView(rcp, releaseFooter.Account.Name);
                    ViewData[rcpview.CPName.Trim().Replace(" ", String.Empty)] = rcpview.PlannedDate != null ? "(" + String.Format("{0:d-MMM-yyyy}", rcpview.PlannedDate) + ")" : String.Empty;
                    ViewData[rcpview.CPName.Trim().Replace(" ", String.Empty) + "id"] = rcpview.ReleaseCPID;
                }
            }
            return PartialView();
        }

        public PartialViewResult releaseCPDetails(int CPID)
        {
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                var CPDetails = db.GetReleaseCPViewById(CPID);
                CPDetails.PublicationMailLink = CPDetails.PublicationMailLink != null ? ConfigurationManager.AppSettings["PublicationMailsFolder"] + CPDetails.PublicationMailLink : "#";
                return PartialView(CPDetails);
            }
        }
        [HttpGet]
        public PartialViewResult EditReleaseCPDetails(int releaseCP)
        {
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                return PartialView(db.GetReleaseCPViewById(releaseCP));
            }
        }

        [HttpPost]
        public async Task<int> SaveReleaseCPDetails(ReleaseCPView releaseCP)
        {
            int count = 0;
            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                count += await db.Edit(releaseCP, (WindowsPrincipal)User);
            }
            return count;
        }

        #endregion

        #region Details



        #region Navigation Tabs
        private IEnumerable<ReleaseCharacteristicVM> GetReleaseCharacteristic()
        {
            return from tbl in CurrentRelease.Release.ReleaseCharacteristics
               .Where(a => a.Characteristic != null )
               .OrderBy(a => a.Characteristic.Order)
                   select
                   new ReleaseCharacteristicVM()
                   {
                       CharacteristicID=tbl.CharacteristicID,
                       CharacteristicName = tbl.Characteristic.Name != null ? tbl.Characteristic.Name: String.Empty,
                       ToolTip = tbl.Characteristic.ToolTip !=null? tbl.Characteristic.ToolTip: String.Empty,
                       IsChecked = tbl.IsChecked
                   };         
        }
        private ReleaseGeneralDetails GetReleaseGeneralDetails()
        {
            var CurrentGeneralDetails = new ReleaseGeneralDetails(CurrentRelease.Release);
            //{
            //    // IsFullTrack = (CurrentRelease.Release.ReleaseCPReviewModeQs.Count > 0 ? CurrentRelease.Release.ReleaseCPReviewModeQs.FirstOrDefault().IsFullTrack : false),
            //    //ReleaseID = CurrentRelease.ReleaseId != 0 ? CurrentRelease.Release.ReleaseID : new Nullable<Int32>(),
            //    //AccountID = CurrentRelease.Release.Account != null ? CurrentRelease.Release.Account.AccountID : 0,
            //    //AccountName = CurrentRelease.Release.Account != null ? CurrentRelease.Release.Account.Name : "",
            //    //Name = CurrentRelease.Release.Name,
            //    //Size = CurrentRelease.Release.Size,
            //    //LOB = CurrentRelease.Release.LOB,
            //    //LastCP = CurrentRelease.Release.ReleaseCPs.OrderByDescending(rcp => rcp.PublicationMailDate).First().CP.Name,
            //    //LastCPDate = CurrentRelease.Release.ReleaseCPs.OrderByDescending(rcp => rcp.PublicationMailDate).First().CP.Name
            //    //Accounts = new AccountRepository().Get(a => a.IsDeleted == false).ToList(),

            //    //DismissIndicator = CurrentRelease.Release.DismissIndicator

            //};
            using (IHistoryRepository db = new HistoryRepository())
            {
                var activityLogCreated = db.GetLastActivityLogUpdate(CurrentRelease.Release.ReleaseID, ActivityType.Create);
                if (activityLogCreated != null)
                {
                    CurrentGeneralDetails.CreatedDate = activityLogCreated.ActivityLog.Date;
                    CurrentGeneralDetails.CreatedBy = activityLogCreated.ActivityLog.Employee.FirstName + " " + activityLogCreated.ActivityLog.Employee.LastName;
                }
                var activityLogUpdated = db.GetLastActivityLogUpdate(CurrentRelease.Release.ReleaseID, null);
                if (activityLogUpdated != null)
                {
                    CurrentGeneralDetails.ModifiedDate = activityLogUpdated.ActivityLog.Date;
                    CurrentGeneralDetails.ModifiedBy = activityLogUpdated.ActivityLog.Employee.FirstName + " " + activityLogUpdated.ActivityLog.Employee.LastName;
                }

            }

            if (CurrentRelease.ReleaseId == 0)
            {
                CurrentGeneralDetails.CreatedDate = DateTime.Today.Date;
                CurrentGeneralDetails.ModifiedDate = DateTime.Today.Date;// null;
            }
            return CurrentGeneralDetails;


        }
        private ReleaseAndFamilyProducts GetProductsInScope()
        {
            ReleaseAndFamilyProducts ReleaseFamilyProducts = new ReleaseAndFamilyProducts();
            ReleaseFamilyProducts.FamilyProducts = CurrentRelease.Release.ReleaseFamilyProducts.Select(fp =>
                                            new ReleaseFamilyProducts
                                            {
                                                ID = fp.FamilyProductID,
                                                IsChecked = fp.IsChecked,
                                                Name = fp.FamilyProduct.Name,
                                                ToolTip = fp.FamilyProduct.ToolTip,
                                                Products = CurrentRelease.Release.ReleaseProducts
                                                            .Where(r => r.Product.FamilyProductID == fp.FamilyProductID)
                                                            .Select(rp =>
                                                                        new ReleaseProducts
                                                                        {
                                                                            ID = rp.ProductID,
                                                                            Name = rp.Product.Name,
                                                                            IsChecked = rp.IsChecked,
                                                                            ToolTip = rp.Product.ToolTip
                                                                        }).ToList()
                                            }).ToList();
            ReleaseFamilyProducts.Release = CurrentRelease.Release;
            return ReleaseFamilyProducts;

        }
        private IEnumerable<ReleaseMilestoneVM> GetReleaseMilestoneVM()
        {
            var query = from rm in CurrentRelease.Release.ReleaseMilestones
                       
                        orderby rm.Milestone.Order
                        select new ReleaseMilestoneVM
                        {
                            MilestoneID = rm.MilestoneID,
                            MilestoneDate = rm.MilestoneDate,
                            MilestoneName = rm.Milestone.Name,
                            ToolTip = rm.Milestone.ToolTip,
                        };

            return query.ToList();
        }
        public IEnumerable<ReleaseStakeholderVM> GetReleaseStakeholder()
        {

            var query = from rs in CurrentRelease.Release.ReleaseStakeholders
                      
                        orderby rs.Stakeholder.Order
                        select new ReleaseStakeholderVM
                        {
                            FullNameEmployee1 = rs.Employee1 != null ? rs.Employee1.FirstName + " " + rs.Employee1.LastName : string.Empty,
                            FullNameEmployee2 = rs.Employee2 != null ? rs.Employee2.FirstName + " " + rs.Employee2.LastName : string.Empty,
                            EmployeeID1 = rs.EmployeeID1,
                            EmployeeID2 = rs.EmployeeID2,
                            StakeholderName = rs.Stakeholder.Name,
                            ReleaseStakeholderID = rs.ReleaseStakeholderID,
                            ToolTip = rs.Stakeholder.ToolTip,
                            StakeholderID = rs.StakeholderID
                        };

            return query.ToList();

        }
        private IEnumerable<ReleaseAreaOwners> GetReleaseAreaOwners()
        {

            IEnumerable<ReleaseAreaOwners> ReleaseAreaOwners =
                CurrentRelease.Release.ReleaseAreaOwners
                
                .OrderBy(a => a.Area.Order)
                    .Select(ra => new ReleaseAreaOwners
                    {
                        AreaID = ra.AreaID,
                        IsChecked = ra.IsChecked,
                        AreaName = ra.Area.Name,
                        Resposibility = ra.Resposibility,
                        AmdocsFocalPoint1ID = ra.AmdocsFocalPoint1ID,
                        AmdocsFocalPoint2ID = ra.AmdocsFocalPoint2ID,
                        AmdocsFocalPoint1Name = ra.AmdocsFocalPoint1 != null ? ra.AmdocsFocalPoint1.FirstName + ' ' + ra.AmdocsFocalPoint1.LastName : "",
                        AmdocsFocalPoint2Name = ra.AmdocsFocalPoint2 != null ? ra.AmdocsFocalPoint2.FirstName + ' ' + ra.AmdocsFocalPoint2.LastName : "",
                        CustomerFocalPoint1 = ra.CustomerFocalpoint1,
                        CustomerFocalPoint2 = ra.CustomerFocalPoint2,
                        ToolTip = ra.Area.ToolTip,
                    }).ToList();
            return ReleaseAreaOwners;


        }

        [HttpPost]
        public JsonResult GetModifiedByandDate(int releaseID)
        {
            string ModifiedDate = "", ModifiedBy = "";
            using (IHistoryRepository db = new HistoryRepository())
            { 
                var activityLogUpdated = db.GetLastActivityLogUpdate(releaseID, null);
                if (activityLogUpdated != null)
                {
                    ModifiedDate = String.Format("{0:d-MMM-yyyy}", activityLogUpdated.ActivityLog.Date); 
                    ModifiedBy = activityLogUpdated.ActivityLog.Employee.FirstName + " " + activityLogUpdated.ActivityLog.Employee.LastName;
                }
            }
            return Json(new { ModifiedDate = ModifiedDate, ModifiedBy = ModifiedBy });
        }
        #endregion

        public ActionResult Details(string ModeName, int ReleaseId = 0, int TabIndex = 0)
        {
            Mode CurrentMode = (Mode)Enum.Parse(typeof(Mode), ModeName.ToUpper());
            SetCurrentRelease(ReleaseId, CurrentMode, TabIndex);
            NavigationBreadCrums.SetSesionReleaseID(ReleaseId);
            return View(CurrentRelease);
        }

        #region SaveRelease
        [HttpPost]
        public async Task<object> SaveRelease(int ReleaseID, ReleaseTabs UpdateRelease, IDictionary<string, bool> ListUpdated, bool IsInitiated = false)
        {
            int count = 0;
            Session["isExecuteAsyncTask"] = false;
            using (IReleaseRepository db = new ReleaseRepository())
            {
                if (ReleaseID == 0)
                {
                    count += await SaveNewRelease(ReleaseID, UpdateRelease);
                    //   return count;
                }
                else
                {

                    count += await SaveEditRelease(ReleaseID, UpdateRelease, ListUpdated);


                    if (IsInitiated)
                    {
                        count += await InitiateRelease(ReleaseID);
                    }
                }
            }
            //after Save Update Current Release To null
            Session["currentRelease"] = null;
            return Json(new { count = count, isExecuteAsyncTask = (bool)Session["isExecuteAsyncTask"] });

        }


        //private void addToChangedQuestionOwnerIDs(int updatedID, int newID, List<int> questionOwnerID)
        //{
        //    if (updatedID != newID)


        //}
        public async Task<int> SaveEditRelease(int ReleaseID, ReleaseTabs UpdateRelease, IDictionary<string, bool> ListUpdated)
        {
            // bool isExecuteAsyncTask = false;
            List<int?> questionOwnerIds = new List<int?>();
            int Count = 0;
            List<int> changedMilestonesIds = null;
            List<int> changedStakeholderIds = null;
            List<ReleaseAreaOwner> changedAreaOwnersIds = new List<ReleaseAreaOwner>();
            Release newRelease = null;
            using (IReleaseRepository db = new ReleaseRepository())
            {
                StringBuilder includeProperty = new StringBuilder();
                if (ListUpdated["ReleaseStakeholders"])
                    includeProperty.Append("ReleaseStakeholders,");
                if (ListUpdated["AreaOwners"])
                    includeProperty.Append("ReleaseAreaOwners,");
                if (ListUpdated["ReleaseMilestones"])
                    includeProperty.Append("ReleaseMilestones,");
                if (ListUpdated["ReleaseCharacteristics"])
                    includeProperty.Append("ReleaseCharacteristics,");
                if (ListUpdated["ProductsInScope"])
                {
                    includeProperty.Append("ReleaseFamilyProducts,");
                    includeProperty.Append("ReleaseProducts,");
                }
                newRelease = db.Get(r => r.ReleaseID == ReleaseID, null, includeProperty.ToString()).FirstOrDefault();
                newRelease.AccountID = UpdateRelease.GeneralDetails.AccountID;
                newRelease.Name = UpdateRelease.GeneralDetails.Name;
                newRelease.Size = UpdateRelease.GeneralDetails.Size;
                newRelease.LOB = UpdateRelease.GeneralDetails.LOB;
                if (ListUpdated["ReleaseMilestones"])
                {
                    changedMilestonesIds = UpdateRelease.ReleaseMilestones.Where(rv => !DateTime.Equals(newRelease.ReleaseMilestones.First(rm => rm.MilestoneID == rv.MilestoneID).MilestoneDate, rv.MilestoneDate)).Select(m => (int)m.MilestoneID).ToList();
                    UpdateRelease.ReleaseMilestones.ToList().ForEach(rv => newRelease.ReleaseMilestones.First(rm => rm.MilestoneID == rv.MilestoneID).MilestoneDate = rv.MilestoneDate);
                }
                if (ListUpdated["ReleaseCharacteristics"])
                {
                    
                    UpdateRelease.ReleaseCharacteristic.ToList().ForEach(rv => newRelease.ReleaseCharacteristics.First(rm => rm.CharacteristicID == rv.CharacteristicID).IsChecked = rv.IsChecked);
                }
                if (ListUpdated["ReleaseStakeholders"])
                {
                    changedStakeholderIds = UpdateRelease.ReleaseStakeholders.Where(rn => !(newRelease.ReleaseStakeholders.First(rs => rs.StakeholderID == rn.StakeholderID).EmployeeID1==rn.EmployeeID1 )).Select(s => (int)s.StakeholderID).ToList();
                    UpdateRelease.ReleaseStakeholders.ToList().ForEach(rs =>
                    {
                        var rsh = newRelease.ReleaseStakeholders.First(rm => rm.ReleaseStakeholderID == rs.ReleaseStakeholderID);
                        if (rsh.EmployeeID1 != rs.EmployeeID1)
                        {
                            rsh.EmployeeID1 = rs.EmployeeID1;
                            if (!questionOwnerIds.Contains(rs.EmployeeID1))
                                questionOwnerIds.Add(rs.EmployeeID1);
                        }
                        if (rsh.EmployeeID2 != rs.EmployeeID2)
                        {
                            rsh.EmployeeID2 = rs.EmployeeID2;
                            if (!questionOwnerIds.Contains(rs.EmployeeID2))
                                questionOwnerIds.Add(rs.EmployeeID2);
                        }

                    });
                }
                if (ListUpdated["AreaOwners"])
                {
                    UpdateRelease.AreaOwners.ToList().ForEach(r =>
                    {
                        var current = newRelease.ReleaseAreaOwners.FirstOrDefault(rm => rm.AreaID == r.AreaID);
                        if (current != null)
                        {
                            if (!r.IsChecked)
                            {
                                current.CustomerFocalpoint1 = string.Empty;
                                current.CustomerFocalPoint2 = string.Empty;
                                current.Resposibility = Responsibility.Amdocs;
                                current.AmdocsFocalPoint1ID = current.AmdocsFocalPoint2ID = null;
                                current.AmdocsFocalPoint1 = current.AmdocsFocalPoint2 = null;
                            }
                            else
                            {

                                current.Resposibility = r.Resposibility;
                                if (current.AmdocsFocalPoint1ID != r.AmdocsFocalPoint1ID)
                                {
                                    current.AmdocsFocalPoint1ID = r.AmdocsFocalPoint1ID;
                                    changedAreaOwnersIds.Add(current);
                                }


                                current.AmdocsFocalPoint2ID = r.AmdocsFocalPoint2ID;
                                current.CustomerFocalpoint1 = r.CustomerFocalPoint1;
                                current.CustomerFocalPoint2 = r.CustomerFocalPoint2;
                            }
                            current.IsChecked = r.IsChecked;
                        }
                    });
                }

                if (ListUpdated["ProductsInScope"])
                {
                    UpdateRelease.ProductsInScope.FamilyProducts.ToList().ForEach(rf =>
                            {
                                var current = newRelease.ReleaseFamilyProducts.FirstOrDefault(f => f.FamilyProductID == rf.ID);
                                if (current != null)
                                {
                                    current.IsChecked = rf.IsChecked;
                                    if (rf.Products != null)
                                        rf.Products.ForEach(rp =>
                                            {
                                                var currentrp = newRelease.ReleaseProducts.FirstOrDefault(p => p.ProductID == rp.ID);
                                                if (currentrp != null) currentrp.IsChecked = rp.IsChecked;
                                            });

                                }
                            });
                    newRelease.AdditionalProducts = UpdateRelease.ProductsInScope.Release.AdditionalProducts;
                }

                Count += await db.SaveAsync((WindowsPrincipal)User);

            }
            //Release Release, Milestone mileston, Milestone prevMilestone, double ratio
            if (ListUpdated["ReleaseMilestones"])
            {
                if (changedMilestonesIds != null && changedMilestonesIds.Count() > 0)
                {
                    using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                    {
                        try
                        {
                            IEnumerable<ReleaseChecklistAnswer> releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question).Where(cl => changedMilestonesIds.Where(m => m == cl.Question.MilestoneID || m == cl.Question.PreviousMilestoneID).Count() > 0).ToList();
                            Session["isExecuteAsyncTask"] = releaseChecklistAnswer.Count() > 0;
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }
                    }

                    await changeCheckListHandlingStartDate(ReleaseID, changedMilestonesIds, newRelease);
                    
                }
            }
            if (ListUpdated["ReleaseStakeholders"])
            {
                if (changedStakeholderIds != null && changedStakeholderIds.Count() > 0)
                {
                    using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                    {
                        IEnumerable<ReleaseChecklistAnswer> releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question).Where(cl => changedStakeholderIds.Where(s => s == cl.Question.QuestionOwnerID).Count() > 0).ToList();
                        Session["isExecuteAsyncTask"] = releaseChecklistAnswer.Count() > 0;
                    }
                    await changeCheckListStakeholder(ReleaseID, changedStakeholderIds, newRelease);
                }
            }

            if (ListUpdated["AreaOwners"] && changedAreaOwnersIds != null && changedAreaOwnersIds.Count() > 0)
            {
                int count = 0, employeeId;
                var histories = new List<History>();
                using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                {
                    try
                    {
                        IEnumerable<ReleaseChecklistAnswer> releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question);
                        releaseChecklistAnswer = releaseChecklistAnswer.Where(cl => cl.Question.QuestionOwnerID == null && changedAreaOwnersIds.Where(ao => cl.AreaID == ao.AreaID).Count() > 0);
                        if (releaseChecklistAnswer != null && releaseChecklistAnswer.ToList().Count() > 0)
                        {
                            Session["isExecuteAsyncTask"] = true;
                            using (IEmployeeRepository current_db = new EmployeeRepository())
                            {
                                employeeId = current_db.GetEmployee(ConfigurationManager.AppSettings["SystemUserNtnet"]);
                            }
                            foreach (ReleaseChecklistAnswer cl in releaseChecklistAnswer)
                            {
                                var history = new History();
                                history.ReleaseID = cl.ReleaseID;
                                history.ItemID = cl.ReleaseChecklistAnswerID;
                                history.TableID = 26;
                                history.OldValue = cl.QuestionOwnerID.ToString();
                                history.FieldName = "QuestionOwnerID";
                                cl.QuestionOwnerID = changedAreaOwnersIds.FirstOrDefault(ao => cl.AreaID == ao.AreaID).AmdocsFocalPoint1ID;
                                history.NewValue = cl.QuestionOwnerID.ToString();
                                histories.Add(history);
                            }
                            count += await db.SaveAsync(null, true, histories);
                        }
                    }
                    catch (Exception ex)
                    {
                        Errors.Write(ex.Message);
                    }
                }
            }
            
            //if (ListUpdated["AreaOwners"] || ListUpdated["ReleaseStakeholders"])
            //{
            //    if (changedMilestonesIds != null && changedMilestonesIds.Count() > 0)
            //    {
            //        using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            //        {
            //            IEnumerable<ReleaseChecklistAnswer> releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question).Where(cl => changedMilestonesIds.Where(m => m == cl.Question.MilestoneID || m == cl.Question.PreviousMilestoneID).Count() > 0).ToList();
            //            Session["isExecuteAsyncTask"] = releaseChecklistAnswer.Count() > 0;
            //        }
            //        changeCheckListHandlingStartDate(ReleaseID, changedMilestonesIds, newRelease);
            //    }
            //}
            //  OutputDebug.Write("Savemilestone end");
            // OutputDebug.WriteToOutput();
            return Count;
        }



        public Task changeCheckListHandlingStartDateTask(int ReleaseID, List<int> changedMilestonesIds, Release newRelease)
        {
            try
            {
                return Task.Run(() =>
                               changeCheckListHandlingStartDate(ReleaseID, changedMilestonesIds, newRelease)
                );
            }
            catch (Exception ex)
            {
                string s = ex.Message;

            }
            return Task.Run(() =>
                changeCheckListHandlingStartDate(ReleaseID, changedMilestonesIds, newRelease)
                 );
        }
        public async Task<int> changeCheckListHandlingStartDate(int ReleaseID, List<int> changedMilestonesIds, Release newRelease)
        {
            int count = 0, employeeId;
            var histories = new List<History>();

            using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            {
                IEnumerable<ReleaseChecklistAnswer> releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question).Where(cl => changedMilestonesIds.Where(m => m == cl.Question.MilestoneID || m == cl.Question.PreviousMilestoneID).Count() > 0);

                if (releaseChecklistAnswer != null && releaseChecklistAnswer.ToList().Count() > 0)
                {
                    using (IEmployeeRepository current_db = new EmployeeRepository())
                    {
                        employeeId = current_db.GetEmployee(ConfigurationManager.AppSettings["SystemUserNtnet"]);
                    }

                    //ActivityLog activityLog = new ActivityLog(employeeId, ActivityType.Edit);
                    //using (IActivityLogRepository current_db = new ActivityLogRepository())
                    //{
                    //    current_db.Add(activityLog);
                    //    await current_db.SaveChangesAsync();
                    //}

                    foreach (ReleaseChecklistAnswer cl in releaseChecklistAnswer)
                    {
                        var history = new History();
                        history.ReleaseID = cl.ReleaseID;
                        history.ItemID = cl.ReleaseChecklistAnswerID;
                        history.TableID = StaticResources.GetTableID(ConfigurationManager.AppSettings["ReleaseChecklistAnswer"]);
                      //  history.ActivityLogID = activityLog.ActivityLogID;
                        history.OldValue = cl.HandlingStartDate.ToString();
                        history.FieldName = "HandlingStartDate";
                        cl.HandlingStartDate = new QuestionRepository().HandlingStartDatecalculation(newRelease, cl.Question.MilestoneID, cl.Question.PreviousMilestoneID, cl.Question.RatioBetweenMilestones);
                        history.NewValue = cl.HandlingStartDate.ToString();
                        if(history.OldValue != history.NewValue)
                        histories.Add(history);
                    }



                    //    var history = new History();
                    //    history.OldValue = cl.HandlingStartDate.ToString();
                    //    history.FieldName = "HandlingStartDate";
                    //    cl.HandlingStartDate = new QuestionRepository().HandlingStartDatecalculation(newRelease, cl.Question.MilestoneID, cl.Question.PreviousMilestoneID, cl.Question.RatioBetweenMilestones);
                    //    history.NewValue = cl.HandlingStartDate.ToString();
                    //    histories.Add(history);
                    //    history.ActivityLogs = new List<ActivityLog>() { new ActivityLog(employeeId,
                    //            cl.ReleaseChecklistAnswerID, cl.ReleaseID, 26 , ActivityType.Edit)};
                    //});




                    //await releaseChecklistAnswer.ForEachAsync(cl =>
                    //{

                    //});
                    count += await db.SaveAsync(null, true, histories);
                }
            }
        
            return count;

        }
        public async Task<int> changeCheckListStakeholder(int ReleaseID, List<int> changedStakeholderIds, Release newRelease)
        {
            int count = 0, employeeId;
            var histories = new List<History>();
            using (IEmployeeRepository db = new EmployeeRepository())
            {
                employeeId = db.GetEmployee(ConfigurationManager.AppSettings["SystemUserNtnet"]);
            }
            using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            {
                var releaseChecklistAnswer = db.Where(cl => cl.ReleaseID == ReleaseID).Include(cl => cl.Question).Where(cl => changedStakeholderIds.Where(s =>s == cl.Question.QuestionOwnerID).Count() > 0);



                foreach (ReleaseChecklistAnswer cl in releaseChecklistAnswer)
                {
                    var history = new History();
                    history.ReleaseID = cl.ReleaseID;
                    history.ItemID = cl.ReleaseChecklistAnswerID;
                    history.TableID = 26;
                    //  history.ActivityLogID = activityLog.ActivityLogID;
                    history.OldValue = cl.QuestionOwnerID.ToString();
                    history.FieldName = "QuestionOwner";
                    cl.QuestionOwnerID = newRelease.ReleaseStakeholders.Where(s => s.StakeholderID == cl.Question.QuestionOwnerID).FirstOrDefault().EmployeeID1;
                    history.NewValue = cl.QuestionOwnerID.ToString();
                    histories.Add(history);
                }
                count += await db.SaveAsync(null, true, histories);
            }
            return count;
        }
        public async Task<int> SaveNewRelease(int ReleaseID, ReleaseTabs UpdateRelease)
        {
            Release ReleaseTemp = null;
            int count = 0;

            try
            {
               using (IReleaseRepository db = new ReleaseRepository())
                {

                    ReleaseTemp = new Release()
                    {
                        AccountID = UpdateRelease.GeneralDetails.AccountID,
                        Name = UpdateRelease.GeneralDetails.Name,
                        Size = UpdateRelease.GeneralDetails.Size,
                        LOB = UpdateRelease.GeneralDetails.LOB,
                        AdditionalProducts = UpdateRelease.ProductsInScope.Release.AdditionalProducts,
                        ReleaseStakeholders = new List<ReleaseStakeholder>(),
                        ReleaseAreaOwners = new List<ReleaseAreaOwner>(),
                        ReleaseFamilyProducts = new List<ReleaseFamilyProduct>(),
                        ReleaseProducts = new List<ReleaseProduct>(),
                        ReleaseMilestones = new List<ReleaseMilestone>(),
                        ReleaseCharacteristics=new List<ReleaseCharacteristic>()

                    };

                    UpdateRelease.ReleaseMilestones.ToList().ForEach(
                        prop =>
                         ReleaseTemp.ReleaseMilestones.Add(new ReleaseMilestone() { MilestoneID = prop.MilestoneID, MilestoneDate = prop.MilestoneDate })
                        );

                    UpdateRelease.ReleaseStakeholders.ToList().ForEach(
                       prop =>
                        ReleaseTemp.ReleaseStakeholders.Add(new ReleaseStakeholder() { StakeholderID = prop.StakeholderID, EmployeeID1 = prop.EmployeeID1, EmployeeID2 = prop.EmployeeID2 })
                       );

                    UpdateRelease.AreaOwners.ToList().ForEach(
                       prop =>
                        ReleaseTemp.ReleaseAreaOwners.Add(new ReleaseAreaOwner() { AreaID = prop.AreaID, AmdocsFocalPoint1ID = prop.AmdocsFocalPoint1ID, AmdocsFocalPoint2ID = prop.AmdocsFocalPoint2ID, IsChecked = prop.IsChecked, CustomerFocalpoint1 = prop.CustomerFocalPoint1, CustomerFocalPoint2 = prop.CustomerFocalPoint2, Resposibility = prop.Resposibility })
                       );

                    UpdateRelease.ReleaseCharacteristic.ToList().ForEach(
                      prop =>
                       ReleaseTemp.ReleaseCharacteristics.Add(new ReleaseCharacteristic() { CharacteristicID = prop.CharacteristicID, IsChecked = prop.IsChecked })
                      );


                    UpdateRelease.ProductsInScope.FamilyProducts.ToList().ForEach(prop =>
                       {
                           ReleaseTemp.ReleaseFamilyProducts.Add(new ReleaseFamilyProduct() { FamilyProductID = prop.ID, IsChecked = prop.IsChecked });
                           prop.Products.ToList().ForEach(rp =>
                                   ReleaseTemp.ReleaseProducts.Add(new ReleaseProduct() { ProductID = rp.ID, IsChecked = rp.IsChecked })
                       );
                       });

                    //ReleaseTemp.ReleaseCharacteristics = new List<ReleaseCharacteristic>();
                    //using (IReleaseRepository db = new ReleaseRepository())
                    //{
                    //    using (var transactionScope = new TransactionScope())
                    //    {
                    //        db.Add(ReleaseTemp);
                    //        await db.SaveAsync((WindowsPrincipal)User);
                    //        transactionScope.Complete();
                    //    }
                    //}
                   db.Add(ReleaseTemp);

                 count += await db.SaveAsync((WindowsPrincipal)User);

                    await AddCPs(ReleaseTemp.ReleaseID);
               }
            }
            catch (Exception ex)
            {

                count = -1;
            }

            return count > 0 ? ReleaseTemp.ReleaseID : count;
        }
        public async Task<int> AddCPs(int ReleaseID)
        {
            IEnumerable<CP> CPs = null;
            int count = 0;
            using (ICPRepository db = new CPRepository())
            {
                CPs = db.Get().Where(cp => cp.EffectiveDate < DateTime.Today.Date && cp.ExpirationDate > DateTime.Today.Date && cp.IsDeleted == false);
            }

            using (IReleaseCPRepository db = new ReleaseCPRepository())
            {
                IList<ReleaseCP> ReleaseCP = new List<ReleaseCP>();
                CPs.ToList().ForEach(cp => ReleaseCP.Add(new ReleaseCP() { CPID = cp.CPID, ReleaseID = ReleaseID, ExceptionIndicator = false, IsDeleted = false }));
                db.AddRange(ReleaseCP);
                count += await db.SaveAsync((WindowsPrincipal)User);
            }
            return count;
        }
        public async Task<int> InitiateRelease(int ReleaseID)
        {
            int count = 0;
            Release currentRelease;
            using (IReleaseRepository db = new ReleaseRepository())
            {
                currentRelease = db.getReleaseForInitiateCheckList(ReleaseID);
            }
            using (IVendorRepository DB = new VendorRepository())
            {
                Vendor currentVendor = new VendorRepository().Where(v => v.Name == "Amdocs").Include(v => v.ReleaseVendors).FirstOrDefault();
                var Areas = currentRelease.ReleaseAreaOwners.Where(a => a.IsChecked == true).ToList();

                List<VendorAreas> newVendorAreas = new List<VendorAreas>();
                List<ReleaseVendor> ReleaseVendors = new List<ReleaseVendor>();
                Areas.ForEach(r =>
                {
                    var newObj = new VendorAreas()
                    {
                        VendorID = currentVendor.VendorID,
                        AreaID = r.AreaID,
                        IsChecked = true
                    };
                    newVendorAreas.Add(newObj);
                });
                currentVendor.VendorAreass = newVendorAreas;

                var currentReleaseVendor = currentVendor.ReleaseVendors.FirstOrDefault(v => v.ReleaseID == ReleaseID);

                if (currentReleaseVendor == null)
                    currentVendor.ReleaseVendors = new List<ReleaseVendor>() { new ReleaseVendor() { ReleaseID = ReleaseID, VendorID = currentVendor.VendorID, IsFullTrack = true } };
                count += await DB.EditVendor(currentVendor, currentRelease, (WindowsPrincipal)User);

            }
            return count;
        }
        #endregion
        #endregion

        public ActionResult EmptyPage()
        {
            return View();
        }
    }
}
