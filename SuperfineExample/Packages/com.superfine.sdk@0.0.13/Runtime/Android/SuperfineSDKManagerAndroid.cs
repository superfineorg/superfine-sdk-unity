#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerAndroid : SuperfineSDKManagerBase
    {
        private const string JavaClassName = "com.superfine.sdk.unity.SuperfineSDKUnityPlugin";

        class LifecycleListener : AndroidJavaProxy
        {
            public LifecycleListener() : base("com.superfine.sdk.LifecycleListener") { }

            public void onStart()
            {
                if (SuperfineSDKManagerBase.onStart != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onStart());
                }
            }

            public void onStop()
            {
                if (SuperfineSDKManagerBase.onStop != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onStop());
                }
            }

            public void onPause()
            {
                if (SuperfineSDKManagerBase.onPause != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onPause());
                }
            }

            public void onResume()
            {
                if (SuperfineSDKManagerBase.onResume != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onResume());
                }
            }
        }

        private LifecycleListener lifecycleListener = null;
        private int lifecycleListenerRefCount = 0;

        class SendEventListener : AndroidJavaProxy
        {
            public SendEventListener() : base("com.superfine.sdk.SendEventListener") { }

            public void onSendEvent(string eventName, string eventData)
            {
                if (SuperfineSDKManagerBase.onSendEvent != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onSendEvent(eventName, eventData));
                }
            }
        }

        private DeepLinkListener deepLinkListener = null;

        class DeepLinkListener : AndroidJavaProxy
        {
            public DeepLinkListener() : base("com.superfine.sdk.DeepLinkListener") { }

            public void onSetDeepLink(string url)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onSetDeepLink(url));
            }
        }

        private PushTokenListener pushTokenListener = null;

        class PushTokenListener : AndroidJavaProxy
        {
            public PushTokenListener() : base("com.superfine.sdk.PushTokenListener") { }

            public void onSetPushToken(string token)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onSetPushToken(token));
            }
        }

        private SendEventListener sendEventListener = null;

        class RemoteConfigListener : AndroidJavaProxy
        {
            public RemoteConfigListener() : base("com.superfine.sdk.RemoteConfigListener") { }

            public void onReceiveRemoteConfig(string data)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => SuperfineSDKManagerBase.onReceiveRemoteConfig(data));
            }
        }

        private RemoteConfigListener remoteConfigListener = null;

        private AndroidJavaClass javaClass = null; 

        private bool hasInited = false;

        public override void Initialize(SuperfineSDKSettings settings)
        {
            javaClass = new AndroidJavaClass(JavaClassName);
            CallStatic("Init", GetString(settings));

            lifecycleListener = new LifecycleListener();
            lifecycleListenerRefCount = 0;

            deepLinkListener = new DeepLinkListener();
            pushTokenListener = new PushTokenListener();

            sendEventListener = new SendEventListener();

            remoteConfigListener = new RemoteConfigListener();

            hasInited = true;
        }

        public override void Destroy()
        {
            if (hasInited)
            {
                javaClass.CallStatic("Shutdown");

                lifecycleListener = null;
                lifecycleListenerRefCount = 0;

                deepLinkListener = null;
                pushTokenListener = null;

                sendEventListener = null;

                hasInited = false;
            }
        }

        private T CallStatic<T>(string methodName)
        {
            return javaClass.CallStatic<T>(methodName);
        }

        private void CallStatic(string methodName, params object[] args)
        {
            javaClass.CallStatic(methodName, args);
        }

        public override void Start()
        {
             javaClass.CallStatic("Start");
        }

        public override void Stop()
        {
             javaClass.CallStatic("Stop");
        }

        public override void SetOffline(bool value)
        {
            javaClass.CallStatic("SetOffline", value);
        }

        public override string GetVersion()
        {
            return javaClass.CallStatic<string>("GetVersion");
        }

        public override void SetConfigId(string configId)
        {
            javaClass.CallStatic("SetConfigId", configId);
        }

        public override void SetCustomUserId(string customUserId)
        {
            javaClass.CallStatic("SetCustomUserId", customUserId);
        }

        public override void SetAdvertisingId(string advertisingId)
        {
            throw new System.Exception("SDK doesn't support setting advertising id");
        }

        public override string GetAppId()
        {
            return javaClass.CallStatic<string>("GetAppId");
        }

        public override string GetUserId()
        {
            return javaClass.CallStatic<string>("GetUserId");
        }

        public override string GetSessionId()
        {
            return javaClass.CallStatic<string>("GetSessionId");
        }

        public override string GetHost()
        {
            return javaClass.CallStatic<string>("GetHost");
        }

        public override StoreType GetStoreType()
        {
            string storeTypeString = javaClass.CallStatic<string>("GetStoreType");

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
            return javaClass.CallStatic<string>("GetFacebookAppId");
        }

        public override string GetInstagramAppId()
        {
            return javaClass.CallStatic<string>("GetInstagramAppId");
        }

        public override string GetAppleAppId()
        {
            return javaClass.CallStatic<string>("GetAppleAppId");
        }

        public override string GetAppleSignInClientId()
        {
            return javaClass.CallStatic<string>("GetAppleSignInClientId");
        }

        public override string GetAppleDeveloperTeamId()
        {
            return javaClass.CallStatic<string>("GetAppleDeveloperTeamId");
        }

        public override string GetGooglePlayGameServicesProjectId()
        {
            return javaClass.CallStatic<string>("GetGooglePlayGameServicesProjectId");
        }

        public override string GetGooglePlayDeveloperAccountId()
        {
            return javaClass.CallStatic<string>("GetGooglePlayDeveloperAccountId");
        }

        public override string GetLinkedInAppId()
        {
            return javaClass.CallStatic<string>("GetLinkedInAppId");
        }

        public override string GetQQAppId()
        {
            return javaClass.CallStatic<string>("GetQQAppId");
        }

        public override string GetWeChatAppId()
        {
            return javaClass.CallStatic<string>("GetWeChatAppId");
        }

        public override string GetTikTokAppId()
        {
            return javaClass.CallStatic<string>("GetTikTokAppId");
        }

        public override string GetDeepLinkUrl()
        {
            return javaClass.CallStatic<string>("GetDeepLinkUrl");
        }

        public override void SetPushToken(string token)
        {
            javaClass.CallStatic("SetPushToken", token);
        }

        public override string GetPushToken()
        {
            return javaClass.CallStatic<string>("GetPushToken");
        }

        public string GetIMEI()
        {
            return javaClass.CallStatic<string>("GetIMEI");
        }

        protected override void FetchNativeRemoteConfig()
        {
           javaClass.CallStatic("FetchRemoteConfig", remoteConfigListener);
        }

        protected override void RegisterNativeStartCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                javaClass.CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        protected override void UnregisterNativeStartCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                javaClass.CallStatic("RemoveLifecycleListener", lifecycleListener);
            }
        }

        protected override void RegisterNativeStopCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                javaClass.CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        protected override void UnregisterNativeStopCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                javaClass.CallStatic("RemoveLifecycleListener", lifecycleListener);
            }
        }

        protected override void RegisterNativePauseCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                javaClass.CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        protected override void UnregisterNativePauseCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                javaClass.CallStatic("RemoveLifecycleListener", lifecycleListener);
            }
        }

        protected override void RegisterNativeResumeCallback()
        {
            lifecycleListenerRefCount++;
            if (lifecycleListenerRefCount == 1)
            {
                javaClass.CallStatic("AddLifecycleListener", lifecycleListener);
            }
        }

        protected override void UnregisterNativeResumeCallback()
        {
            lifecycleListenerRefCount--;
            if (lifecycleListenerRefCount == 0)
            {
                javaClass.CallStatic("RemoveLifecycleListener", lifecycleListener);
            }
        }

        protected override void RegisterNativeDeepLinkCallback()
        {
            javaClass.CallStatic("AddDeepLinkListener", deepLinkListener);
        }

        protected override void UnregisterNativeDeepLinkCallback()
        {
            javaClass.CallStatic("RemoveDeepLinkListener", deepLinkListener);
        }

        protected override void RegisterNativePushTokenCallback()
        {
            javaClass.CallStatic("AddPushTokenListener", pushTokenListener);
        }

        protected override void UnregisterNativePushTokenCallback()
        {
            javaClass.CallStatic("RemovePushTokenListener", pushTokenListener);
        }

        protected override void RegisterNativeSendEventCallback()
        {
            javaClass.CallStatic("AddSendEventListener", sendEventListener);
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            javaClass.CallStatic("RemoveSendEventListener", sendEventListener);
        }

        public override void GdprForgetMe()
        {
            javaClass.CallStatic("GdprForgetMe");
        }

        public override void DisableThirdPartySharing()
        {
            javaClass.CallStatic("DisableThirdPartySharing");
        }

        public override void EnableThirdPartySharing()
        {
            javaClass.CallStatic("EnableThirdPartySharing");
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            javaClass.CallStatic("LogThirdPartySharing", GetString(settings));
        }

        public override void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            javaClass.CallStatic("LogWithFlag", eventName, (int)eventFlag);
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            javaClass.CallStatic("LogWithIntValueAndFlag", eventName, data, (int)eventFlag);
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (string.IsNullOrEmpty(data))
            {
                javaClass.CallStatic("LogWithFlag", eventName, (int)eventFlag);
            }
            else
            {
                javaClass.CallStatic("LogWithStringValueAndFlag", eventName, data, (int)eventFlag);
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (data == null)
            {
                javaClass.CallStatic("LogWithFlag", eventName, (int)eventFlag);
            }
            else
            {
                javaClass.CallStatic("LogWithMapValueAndFlag", eventName, GetMapString(data), (int)eventFlag);
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            javaClass.CallStatic("LogWithJsonValueAndFlag", eventName, (data == null ? string.Empty : data.ToString()), (int)eventFlag);
        }

        public override void LogBootStart()
        {
            javaClass.CallStatic("LogBootStart");
        }

        public override void LogBootEnd()
        {
            javaClass.CallStatic("LogBootEnd");
        }

        public override void LogLevelStart(int id, string name)
        {
            javaClass.CallStatic("LogLevelStart", id, name);
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            javaClass.CallStatic("LogLevelEnd", id, name, isSuccess);
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdLoad", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdClose", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdClick", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdImpression", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            javaClass.CallStatic("LogIAPInitialization", isSuccess);
        }

        public override void LogIAPRestorePurchase()
        {
            javaClass.CallStatic("LogIAPRestorePurchase");
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            javaClass.CallStatic("LogIAPResult", pack, price, amount, currency, isSuccess);
        }

        public override void LogIAPReceipt_Apple(string receipt)
        {
            javaClass.CallStatic("LogIAPReceipt_Apple", receipt);
        }

        public override void LogIAPReceipt_Google(string data, string signature)
        {
            javaClass.CallStatic("LogIAPReceipt_Google", data, signature);
        }

        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            javaClass.CallStatic("LogIAPReceipt_Amazon", userId, receiptId);
        }

        public override void LogIAPReceipt_Roku(string transactionId)
        {
            javaClass.CallStatic("LogIAPReceipt_Roku", transactionId);
        }

        public override void LogIAPReceipt_Windows(string receipt)
        {
            javaClass.CallStatic("LogIAPReceipt_Windows", receipt);
        }

        public override void LogIAPReceipt_Facebook(string receipt)
        {
            javaClass.CallStatic("LogIAPReceipt_Facebook", receipt);
        }

        public override void LogIAPReceipt_Unity(string receipt)
        {
            javaClass.CallStatic("LogIAPReceipt_Unity", receipt);
        }

        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            javaClass.CallStatic("LogIAPReceipt_AppStoreServer", transactionId);
        }

        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            javaClass.CallStatic("LogIAPReceipt_GooglePlayProduct", productId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            javaClass.CallStatic("LogIAPReceipt_GooglePlaySubscription", subscriptionId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            javaClass.CallStatic("LogIAPReceipt_GooglePlaySubscriptionv2", token);
        }

        public override void LogUpdateApp(string newVersion)
        {
            javaClass.CallStatic("LogUpdateApp", newVersion);
        }

        public override void LogRateApp()
        {
            javaClass.CallStatic("LogRateApp");
        }

        public override void LogLocation(double latitude, double longitude)
        {
            javaClass.CallStatic("LogLocation", latitude, longitude);
        }

        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            javaClass.CallStatic("LogAuthorizationTrackingStatus", status.ToString());
        }

        public override void LogFacebookLink(string userId)
        {
            javaClass.CallStatic("LogFacebookLink", userId);
        }

        public override void LogFacebookUnlink()
        {
            javaClass.CallStatic("LogFacebookUnlink");
        }

        public override void LogInstagramLink(string userId)
        {
            javaClass.CallStatic("LogInstagramLink", userId);
        }

        public override void LogInstagramUnlink()
        {
            javaClass.CallStatic("LogInstagramUnlink");
        }

        public override void LogAppleLink(string userId)
        {
            javaClass.CallStatic("LogAppleLink", userId);
        }

        public override void LogAppleUnlink()
        {
            javaClass.CallStatic("LogAppleUnlink");
        }

        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            javaClass.CallStatic("LogAppleGameCenterLink", gamePlayerId);
        }

        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            javaClass.CallStatic("LogAppleGameCenterTeamLink", teamPlayerId);
        }

        public override void LogAppleGameCenterUnlink()
        {
            javaClass.CallStatic("LogAppleGameCenterUnlink");
        }

        public override void LogGoogleLink(string userId)
        {
            javaClass.CallStatic("LogGoogleLink", userId);
        }

        public override void LogGoogleUnlink()
        {
            javaClass.CallStatic("LogGoogleUnlink");
        }

        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            javaClass.CallStatic("LogGooglePlayGameServicesLink", gamePlayerId);
        }

        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            javaClass.CallStatic("LogGooglePlayGameServicesDeveloperLink", developerPlayerKey);
        }

        public override void LogGooglePlayGameServicesUnlink()
        {
            javaClass.CallStatic("LogGooglePlayGameServicesUnlink");
        }

        public override void LogLinkedInLink(string personId)
        {
            javaClass.CallStatic("LogLinkedInLink", personId);
        }

        public override void LogLinkedInUnlink()
        {
            javaClass.CallStatic("LogLinkedInUnlink");
        }

        public override void LogMeetupLink(string userId)
        {
            javaClass.CallStatic("LogMeetupLink", userId);
        }

        public override void LogMeetupUnlink()
        {
            javaClass.CallStatic("LogMeetupUnlink");
        }

        public override void LogGitHubLink(string userId)
        {
            javaClass.CallStatic("LogGitHubLink", userId);
        }

        public override void LogGitHubUnlink()
        {
            javaClass.CallStatic("LogGitHubUnlink");
        }

        public override void LogDiscordLink(string userId)
        {
            javaClass.CallStatic("LogDiscordLink", userId);
        }

        public override void LogDiscordUnlink()
        {
            javaClass.CallStatic("LogDiscordUnlink");
        }

        public override void LogTwitterLink(string userId)
        {
            javaClass.CallStatic("LogTwitterLink", userId);
        }

        public override void LogTwitterUnlink()
        {
            javaClass.CallStatic("LogTwitterUnlink");
        }

        public override void LogSpotifyLink(string userId)
        {
            javaClass.CallStatic("LogSpotifyLink", userId);
        }

        public override void LogSpotifyUnlink()
        {
            javaClass.CallStatic("LogSpotifyUnlink");
        }

        public override void LogMicrosoftLink(string userId)
        {
            javaClass.CallStatic("LogMicrosoftLink", userId);
        }

        public override void LogMicrosoftUnlink()
        {
            javaClass.CallStatic("LogMicrosoftUnlink");
        }

        public override void LogLINELink(string userId)
        {
            javaClass.CallStatic("LogLINELink", userId);
        }

        public override void LogLINEUnlink()
        {
            javaClass.CallStatic("LogLINEUnlink");
        }

        public override void LogVKLink(string userId)
        {
            javaClass.CallStatic("LogVKLink", userId);
        }

        public override void LogVKUnlink()
        {
            javaClass.CallStatic("LogVKUnlink");
        }

        public override void LogQQLink(string openId)
        {
            javaClass.CallStatic("LogQQLink", openId);
        }

        public override void LogQQUnionLink(string unionId)
        {
            javaClass.CallStatic("LogQQUnionLink", unionId);
        }

        public override void LogQQUnlink()
        {
            javaClass.CallStatic("LogQQUnlink");
        }

        public override void LogWeChatLink(string openId)
        {
            javaClass.CallStatic("LogWeChatLink", openId);
        }

        public override void LogWeChatUnionLink(string unionId)
        {
            javaClass.CallStatic("LogWeChatUnionLink", unionId);
        }

        public override void LogWeChatUnlink()
        {
            javaClass.CallStatic("LogWeChatUnlink");
        }

        public override void LogTikTokLink(string openId)
        {
            javaClass.CallStatic("LogTikTokLink", openId);
        }

        public override void LogTikTokUnionLink(string unionId)
        {
            javaClass.CallStatic("LogTikTokUnionLink", unionId);
        }

        public override void LogTikTokUnlink()
        {
            javaClass.CallStatic("LogTikTokUnlink");
        }

        public override void LogWeiboLink(string userId)
        {
            javaClass.CallStatic("LogWeiboLink", userId);
        }

        public override void LogWeiboUnlink()
        {
            javaClass.CallStatic("LogWeiboUnlink");
        }

        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
            if (string.IsNullOrEmpty(scopeId))
            {
                javaClass.CallStatic("LogAccountLink", id, type);
            }
            else if (string.IsNullOrEmpty(scopeType))
            {
                javaClass.CallStatic("LogAccountLink", id, type, scopeId);
            }
            else
            {
                javaClass.CallStatic("LogAccountLink", id, type, scopeId, scopeType);
            }
        }

        public override void LogAccountUnlink(string type)
        {
            javaClass.CallStatic("LogAccountUnlink", type);
        }

         public override void AddUserPhoneNumber(int countryCode, string number)
        {
            javaClass.CallStatic("AddUserPhoneNumber", countryCode, number);
        }

        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            javaClass.CallStatic("RemoveUserPhoneNumber", countryCode, number);
        }

        public override void AddUserEmail(string email)
        {
            javaClass.CallStatic("AddUserEmail", email);
        }

        public override void RemoveUserEmail(string email)
        {
            javaClass.CallStatic("RemoveUserEmail", email);
        }

        public override void SetUserName(string firstName, string lastName)
        {
            javaClass.CallStatic("SetUserName", firstName, lastName);
        }

        public override void SetUserFirstName(string firstName)
        {
            javaClass.CallStatic("SetUserFirstName", firstName);
        }

        public override void SetUserLastName(string lastName)
        {
            javaClass.CallStatic("SetUserLastName", lastName);
        }

        public override void SetUserCity(string city)
        {
            javaClass.CallStatic("SetUserCity", city);
        }

        public override void SetUserState(string state)
        {
            javaClass.CallStatic("SetUserState", state);
        }

        public override void SetUserCountry(string country)
        {
            javaClass.CallStatic("SetUserCountry", country);
        }

        public override void SetUserZipCode(string zipCode)
        {
            javaClass.CallStatic("SetUserZipCode", zipCode);
        }

        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            javaClass.CallStatic("SetUserDateOfBirth", day, month, year);
        }

        public override void SetUserDateOfBirth(int day, int month)
        {
            javaClass.CallStatic("SetUserDateOfBirth", day, month);
        }

        public override void SetUserYearOfBirth(int year)
        {
            javaClass.CallStatic("SetUserYearOfBirth", year);
        }

        public override void SetUserGender(UserGender gender)
        {
            javaClass.CallStatic("SetUserGender", gender.ToString());
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            javaClass.CallStatic("LogWalletLink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            javaClass.CallStatic("LogWalletUnlink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            javaClass.CallStatic("LogCryptoPayment", pack, price, amount, currency, chain == null ? "ethereum" : chain);
        }

        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            javaClass.CallStatic("LogAdRevenue", source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString());
        }

        public override void OpenURL(string url)
        {
            javaClass.CallStatic("OpenURL", url);
        }
    }
}
#endif