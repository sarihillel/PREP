using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.DAL.Models;
using PREP.DAL.TableViews;
using PREP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Data.Entity;
using PREP.Functions;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.UI.WebControls;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;

namespace PREP.Controllers
{

    public class StatusController : BaseController
    {
        #region Actions
        StatusVM status = new StatusVM();
        List<ScoreIndicator> IndicatorList = new List<ScoreIndicator>()
            {
                new ScoreIndicator() {Min = -1,   Max = 0, ScoreLevel = 0, ScoreIMG = ScoreIMG.blank },
                new ScoreIndicator() {Min = 0, Max = 70,  ScoreLevel = 1, ScoreIMG = ScoreIMG.stop },
                new ScoreIndicator() {Min = 70,  Max = 90,  ScoreLevel = 2, ScoreIMG = ScoreIMG.flag },
                new ScoreIndicator() {Min = 90,  Max = 101, ScoreLevel = 3, ScoreIMG = ScoreIMG.like }

            };

        public ActionResult ViewStatus(int ReleaseId, int CPID = 0, bool IsDraft = true)
        {

            using (ReleaseCPRepository db = new ReleaseCPRepository())
            {
                status = new StatusVM();
                status.ReleaseID = ReleaseId;

                NavigationBreadCrums.SetSesionReleaseID(ReleaseId);

                List<StatusAreaText> statusAreaTexts;
                using (IStatusAreaTextRepository dbStatusAreaText = new StatusAreaTextRepository())
                {
                    statusAreaTexts = dbStatusAreaText.Where(a => a.ReleaseID == ReleaseId).ToList();
                }
                using (IStatusTextRepository dbStatusText = new StatusTextRepository())
                {
                    StatusText statusText = dbStatusText.Where(a => a.ReleaseID == ReleaseId).FirstOrDefault();
                    if (statusText != null)
                        status.StatusText = new StatusTextVM() { ReleaseID = statusText.ReleaseID, HighLightText = statusText.HighLightText };
                }
                using (IReleaseChecklistAnswerRepository dbChecklist = new ReleaseChecklistAnswerRepository())
                {
                    status.AreaScores = dbChecklist.GetStatus(ReleaseId).Select(q =>
                        new AreaScoreVM()
                        {
                            AreaID = q.Area.AreaID,
                            ReleaseID = ReleaseId,
                            Name = q.Area.Name,
                            Score = q.Score,
                            ScoreStatus = IndicatorList.First(i => q.Score >= i.Min && q.Score < i.Max).ScoreIMG,
                            Trand = q.LastScore != null ? TrandCalculation(q.Score, (double)q.LastScore) : Trand.none,
                            StatusAreaText = statusAreaTexts != null && statusAreaTexts.Where(s => s.AreaID == q.Area.AreaID).FirstOrDefault() != null ? statusAreaTexts.Where(s => s.AreaID == q.Area.AreaID).FirstOrDefault().AreaText : "",
                            SubAreaScors = q.SubAreaScores.Select(s =>
                            new SubAreaScoreVM()
                            {
                                Name = s.SubArea.Name,
                                Score = s.Score,
                                ScoreStatus = IndicatorList.First(i => s.Score >= i.Min && s.Score < i.Max).ScoreIMG,
                                SubAreaID = s.SubArea.SubAreaID,
                                Trand = s.LastScore != null ? TrandCalculation(s.Score, (double)s.LastScore) : Trand.none,
                            }).ToList()
                        }).ToList();
                }

            }
            status.Details = new ReleaseGeneralDetails() { ReleaseID = ReleaseId };
            status.CPID = CPID;
            status.IsDraft = IsDraft;
            return View(status);
        }

        private Trand TrandCalculation(double Score, double LastScore)
        {
            var ScoreLevel = IndicatorList.First(i => Score >= i.Min && Score < i.Max).ScoreLevel;
            var LastScoreLevel = IndicatorList.First(i => LastScore >= i.Min && LastScore < i.Max).ScoreLevel;
            Trand newTrand = ScoreLevel == 0 && LastScoreLevel == 0 ? Trand.none : ScoreLevel > LastScoreLevel ? Trand.up : ScoreLevel < LastScoreLevel ? Trand.down : Trand.nochange;
            return newTrand;
        }


