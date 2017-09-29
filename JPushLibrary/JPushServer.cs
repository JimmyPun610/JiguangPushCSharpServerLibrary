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
        private Jiguang.JPush.Model.IOS createIOSNotification(string message, Dictionary<string, object> extraContent, string sound = "default", string badge = "+1")
        {
            return new Jiguang.JPush.Model.IOS
            {
                Alert = message,
                Badge = badge,
                Extras = extraContent,
                Sound = sound,
            };
        }
        private Jiguang.JPush.Model.Android createAndroidNotification(string message, Enum.AndroidPriority priority, Dictionary<string, object> extraContent, Enum.AndroidAlertType alertType)
        {
            return new Jiguang.JPush.Model.Android
            {
                Alert = message,
                Extras = extraContent,
                Priority = (int)priority,
                AlertType = (int)alertType
            };
        }
        private Jiguang.JPush.Model.IOS createSlientIOSNotification(string message , Dictionary<string, object> extraContent, string badge = "+1")
        {
            return new Jiguang.JPush.Model.IOS
            {
                Extras = extraContent,
                ContentAvailable = true,
                Alert = message,
                Badge = badge
            };
        }
        private Jiguang.JPush.Model.Android createSlientAndroidNotification(string message, Enum.AndroidPriority priority, Dictionary<string, object> extraContent, Enum.AndroidAlertType alertType)
        {
            return new Jiguang.JPush.Model.Android
            {
                Extras = extraContent,
                Priority = (int)priority,
                AlertType = (int)alertType,
                Alert = message
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
        public PushResult Push(string message, Enum.Platform platform = Enum.Platform.iOSProductionAndAndroid, object targetAudience = null, Dictionary<string, object> extraContent = null
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
                    payload.Notification.Android = createAndroidNotification(message, androidPriority, extraContent, androidAlertType);
                }else if(platform == Enum.Platform.iOSDevelopment || platform == Enum.Platform.iOSProduction)
                {
                    payload.Notification.IOS = createIOSNotification(message, extraContent);
                }else if(platform == Enum.Platform.iOSDevelopmentAndAndroid || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    payload.Notification.IOS = createIOSNotification(message, extraContent);
                    payload.Notification.Android = createAndroidNotification(message, androidPriority, extraContent, androidAlertType);
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
            , string message = "", Enum.AndroidAlertType alertType = Enum.AndroidAlertType.LightOnly)
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
                    payload.Notification.Android = createSlientAndroidNotification(message, androidPriority, extraContent, alertType);
                }
                else if (platform == Enum.Platform.iOSDevelopment || platform == Enum.Platform.iOSProduction)
                {
                    payload.Notification.IOS = createSlientIOSNotification(message, extraContent);
                }
                else if (platform == Enum.Platform.iOSDevelopmentAndAndroid || platform == Enum.Platform.iOSProductionAndAndroid)
                {
                    payload.Notification.IOS = createSlientIOSNotification(message, extraContent);
                    payload.Notification.Android = createSlientAndroidNotification(message, androidPriority, extraContent, alertType);
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
