using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        //DateTime? HandlingStartDatecalculation(int? QuestionID, int? ReleaseID);
        IEnumerable<Object> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        Object GetField(int QuestionID);

        /// <summary>
        /// returns the next 10 available order in table
        /// </summary>
        /// <returns></returns>
        IEnumerable<Options> GetOrderTable();

        /// <summary>
        /// return Table By Type of Entity
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        Task<List<Options>> GetParametersOptions(string TableName, bool isDeleted);
        /// <summary>
        /// Return List View  of all QuestionsClassification List 
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="StartIndex"></param>
        /// <param name="Count"></param>
        /// <param name="Sorting"></param>
        /// <param name="Filtering"></param>
        /// <param name="CountRecords"></param>
        /// <returns></returns>
        IEnumerable<QuestionClasifications> GetQuestionClasificationByFiltering(int QuestionID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);

        /// <summary>
        /// get QuestionClasification Field By QuestionID and RecordID
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <param name="RecordID"></param>
        /// <returns></returns>
        QuestionClasifications GetQuestionClasificationField(int QuestionID, int ParameterID, string TableName);

        /// <summary>
        /// Add new Record By Get QuestionClasifications Object
        /// return if sucsses or not 
        /// </summary>
        /// <param name="Record"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        Task<int> AddQuestionClasificationAndSaveAsync(QuestionClasifications Record, WindowsPrincipal NTUser);

        /// <summary>
        /// Edit QuestionClasifications and Save 
        /// </summary>
        /// <param name="QuestionClasifications"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        Task<int> EditQuestionClasificationAndSaveAsync(QuestionClasifications QuestionClasifications, WindowsPrincipal NTUser);

        /// <summary>
        /// Delete Record By Get QuestionClasifications Object
        /// return if sucsses or not 
        /// </summary>
        /// <param name="Record"></param>
        /// <param name="NTUser"></param>
        /// <returns></returns>
        Task<int> DeleteQuestionClasificationAndSaveAsync(QuestionClasifications Record, WindowsPrincipal NTUser);
        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="ReleaseQuestion"></param>
        Task SetInitiateQuestions(Release CurrentRelease, List<int> ReleaseQuestion, WindowsPrincipal user);
        Task<int> DeleteQuestionAndSaveAsync(Question Record, WindowsPrincipal NTUser);//new
        bool IsQuestionHasParameters(int QuestionId);//new

    }

}
