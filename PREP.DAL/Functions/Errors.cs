using System;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Security.Principal;

namespace PREP.DAL.Functions
{
    public static class Errors
    {
        const string sourceName = "PREP.DAL";
        public static string SQLScript = string.Empty;
        public static void Write(Exception ex, ObjectQuery result = null)
        {
            if (result != null)
                SQLScript = result.ToTraceString();
#if !DEBUG
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
            EventLog.WriteEntry(sourceName, string.Format("{0} {1}SQL Script: {2}", ex, Environment.NewLine, SQLScript), EventLogEntryType.Error);
#endif
        }
        public static void Write(string ErrorMessage)
        {
#if !DEBUG
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
            EventLog.WriteEntry(sourceName, ErrorMessage,EventLogEntryType.Error);
#endif
        }
        public static void SaveLastSQLScript(string sqlScript)
        {
            SQLScript = sqlScript;
        }
    }
}
