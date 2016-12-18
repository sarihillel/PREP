using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Functions
{
    public class NavigationBreadCrums
    {
        public static void SetSesionReleaseID(int ReleaseID)
        {
            if(ReleaseID==0)
            {
                System.Web.HttpContext.Current.Session["ReleaseID"] = null;
                System.Web.HttpContext.Current.Session["ReleaseName"] = null;
                return;
            }

            if (System.Web.HttpContext.Current.Session["ReleaseID"] == null || System.Web.HttpContext.Current.Session["ReleaseName"] == null || (int)System.Web.HttpContext.Current.Session["ReleaseID"] != ReleaseID)
            {
                using (IReleaseRepository db = new ReleaseRepository())
                {
                    System.Web.HttpContext.Current.Session["ReleaseName"] = db.GetSelect(r => r.ReleaseID == ReleaseID, r => new { r.Name }).FirstOrDefault().Name;
                    System.Web.HttpContext.Current.Session["ReleaseID"] = ReleaseID;
                }
            }
        }
    }
}