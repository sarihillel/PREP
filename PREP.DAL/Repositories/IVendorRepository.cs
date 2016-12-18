using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IVendorRepository : IGenericRepository<Vendor>
    {
        Task<int> EditVendor(Vendor currentVendor, Release currentRelease, WindowsPrincipal user);
    }
}
