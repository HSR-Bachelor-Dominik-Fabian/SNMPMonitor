using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text;


public partial class Triggers
{
    public static void AgentUpdatedTrigger()
    {
        try
        {
            SqlTriggerContext context = SqlContext.TriggerContext;
            if (context.TriggerAction == TriggerAction.Update)
            {
                Uri uri = new Uri("http://152.96.56.75/Data/AgentUpdatedTrigger");
                WebClient client = new WebClient();
                SqlCommand command;
                SqlDataReader reader;
                string values = "{param:[";
                bool hasRows = false;
                using (SqlConnection connection = new SqlConnection(@"context connection=true"))
                {
                    connection.Open();
                    command = new SqlCommand(@"SELECT i.AgentNr, i.Name, i.IPAddress, i.Status, i.Port, i.sysDesc, i.sysName, i.sysUptime FROM INSERTED i;",
                       connection);
                    reader = command.ExecuteReader();
                    hasRows = reader.HasRows;
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i).Equals("sysDesc") || reader.GetName(i).Equals("sysName") || reader.GetName(i).Equals("sysUptime"))
                            {
                                values += "{\"" + reader.GetName(i) + "\":" + reader.GetValue(i) + "},";
                            }
                            else
                            {
                                values += "{\"" + reader.GetName(i) + "\":\"" + reader.GetValue(i) + "\"},";
                            }
                        }
                    }

                    reader.Close();

                }
                values += "]}";



                if (hasRows)
                {
                    string param = "param=" + values;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.UploadString(uri, "POST", param);
                }
            }
        }
        catch (Exception exc)
        {
            SqlPipe sqlP = SqlContext.Pipe;
            sqlP.Send("Fehler UpdatedTrigger: " + exc.Message);
        }
    }
    
    public static void MonitorDataInsertedTrigger()
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
                    command = new SqlCommand(@"SELECT i.AgentNr, mt.ObjectID, i.Result, i.MonitorTimestamp FROM INSERTED i INNER JOIN MonitoringType mt ON i.MonitoringTypeNr = mt.MonitoringTypeNr;",
                       connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if(reader.GetName(i).Equals("Result")) {
                                values += "{\"" + reader.GetName(i) + "\":" + reader.GetValue(i) + "},";
                            }
                            else
                            {
                                values += "{\"" + reader.GetName(i) + "\":\"" + reader.GetValue(i) + "\"},";
                            }
                        }
                    } 
                    
                    reader.Close();
                    
                }
                values += "]}";




                string param = "param=" + values;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.UploadString(uri,"POST", param);
            }
        }
        catch (Exception exc)
        {
            SqlPipe sqlP = SqlContext.Pipe;
            sqlP.Send("Fehler: " + exc.Message);
        }
    }

    public static void NewEventTrigger()
    {
        try
        {
            SqlTriggerContext myContext = SqlContext.TriggerContext;
            if (myContext.TriggerAction == TriggerAction.Insert)
            {
                Uri uri = new Uri("http://152.96.56.75/Data/NewEventTrigger");
                WebClient client = new WebClient();
                SqlCommand command;
                SqlDataReader reader;
                string values = "{param:[";
                using (SqlConnection connection
                   = new SqlConnection(@"context connection=true"))
                {
                    connection.Open();
                    command = new SqlCommand(@"SELECT * FROM INSERTED i;",
                       connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i).Equals("Message") || reader.GetName(i).Equals("Stacktrace")) 
                            {
                                values += "{\"" + reader.GetName(i) + "\":\"" + cleanForJSON(reader.GetValue(i).ToString()) +"\"},";
                            }
                            else
                            {
                                values += "{\"" + reader.GetName(i) + "\":\"" + reader.GetValue(i) + "\"},";
                            }
                        }
                    }
                    reader.Close();
                }
                values += "]}";
                string param = "param=" + values;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.UploadString(uri, "POST", param);
            }
        }
        catch (Exception exc)
        {
            SqlPipe sqlP = SqlContext.Pipe;
            sqlP.Send("Fehler: " + exc.Message);
        }
    }

    public static void InsertDeleteTrigger()
    {
        try
        {
            SqlTriggerContext myContext = SqlContext.TriggerContext;
            if (myContext.TriggerAction == TriggerAction.Insert || myContext.TriggerAction == TriggerAction.Delete)
            {
                Uri uri = new Uri("http://152.96.56.75/Data/InsertDeleteTrigger");
                WebClient client = new WebClient();
               
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.UploadString(uri, "POST", myContext.TriggerAction.ToString());
            }
        }
        catch (Exception exc)
        {
            SqlPipe sqlP = SqlContext.Pipe;
            sqlP.Send("Fehler InsertDeleteTrigger: " + exc.Message);
        }
    }

    public static string cleanForJSON(string s)
    {
        if (s == null || s.Length == 0)
        {
            return "";
        }

        char c = '\0';
        int i;
        int len = s.Length;
        StringBuilder sb = new StringBuilder(len + 4);
        String t;

        for (i = 0; i < len; i += 1)
        {
            c = s[i];
            switch (c)
            {
                case '\\':
                case '"':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '/':
                    sb.Append('\\');
                    sb.Append(c);
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                default:
                    if (c < ' ')
                    {
                        t = "000" + String.Format("X", c);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }
        return sb.ToString();
    }
}
