using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseFamilyProductRepository : GenericRepository<PREPContext, ReleaseFamilyProduct>, IReleaseFamilyProductRepository
    {
        public List<ReleaseFamilyProduct> AddFamilyProduct()
        {
            List<ReleaseFamilyProduct> ReleaseFamilyProduct = new List<ReleaseFamilyProduct> { };
            using (IFamilyProductRepository dbFamilyProduct = new FamilyProductRepository())
            {
                //get all family Product who's active
                IEnumerable<FamilyProduct> FamilyProducts = dbFamilyProduct.Get(f => f.IsDeleted == false); //m => m.IsVisible == true

                //in save needs get releaseid and add it
                FamilyProducts.ToList().ForEach(fp => ReleaseFamilyProduct.Add(new ReleaseFamilyProduct() { FamilyProductID = fp.FamilyProductID, FamilyProduct = fp }));

            }
            return ReleaseFamilyProduct;

        }
    }
    
}
