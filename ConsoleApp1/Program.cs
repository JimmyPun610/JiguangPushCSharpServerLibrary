using JPushLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            JPushServer server = new JPushServer("YOUR_APP_KEY", "YOUR_MASTER_SECRET");
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("key", "value");

            AudienceObject.RegistrationIDs regIDs = new AudienceObject.RegistrationIDs
            {
                registration_id = new string[] { "REGISTRATIONID" }
            };
          
            PushResult pushResult = server.Push("message", JPushLibrary.Enum.Platform.iOSProductionAndAndroid, regIDs, dict, JPushLibrary.Enum.AndroidPriority.PRIORITY_HIGH, JPushLibrary.Enum.AndroidAlertType.All);
            PushResult slientPushResult = server.PushSlient(dict, JPushLibrary.Enum.Platform.iOSProductionAndAndroid, null, JPushLibrary.Enum.AndroidPriority.PRIORITY_HIGH, "message");
        }
    }
}
