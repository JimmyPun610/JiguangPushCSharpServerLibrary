using System;
using System.Collections.Generic;
using System.Text;
using Jiguang;
using Newtonsoft.Json.Linq;

namespace JPushLibrary
{
    public class JPushServer
    {
        Jiguang.JPush.JPushClient jPushClient = null;
        public JPushServer(string appKey, string masterSecret)
        {
            jPushClient = new Jiguang.JPush.JPushClient(appKey, masterSecret);
        }
        private Jiguang.JPush.Model.IOS createIOSNotification(string message, string title, Dictionary<string, object> extraContent, string sound = "default", string badge = "+1")
        {
            JObject jobj = new JObject();
            jobj.Add("title", title);
            jobj.Add("body", message);
            return new Jiguang.JPush.Model.IOS
            {
                Alert = jobj,
                Badge = badge,
                Extras = extraContent,
                Sound = sound,
            };
        }
        private Jiguang.JPush.Model.Android createAndroidNotification(string message, string title, Enum.AndroidPriority priority, Dictionary<string, object> extraContent, Enum.AndroidAlertType alertType)
        {
            return new Jiguang.JPush.Model.Android
            {
                Alert = message,
                Extras = extraContent,
                Priority = (int)priority,
                AlertType = (int)alertType,
                Title = title
            };
        }
        private Jiguang.JPush.Model.IOS createSlientIOSNotification(string message, string title, Dictionary<string, object> extraContent, string badge = "+1")
        {
            JObject jobj = new JObject();
            jobj.Add("title", title);
            jobj.Add("body", message);
            return new Jiguang.JPush.Model.IOS
            {
                Extras = extraContent,
                ContentAvailable = true,
                Alert = jobj,
                Badge = badge
            };
        }
        private Jiguang.JPush.Model.Android createSlientAndroidNotification(string message, string title, Enum.AndroidPriority priority, Dictionary<string, object> extraContent, Enum.AndroidAlertType alertType)
        {
            return new Jiguang.JPush.Model.Android
            {
                Extras = extraContent,
                Priority = (int)priority,
                AlertType = (int)alertType,
                Alert = message,
                Title = title
            };
        }
        /// <summary>
        /// Push notification to client
        /// </summary>
        /// <param name="targetAudience">set the target audience</param>
        /// <param name="message">Message on notification</param>
        /// <param name="extraContent">Extra content on notification, will translate to json</param>
        /// <param name="androidPriority">High/Max : Will have banner in Android</param>
        /// <param name="androidAlertType">Indicator the notification method in Android</param>
        /// <param name="platform">indicate target platform</param>
        /// <returns>HttpResponse content</returns>
        public PushResult Push(string message, string title , Enum.Platform platform = Enum.Platform.iOSProductionAndAndroid, object targetAudience = null, Dictionary<string, object> extraContent = null
            , Enum.AndroidPriority androidPriority = Enum.AndroidPriority.PRIORITY_DEFAULT
            , Enum.AndroidAlertType androidAlertType = Enum.AndroidAlertType.All)
        {
            
            PushResult pushResult = null;
            Jiguang.JPush.Model.PushPayload payload = new Jiguang.JPush.Model.PushPayload();
            try
            {
                if(targetAudience != null)
                {
                    payload.Audience = targetAudience;
                }
                bool isApnsProduction = false;
                if (platform == Enum.Platform.iOSProduction || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    isApnsProduction = true;
                }
                payload.Options = new Jiguang.JPush.Model.Options
                {
                    IsApnsProduction = isApnsProduction
                };
                payload.Notification = new Jiguang.JPush.Model.Notification();
                if(platform == Enum.Platform.Android)
                {
                    payload.Notification.Android = createAndroidNotification(message, title, androidPriority, extraContent, androidAlertType);
                }else if(platform == Enum.Platform.iOSDevelopment || platform == Enum.Platform.iOSProduction)
                {
                    payload.Notification.IOS = createIOSNotification(message, title, extraContent);
                }else if(platform == Enum.Platform.iOSDevelopmentAndAndroid || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    payload.Notification.IOS = createIOSNotification(message, title, extraContent);
                    payload.Notification.Android = createAndroidNotification(message, title, androidPriority, extraContent, androidAlertType);
                }
                
                   
                    
                var result = jPushClient.SendPush(payload);
                pushResult = new PushResult(result.Content);
            }
            catch (Exception ex)
            {
                pushResult = new PushResult
                {
                    isSuccess = false,
                    errorMsg = ex.Message
                };
            }
          
            return pushResult;
        }
        /// <summary>
        /// Push a slient notification to client
        /// </summary>    
        /// <param name="targetAudience">set the target audience</param>
        /// <param name="extraContent">Extra content on the notification</param>
        /// <param name="platform">indicate target platform</param>
        /// <param name="androidPriority">High/Max : Will have banner in Android</param>
        /// <param name="message">The banner information. Pass in empty will not show any banner and disappear in notification center</param>
        /// <returns>HttpResponse content</returns>
        public PushResult PushSlient(Dictionary<string,object> extraContent, Enum.Platform platform = Enum.Platform.iOSProductionAndAndroid, object targetAudience = null
            , Enum.AndroidPriority androidPriority = Enum.AndroidPriority.PRIORITY_DEFAULT
            , string message = "", string title = "", Enum.AndroidAlertType alertType = Enum.AndroidAlertType.LightOnly)
        {
            PushResult pushResult = null;
            Jiguang.JPush.Model.PushPayload payload = new Jiguang.JPush.Model.PushPayload();

            try
            {
                if (targetAudience != null)
                {
                    payload.Audience = targetAudience;
                }
                bool isApnsProduction = false;
                if (platform == Enum.Platform.iOSProduction || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    isApnsProduction = true;
                }
                payload.Options = new Jiguang.JPush.Model.Options
                {
                    IsApnsProduction = isApnsProduction
                };
                payload.Notification = new Jiguang.JPush.Model.Notification();
                if (platform == Enum.Platform.Android)
                {
                    payload.Notification.Android = createSlientAndroidNotification(message, title, androidPriority, extraContent, alertType);
                }
                else if (platform == Enum.Platform.iOSDevelopment || platform == Enum.Platform.iOSProduction)
                {
                    payload.Notification.IOS = createSlientIOSNotification(message, title, extraContent);
                }
                else if (platform == Enum.Platform.iOSDevelopmentAndAndroid || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    payload.Notification.IOS = createSlientIOSNotification(message, title, extraContent);
                    payload.Notification.Android = createSlientAndroidNotification(message, title, androidPriority, extraContent, alertType);
                }

                var result = jPushClient.SendPush(payload);
                pushResult = new PushResult(result.Content);
            }
            catch (Exception ex)
            {
                pushResult = new PushResult
                {
                    isSuccess = false,
                    errorMsg = ex.Message
                };
            }

            return pushResult;
        }
    }
}
