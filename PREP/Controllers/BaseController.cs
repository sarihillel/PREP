using OfficeOpenXml;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using PREP.Functions;
using PREP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PREP.Controllers
{
    //[MyAuthorize(Roles = @"NTNET\PRRDBAdminIMIS")]
    public class BaseController : Controller
    {
        private IEmployeeRepository db = new EmployeeRepository();
        /// <summary>
        /// Export data to xlsx file
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="data">List</param>
        public void ExportToExcel<T>(List<T> data)
        {
            var grid = new GridView();
            grid.DataSource = data;
            grid.DataBind();
            ExcelPackage excel = new ExcelPackage();
            string itemName = typeof(T).Name;
            var workSheet = excel.Workbook.Worksheets.Add(itemName);
            var totalRows = grid.Rows.Count;
            var headerRow = grid.HeaderRow;
            if (totalRows > 0)
            {
                var totalCols = grid.Rows[0].Cells.Count;
                for (var i = 1; i <= totalCols; i++)
                {
                    workSheet.Cells[1, i].Value = headerRow.Cells[i - 1].Text;
                }
                for (var j = 1; j <= totalRows; j++)
                {
                    for (var i = 1; i <= totalCols; i++)
                    {
                        var item = data.ElementAt(j - 1);
                        workSheet.Cells[j + 1, i].Value = item.GetType().GetProperty(headerRow.Cells[i - 1].Text).GetValue(item, null);
                    }
                }
            }
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + itemName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult GetContentEmployee(jQueryDataTableParamModel param, string name, string code)
        {
            var list = db.GetEmployeeByNameOrId(name, code);

            var result = Json(new
            {
                param.sEcho,
                iTotalRecords = list.Count(),
                iTotalDisplayRecords = list.Count(),
                aaData = list
            }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;

        }

    }
}