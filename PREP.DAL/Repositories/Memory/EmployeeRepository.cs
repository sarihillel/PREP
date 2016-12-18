using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class EmployeeRepository : GenericRepository<PREPContext, Employee>, IEmployeeRepository
    {
        /// <summary>
        /// Returns list of employees based on Employee Name and Employee Id
        /// </summary>
        /// <param name="name">Emp Name</param>
        /// <param name="code">Emp Code</param>
        /// <returns></returns>
        public IEnumerable<Object> GetEmployeeByNameOrId(string name, string code)
        {
            IEnumerable<Object> baseList = new List<Object>();
            string[] id = null;
            if (!string.IsNullOrEmpty(code))
            {
                id = code.Split(',');
            }

            if (!string.IsNullOrEmpty(name))
            {
                baseList = DbSet
                           .Where(e => e.MDMCode > 0 && ((string.IsNullOrEmpty(name) == false && (e.FirstName.Contains(name.ToLower()) || e.LastName.Contains(name.ToLower()) || (e.FirstName.Trim().ToLower() + " " + e.LastName.Trim().ToLower()).Contains(name.Trim().ToLower())))))
                           .Select(emp => new
                           {
                               EmployeeID = emp.EmployeeID,
                               FirstName = emp.FirstName,
                               LastName = emp.LastName,
                               MDMCode = emp.MDMCode,
                               Email = emp.Email,
                               UserName = emp.UserName,
                           }).ToList();
                return baseList;
            }
            if (!string.IsNullOrEmpty(code))
            {

                //baseList = (from emp in ctx.Employee
                //            where SqlFunctions.StringConvert((double)emp.MDMCode.Value).Contains(code)
                //            select new Employee

                baseList = DbSet
                    .Where(e => SqlFunctions.StringConvert((double)e.MDMCode).Contains(code))
                    .Select(emp => new
                    {
                        EmployeeID = emp.EmployeeID,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        MDMCode = emp.MDMCode,
                        Email = emp.Email,
                        UserName = emp.UserName,
                    }).ToList();

                return baseList;
            }
            return baseList;
        }
        public string GetNameByUsername(string name)//for masterPage to show the employee
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '\\' };
            string[] arr = name.Split(delimiterChars);
            string s = arr[1].ToLower();
            var employee = DbSet.AsNoTracking().Where(a => a.UserName != "" && (a.UserName.ToLower() == s)).FirstOrDefault();
            return employee != null ? employee.FirstName + " " + employee.LastName : "";
        }
        public string GetFullNameByEmpId(int empId)
        {
            if (empId == 0) return null;
            var e = DbSet.AsNoTracking().Where(a => a.EmployeeID == empId).FirstOrDefault();
            if (e != null)
            {
                return e.FirstName+" "+e.LastName;
            }
            return null;
        }
        public int? GetMDMCodeByEmpId(int empId)
        {
            if (empId == 0) return null;
            var e = DbSet.AsNoTracking().Where(a => a.EmployeeID == empId).FirstOrDefault();
            if (e != null)
            {
                return e.MDMCode;
            }
            return null;
        }
        public int? GetEmpIdByMDMCode(int? mdmCode)
        {
            if (mdmCode == null)
                return 0;
            return DbSet.AsNoTracking().Where(a => a.MDMCode == mdmCode).FirstOrDefault().EmployeeID;
        }

        public int GetEmployee(string NTUser)
        {
            NTUser = NTUser.Replace("NTNET\\", "").Trim();
            if (NTUser != "")
            {
                Employee Emp = FindSingle(prop => prop.UserName.ToUpper() == NTUser.ToUpper());
                if (Emp == null) return -1;
                else return Emp.EmployeeID;
            }
            return -1;
        }
        public Employee GetEmployeeNameByEmail(string UserEmail)
        {
            
            if (UserEmail != "")
            {
                Employee Emp = FindSingle(prop => prop.Email.ToLower() == UserEmail.ToLower());
                if (Emp == null)
                    return null;
                else return Emp;
            }
            return null;
        }
    }
}
