using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PREP.DAL.Functions.Extensions;
using PREP.DAL.Functions;

namespace PREP.DAL.TableViews
{
    public class QuestionClasifications
    {
        #region Properties
        public static List<String> _Tables;
        public static List<String> Tables
        {
            get
            {
                if (_Tables == null)
                {
                    using (ITableRepository TableDB = new TableRepository())
                    {
                        string[] TablesName =
                        {
                            typeof(Characteristic).Name,
                            typeof(FamilyProduct).Name,
                            typeof(Product).Name,
                            typeof(Stakeholder).Name,
                            typeof(Area).Name
                        };

                        _Tables = TableDB.GetSelect<String>((col => TablesName.Contains(col.Name)), c => c.Name).ToList();
                    }
                }
                return _Tables;
            }

        }

        public int QuestionID { get; set; }
        public string TableName { get; set; }
        public int RecordID { get; set; }
        public AdminValue AdminValue { get; set; }
        public int ParameterID { get; set; }
        public string ParameterName { get; set; }
        public string Comments { get; set; }
        public ParameterType ParameterType { get; set; }
        #endregion



        #region Public Methods
        /// <summary>
        /// return Table By Type of Entity
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        internal async static Task<List<Options>> GetParametersOptions(string TableName, bool isDeleted = true)
        {

            //Expression<Func<, bool>> where = (x => x.isDeleted);
            List<Options> Options = null;
            try
            {

                if (TableName == typeof(Characteristic).Name)
                {
                    using (ICharacteristicRepository db = new CharacteristicRepository())
                    {
                        if (isDeleted == true)
                            Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.CharacteristicID });
                        Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.CharacteristicID }, (x => !x.IsDeleted));

                    }
                }
                else if (TableName == typeof(FamilyProduct).Name)
                {
                    using (IFamilyProductRepository db = new FamilyProductRepository())
                    {
                        if (isDeleted == true)
                            Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.FamilyProductID });
                        Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.FamilyProductID }, (x => !x.IsDeleted));
                    }
                }
                else if (TableName == typeof(Product).Name)
                {
                    using (IProductRepository db = new ProductRepository())
                    {
                        if (isDeleted == true)
                            Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.ProductID });
                        Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.ProductID }, (x => !x.IsDeleted));
                    }
                }
                else if (TableName == typeof(Stakeholder).Name)
                {
                    using (IStakeholderRepository db = new StakeholderRepository())
                    {
                        if (isDeleted == true)
                            Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.StakeholderID });
                        Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.StakeholderID }, (x => !x.IsDeleted));
                    }
                }
                else if (TableName == typeof(Area).Name)
                {
                    using (IAreaRepository db = new AreaRepository())
                    {
                        if (isDeleted == true)
                            Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.AreaID });
                        Options = await db.GetOptions(o => new Options() { DisplayText = o.Name, Value = o.AreaID }, (x => !x.IsDeleted));
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return Options;
        }

        /// <summary>
        /// add QuestionClasifications and Save 
        /// </summary>
        /// <param name="QuestionClasifications"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        internal async static Task<int> AddSaveAsync(QuestionClasifications QuestionClasifications, WindowsPrincipal NTUser)
        {

            int Count = 0;
            try
            {
                string TableName = QuestionClasifications.TableName;
                if (TableName == null)
                    return -1;
                else if (TableName == typeof(Characteristic).Name)
                {
                    using (IQuestionCharacteristicRepository db = new QuestionCharacteristicRepository())
                    {
                        var Question = new QuestionCharacteristic()
                        {
                            QuestionID = QuestionClasifications.QuestionID,
                            CharacteristicID = QuestionClasifications.ParameterID,
                            AdminValue = QuestionClasifications.AdminValue,
                            Comments = QuestionClasifications.Comments
                        };
                        db.Add(Question);
                        Count += await db.SaveAsync(NTUser);
                        QuestionClasifications.RecordID = Question.QuestionCharacteristicID;
                    }
                }
                else if (TableName == typeof(FamilyProduct).Name)
                {
                    using (IQuestionFamilyProductRepository db = new QuestionFamilyProductRepository())
                    {
                        var Question = new QuestionFamilyProduct()
                        {
                            QuestionID = QuestionClasifications.QuestionID,
                            FamilyProductID = QuestionClasifications.ParameterID,
                            AdminValue = QuestionClasifications.AdminValue,
                            Comments = QuestionClasifications.Comments
                        };
                        db.Add(Question);
                        Count += await db.SaveAsync(NTUser);
                        QuestionClasifications.RecordID = Question.QuestionFamilyProductID;
                    }
                }
                else if (TableName == typeof(Product).Name)
                {
                    using (IQuestionProductRepository db = new QuestionProductRepository())
                    {
                        var Question = new QuestionProduct()
                        {
                            QuestionID = QuestionClasifications.QuestionID,
                            ProductID = QuestionClasifications.ParameterID,
                            AdminValue = QuestionClasifications.AdminValue,
                            Comments = QuestionClasifications.Comments
                        };
                        db.Add(Question);
                        Count += await db.SaveAsync(NTUser);
                        QuestionClasifications.RecordID = Question.QuestionProductID;
                    }
                }
                else if (TableName == typeof(Stakeholder).Name)
                {
                    using (IQuestionStakeholderRepository db = new QuestionStakeholderRepository())
                    {
                        var Question = new QuestionStakeholder()
                        {
                            QuestionID = QuestionClasifications.QuestionID,
                            StakeholderID = QuestionClasifications.ParameterID,
                            AdminValue = QuestionClasifications.AdminValue,
                            Comments = QuestionClasifications.Comments
                        };
                        db.Add(Question);
                        Count += await db.SaveAsync(NTUser);
                        QuestionClasifications.RecordID = Question.QuestionStakeholderID;
                    }
                }
                else if (TableName == typeof(Area).Name)
                {
                    using (IQuestionAreaRepository db = new QuestionAreaRepository())
                    {
                        var Question = new QuestionArea()
                        {
                            QuestionID = QuestionClasifications.QuestionID,
                            AreaID = QuestionClasifications.ParameterID,
                            AdminValue = QuestionClasifications.AdminValue,
                            Comments = QuestionClasifications.Comments
                        };
                        db.Add(Question);
                        Count += await db.SaveAsync(NTUser);
                        QuestionClasifications.RecordID = Question.QuestionAreaID;
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                Count = -1;
            }

            return Count;

        }
        /// <summary>
        /// Edit QuestionClasifications and Save 
        /// </summary>
        /// <param name="QuestionClasifications"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        internal async static Task<int> EditSaveAsync(QuestionClasifications QuestionClasifications, WindowsPrincipal NTUser)
        {
            int Count = 0;
            try
            {
                string TableName = QuestionClasifications.TableName;
                if (TableName == null)
                    return -1;
                else if (TableName == typeof(Characteristic).Name)
                {
                    using (IQuestionCharacteristicRepository db = new QuestionCharacteristicRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        Question.AdminValue = QuestionClasifications.AdminValue;
                        Question.Comments = QuestionClasifications.Comments;
                        db.Edit(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(FamilyProduct).Name)
                {
                    using (IQuestionFamilyProductRepository db = new QuestionFamilyProductRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        Question.AdminValue = QuestionClasifications.AdminValue;
                        Question.Comments = QuestionClasifications.Comments;
                        db.Edit(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Product).Name)
                {
                    using (IQuestionProductRepository db = new QuestionProductRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        Question.AdminValue = QuestionClasifications.AdminValue;
                        Question.Comments = QuestionClasifications.Comments;
                        db.Edit(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Stakeholder).Name)
                {
                    using (IQuestionStakeholderRepository db = new QuestionStakeholderRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        Question.AdminValue = QuestionClasifications.AdminValue;
                        Question.Comments = QuestionClasifications.Comments;
                        db.Edit(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Area).Name)
                {
                    using (IQuestionAreaRepository db = new QuestionAreaRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        Question.AdminValue = QuestionClasifications.AdminValue;
                        Question.Comments = QuestionClasifications.Comments;
                        db.Edit(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                Count = -1;
            }

            return Count;

        }

        /// <summary>
        /// Delete QuestionClasifications and Save 
        /// </summary>
        /// <param name="QuestionClasifications"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        internal async static Task<int> DeleteSaveAsync(QuestionClasifications QuestionClasifications, WindowsPrincipal NTUser)
        {
            int Count = 0;
            try
            {

                string TableName = QuestionClasifications.TableName;
                if (TableName == null)
                    return -1;
                else if (TableName == typeof(Characteristic).Name)
                {
                    using (IQuestionCharacteristicRepository db = new QuestionCharacteristicRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        db.Delete(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(FamilyProduct).Name)
                {
                    using (IQuestionFamilyProductRepository db = new QuestionFamilyProductRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        db.Delete(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Product).Name)
                {
                    using (IQuestionProductRepository db = new QuestionProductRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        db.Delete(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Stakeholder).Name)
                {
                    using (IQuestionStakeholderRepository db = new QuestionStakeholderRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        db.Delete(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
                else if (TableName == typeof(Area).Name)
                {
                    using (IQuestionAreaRepository db = new QuestionAreaRepository())
                    {
                        var Question = db.Find(QuestionClasifications.QuestionID, QuestionClasifications.ParameterID);
                        db.Delete(Question);
                        Count += await db.SaveAsync(NTUser);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                Count = -1;
            }

            return Count;

        }

        /// <summary>
        /// Return Query View of all QuestionsClassification List
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="Sorting"></param>
        /// <param name="Filtering"></param>
        /// <returns></returns>
        internal static IQueryable<QuestionClasifications> SelectQuestionClasificationsView(DbContext Context, Expression<Func<QuestionClasifications, bool>> Expression = null)
        {
            IQueryable<QuestionClasifications> Query = null;

            Query = Context.Set<QuestionCharacteristic>()
                    .Include(c => c.Characteristic)
                    .Select(Tbl => new QuestionClasifications()
                    {
                        QuestionID = Tbl.QuestionID,
                        RecordID = Tbl.QuestionCharacteristicID,
                        ParameterID = Tbl.CharacteristicID,
                        ParameterName = Tbl.Characteristic.Name,
                        TableName = "Characteristic",
                        AdminValue = Tbl.AdminValue,
                        ParameterType = Tbl.Characteristic.Type,
                        Comments = Tbl.Comments

                    }
                    );

            Query = Query.Union(Context.Set<QuestionFamilyProduct>()
                                    .Include(c => c.FamilyProduct)
                     .Select(Tbl => new QuestionClasifications()
                     {
                         QuestionID = Tbl.QuestionID,
                         RecordID = Tbl.QuestionFamilyProductID,
                         ParameterID = Tbl.FamilyProduct.FamilyProductID,
                         ParameterName = Tbl.FamilyProduct.Name,
                         TableName = "FamilyProduct",
                         AdminValue = Tbl.AdminValue,
                         ParameterType = Tbl.FamilyProduct.Type,
                         Comments = Tbl.Comments
                     }
                     ));
            Query = Query.Union(Context.Set<QuestionProduct>()
                                    .Include(c => c.Product)
                     .Select(Tbl => new QuestionClasifications()
                     {
                         QuestionID = Tbl.QuestionID,
                         RecordID = Tbl.QuestionProductID,
                         ParameterID = Tbl.Product.ProductID,
                         ParameterName = Tbl.Product.Name,
                         TableName = "Product",
                         AdminValue = Tbl.AdminValue,
                         ParameterType = Tbl.Product.Type,
                         Comments = Tbl.Comments
                     }
                     ));


            Query = Query.Union(Context.Set<QuestionStakeholder>()
                                    .Include(c => c.Stakeholder)
                     .Select(Tbl => new QuestionClasifications()
                     {
                         QuestionID = Tbl.QuestionID,
                         RecordID = Tbl.QuestionStakeholderID,
                         ParameterID = Tbl.Stakeholder.StakeholderID,
                         ParameterName = Tbl.Stakeholder.Name,
                         TableName = "Stakeholder",
                         AdminValue = Tbl.AdminValue,
                         ParameterType = Tbl.Stakeholder.Type,
                         Comments = Tbl.Comments
                     }
                     ));

            Query = Query.Union(Context.Set<QuestionArea>()
                                    .Include(c => c.Area)
                     .Select(Tbl => new QuestionClasifications()
                     {
                         QuestionID = Tbl.QuestionID,
                         RecordID = Tbl.QuestionAreaID,
                         ParameterID = Tbl.Area.AreaID,
                         ParameterName = Tbl.Area.Name,
                         TableName = "Area",
                         AdminValue = Tbl.AdminValue,
                         ParameterType = Tbl.Area.Type,
                         Comments = Tbl.Comments
                     }
                     ));

            if (Expression != null) Query = Query.Where(Expression);

            return Query;
        }

        #endregion

    }
}

