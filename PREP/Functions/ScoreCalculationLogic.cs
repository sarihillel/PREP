using PREP.DAL.Models;
using PREP.DAL.Repositories.Memory;
using PREP.Models;
using System.Collections.Generic;
using System.Linq;

namespace PREP.Functions
{
    public class ScoreCalculationLogic
    {
        //public static List<SubAreaScoreVM> ScoreCalculation(int releaseID)
        //{
        //    var checklist = (new ReleaseChecklistAnswerRepository()).Where(q => q.ReleaseID == releaseID);
        //    var subAreaScores = new List<SubAreaScoreVM>();
        //    foreach (var question in checklist.Where(q => q.RiskLevel.Value > 0).GroupBy(q => q.Question.SubAreaID))
        //    {
        //        var reductionList = question.Where(q => q.RiskLevel.Value == 0).OrderByDescending(q => q.HandlingStartDate);
        //        int highCount = reductionList.Skip(1).Where(q => q.RiskLevel.RiskLevelID == RiskLevels.High).Count();
        //        SubAreaScoreVM subAreaScore = new SubAreaScoreVM()
        //        {
        //            SubArea = question.FirstOrDefault().Question.SubArea,
        //            Score = question.Average(q => q.RiskLevel.Value),
        //            Reduction = (reductionList.FirstOrDefault().RiskLevel.RiskLevelID == RiskLevels.High ? 10 : 30) +//TODO - Check reduction issue and order
        //                (reductionList.Skip(1).Count() - highCount) * 6 + highCount * 2
        //        };
        //        subAreaScore.FinalScore = subAreaScore.Score * (100 - subAreaScore.Reduction) / 100;
        //        subAreaScore.Color = subAreaScore.FinalScore > 89.5 ? ScoreColor.Green : subAreaScore.FinalScore > 69.5 ? ScoreColor.Yellow : ScoreColor.Red;
        //        subAreaScores.Add(subAreaScore);
        //    }
        //    return subAreaScores;
        //}
    }
}