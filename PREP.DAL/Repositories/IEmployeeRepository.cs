using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<Object> GetEmployeeByNameOrId(string name, string code);
        string GetNameByUsername(string name);
        int? GetMDMCodeByEmpId(int empId);
        int? GetEmpIdByMDMCode(int? mdmCode);
        int GetEmployee(string NTUser);
        string GetFullNameByEmpId(int empId);
        Employee GetEmployeeNameByEmail(string UserEmail);
    }

}
