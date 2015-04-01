using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.IO;


public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    //[Microsoft.SqlServer.Server.SqlTrigger(Name = "SNMPMonitorRowInsertedTrigger", Event = "FOR INSERT")]
    public static void SNMPMonitorRowInsertedTrigger()
    {
        try
        {
            SqlTriggerContext myContext = SqlContext.TriggerContext;
            if (myContext.TriggerAction == TriggerAction.Insert)
            {
                Uri uri = new Uri("http://152.96.56.75/Data/RowInsertedTrigger");
                Stream dataStream;
                WebRequest request = WebRequest.Create(uri);
                request.Method = "POST";

                byte[] byteArray = Encoding.UTF8.GetBytes(myContext.EventData.Value);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";

                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                // Get the request stream.
                dataStream = request.GetRequestStream();

                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);

                // Close the Stream object.
                dataStream.Close();

            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }
    }
}
