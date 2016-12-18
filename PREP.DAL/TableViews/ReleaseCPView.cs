using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PREP.DAL.Models;

namespace PREP.DAL.TableViews
{
    public class ReleaseCPView 
    {
        public ReleaseCPView()
        {
                
        }
        public ReleaseCPView(ReleaseCP releaseCP, string accountName)
        {
            if (releaseCP == null)
                return;
            ReleaseCPID = releaseCP.ReleaseCPID;
            CPID = releaseCP.CPID;
            CPName = releaseCP.CP.Name;
            ReleaseName = releaseCP.Release.Name;
            // CP = releaseCP.CPID,
            //  PublicationID = releaseCP.PublicationID,
            ReleaseID = releaseCP.ReleaseID;
            //  Release = releaseCP.ReleaseID,
            AccountName = accountName;
            //PlannedDate = TableCp.PlannedDate ,
            PublicationMailDate = releaseCP.PublicationMailDate;
            PublicationMailLink = releaseCP.PublicationMail;
            ExceptionIndicator = releaseCP.ExceptionIndicator ?? false;
            ExceptionDate = releaseCP.ExceptionDate;
            ExceptionRemarks = releaseCP.ExceptionRemarks;
            DelayReason = releaseCP.DelayReason;
            DelayReasonHistory = releaseCP.DelayReasonHistory;
            //DelayDays = TableCp.DelayDay,
            Comments = releaseCP.Comments;
            IsDeleted = releaseCP.IsDeleted;
            PublicationCount = releaseCP.PublicationCount;
            PublicationMailDate = releaseCP.PublicationMailDate;
            using (IEmployeeRepository db=new EmployeeRepository())
            {
                SendByID = db.GetMDMCodeByEmpId(releaseCP.SendByID!=null?(int)releaseCP.SendByID:0);
            }
           
            SendByName =  releaseCP.SendByName;
        }
        #region Properties
        //
        [DisplayName("Release CP ID:")]
        public int ReleaseCPID { get; set; }
        //
        [DisplayName("Release ID:")]
        public int ReleaseID { get; set; }
        //
        [DisplayName("Release Name:")]
        public string ReleaseName { get; set; }
        [DisplayName("CP ID:")]
        public int CPID { get; set; }
        //
        [DisplayName("CP Name:")]
        public string CPName { get; set; }
        public int? PublicationID { get; set; }
        //
        [DisplayName("Exception Indicator:")]
        public bool? ExceptionIndicator { get; set; }
        //
        [DisplayName("Exception Date:")]
        public DateTime? ExceptionDate { get; set; }
        //
        [DisplayName("Exception Remarks:")]
        public string ExceptionRemarks { get; set; }

        [DisplayName("Delay Reason:")]
        public string DelayReason { get; set; }
        [DisplayName("Delay Reason History:")]
        public string DelayReasonHistory { get; set; }
        //
        [DisplayName("Comments:")]
        public string Comments { get; set; }
        [DisplayName("Is Visible:")]
        public bool IsDeleted { get; set; }
        //
        [DisplayName("Publication Mail Date:")]
        public DateTime? PublicationMailDate { get; set; }
        //
        [DisplayName("Publication Mail Link:")]
        public string PublicationMailLink { get; set; }
        //public string PublicationMailLinkText { get; set; }
        [DisplayName("Publication Count:")]
        public int PublicationCount { get; set; }
        [DisplayName("Send By ID:")]
        public int? SendByID { get; set; }
        [DisplayName("Send By Name:")]
        public string SendByName { get; set; }
        //
        [DisplayName("Account Name:")]
        public string AccountName { get; set; }

        #endregion

        #region Calculated Properties
        //
        [NotMapped]
        [DisplayName("Delay Days:")]
      
        public int? DelayDays
        {
            get
            {
                int? value = null;
                if (PlannedDate.HasValue)
                {
                    value = ((PublicationMailDate ?? DateTime.Now) - (PlannedDate ?? DateTime.Now)).Days; ;
                }
                value = (value.HasValue && value.Value < 0) ? 0 : value;
                return value;
            }
            set { }

        }

        #region PlannedDate
        //value who's calculated
        private DateTime? _plannedDate;
        //Is calculate at the first time
        private bool IsPlannedDate;
        //return _plannedDate , if not initial at all calculate into _plannedDate
        //
        [NotMapped]
        [DisplayName("Planned Date:")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PlannedDate
        {
            get
            {
                DateTime? value = null;
                if (!IsPlannedDate)
                {
                    IsPlannedDate = true;
                    if (ExceptionIndicator==true)
                        value = ExceptionDate;
                    else
                        switch (CPName.Trim().ToLower())
                        {
                            case "sdr1 proposal":
                                value = GetMilestonDate("Proposal Delivery");
                                break;
                            case "sdr2 kick off":
                                value = GetMilestonDate("Scoping");
                                break;
                            case "sdr3 contract sign off":
                                value = GetMilestonDate("Contract Sign off");
                                break;
                            case "sdr4 targets":
                                value = GetMilestonDate("Construct");
                                break;
                            case "sdr5 deployment":
                                value = GetMilestonDate("AT");
                                break;
                            case "sdr6 operate":
                                value = GetMilestonDate("Production");
                                break;
                            case "sdr7 bau":
                                value = GetMilestonDate("BAU");
                                break;
                        }
                }
                else
                    value = _plannedDate;

                _plannedDate = value;
                return value;
            }
        }
        #endregion

        [Column("ReleaseName")]
        public int ReleaseID1
        {
            get { return ReleaseID; }
            set { ReleaseID = value; }
        }
        [Column("CPName")]
        public int CPID1
        {
            get { return CPID; }
            set
            {
                CPID = value;
            }
        }

        #endregion

        #region Private Methods
        private DateTime? GetMilestonDate(string Name)
        {
            DateTime? Date = null;
            using (IReleaseMilestoneRepository db = new ReleaseMilestoneRepository())
            {
                Date = db.GetMilestoneDate(ReleaseID, Name);
                Date = (Date == null) ? null : (DateTime?)Date.Value.AddDays(-7);
            }
            return Date;
        }
        #endregion
    }
}
