using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPushLibrary
{
    public class PushResult
    {
        public bool isSuccess { get; set; }
        public string errorMsg { get; set; }
        public int errorCode { get; set; }
        public string msgID { get; set; }
        public PushResult() { }
        public PushResult(string pushResultStr)
        {
            JObject resultObj = JObject.Parse(pushResultStr);
            bool isSuccess = resultObj["error"] == null ? true : false;
            this.isSuccess = isSuccess;
            this.msgID = resultObj["msg_id"]?.ToString();
            if (!isSuccess)
            {
                this.errorMsg = resultObj["error"]?["message"]?.ToString();
                this.errorCode = int.Parse(resultObj["error"]?["code"]?.ToString());
            }
        }
    }
}
