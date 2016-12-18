using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IReleaseProductRepository : IGenericRepository<ReleaseProduct>
    {
        List<ReleaseProduct> AddProduct();
        IEnumerable<string> GetAllFamilyProductsNamesByRealeaseId(int? releaseId);
    }

}
