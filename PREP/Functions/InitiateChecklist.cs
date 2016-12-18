using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PREP.Models.Functions
{
    public class InitiateChecklist
    {
        public static void InitiateChecklist1(int releaseID)
        {
            var vendorAreas = new VendorAreasRepository();
            if (new ReleaseCharacteristicRepository().Where(r => r.ReleaseID == releaseID && r.Characteristic.Name == "Vendor Management" && r.IsChecked).Count() > 0)
            {
                foreach (var area in new ReleaseAreaOwnerRepository().Where(a => a.ReleaseID == releaseID))
                {
                    vendorAreas.Add(new VendorAreas()
                    {
                        AreaID = area.ReleaseAreaOwnerID,
                        VendorID = new VendorRepository().FindBy(v => v.Name == "Amdocs").FirstOrDefault().VendorID,
                        IsChecked = true
                    });
                }
            }
            var aaa = new List<ReleaseChecklistAnswer>();
            foreach (var vendorArea in vendorAreas.GetAll())
            {
                foreach (var question in new QuestionRepository().Where(q => q.SubArea.AreaID == vendorArea.AreaID))
                {

                }
                //aaa.Add(new ReleaseChecklistAnswer()
                //{
                //    VendorArea = vendorArea,
                //    QuestionList = 
                //});
            }
        }
        //private bool IsAdded<T>(T questionParameter, int questionID)
        //{
        //    var a = new QuestionCharacteristicRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var b = new QuestionCPRevModeRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var c = new QuestionProductRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var d= new QuestionCPRevModeQRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var e = new QuestionStakeholderRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var f = new QuestionFamilyProductRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    var g = new QuestionSubAreaRepository().Where(q => q.QuestionID == questionID).OrderByDescending(q => q.AdminValue).FirstOrDefault();
        //    return (a.AdminValue == new ReleaseCharacteristicRepository().ToList().Exists(p=>p.CharacteristicID == a.CharacteristicID)< && p.IsChecked && a.AdminValue)
        //        || b.AdminValue == new ReleaseCPReviewModeRepository().ToList().Exists(p => p.CPReviewModeID == b.CPReviewModeID && p.IsChecked && b.AdminValue)
        //        || c.AdminValue == new ReleaseProductRepository().ToList().Exists(p => p.ProductID == c.ProductID && p.IsChecked && c.AdminValue)
        //        || d.AdminValue == new ReleaseCPReviewModeQRepository().ToList().Exists(p => p.CPReviewModeQID == d.CPReviewModeQID && p.IsFullTrack && d.AdminValue)
        //        || e.AdminValue == new ReleaseStakeholderRepository().ToList().Exists(p => p.StakeholderID == e.StakeholderID && p.IsChecked && e.AdminValue)
        //        || f.AdminValue == new ReleaseFamilyProductRepository().ToList().Exists(p => p.FamilyProductID == f.FamilyProductID && p.IsChecked && f.AdminValue)
        //        || g.AdminValue == new ReleaseAreaOwnerRepository().ToList().Exists(p => p.CharacteristicID == g.SubAreaID && p.IsChecked && g.AdminValue))
        //        && (a.AdminValue == new ReleaseCharacteristicRepository().ToList().Exists(p => p.CharacteristicID == a.CharacteristicID && !(p.IsChecked && a.AdminValue))
        //        || b.AdminValue == new ReleaseCPReviewModeRepository().ToList().Exists(p => p.CPReviewModeID == b.CPReviewModeID && (p.IsChecked && b.AdminValue))
        //        || c.AdminValue == new ReleaseProductRepository().ToList().Exists(p => p.ProductID == c.ProductID && p.IsChecked && c.AdminValue)
        //        || d.AdminValue == new ReleaseCPReviewModeQRepository().ToList().Exists(p => p.CPReviewModeQID == d.CPReviewModeQID && p.IsFullTrack && d.AdminValue)
        //        || e.AdminValue == new ReleaseStakeholderRepository().ToList().Exists(p => p.StakeholderID == e.StakeholderID && p.IsChecked && e.AdminValue)
        //        || f.AdminValue == new ReleaseFamilyProductRepository().ToList().Exists(p => p.FamilyProductID == f.FamilyProductID && p.IsChecked && f.AdminValue)
        //        || g.AdminValue == new ReleaseAreaOwnerRepository().ToList().Exists(p => p.CharacteristicID == g.SubAreaID && p.IsChecked && g.AdminValue)); 
        //}
    }
}