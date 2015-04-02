using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;


public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    //[Microsoft.SqlServer.Server.SqlTrigger(Name = "SNMPMonitorRowInsertedTrigger")]
    public static void SNMPMonitorRowInsertedTrigger()
    {
        try
        {
            SqlTriggerContext myContext = SqlContext.TriggerContext;
            if (myContext.TriggerAction == TriggerAction.Insert)
            {
                Uri uri = new Uri("http://152.96.56.75/Data/RowInsertedTrigger");
                WebClient client = new WebClient();
                SqlCommand command;
                SqlDataReader reader;
                string values = "{param:[";
                using (SqlConnection connection
                   = new SqlConnection(@"context connection=true"))
                {
                    connection.Open();
                    command = new SqlCommand(@"SELECT * FROM INSERTED;",
                       connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            values += "{name:\"" + reader.GetName(i) + "\",value:\"" + reader.GetValue(i) + "\"},";
                        }
                    } 
                    
                    reader.Close();
                    
                }
                values += "]}";




                string param = "param=" + values;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.UploadString(uri,"POST", param);
                /*client.UploadValues(uri, "POST", new NameValueCollection()
                {
                    {"param", param}
                });*/
                /*
                SqlPipe sqlP = SqlContext.Pipe;
                sqlP.Send(myContext.ColumnCount.ToString());
                */

                /*client.UploadValues(uri,"POST", new NameValueCollection()
                {
                    {"param",myContext.EventData.Value}
                });*/
                /*SqlPipe sqlP = SqlContext.Pipe;
                sqlP.Send(myContext.EventData.Value);*/
                //client.OpenRead(uri).Close();
            }
        }
        catch (Exception exc)
        {
            SqlPipe sqlP = SqlContext.Pipe;
            sqlP.Send("Fehler: " + exc.Message);
        }
    }
}
