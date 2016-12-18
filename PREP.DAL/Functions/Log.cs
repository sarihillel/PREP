//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using PREP.DAL.Models;
//using PREP.DAL.Repositories;
//using System.Security.Principal;
//using System.Reflection;
//using System.Threading.Tasks;
//using PREP.DAL.Repositories.Memory;

//namespace PREP.DAL.Functions
//{
//    public class Log
//    {
//        private ITableRepository tableRep = new TableRepository();
//        private IEmployeeRepository EmpRep = new EmployeeRepository();
//        private IActivityLogRepository db = new ActivityLogRepository();
//        private IHistoryRepository HistRep = new HistoryRepository();
//        //ActivityLog activityLog = new ActivityLog();

//        Table table = new Table();

//        public async Task<bool> UpdateLogs(object entity, int EntityID, int? releaseId, List<History> Histories, ActivityType ActivityType, string NTUser)
//        {
//            bool result = true;
//            try
//            {
//                for (int i = 0; i < Histories.Count; i++)
//                {
//                    if (await UpdateHistory(Histories[i]) == false) result = false;
//                    if (await UpdateActivityLog(entity, EntityID, releaseId, Histories[i].HistoryID, ActivityType, NTUser) == false) result = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                Errors.Write(ex);
//                result = false;
//            }
//            return result;
//        }
//        public async Task<bool> UpdateHistory(History History)
//        {
//            try
//            {
//                HistRep.Add(History);
//                await HistRep.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                Errors.Write(ex);
//                return false;
//            }
//            return true;
//        }
//        public async Task<bool> UpdateActivityLog(object entity, int EntityID, int? releaseId, int HistoryID, ActivityType ActivityType, string NTUser)
//        {

//            //in sql server call stored procedure 'exec AddNewTablesToTable' if you added new Table to database
//            var table = entity.GetType();
            
//            var TableDB = tableRep.FindSingle(prop => prop.Name == table.Name || prop.Name == table.BaseType.Name);
            
//            if (TableDB == null)
//            {
//                Errors.Write("Table " + table + " not exists in [Table]");
//                return false;
//            }
//            try
//            {
//                activityLog.ReleaseID = releaseId;
//                activityLog.TableID = TableDB.TableID;
//                var employee = EmpRep.GetEmployee(NTUser);
//                if (employee != -1)
//                activityLog.EmployeeID = employee;
//                activityLog.Date = DateTime.Now;
//                activityLog.HistoryID = HistoryID;
//                activityLog.ActivityID = ActivityType;
//                activityLog.ItemID = EntityID;
                
//                db.Add(activityLog);
//                await db.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                Errors.Write(ex);
//                return false;
//            }
//            return true;
//        }
//    }
//}