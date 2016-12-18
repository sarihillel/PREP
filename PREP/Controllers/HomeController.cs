
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text;
using System.Data.Entity;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.Models;
using PREP.Functions;

namespace PREP.Controllers
{
    //[RequireHttps]

    public class HomeController : BaseController
    {
        const string sourceName = "PREP";
//PRRDBAdminIMIS ITPrepTest
    
        public ActionResult Index()
        {
            //throw new SystemException();
            //AddAlert(new Alert
            //{
            //    Title = "Error",
            //    Message = "Looks like something went wrong. Please check your form.",
            //    AlertStyle = AlertStyles.Information,
            //    AlertConfirm = new AlertConfirm() { ButtonText1 = AlertButton.Ok }
            //});

            //ScoreCalculationLogic.ScoreCalculation(1);
            return View();
        }

        public ActionResult Autocomplete(String term = null)
        {

            // List<Releases> Releases = new List<Releases>();
            // using (ReleaseRepository Db = new ReleaseRepository())
            // {
            //     Db.GetReleaseJoinAccount().ToList().ForEach(ra => Releases.Add(new Releases(ra)));

            // }
            // Releases.Where(r => (r.ReleaseID.ToString().Contains(term)
            // || r.AccountName.Contains(term)
            // || r.PrepFPName.Contains(term)
            // || r.ReleaseName.Contains(term)
            // || r.SPNameEmployee.Contains(term)
            // || r.ProgramMeEmployee.Contains(term)
            //  || r.ProductionStartDate.Contains(term)
            //)
            // );
            // Releases= Releases.Skip(10).ToList();
            // return Json(Releases, JsonRequestBehavior.AllowGet);
            using (IReleaseRepository db = new ReleaseRepository())
            {
                List<String> filteredItems = new List<String>();
                var filter = db.GetReleaseJoinAccountBySearch(term);
                if (filter == null) return null;
                filter.ToList().ForEach(
                    item => filteredItems.Add(item.Name)
                    );

                //foreach (var item in collection)
                //{


                //}
                // var filteredItems = items.Where(
                //item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                //);
                return Json(filteredItems, JsonRequestBehavior.AllowGet);
            }



        }
        [HttpPost]
        public ActionResult Index(string userSearch)
        {
            TempData["userSearch"] = userSearch;
            return RedirectToAction("Releases", "Index");
            //            return View();
        }

       
    }
}