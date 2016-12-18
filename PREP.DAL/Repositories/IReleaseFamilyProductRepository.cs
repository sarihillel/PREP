using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseFamilyProductRepository : IGenericRepository<ReleaseFamilyProduct>
    {
        List<ReleaseFamilyProduct> AddFamilyProduct();
    }
}
