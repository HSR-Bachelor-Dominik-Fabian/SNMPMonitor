using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net;
using System.Diagnostics;


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
                WebClient client = new WebClient();
                client.OpenRead(uri);
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }
    }
}
