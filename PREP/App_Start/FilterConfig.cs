﻿
using PREP.Models;
using System.Web;
using System.Web.Mvc;

namespace PREP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           filters.Add(new CustomHandleErrorAttribute());

          //  filters.Add(new RequireHttpsAttribute());
        }
    }
}
