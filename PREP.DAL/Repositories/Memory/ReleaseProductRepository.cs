using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseProductRepository : GenericRepository<PREPContext, ReleaseProduct>, IReleaseProductRepository
    {
        public List<ReleaseProduct> AddProduct()
        {
            List<ReleaseProduct> ReleaseProduct = new List<ReleaseProduct> { };
            using (IProductRepository dbProduct = new ProductRepository())
            {
                //get all milestone who's active
                IEnumerable<Product> ReleaseProducts = dbProduct.Get(f => f.IsDeleted == false); //m => m.IsVisible == true

                //in save needs get releaseid and add it
                ReleaseProducts.ToList().ForEach(fp => ReleaseProduct.Add(new ReleaseProduct() { ProductID = fp.ProductID, Product = fp }));

            }
            return ReleaseProduct;

        }
        public IEnumerable<string> GetAllFamilyProductsNamesByRealeaseId(int? releaseId)
        {
            if (releaseId != null)
            {
                List<string> rp;
                using (IReleaseFamilyProductRepository db = new ReleaseFamilyProductRepository())
                {
                     rp=  db.Where(a => a.ReleaseID == releaseId && a.IsChecked == true && a.FamilyProduct.Products.Count() > 0).Select(z => z.FamilyProduct.Name).ToList();
                }
                return rp;
            }
            return null;
        }
    }
}
