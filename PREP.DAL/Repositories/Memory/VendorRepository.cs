using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.TableViews;
using System.Diagnostics;

namespace PREP.DAL.Repositories.Memory
{
    public class VendorRepository : GenericRepository<PREPContext, Vendor>, IVendorRepository
    {
        // Add Related Release and Areas to Vendor
        public async Task<int> EditVendor(Vendor currentVendor, Release currentRelease, WindowsPrincipal user)
        {
            int result = 1;
            var ExistVendor = DbSet.Where(v => v.VendorID == currentVendor.VendorID)
                                .Include(v => v.ReleaseVendors)
                                .Include(v => v.VendorAreass).FirstOrDefault();
            // update VendorArea
            foreach (var newObj in currentVendor.VendorAreass)
            {
                var DBObj = ExistVendor.VendorAreass.FirstOrDefault(e => e.AreaID == newObj.AreaID);
                if (DBObj == null)
                    ExistVendor.VendorAreass.Add(newObj);
            }

            // update ReleaseVendor
            var newVendorRelease = currentVendor.ReleaseVendors.First();
            var ObjDB = ExistVendor.ReleaseVendors.FirstOrDefault(e => e.ReleaseVendorID == newVendorRelease.ReleaseVendorID);
            if (ObjDB != null)
                Context.Entry(ObjDB).CurrentValues.SetValues(newVendorRelease);
            else
                ExistVendor.ReleaseVendors.Add(newVendorRelease);

            result = await SaveAsync(user);
            if (result != -1)
               await GetVendorQuestions(currentVendor.VendorID, currentRelease, user);

            return result;
        }

        public async Task GetVendorQuestions(int VendorID, Release currentRelease, WindowsPrincipal User)
        {
            try
            {
                Release updatedRelease;
                using (IReleaseRepository db = new ReleaseRepository())
                {

                    updatedRelease = db.WhereAndInclude(r => r.ReleaseID == currentRelease.ReleaseID, r => r.ReleaseVendors).FirstOrDefault();
                }
                var vendorIDs = updatedRelease.ReleaseVendors.Select(rv => rv.VendorID).ToArray();
                IEnumerable<Vendor> Vendors = DbSet.Where(v => vendorIDs.Contains(v.VendorID))
                                                .Include(v => v.VendorAreass)
                                                .Include(v => v.VendorAreass.Select(a => a.Area.QuestionAreas)).ToList();

                List<int> AreaIDs = new List<int>();
                List<int> ReleaseQuestions = new List<int>();

                Vendors.ToList().ForEach(v =>
                                    v.VendorAreass.ToList().ForEach(e =>
                                    {
                                        var IsReleaseArea = currentRelease.ReleaseAreaOwners.Any(a => a.AreaID == e.AreaID && a.IsChecked == true);
                                        if (IsReleaseArea)
                                            AreaIDs.Add(e.AreaID);
                                    }));

                List<int> ReleaseQuestion=new List<int>();
                if (AreaIDs.Count > 0)
                {
                    using (IQuestionRepository db = new QuestionRepository())
                    {
                        ReleaseQuestions = db.Where(q => AreaIDs.Contains(q.SubArea.AreaID)).Select(q => q.QuestionID).ToList();
                    }

                    var Qustions = QuestionClasifications.SelectQuestionClasificationsView(Context);
                    var AreaQuestions = Qustions.Where(q => ReleaseQuestions.Contains(q.QuestionID)).ToList();
                    List<QuestionAreaView> arrQuestions = new List<QuestionAreaView>();
                    AreaQuestions.ToList().ForEach(e =>
                    {
                        AdminValue userValue = AdminValue.NO;
                        switch (e.TableName)
                        {
                            case "Product":
                                {
                                    var obj = currentRelease.ReleaseProducts.Count > 0 ? currentRelease.ReleaseProducts.FirstOrDefault(p => p.ProductID == e.ParameterID) : null;
                                    userValue = obj != null && obj.IsChecked ? AdminValue.YES : AdminValue.NO;
                                    break;
                                }
                            case "FamilyProduct":
                                {
                                    var obj = currentRelease.ReleaseFamilyProducts.Count > 0 ? currentRelease.ReleaseFamilyProducts.FirstOrDefault(p => p.FamilyProductID == e.ParameterID) : null;
                                    userValue = obj != null && obj.IsChecked ? AdminValue.YES : AdminValue.NO;
                                    break;
                                }
                            case "Characteristic":
                                {
                                    var obj = currentRelease.ReleaseCharacteristics.Count > 0 ? currentRelease.ReleaseCharacteristics.FirstOrDefault(p => p.CharacteristicID == e.ParameterID) : null;
                                    userValue = obj != null && obj.IsChecked ? AdminValue.YES : AdminValue.NO;
                                    break;
                                }
                            case "Area":
                                {
                                    var obj = currentRelease.ReleaseAreaOwners.Count > 0 ? currentRelease.ReleaseAreaOwners.FirstOrDefault(p => p.AreaID == e.ParameterID) : null;
                                    userValue = obj != null && obj.IsChecked ? AdminValue.YES : AdminValue.NO;
                                    break;
                                }
                            case "Stakeholder":
                                {
                                    var obj = currentRelease.ReleaseStakeholders.Count > 0 ? currentRelease.ReleaseStakeholders.FirstOrDefault(p => p.StakeholderID == e.ParameterID && p.EmployeeID1 != null) : null;
                                    userValue = obj != null ? AdminValue.YES : AdminValue.NO;
                                    break;
                                }
                            default:
                                break;


                        };

                        arrQuestions.Add(new QuestionAreaView()
                        {

                            TableName = e.TableName,
                            ParameterID = e.ParameterID,
                            AdminValue = e.AdminValue,
                            ParameterType = e.ParameterType,
                            QuestionID = e.QuestionID,
                            UserValue = userValue
                        });
                    });

                    var AddQuestion = arrQuestions.Where(q => q.ParameterType == ParameterType.Add && (q.UserValue == AdminValue.YES && q.AdminValue == AdminValue.YES)).Select(q => q.QuestionID).Distinct();
                    var removeQuestions = arrQuestions.Where(q => q.ParameterType == ParameterType.Remove && (q.AdminValue != q.UserValue)).Select(q => q.QuestionID).Distinct();
                    ReleaseQuestion = AddQuestion.Where(q => !removeQuestions.Contains(q)).ToList();
                    //if (ReleaseQuestion.Count > 0)
                    //{
                        
                    //}
                }
                using (IQuestionRepository db = new QuestionRepository())
                {
                    await db.SetInitiateQuestions(currentRelease, ReleaseQuestion, (WindowsPrincipal)User);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
