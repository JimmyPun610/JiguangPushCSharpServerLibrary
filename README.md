# JiguangPushCSharpServer
Based on C# server sdk to implement some API for push and slient push to target audience with registration ID

Based on SDK from Jiguang https://docs.jiguang.cn/jpush/server/sdk/csharp_sdk/
You may check for sample for more information.

Parameter information:

1. AndroidPriority 

  This indicator refer to Android notification priority
  To show notification banner on Android, use PRIORITY_HIGH. To only show in notification center, use PRIORITY_DEFAULT

2. AndroidAlertType

  This indicator refer to Jiguang push alert type, select the desire type for own purpose. Default is All.
  https://docs.jiguang.cn/jpush/server/push/rest_api_v3_push/#notification


Notes for slient push notification
  There are two kinds of slient push, one will appear in notification center and another one will not.
  Pass an empty message to method PushSlient will make the notification disappear in notification center.