        public PartialViewResult _ViewStatusHeader(int releaseId)
        {//need to delete this view from releases
            using (ReleaseProductRepository db = new ReleaseProductRepository())
            {
                ViewBag.productsName = db.GetAllFamilyProductsNamesByRealeaseId(releaseId);
            }
            using (ReleaseRepository db = new ReleaseRepository())
            {
                var release = db.GetReleseForStatus(releaseId);
                status.Details = new ReleaseGeneralDetails();
                status.Details.AccountName = release.Account.Name;
                status.Details.Name = release.Name;
                status.Details.Size = release.Size;
                status.Details.LOB = release.LOB;
                status.Details.ReleaseID = release.ReleaseID;
            }
            return PartialView(status.Details);
        }
        //[HttpGet]
        //public PartialViewResult _HighLightText(int ReleaseId = 19)
        //{
        //    //need to send the currentstatustext for releaseid
        //    //using (StatusTextRepository db = new StatusTextRepository())
        //    //{
        //    //    var statusText = db.FindBy(a => a.ReleaseID == releaseId).FirstOrDefault();
        //    //    if (statusText != null)
        //    //    {
        //    //        status.StatusText = new StatusTextVM();
        //    //        //status.StatusText.HighLightText = statusText.HighLightText;ask if need
        //    //        status.StatusText.ReleaseID = statusText.ReleaseID;
        //    //    }
        //    //}
        //    status.StatusText = new StatusTextVM() { ReleaseID = ReleaseId };

        //    return PartialView(status.StatusText);
        //}

