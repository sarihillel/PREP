using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.DAL.TableViews;
using PREP.Functions;
using PREP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PREP.Controllers
{
    public class CheckListController : BaseController
    {
        // GET: CheckList
      
        public ActionResult Index()
        {

            return View();
        }
        //[Route("ReleaseCheckList/{ReleaseID}")]
      
        public ActionResult ViewCheckList(int ReleaseID)
        {
            CheckListVM checkListVM = new CheckListVM();
            ViewBag.includeBootsrap = false;
            checkListVM.ReleaseID = ReleaseID;

            //for rout config navigationBreadCrums
            NavigationBreadCrums.SetSesionReleaseID(ReleaseID);

            using (IEmployeeRepository db = new EmployeeRepository())
            {
                checkListVM.UserID = db.GetEmployee(User.Identity.Name);
            }
            //using (IAreaRepository db = new AreaRepository())
            //{
            //    checkListVM.AreaList = db.GetAll();
            //}

            checkListVM.Responsibility = JsonConvert.SerializeObject(
                    Enum.GetValues(typeof(Responsibility))
                    .Cast<Responsibility>().ToDictionary(t => ((int)t).ToString(), t => Enum.GetName(typeof(Responsibility), (int)t)), new KeyValuePairConverter());

            var RiskLevels = Enum.GetValues(typeof(RiskLevels))
                .Cast<RiskLevels>().ToDictionary(t => (int)t, t => EnumExtentions<RiskLevels>.GetDisplayValue(t));

            ViewBag.RiskLevel = RiskLevels;

           checkListVM.RiskLevelList =JsonConvert.SerializeObject(RiskLevels, new KeyValuePairConverter());
            return View(checkListVM);
        }

        [HttpPost]
        public JsonResult getSubAreaListByAreaID(int areaId)
        {
            // var y = convertValue("0", "Responsibility");
            IEnumerable<SubArea> list;
            using (ISubAreaRepository db = new SubAreaRepository())
            {
                list = db.getSubAreaByAreaId(areaId);
            }
            return Json(list);
        }
        //[HttpPost]
        //public DateTime? GetStartHandlingDateCalculate(int releaseId, int questionId)
        //{
        //    using (IQuestionRepository db = new QuestionRepository())
        //    {
        //        return db.HandlingStartDatecalculation(questionId, releaseId);
        //    }
        //}
        [HttpPost]
        public object GetActivityLogByReleaseCheckListId(int releaseChecklistAnswer)
        {
            IEnumerable<History> list;
            using (IHistoryRepository db = new HistoryRepository())
            {
                list = db.GetHistoryReleaseCheckListId(releaseChecklistAnswer);
            }
            string s = "";
            string changeBy;
            foreach (History item in list)
            {
                PREP.DAL.Models.Employee employee = item.ActivityLog.Employee;
                changeBy = employee.FirstName.ToLower() == "system" ? " was changed by the system" : " was changed by " + employee.FirstName + " " + employee.LastName;
                s += getFieldName(item.FieldName) + changeBy + " From " +
                      convertValue(item.OldValue, item.FieldName) +
                        " to " + convertValue(item.NewValue, item.FieldName) + " on " + item.ActivityLog.Date + ". <br/>" + Environment.NewLine;
            }
            return s;
        }

        private string convertValue(string value, string fieldName)
        {
            if (value == String.Empty)
                return "null";

            //GetWhere
            if (fieldName == "AreaID")
                using (IAreaRepository db = new AreaRepository())
                {
                    return db.Find(Int32.Parse(value)).Name;
                }
            if (fieldName == "SubAreaID")
                using (ISubAreaRepository db = new SubAreaRepository())
                {
                    return db.Find(Int32.Parse(value)).Name;
                }
            if (fieldName == "QuestionOwner")
                using (IEmployeeRepository db = new EmployeeRepository())
                {
                    DAL.Models.Employee emp = db.Find(Int32.Parse(value));
                    return emp.FirstName + " " + emp.LastName;
                }
            if (fieldName == "RiskLevelID")
              //  return EnumExtentions<RiskLevels>.GetDisplayValue(value);
               return EnumExtentions<RiskLevels>.GetDisplayValue((RiskLevels)Int32.Parse(value));

            if (fieldName == "Responsibility")
                return Enum.GetName(typeof(Responsibility), Int32.Parse(value));

            return value;
        }


        private string getFieldName(string fieldName)
        {
            switch (fieldName)
            {
                case "RiskLevelID": return "Risk Level";
                case "AreaID": return "Area";
                case "SubAreaID": return "Sub Area";
                case "QuestionText": return "Question Text";
                case "ActualComplation": return "Actual Complation";
                case "HandlingStartDate": return "Handling Start Date";
                case "QuestionOwner": return "Question Owner";
                default: return fieldName;
            }
        }
        public PartialViewResult ViewFilter()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetCheckList(int ReleaseID)
        {
           
            using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
            {
                return Json(db.GetByFiltering(ReleaseID).ToList());
                // while (List.Count < 300) List.AddRange(List);
            }

           // return Json(List);
        }

        [HttpPost]
        public async Task<int> SaveCheckList(List<ReleaseChecklistAnswerView> Checklist)
        {
            var count = 0;
            if (Checklist != null)
            {
                using (IReleaseChecklistAnswerRepository db = new ReleaseChecklistAnswerRepository())
                {
                    count = await db.EditReleaseChecklist(Checklist, (WindowsPrincipal)User);
                    // while (List.Count < 300) List.AddRange(List);
                }
            }
            return count;
        }
    }
}