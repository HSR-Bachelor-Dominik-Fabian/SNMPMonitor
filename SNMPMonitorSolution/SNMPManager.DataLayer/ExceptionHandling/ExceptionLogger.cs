using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SNMPManager.DataLayer.ExceptionHandling
{
    public class ExceptionLogger
    {
        public static void LogException(string category, Exception exc)
        {
            string absolute_path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), Properties.Settings.Default.LogPath);
            Uri uri = new Uri(absolute_path);
            string path = Path.GetFullPath(uri.AbsolutePath);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            TextWriterTraceListener listener = new TextWriterTraceListener(path);
            Trace.Listeners.Add(listener);

            Trace.WriteLine("SNMP Manager Exception");
            Trace.Indent();
            Trace.WriteLine("ExceptionType: " + exc.GetType().ToString());
            Trace.WriteLine("Category: " + category);
            Trace.WriteLine("Timestamp: " + DateTime.Now.ToString());
            Trace.WriteLine("HResult" + exc.HResult);
            Trace.WriteLine("Message: " + exc.Message);
            Trace.WriteLine("StackTrace: " + exc.StackTrace);
            Trace.Unindent();
            Trace.WriteLine("End Exception SNMP Manager");
            Trace.WriteLine("----");
            listener.Flush();
            listener.Close();
        }

        public static void SaveExceptionToDB(string category, Exception exc)
        {

        }
    }
}
