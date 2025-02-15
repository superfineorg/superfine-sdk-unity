#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerAndroid : SuperfineSDKManagerBase
    {
        private const string JavaClassName = "com.superfine.sdk.unity.SuperfineSDKUnityPlugin";
        
        private static AndroidJavaClass javaClass = null; 

        private static T CallStatic<T>(string methodName)
        {
            if (javaClass == null) javaClass = new AndroidJavaClass(JavaClassName);
            return javaClass.CallStatic<T>(methodName);
        }

        private static void CallStatic(string methodName, params object[] args)
        {
            if (javaClass == null) javaClass = new AndroidJavaClass(JavaClassName);
            javaClass.CallStatic(methodName, args);
        }

        class InitializationListener : AndroidJavaProxy
        {
            public InitializationListener() : base("com.superfine.sdk.InitializationListener") { }

            public void onSdkInitialized()
            {
                SuperfineSDK.InvokeOnInitialized();
            }
        }

        private static InitializationListener initializationListener = null;

        class LifecycleListener : AndroidJavaProxy
        {
            public LifecycleListener() : base("com.superfine.sdk.LifecycleListener") { }

            public void onStart()
            {
                SuperfineSDK.InvokeOnStart();
            }

            public void onStop()
            {
                SuperfineSDK.InvokeOnStop();
            }

            public void onPause()
            {
                SuperfineSDK.InvokeOnPause();
            }

            public void onResume()
            {
                SuperfineSDK.InvokeOnResume();
            }
        }

        private static LifecycleListener lifecycleListener = null;
        private static int lifecycleListenerRefCount = 0;

        class SendEventListener : AndroidJavaProxy
        {
            public SendEventListener() : base("com.superfine.sdk.SendEventListener") { }

            public void onSendEvent(string eventName, string eventData)
            {
                SuperfineSDK.InvokeOnSendEvent(eventName, eventData);
            }
        }

        private static SendEventListener sendEventListener = null;

        class DeepLinkListener : AndroidJavaProxy
        {
            public DeepLinkListener() : base("com.superfine.sdk.DeepLinkListener") { }

            public void onSetDeepLink(string url)
            {
                SuperfineSDK.InvokeOnSetDeepLink(url);
            }
        }

        private static DeepLinkListener deepLinkListener = null;

        class PushTokenListener : AndroidJavaProxy
        {
            public PushTokenListener() : base("com.superfine.sdk.PushTokenListener") { }

            public void onSetPushToken(string token)
            {
                SuperfineSDK.InvokeOnSetPushToken(token);
            }
        }

        private static PushTokenListener pushTokenListener = null;

        class RemoteConfigListener : AndroidJavaProxy
        {
            public RemoteConfigListener() : base("com.superfine.sdk.RemoteConfigListener") { }

            public void onReceiveRemoteConfig(string data)
            {
                SuperfineSDK.InvokeOnReceiveRemoteConfig(data);
            }
        }

        private static RemoteConfigListener remoteConfigListener = null;

        class PermissionRequestListener : AndroidJavaProxy
        {
            public PermissionRequestListener() : base("com.superfine.sdk.PermissionRequestListener") { }

            public void onRequestPermissionsResult(int[] nativeResults)
            {
                AndroidPermissionResult[] results = null;
                if (nativeResults != null)
                {
                    int numResults = nativeResults.Length;

                    results = new AndroidPermissionResult[numResults];
                    for (int i = 0; i < numResults; ++i)
                    {
                        results[i] = (AndroidPermissionResult)nativeResults[i];
                    }
                }

                SuperfineSDK.Android.InvokeOnPermissionRequestResults(results);
            }
        }

        private static PermissionRequestListener permissionRequestListener = null;

        protected override void InitializeNative(SuperfineSDKSettings settings, List<string> moduleNameList)
        {
            if (initializationListener == null)
            {
                initializationListener = new InitializationListener();
            }

            CallStatic("Initialize", CreateInitializationJSONObject(settings, moduleNameList).ToString(), initializationListener);
        }

        public override void Destroy()
        {
            CallStatic("Shutdown");

            initializationListener = null;
            remoteConfigListener = null;
            permissionRequestListener = null;
        }

        public override bool IsInitialized()
        {
            return CallStatic<bool>("IsInitialized");
        }

        public override void Start()
        {
             CallStatic("Start");
        }

        public override void Stop()
        {
             CallStatic("Stop");
        }

        public override void SetOffline(bool value)
        {
            CallStatic("SetOffline", value);
        }

        public override string GetVersion()
        {
            return CallStatic<string>("GetVersion");
        }

        public override void SetConfigId(string configId)
        {
            CallStatic("SetConfigId", configId);
        }

        public override void SetCustomUserId(string customUserId)
        {
            CallStatic("SetCustomUserId", customUserId);
        }

        public override string GetAppId()
        {
            return CallStatic<string>("GetAppId");
        }

        public override string GetUserId()
        {
            return CallStatic<string>("GetUserId");
        }

        public override string GetSessionId()
        {
            return CallStatic<string>("GetSessionId");
        }

        public override string GetHost()
        {
            return CallStatic<string>("GetHost");
        }

        public override string GetConfigUrl()
        {
            return CallStatic<string>("GetConfigUrl");
        }

        public override string GetSdkConfig()
        {
            return CallStatic<string>("GetSdkConfig");
        }

        public override StoreType GetStoreType()
        {
            string storeTypeString = CallStatic<string>("GetStoreType");

            if (Enum.TryParse<StoreType>(storeTypeString, out StoreType storeType))
            {
                return storeType;
            }
            else
            {
                return StoreType.UNKNOWN;
            }
        }

        public override string GetFacebookAppId()
        {
            return CallStatic<string>("GetFacebookAppId");
        }

        public override string GetInstagramAppId()
        {
            return CallStatic<string>("GetInstagramAppId");
        }

        public override string GetAppleAppId()
        {
            return CallStatic<string>("GetAppleAppId");
        }

        public override string GetAppleSignInClientId()
        {
            return CallStatic<string>("GetAppleSignInClientId");
        }

        public override string GetAppleDeveloperTeamId()
        {
            return CallStatic<string>("GetAppleDeveloperTeamId");
        }

        public override string GetGooglePlayGameServicesProjectId()
        {
            return CallStatic<string>("GetGooglePlayGameServicesProjectId");
        }

        public override string GetGooglePlayDeveloperAccountId()
        {
            return CallStatic<string>("GetGooglePlayDeveloperAccountId");
        }

        public override string GetLinkedInAppId()
        {
            return CallStatic<string>("GetLinkedInAppId");
        }

        public override string GetQQAppId()
        {
            return CallStatic<string>("GetQQAppId");
        }

        public override string GetWeChatAppId()
        {
            return CallStatic<string>("GetWeChatAppId");
        }

        public override string GetTikTokAppId()
        {
            return CallStatic<string>("GetTikTokAppId");
        }

        public override string GetSnapAppId()
        {
            return CallStatic<string>("GetSnapAppId");
        }
        
        public static void OpenURL(string url)
        {
            CallStatic("OpenURL", url);
        }

        public static string GetDeepLinkUrl()
        {
            return CallStatic<string>("GetDeepLinkUrl");
        }

        public static void SetPushToken(string token)
        {
            CallStatic("SetPushToken", token);
        }

        public static string GetPushToken()
        {
            return CallStatic<string>("GetPushToken");
        }

        public string GetIMEI()
        {
            return CallStatic<string>("GetIMEI");
        }

        public override void FetchRemoteConfig()
        {
            if (remoteConfigListener == null) remoteConfigListener = new RemoteConfigListener();
            CallStatic("FetchRemoteConfig", remoteConfigListener);
        }

        public static void RegisterNativeStartCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                lifecycleListener = new LifecycleListener();
                CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        public static void UnregisterNativeStartCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                CallStatic("RemoveLifecycleListener", lifecycleListener);
                lifecycleListener = null;
            }
        }

        public static void RegisterNativeStopCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                lifecycleListener = new LifecycleListener();
                CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        public static void UnregisterNativeStopCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                CallStatic("RemoveLifecycleListener", lifecycleListener);
                lifecycleListener = null;
            }
        }

        public static void RegisterNativePauseCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                lifecycleListener = new LifecycleListener();
                CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        public static void UnregisterNativePauseCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                CallStatic("RemoveLifecycleListener", lifecycleListener);
                lifecycleListener = null;
            }
        }

        public static void RegisterNativeResumeCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                lifecycleListener = new LifecycleListener();
                CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        public static void UnregisterNativeResumeCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                CallStatic("RemoveLifecycleListener", lifecycleListener);
                lifecycleListener = null;
            }
        }

        public static void RegisterNativeDeepLinkCallback()
        {
            deepLinkListener = new DeepLinkListener();
            CallStatic("AddDeepLinkListener", deepLinkListener);
        }

        public static void UnregisterNativeDeepLinkCallback()
        {
            CallStatic("RemoveDeepLinkListener", deepLinkListener);
            deepLinkListener = null;
        }

        public static void RegisterNativePushTokenCallback()
        {
            pushTokenListener = new PushTokenListener();
            CallStatic("AddPushTokenListener", pushTokenListener);
        }

        public static void UnregisterNativePushTokenCallback()
        {
            CallStatic("RemovePushTokenListener", pushTokenListener);
            pushTokenListener = null;
        }

        public static void RegisterNativeSendEventCallback()
        {
            sendEventListener = new SendEventListener();
            CallStatic("AddSendEventListener", sendEventListener);
        }

        public static void UnregisterNativeSendEventCallback()
        {
            CallStatic("RemoveSendEventListener", sendEventListener);
            sendEventListener = null;
        }

        public override void GdprForgetMe()
        {
            CallStatic("GdprForgetMe");
        }

        public override void DisableThirdPartySharing()
        {
            CallStatic("DisableThirdPartySharing");
        }

        public override void EnableThirdPartySharing()
        {
            CallStatic("EnableThirdPartySharing");
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            CallStatic("LogThirdPartySharing", GetString(settings));
        }

        public override void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            CallStatic("LogWithFlag", eventName, (int)eventFlag);
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            CallStatic("LogWithIntValueAndFlag", eventName, data, (int)eventFlag);
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (string.IsNullOrEmpty(data))
            {
                CallStatic("LogWithFlag", eventName, (int)eventFlag);
            }
            else
            {
                CallStatic("LogWithStringValueAndFlag", eventName, data, (int)eventFlag);
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (data == null)
            {
                CallStatic("LogWithFlag", eventName, (int)eventFlag);
            }
            else
            {
                CallStatic("LogWithMapValueAndFlag", eventName, GetMapString(data), (int)eventFlag);
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            CallStatic("LogWithJsonValueAndFlag", eventName, (data == null ? string.Empty : data.ToString()), (int)eventFlag);
        }

        private static AndroidJavaObject CreateNativeLogEvent(SuperfineSDKEvent eventData)
        {
            AndroidJavaObject eventJavaObject = new AndroidJavaObject("com.superfine.sdk.SuperfineSDKEvent", eventData.eventName);
            if (eventJavaObject == null) return null;

            SuperfineSDKEvent.ValueType valueType = eventData.valueType;
            switch (valueType)
            {
                case SuperfineSDKEvent.ValueType.INT:
                    {
                        int data = (int)eventData.value;
                        CallStatic("SetEventIntValue", data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.STRING:
                    {
                        string data = (string)eventData.value;
                        CallStatic("SetEventStringValue", data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.MAP:
                    {
                        Dictionary<string, string> data = (Dictionary<string, string>)eventData.value;
                        CallStatic("SetEventMapValue", GetMapString(data));
                    }
                    break;

                case SuperfineSDKEvent.ValueType.JSON:
                    {
                        SimpleJSON.JSONObject data = (SimpleJSON.JSONObject)eventData.value;
                        CallStatic("SetEventJsonValue", data.ToString());
                    }
                    break;

                default:
                    break;
            }

            EventFlag eventFlag = eventData.eventFlag;
            if (eventFlag != EventFlag.NONE)
            {
                CallStatic("SetEventFlag", eventJavaObject, (int)eventFlag);
            }

            if (eventData.HasRevenue())
            {
                CallStatic("SetEventRevenue", eventJavaObject, eventData.revenue, eventData.currency);
            }

            return eventJavaObject;
        }

        public override void Log(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;

            AndroidJavaObject eventJavaObject = CreateNativeLogEvent(eventData);
            if (eventJavaObject == null) return;

            CallStatic("Log", eventJavaObject);
        }

        public static void LogCache(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;

            AndroidJavaObject eventJavaObject = CreateNativeLogEvent(eventData);
            if (eventJavaObject == null) return;

            CallStatic("LogCache", eventJavaObject);
        }

        public override void LogBootStart()
        {
            CallStatic("LogBootStart");
        }

        public override void LogBootEnd()
        {
            CallStatic("LogBootEnd");
        }

        public override void LogLevelStart(int id, string name)
        {
            CallStatic("LogLevelStart", id, name);
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            CallStatic("LogLevelEnd", id, name, isSuccess);
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            CallStatic("LogAdLoad", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            CallStatic("LogAdClose", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            CallStatic("LogAdClick", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            CallStatic("LogAdImpression", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            CallStatic("LogIAPInitialization", isSuccess);
        }

        public override void LogIAPRestorePurchase()
        {
            CallStatic("LogIAPRestorePurchase");
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            CallStatic("LogIAPResult", pack, price, amount, currency, isSuccess);
        }

        public override void LogIAPReceipt_Apple(string receipt)
        {
            CallStatic("LogIAPReceipt_Apple", receipt);
        }

        public override void LogIAPReceipt_Google(string data, string signature)
        {
            CallStatic("LogIAPReceipt_Google", data, signature);
        }

        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            CallStatic("LogIAPReceipt_Amazon", userId, receiptId);
        }

        public override void LogIAPReceipt_Roku(string transactionId)
        {
            CallStatic("LogIAPReceipt_Roku", transactionId);
        }

        public override void LogIAPReceipt_Windows(string receipt)
        {
            CallStatic("LogIAPReceipt_Windows", receipt);
        }

        public override void LogIAPReceipt_Facebook(string receipt)
        {
            CallStatic("LogIAPReceipt_Facebook", receipt);
        }

        public override void LogIAPReceipt_Unity(string receipt)
        {
            CallStatic("LogIAPReceipt_Unity", receipt);
        }

        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            CallStatic("LogIAPReceipt_AppStoreServer", transactionId);
        }

        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            CallStatic("LogIAPReceipt_GooglePlayProduct", productId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            CallStatic("LogIAPReceipt_GooglePlaySubscription", subscriptionId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            CallStatic("LogIAPReceipt_GooglePlaySubscriptionv2", token);
        }

        public override void LogUpdateApp(string newVersion)
        {
            CallStatic("LogUpdateApp", newVersion);
        }

        public override void LogRateApp()
        {
            CallStatic("LogRateApp");
        }

        public override void LogLocation(double latitude, double longitude)
        {
            CallStatic("LogLocation", latitude, longitude);
        }

        public override void LogFacebookLink(string userId)
        {
            CallStatic("LogFacebookLink", userId);
        }

        public override void LogFacebookUnlink()
        {
            CallStatic("LogFacebookUnlink");
        }

        public override void LogInstagramLink(string userId)
        {
            CallStatic("LogInstagramLink", userId);
        }

        public override void LogInstagramUnlink()
        {
            CallStatic("LogInstagramUnlink");
        }

        public override void LogAppleLink(string userId)
        {
            CallStatic("LogAppleLink", userId);
        }

        public override void LogAppleUnlink()
        {
            CallStatic("LogAppleUnlink");
        }

        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            CallStatic("LogAppleGameCenterLink", gamePlayerId);
        }

        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            CallStatic("LogAppleGameCenterTeamLink", teamPlayerId);
        }

        public override void LogAppleGameCenterUnlink()
        {
            CallStatic("LogAppleGameCenterUnlink");
        }

        public override void LogGoogleLink(string userId)
        {
            CallStatic("LogGoogleLink", userId);
        }

        public override void LogGoogleUnlink()
        {
            CallStatic("LogGoogleUnlink");
        }

        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            CallStatic("LogGooglePlayGameServicesLink", gamePlayerId);
        }

        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            CallStatic("LogGooglePlayGameServicesDeveloperLink", developerPlayerKey);
        }

        public override void LogGooglePlayGameServicesUnlink()
        {
            CallStatic("LogGooglePlayGameServicesUnlink");
        }

        public override void LogLinkedInLink(string personId)
        {
            CallStatic("LogLinkedInLink", personId);
        }

        public override void LogLinkedInUnlink()
        {
            CallStatic("LogLinkedInUnlink");
        }

        public override void LogMeetupLink(string userId)
        {
            CallStatic("LogMeetupLink", userId);
        }

        public override void LogMeetupUnlink()
        {
            CallStatic("LogMeetupUnlink");
        }

        public override void LogGitHubLink(string userId)
        {
            CallStatic("LogGitHubLink", userId);
        }

        public override void LogGitHubUnlink()
        {
            CallStatic("LogGitHubUnlink");
        }

        public override void LogDiscordLink(string userId)
        {
            CallStatic("LogDiscordLink", userId);
        }

        public override void LogDiscordUnlink()
        {
            CallStatic("LogDiscordUnlink");
        }

        public override void LogTwitterLink(string userId)
        {
            CallStatic("LogTwitterLink", userId);
        }

        public override void LogTwitterUnlink()
        {
            CallStatic("LogTwitterUnlink");
        }

        public override void LogSpotifyLink(string userId)
        {
            CallStatic("LogSpotifyLink", userId);
        }

        public override void LogSpotifyUnlink()
        {
            CallStatic("LogSpotifyUnlink");
        }

        public override void LogMicrosoftLink(string userId)
        {
            CallStatic("LogMicrosoftLink", userId);
        }

        public override void LogMicrosoftUnlink()
        {
            CallStatic("LogMicrosoftUnlink");
        }

        public override void LogLINELink(string userId)
        {
            CallStatic("LogLINELink", userId);
        }

        public override void LogLINEUnlink()
        {
            CallStatic("LogLINEUnlink");
        }

        public override void LogVKLink(string userId)
        {
            CallStatic("LogVKLink", userId);
        }

        public override void LogVKUnlink()
        {
            CallStatic("LogVKUnlink");
        }

        public override void LogQQLink(string openId)
        {
            CallStatic("LogQQLink", openId);
        }

        public override void LogQQUnionLink(string unionId)
        {
            CallStatic("LogQQUnionLink", unionId);
        }

        public override void LogQQUnlink()
        {
            CallStatic("LogQQUnlink");
        }

        public override void LogWeChatLink(string openId)
        {
            CallStatic("LogWeChatLink", openId);
        }

        public override void LogWeChatUnionLink(string unionId)
        {
            CallStatic("LogWeChatUnionLink", unionId);
        }

        public override void LogWeChatUnlink()
        {
            CallStatic("LogWeChatUnlink");
        }

        public override void LogTikTokLink(string openId)
        {
            CallStatic("LogTikTokLink", openId);
        }

        public override void LogTikTokUnionLink(string unionId)
        {
            CallStatic("LogTikTokUnionLink", unionId);
        }

        public override void LogTikTokUnlink()
        {
            CallStatic("LogTikTokUnlink");
        }

        public override void LogWeiboLink(string userId)
        {
            CallStatic("LogWeiboLink", userId);
        }

        public override void LogWeiboUnlink()
        {
            CallStatic("LogWeiboUnlink");
        }

        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
            if (string.IsNullOrEmpty(scopeId))
            {
                CallStatic("LogAccountLink", id, type);
            }
            else if (string.IsNullOrEmpty(scopeType))
            {
                CallStatic("LogAccountLink", id, type, scopeId);
            }
            else
            {
                CallStatic("LogAccountLink", id, type, scopeId, scopeType);
            }
        }

        public override void LogAccountUnlink(string type)
        {
            CallStatic("LogAccountUnlink", type);
        }

         public override void AddUserPhoneNumber(int countryCode, string number)
        {
            CallStatic("AddUserPhoneNumber", countryCode, number);
        }

        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            CallStatic("RemoveUserPhoneNumber", countryCode, number);
        }

        public override void AddUserEmail(string email)
        {
            CallStatic("AddUserEmail", email);
        }

        public override void RemoveUserEmail(string email)
        {
            CallStatic("RemoveUserEmail", email);
        }

        public override void SetUserName(string firstName, string lastName)
        {
            CallStatic("SetUserName", firstName, lastName);
        }

        public override void SetUserFirstName(string firstName)
        {
            CallStatic("SetUserFirstName", firstName);
        }

        public override void SetUserLastName(string lastName)
        {
            CallStatic("SetUserLastName", lastName);
        }

        public override void SetUserCity(string city)
        {
            CallStatic("SetUserCity", city);
        }

        public override void SetUserState(string state)
        {
            CallStatic("SetUserState", state);
        }

        public override void SetUserCountry(string country)
        {
            CallStatic("SetUserCountry", country);
        }

        public override void SetUserZipCode(string zipCode)
        {
            CallStatic("SetUserZipCode", zipCode);
        }

        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            CallStatic("SetUserDateOfBirth", day, month, year);
        }

        public override void SetUserDateOfBirth(int day, int month)
        {
            CallStatic("SetUserDateOfBirth", day, month);
        }

        public override void SetUserYearOfBirth(int year)
        {
            CallStatic("SetUserYearOfBirth", year);
        }

        public override void SetUserGender(UserGender gender)
        {
            CallStatic("SetUserGender", gender.ToString());
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            CallStatic("LogWalletLink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            CallStatic("LogWalletUnlink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            CallStatic("LogCryptoPayment", pack, price, amount, currency, chain == null ? "ethereum" : chain);
        }

        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            CallStatic("LogAdRevenue", source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString());
        }

        public static void RequestPermissions(SuperfineSDKAndroidPermissionRequest request)
        {
            if (permissionRequestListener == null) permissionRequestListener = new PermissionRequestListener();
            CallStatic("RequestPermissions", request.CreateJSONObject().ToString());
        }
    }
}
#endif