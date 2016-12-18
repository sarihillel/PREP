using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Functions
{
    public class OutputDebugView
    {
        public DateTime Date;
        public string Name;
        public string OutPut;
        public long Remainder;

    }
    public class OutputDebug
    {
        static List<OutputDebugView> ListDate = new List<OutputDebugView>();
        public static void Write(string Name)
        {
            var DebugString = "";
            var date = DateTime.Now;

            OutputDebugView OutputDebugView = new OutputDebugView();
            OutputDebugView.Date = DateTime.Now;
            OutputDebugView.Name = Name;
            DebugString = Name + ":" + Environment.NewLine;
            if (ListDate.Count == 0)
            {
                DebugString = DebugString + " Init Date Debug..";
                OutputDebugView.Remainder = 0;
            }
            else
            {
                var LastDate = ListDate.Last().Date;
                OutputDebugView.Remainder = (date.Ticks - LastDate.Ticks);
                DebugString = DebugString + " Hefresh:" + OutputDebugView.Remainder.ToString();
            }
            DebugString = DebugString + "Current Date:" + date.ToString()  + Environment.NewLine;
            OutputDebugView.OutPut = DebugString;
            ListDate.Add(OutputDebugView);

        }


        public static void WriteToOutput()
        {
            foreach (var item in ListDate)
            {
                Debug.WriteLine(item.OutPut);
            }
            ListDate = ListDate.OrderByDescending(c => c.Remainder).ToList();
            Debug.WriteLine(" Ordering Save:");
            foreach (var item in ListDate)
            {
                Debug.WriteLine( " Name:" + item.Name +" Time:" +item.Date.ToShortTimeString()  + " Remainder:"+ item.Remainder);
            }
            var MaxRemainder = ListDate.First();
            Debug.WriteLine("Max Time:" + MaxRemainder.Name + " Remainder:" + MaxRemainder.Remainder);
            ListDate.Clear();
        }
    }
}