        public PartialViewResult _ReleaseStakeholders(int ReleaseId = 19)
        {
            int count = 8;
            using (ReleaseStakeholderRepository db = new ReleaseStakeholderRepository())
            {
                List<ReleaseStakeholder> releaseStakeholder = db.Where(a => a.ReleaseID == ReleaseId).Include(a => a.Stakeholder).Include(a => a.Employee1).OrderBy(a => a.Stakeholder.Order).Take(count).ToList();
                //List<ReleaseStakeholder> releaseStakeholder = db.FindBy(a => a.ReleaseID == ReleaseId).OrderBy(a=>a.Stakeholder.Order).Take(count).ToList();
                //foreach (var item in releaseStakeholder)
                //{
                //    using (EmployeeRepository emp = new EmployeeRepository())
                //    {
                //        using (StakeholderRepository sh = new StakeholderRepository())
                //        {
                //            DAL.Models.Employee employee1 = emp.Find(item.EmployeeID1);
                //            Stakeholder stakeholder = sh.Find(item.StakeholderID);
                //            ReleaseStakeholderVM rs = new ReleaseStakeholderVM() { ReleaseID = item.ReleaseID, FullNameEmployee1 = employee1 != null ? employee1.FirstName + " " + employee1.LastName : " ", StakeholderName = stakeholder.Name };
                //            status.ReleaseStakeholder.Add(rs);
                //        }
                //    }
                //}
                status.ReleaseStakeholder = new List<ReleaseStakeholderVM>();
                foreach (var item in releaseStakeholder)
                {
                    ReleaseStakeholderVM rs = new ReleaseStakeholderVM() { ReleaseID = item.ReleaseID, FullNameEmployee1 = item.Employee1 != null ? item.Employee1.FirstName + " " + item.Employee1.LastName : " ", StakeholderName = item.Stakeholder != null ? item.Stakeholder.Name : "" };
                    status.ReleaseStakeholder.Add(rs);
                }
            }
            return PartialView(status.ReleaseStakeholder);
        }
        #endregion
        public async Task<int> SaveStatus(StatusVM status)
        {
            try
            {

                int count = 0;
                StatusText statusText;
                using (IStatusTextRepository db = new StatusTextRepository())
                {
                    statusText = db.Where(a => a.ReleaseID == status.StatusText.ReleaseID).FirstOrDefault();
                    if (statusText != null)//edit
                    {
                        if (statusText.HighLightText != status.StatusText.HighLightText)
                        {
                            statusText.HighLightText = status.StatusText.HighLightText;
                            db.Edit(statusText);
                            count += await db.SaveAsync((WindowsPrincipal)User);
                        }
                    }
                    else//create
                    {
                        statusText = new StatusText();
                        statusText.ReleaseID = status.StatusText.ReleaseID;
                        statusText.HighLightText = status.StatusText.HighLightText;
                        db.Add(statusText);
                        count += await db.SaveAsync((WindowsPrincipal)User);
                    }
                }
                if (status.AreaScores != null)
                {
                    using (IStatusAreaTextRepository db = new StatusAreaTextRepository())
                    {

                        status.AreaScores.ForEach(s =>
                        {
                            var statusArea = db.Where(a => a.AreaID == s.AreaID && a.ReleaseID == status.StatusText.ReleaseID).FirstOrDefault();
                            if (statusArea != null)//edit
                            {
                                if (statusArea.AreaText != s.StatusAreaText)
                                {
                                    statusArea.AreaText = s.StatusAreaText;
                                    db.Edit(statusArea);
                                }
                            }
                            else//create
                            {
                                statusArea = new StatusAreaText()
                                {
                                    AreaID = s.AreaID,
                                    ReleaseID = status.StatusText.ReleaseID,
                                    AreaText = s.StatusAreaText
                                };
                                db.Add(statusArea);
                            }
                        });
                        count += await db.SaveAsync((WindowsPrincipal)User);
                    }
                }

                return count;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #region Publication Methods
        [HttpPost]
        public ActionResult GetReleaseCP(int ReleaseID, bool IsDraft)
        {
            List<ReleaseCPView> CPs;
            using (IReleaseCPRepository dbCP = new ReleaseCPRepository())
            {
                CPs = dbCP.GetReleaseCPByFiltering(rcp => rcp.ReleaseID == ReleaseID).OrderBy(cp => cp.PlannedDate == null).ThenBy(cp => cp.PlannedDate).ToList();
                var DefaultCP = CPs.FirstOrDefault(cp => cp.PlannedDate >= DateTime.Today && cp.PlannedDate != null);
                if (DefaultCP != null)
                {
                    CPs.Remove(DefaultCP);
                    CPs.Insert(0, DefaultCP);
                }
                ViewBag.PopUpTitle = IsDraft ? "Draft Publication" : "Formal publish";

            }
            return PartialView("PopUpPublish", CPs);
        }

        public async Task<JsonResult> publishCP(int CPID, int ReleaseID, bool ChangeRiskLevel = false)
        {
            try
            {
                CP currentCP;
                IEnumerable<ReleaseChecklistAnswer> InitiatedQuestion = null;
                int count = 0;
                using (ICPRepository db = new CPRepository())
                {
                    currentCP = db.Where(cp => cp.CPID == CPID).Include(cp => cp.Milestone).Include(cp => cp.Milestone.ReleaseMilestones).FirstOrDefault();
                }
                using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                {
                    var cpDate = currentCP.Milestone.ReleaseMilestones.Where(rm => rm.ReleaseID == ReleaseID).FirstOrDefault().MilestoneDate;
                    InitiatedQuestion = cpDate != null ? db.Where(s => s.RiskLevelID == RiskLevels.Initiated && s.HandlingStartDate <= cpDate && s.ReleaseID == ReleaseID).ToList() : null;
                    int AllQuestion = db.Where(s => s.HandlingStartDate <= cpDate && s.ReleaseID == ReleaseID).Count();
                    if (InitiatedQuestion != null && InitiatedQuestion.Count() > 0)
                    {
                        if (ChangeRiskLevel)
                        {
                            InitiatedQuestion.ToList().ForEach(q => { q.RiskLevelID = RiskLevels.High; q.HandlingStartDate = DateTime.Today; });
                            count += await db.SaveAsync((WindowsPrincipal)User);
                            //(WindowsPrincipal)User);
                            // ViewStatus(ReleaseID);
                        }
                        return Json(new { Result = "ExistQuestions", IsChanged = ChangeRiskLevel, IsUpdated = true });
                    }
                    return Json(new { Result = "NotExistQuestions", TotalCPQuestion = AllQuestion });

                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult PreparePublicationMail(int ReleaseID, int CPID, string base64string, bool isDraft)
        {
            string PublishCP;
            string ToEmail = "";
            Release PublishRelease;
            PublicationMailDetails PubliactionMail = new PublicationMailDetails();

            using (IReleaseRepository db = new ReleaseRepository())
            {
                PublishRelease = db.WhereAndInclude(r => r.ReleaseID == ReleaseID, r => r.Account)
                                    .Include(r => r.ReleaseCPs.Select(cp => cp.CP)).FirstOrDefault();

                PublishCP = PublishRelease.ReleaseCPs.FirstOrDefault(cp => cp.CPID == CPID).CP.Name;

                db.GetEmployeesMailAddress(ReleaseID, isDraft).ToList().ForEach(s => ToEmail += s + "; ");

                PubliactionMail.To = ToEmail;
                PubliactionMail.BCC = "PRRDB_UAT_Admins@int.amdocs.com";
                // PubliactionMail.CC = "PRRDB_Admins@int.amdocs.com";
                string MailTypeSubject = isDraft ? "Draft " : "Official ";
                PubliactionMail.Subject = PublishRelease.Account.Name + " - Release " + PublishRelease.Name + " - " + PublishCP + " (" + MailTypeSubject + " - PREP ID " + ReleaseID.ToString() + ")";
                PubliactionMail.ReleaseID = ReleaseID;
                PubliactionMail.CPID = CPID;

                // upload status screenshot
                Files.UploadImage(base64string, "Status" + ReleaseID.ToString(), true);

                PubliactionMail.imagePath = VirtualPathUtility.ToAbsolute("~/Content/Images/ScoreImages") + "/Status" + ReleaseID.ToString() + ".png";

            }
            return PartialView("PopUpMail", PubliactionMail);

        }


        public async Task<ActionResult> SendPublicationMail(PublicationMailDetails mail, StatusVM status, bool IsDarft)
        {
            try
            {
                MailMessage message = new MailMessage();
                var SrcImage = VirtualPathUtility.ToAbsolute("~/Content/Images/ScoreImages") + "/Status" + mail.ReleaseID.ToString() + ".png";
                var attachmentPath = System.Web.HttpContext.Current.Server.MapPath(SrcImage);
                var contentID = mail.ReleaseID.ToString() + ".png";
                var src = "cid:" + contentID;
                var addresses = mail.To.Split(';');

                if (!IsDarft)
                    message.Bcc.Add(mail.BCC);

                addresses.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList().ForEach(item => message.To.Add(item));
                message.Subject = mail.Subject;
                message.Body = "<html>" +
                               "<body>" +
                               "<p style='color: black; font - family: calibary !important; font - size: 11px;'>" + (mail.beforeMessage != null ? mail.beforeMessage.Replace("\n", "<br/>") : "") + "</p>" +
                               "<img width='50px' height='70px' src='" + src + "' >" +
                               "<p style='color: black; font - family: calibary !important; font - size: 11px;'>" + (mail.afterMessage != null ? mail.afterMessage.Replace("\n", "<br/>") : "") + "</p>" +
                               "</body>" +
                               "</html>";

                // declaring image settings
                Attachment inline = new Attachment(attachmentPath);
                inline.ContentDisposition.Inline = true;
                inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                inline.ContentId = contentID;
                inline.ContentType.MediaType = "image/png";
                inline.ContentType.Name = Path.GetFileName(attachmentPath);
                message.Attachments.Add(inline);

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                message.IsBodyHtml = true;
           
                var FileName = message.SendMail("_PRR", !IsDarft);
                inline.Dispose();

                // formal publish - update Publicction details and save area scores in db
                if (!IsDarft) // is formal publish                
                {
                    await updatePublicationDetails(mail.ReleaseID, status.CPID, FileName);
                    await SaveAreaScore(status);
                }

                deletePublicationMailIMG(mail.imagePath);

                return Json(new { IsSent = true });
            }
            catch (Exception ex)
            {
                return Json(new { IsSent = false, Error = ex });
            }
        }


        public async Task updatePublicationDetails(int ReleaseID, int CPID, string FileName)
        {
            try
            {
                int count = 0;
                DAL.Models.Employee publishEmp;

                using (IEmployeeRepository dbEmp = new EmployeeRepository())
                {
                    publishEmp = dbEmp.Where(e => e.UserName == User.Identity.Name.Replace("NTNET\\", "")).FirstOrDefault();
                }

                using (IReleaseCPRepository db = new ReleaseCPRepository())
                {
                    var publishCP = db.Where(rcp => rcp.ReleaseID == ReleaseID & rcp.CPID == CPID).FirstOrDefault();
                    if (publishCP != null)
                    {
                        publishCP.PublicationMailDate = DateTime.Now;
                        publishCP.PublicationMail = FileName;
                        publishCP.SendByID = publishEmp.EmployeeID;
                        publishCP.SendByName = publishEmp.FirstName + " " + publishEmp.LastName;
                        publishCP.PublicationCount++;
                        count += await db.SaveAsync((WindowsPrincipal)User);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void deletePublicationMailIMG(string FilePath)
        {
            if (FilePath != null)
                Files.DeleteFile(Server.MapPath("~/" + FilePath));
        }

        public async Task<int> SaveAreaScore(StatusVM status)
        {
            int count = 0;
            using (IAreaScoreRepository db = new AreaScoreRepository())
            {
                status.AreaScores.ForEach(a =>
                {
                    var areaScore = db.Where(r => r.ReleaseID == status.ReleaseID && r.AreaID == a.AreaID && r.CPID == status.CPID).FirstOrDefault();
                    if (areaScore != null)//edit areaScore
                    {
                        if (areaScore.Score != a.Score)
                        {
                            areaScore.Score = a.Score;
                            db.Edit(areaScore);
                        }
                    }
                    else//add areaScore
                    {
                        areaScore = new AreaScore() { ReleaseID = status.ReleaseID, AreaID = a.AreaID, CPID = status.CPID, Score = a.Score };
                        db.Add(areaScore);
                    }
                });
                count += await db.SaveAsync((WindowsPrincipal)User);

                using (ISubAreaScoreRepository dbSubAreaScore = new SubAreaScoreRepository())
                {
                    status.AreaScores.ForEach(a =>
                    {
                        var areaScore = db.Where(r => r.ReleaseID == status.ReleaseID && r.AreaID == a.AreaID && r.CPID == status.CPID).FirstOrDefault();
                        a.SubAreaScors.ForEach(sas =>
                      {
                          var subAreaScore = dbSubAreaScore.Where(s => s.SubAreaID == sas.SubAreaID && s.AreaScoreID == areaScore.AreaScoreID).FirstOrDefault();
                          if (subAreaScore != null)//edit subAreaScore
                          {
                              if (subAreaScore.Score != sas.Score)
                              {
                                  subAreaScore.Score = sas.Score;
                                  dbSubAreaScore.Edit(subAreaScore);
                              }

                          }
                          else//add subAreaScore
                          {
                              subAreaScore = new SubAreaScore() { SubAreaID = sas.SubAreaID, AreaScoreID = areaScore.AreaScoreID, Score = sas.Score };
                              dbSubAreaScore.Add(subAreaScore);
                          }
                      });
                    });
                    count += await dbSubAreaScore.SaveAsync((WindowsPrincipal)User);
                }
            }
            return count;
        }


       //public async Task<int> SaveAreaScoreTemp(int ReleaseID, int CPID, StatusVM status)
       // {
       //     int currentPublishID, count = 0;
       //     Dictionary<int, int> AreaScoresIDs;

       //     try
       //     {
       //         using (IPublicationTempRepository db = new PublicationTempRepository())
       //         {
       //             db.Add(new PublicationTemp() { CPID = status.CPID, ReleaseID = status.ReleaseID, PublicationDate = DateTime.Now });
       //             count += await db.SaveAsync((WindowsPrincipal)User);
       //             currentPublishID = db.Where(p => p.ReleaseID == status.ReleaseID && p.CPID == status.CPID).OrderByDescending(p => p.PublicationDate).FirstOrDefault().PublicationID;
       //         }

       //         if (currentPublishID != 0)
       //         {
       //             using (IAreaScoreTempRepository db = new AreaScoreTempRepository())
       //             {
       //                 status.AreaScores.ForEach(a =>  {
       //                     db.Add(new AreaScoreTemp() { PublicationID = currentPublishID, AreaID = a.AreaID, Score = a.Score });
       //                 });
       //                 count += await db.SaveAsync((WindowsPrincipal)User);
       //                 AreaScoresIDs = db.Where(p => p.PublicationID == currentPublishID).ToDictionary(p => p.AreaID, p => p.AreaScoreTempID);
       //             }

       //             using (ISubAreaScoreTempRepository db = new SubAreaScoreTempRepository())
       //             {
       //                 status.AreaScores.ForEach(a => {
       //                     a.SubAreaScors.ForEach(s => {
       //                         db.Add(new SubAreaScoreTemp() { AreaScoreTempID = AreaScoresIDs[a.AreaID], SubAreaID = s.SubAreaID, Score = s.Score });
       //                     });
       //                 });
       //                 count += await db.SaveAsync((WindowsPrincipal)User);
       //             }
       //         }
       //         return currentPublishID;
       //     }
       //     catch (Exception ex)
       //     {

       //         throw ex;
       //     }
       // }

     
        //public ActionResult openOutlook(int ReleaseID, int CpID, string base64string, bool isDraft, StatusVM status)
        //{
        //    try
        //    {
        //        CP PublishCP;
        //        Release PublishRelease;
        //        MailMessage mailMessage = new MailMessage();
        //        int publicationID = 0;
        //        string MailTypeSubject = isDraft ? "Draft " : "Official ";
        //        var contentID = ReleaseID.ToString() + ".png";
        //        var src = "cid:" + contentID;

        //        // upload status screenshot
        //        var path = Files.UploadImage(base64string, "Status" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ReleaseID.ToString(), true);

        //        // get stakeholders and areaOwners mail addresses
        //        using (IReleaseRepository db = new ReleaseRepository())
        //        {
        //            PublishRelease = db.WhereAndInclude(r => r.ReleaseID == ReleaseID, r => r.Account)
        //                                .Include(r => r.ReleaseCPs.Select(cp => cp.CP)).FirstOrDefault();
        //            PublishCP = PublishRelease.ReleaseCPs.FirstOrDefault(cp => cp.CPID == CpID).CP;
        //            db.GetEmployeesMailAddress(ReleaseID, isDraft).ToList().ForEach(s => mailMessage.To.Add(s));
        //        }

        //        Task.Run(async () => { publicationID += await SaveAreaScoreTemp(ReleaseID, CpID, status); }).Wait();

        //        mailMessage.From = new MailAddress("GRPRRDBMAIL@amdocs.com");
        //        mailMessage.CC.Add("GRPRRDBMAIL@amdocs.com");
        //        mailMessage.Subject = PublishRelease.Account.Name + " - Release " + PublishRelease.Name + " - " + PublishCP.Name + " (" + MailTypeSubject + ": PREP ID - " + ReleaseID.ToString() + " , CP ID - " + PublishCP.CPID.ToString() + ", Publication ID - " + publicationID.ToString() + ")";

        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.Body = "<html>" +
        //                           "<body>" +
        //                           "<input type='hidden' value='test'" +
        //                           "<img width='50px' height='70px' src='" + src + "' >" +
        //                           "</body>" +
        //                           "</html>";

        //        Attachment inline = new Attachment(path);
        //        inline.ContentDisposition.Inline = true;
        //        inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
        //        inline.ContentId = contentID;
        //        inline.ContentType.MediaType = "image/png";
        //        inline.ContentType.Name = Path.GetFileName(path);
        //        mailMessage.Attachments.Add(inline);

        //        //     deletePublicationMailIMG(SrcImage);

        //        var img = mailMessage.SaveMailMessage();


        //        var filePath = "/PublicationMails/" + img;



        //        //RedirectToAction("downloadMail","Status",null);
        //        return RedirectToAction("downloadMail", new { filePath = filePath });
        //        // return Json(new { filePath = filePath});
        //    }
        //    catch (Exception ex)
        //    {
        //        EventLog.WriteEntry("PREP", "failed : " + ex, EventLogEntryType.Error);
        //        throw;
        //    }
        //}

       

        //public ActionResult downloadMail(string filePath)
        //{
        //    ViewBag.filePath = filePath;
        //    return PartialView("PopUpDownloadMail");
        //}
      


        #endregion
    }
}
