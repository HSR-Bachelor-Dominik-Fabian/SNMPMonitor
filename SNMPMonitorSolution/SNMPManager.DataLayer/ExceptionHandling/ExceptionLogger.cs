using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer.ExceptionHandling
{
    public class ExceptionLogger
    {
        public static void LogException(string category, Exception exc)
        {
            string sEvent;

            sEvent = "SNMP Manager Exception\nCategory: " + category + "\nMessage: " + exc.Message + "\n\nStackTrace: " + exc.StackTrace;
            string absolute_path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), Properties.Settings.Default.LogPath);
            Uri uri = new Uri(absolute_path);
            string path = Path.GetFullPath(uri.AbsolutePath);
            TextWriterTraceListener listener = new TextWriterTraceListener(path);
            Debug.Listeners.Add(listener);

            Debug.WriteLine("SNMP Manager Exception");
            Debug.Indent();
            Debug.WriteLine(sEvent);
            Debug.Unindent();
            Debug.WriteLine("End Exception SNMP Manager");
            listener.Flush();
            listener.Close();
        }

        public static void SaveExceptionToDB(string category, Exception exc)
        {

        }
    }
}
