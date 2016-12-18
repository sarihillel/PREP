using PREP.DAL.Models;
using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PREP.DAL.TableViews
{


    /*Note: 
        on change Record Name or Type Please Change in \PREP\Views\CheckList\ViewCheckList.cshtml:
        1. in constractor of new checklistRecords()
        2: in  SortBy select Value
        3. in _FilterModal PartialView:  data-record-name of tabs Names
    */

    public class ReleaseChecklistAnswerView
    {
        public int ReleaseChecklistAnswerID { get; set; }
        public int ReleaseID { get; set; }
        public string ReleaseName { get; set; }
        public int QuestionID { get; set; }
        public string QuestionCode { get; set; }
        public int QuestionMilestoneID { get; set; }
        public string QuestionMilestoneName { get; set; }
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public int SubAreaID { get; set; }
        public string SubAreaName { get; set; }
        public string QuestionText { get; set; }
        public string QuestionInfo { get; set; }
        public int? QuestionOwnerID { get; set; }
        public string QuestionOwnerName { get; set; }
        public DateTime? HandlingStartDate { get; set; }
        public DateTime? LastAutomaticUpdateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        [DefaultValue(0)]
        public int ActualComplation { get; set; }
        public RiskLevels RiskLevelID { get; set; }
        public string ExtrenalFocalPoint { get; set; }
        public Responsibility Responsibility { get; set; }
        public string Comments { get; set; }
        public int QuestionOrder { get; set; } 
        public bool IsEdited { get; set; }

    }
}
