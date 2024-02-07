using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

namespace Superfine.Unity
{
    public static class SuperfineSDK
    {
        private static SuperfineSDKManager instance = null;

        public const string VERSION = "0.0.13-unity";

        public static SuperfineSDKPlatformFlag GetPlatformFlag()
        {
#if UNITY_ANDROID
            return SuperfineSDKPlatformFlag.Android;
#elif UNITY_IOS
            return SuperfineSDKPlatformFlag.iOS;
#elif UNITY_STANDALONE_WIN
            return SuperfineSDKPlatformFlag.Windows;
#elif UNITY_STANDALONE_OSX
            return SuperfineSDKPlatformFlag.macOS;
#elif UNITY_STANDALONE_LINUX
            return SuperfineSDKPlatformFlag.Linux;
#else
            return SuperfineSDKPlatformFlag.None;
#endif
        }

        public static Action<SuperfineSDKManager> OnPostInit = null;

#if UNITY_EDITOR
        public static PackageInfo GetPackageInfo(string name)
        {
            var request = Client.List();
            do { } while (!request.IsCompleted);
            if (request.Status == StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    if (package.name == name) return package;
                }
            }

            return null;
        }

        public static PackageInfo GetSelfPackageInfo()
        {
            return GetPackageInfo("com.superfine.sdk");
        }
#endif

        public static SuperfineSDKManager CreateInstance()
        {
            SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();
            return CreateInstance(settings);
        }

        public static SuperfineSDKManager CreateInstance(SuperfineSDKSettings settings)
        {
            if (instance != null) return instance;

            if (settings == null) return null;

            instance = SuperfineSDKManager.Create();

            instance.Initialize(settings);

            startCallbackCache = null;
            stopCallbackCache = null;
            pauseCallbackCache = null;
            resumeCallbackCache = null;
            deepLinkCallbackCache = null;
            pushTokenCallbackCache = null;
            sendEventCallbackCache = null;

            OnPostInit?.Invoke(instance);
            OnPostInit = null;

            return instance;
        }

        public static void Start()
        {
            if (instance == null) return;
            instance.Start();
        }

        public static void Stop()
        {
            if (instance == null) return;
            instance.Stop();
        }

        public static void SetOffline(bool value)
        {
            if (instance == null) return;
            instance.SetOffline(value);
        }

        public static void Destroy()
        {
            if (instance != null)
            {
                instance.Destroy();
                instance = null;
            }
        }

        public static SuperfineSDKManager GetInstance()
        {
            return instance;
        }

        public static string GetVersion()
        {
            if (instance == null) return VERSION;
            return instance.GetVersion();
        }

        public static void SetConfigId(string configId)
        {
            if (instance == null) return;
            instance.SetConfigId(configId);
        }

        public static void SetCustomUserId(string customUserId)
        {
            if (instance == null) return;
            instance.SetCustomUserId(customUserId);
        }

        public static string GetAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppId();
        }

        public static string GetUserId()
        {
            if (instance == null) return string.Empty;
            return instance.GetUserId();
        }

        public static string GetSessionId()
        {
            if (instance == null) return string.Empty;
            return instance.GetSessionId();
        }

        public static string GetHost()
        {
            if (instance == null) return string.Empty;
            return instance.GetHost();
        }

        public static StoreType GetStoreType()
        {
            if (instance == null) return StoreType.UNKNOWN;
            return instance.GetStoreType();
        }

        public static string GetFacebookAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetFacebookAppId();
        }

        public static string GetInstagramAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetInstagramAppId();
        }

        public static string GetAppleAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleAppId();
        }

        public static string GetAppleSignInClientId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleSignInClientId();
        }

        public static string GetAppleDeveloperTeamId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleDeveloperTeamId();
        }

        public static string GetGooglePlayGameServicesProjectId()
        {
            if (instance == null) return string.Empty;
            return instance.GetGooglePlayGameServicesProjectId();
        }

        public static string GetGooglePlayDeveloperAccountId()
        {
            if (instance == null) return string.Empty;
            return instance.GetGooglePlayDeveloperAccountId();
        }

        public static string GetLinkedInAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetLinkedInAppId();
        }

        public static string GetQQAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetQQAppId();
        }

        public static string GetWeChatAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetWeChatAppId();
        }

        public static string GetTikTokAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetTikTokAppId();
        }

        public static string GetDeepLinkUrl()
        {
            if (instance == null) return string.Empty;
            return instance.GetDeepLinkUrl();
        }

        public static void SetPushToken(string pushToken)
        {
            if (instance == null || string.IsNullOrEmpty(pushToken)) return;
            instance.SetPushToken(pushToken);
        }

        public static string GetPushToken()
        {
            if (instance == null) return string.Empty;
            return instance.GetPushToken();
        }

        private static List<Action> startCallbackCache = null;

        internal static List<Action> GetStartCallbackCache()
        {
            return startCallbackCache;
        }

        private static List<Action> stopCallbackCache = null;

        internal static List<Action> GetStopCallbackCache()
        {
            return stopCallbackCache;
        }

        private static List<Action> pauseCallbackCache = null;

        internal static List<Action> GetPauseCallbackCache()
        {
            return pauseCallbackCache;
        }

        private static List<Action> resumeCallbackCache = null;

        internal static List<Action> GetResumeCallbackCache()
        {
            return resumeCallbackCache;
        }

        private static List<Action<string>> deepLinkCallbackCache = null;

        internal static List<Action<string>> GetDeepLinkCallbackCache()
        {
            return deepLinkCallbackCache;
        }

        private static List<Action<string>> pushTokenCallbackCache = null;

        internal static List<Action<string>> GetPushTokenCallbackCache()
        {
            return pushTokenCallbackCache;
        }

        private static List<Action<string, string>> sendEventCallbackCache = null;

        internal static List<Action<string, string>> GetSendEventCallbackCache()
        {
            return sendEventCallbackCache;
        }

        private static void AddCallbackCache<T>(ref List<T> cache, T callback)
        {
            if (cache == null)
            {
                cache = new List<T>();
            }

            if (!cache.Contains(callback))
            {
                cache.Add(callback);
            }
        }

        private static void RemoveCallbackCache<T>(ref List<T> cache, T callback)
        {
            if (cache == null) return;

            if (cache.Remove(callback))
            {
                if (cache.Count == 0)
                {
                    cache = null;
                }
            }
        }

        public static void AddStartCallback(Action callback)
        {
            if (instance == null)
            {
                AddCallbackCache(ref startCallbackCache, callback);
                return;
            }

            instance.AddStartCallback(callback);
        }

        public static void RemoveStartCallback(Action callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref startCallbackCache, callback);
                return;
            }

            instance.RemoveStartCallback(callback);
        }

        public static void AddStopCallback(Action callback)
        {
            if (instance == null)
            {
                AddCallbackCache(ref stopCallbackCache, callback);
                return;
            }

            instance.AddStopCallback(callback);
        }

        public static void RemoveStopCallback(Action callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref stopCallbackCache, callback);
                return;
            }

            instance.RemoveStopCallback(callback);
        }

        public static void AddPauseCallback(Action callback)
        {
            if (instance == null)
            {
                AddCallbackCache(ref pauseCallbackCache, callback);
                return;
            }

            instance.AddPauseCallback(callback);
        }

        public static void RemovePauseCallback(Action callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref pauseCallbackCache, callback);
                return;
            }

            instance.RemovePauseCallback(callback);
        }

        public static void AddResumeCallback(Action callback)
        {
            if (instance == null)
            {
                AddCallbackCache(ref resumeCallbackCache, callback);
                return;
            }

            instance.AddResumeCallback(callback);
        }

        public static void RemoveResumeCallback(Action callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref resumeCallbackCache, callback);
                return;
            }

            instance.RemoveResumeCallback(callback);
        }

        public static void AddDeepLinkCallback(Action<string> callback, bool autoCall = false)
        {
            if (instance == null)
            {
                AddCallbackCache(ref deepLinkCallbackCache, callback);
                return;
            }

            instance.AddDeepLinkCallback(callback, autoCall);
        }

        public static void RemoveDeepLinkCallback(Action<string> callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref deepLinkCallbackCache, callback);
                return;
            }

            instance.RemoveDeepLinkCallback(callback);
        }

        public static void AddPushTokenCallback(Action<string> callback, bool autoCall = false)
        {
            if (instance == null)
            {
                AddCallbackCache(ref pushTokenCallbackCache, callback);
                return;
            }

            instance.AddPushTokenCallback(callback, autoCall);
        }

        public static void RemovePushTokenCallback(Action<string> callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref pushTokenCallbackCache, callback);
                return;
            }

            instance.RemovePushTokenCallback(callback);
        }

        public static void AddSendEventCallback(Action<string, string> callback)
        {
            if (instance == null)
            {
                AddCallbackCache(ref sendEventCallbackCache, callback);
                return;
            }

            instance.AddSendEventCallback(callback);
        }

        public static void RemoveSendEventCallback(Action<string, string> callback)
        {
            if (instance == null)
            {
                RemoveCallbackCache(ref sendEventCallbackCache, callback);
                return;
            }

            instance.RemoveSendEventCallback(callback);
        }

        public static void SetDefaultRemoteConfig(string data)
        {
            if (instance == null) return;
            instance.SetDefaultRemoteConfig(data);
        }

        public static void SetDefaultRemoteConfig(SimpleJSON.JSONObject remoteConfig)
        {
            if (instance == null) return;
            instance.SetDefaultRemoteConfig(remoteConfig);
        }

        public static void FetchRemoteConfig(Action<SimpleJSON.JSONObject> callback)
        {
            if (instance == null) return;
            instance.FetchRemoteConfig(callback);
        }

        public static void GdprForgetMe()
        {
            if (instance == null) return;
            instance.GdprForgetMe();
        }

        public static void DisableThirdPartySharing()
        {
            if (instance == null) return;
            instance.DisableThirdPartySharing();
        }

        public static void EnableThirdPartySharing()
        {
            if (instance == null) return;
            instance.EnableThirdPartySharing();
        }

        public static void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            if (instance == null) return;
            instance.LogThirdPartySharingSettings(settings);
        }

        public static void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, eventFlag);
        }

        public static void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void LogBootStart()
        {
            if (instance == null) return;
            instance.LogBootStart();
        }

        public static void LogBootEnd()
        {
            if (instance == null) return;
            instance.LogBootEnd();
        }

        public static void LogLevelStart(int id, string name)
        {
            if (instance == null) return;
            instance.LogLevelStart(id, name);
        }

        public static void LogLevelEnd(int id, string name, bool isSuccess)
        {
            if (instance == null) return;
            instance.LogLevelEnd(id, name, isSuccess);
        }

        public static void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdLoad(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdClose(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdClick(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdImpression(adUnit, adPlacementType, adPlacement);
        }

        public static void LogIAPInitialization(bool isSuccess)
        {
            if (instance == null) return;
            instance.LogIAPInitialization(isSuccess);
        }

        public static void LogIAPRestorePurchase()
        {
            if (instance == null) return;
            instance.LogIAPRestorePurchase();
        }

        public static void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            if (instance == null) return;
            instance.LogIAPResult(pack, price, amount, currency, isSuccess);
        }

        public static void LogIAPReceipt_Apple(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Apple(receipt);
        }

        public static void LogIAPReceipt_Google(string data, string signature)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Google(data, signature);
        }

        public static void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Amazon(userId, receiptId);
        }

        public static void LogIAPReceipt_Roku(string transactionId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Roku(transactionId);
        }

        public static void LogIAPReceipt_Windows(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Windows(receipt);
        }

        public static void LogIAPReceipt_Facebook(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Facebook(receipt);
        }

        public static void LogIAPReceipt_Unity(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Unity(receipt);
        }

        public static void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_AppStoreServer(transactionId);
        }

        public static void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlayProduct(productId, token);
        }

        public static void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlaySubscription(subscriptionId, token);
        }

        public static void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlaySubscriptionv2(token);
        }

        public static void LogUpdateApp(string newVersion)
        {
            if (instance == null) return;
            instance.LogUpdateApp(newVersion);
        }

        public static void LogRateApp()
        {
            if (instance == null) return;
            instance.LogRateApp();
        }

        public static void LogLocation(double latitude, double longitude)
        {
            if (instance == null) return;
            instance.LogLocation(latitude, longitude);
        }

        public static void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            if (instance == null) return;
            instance.LogAuthorizationTrackingStatus(status);
        }

        public static void LogFacebookLink(string userId)
        {
            if (instance == null) return;
            instance.LogFacebookLink(userId);
        }

        public static void LogFacebookUnlink()
        {
            if (instance == null) return;
            instance.LogFacebookUnlink();
        }

        public static void LogInstagramLink(string userId)
        {
            if (instance == null) return;
            instance.LogInstagramLink(userId);
        }

        public static void LogInstagramUnlink()
        {
            if (instance == null) return;
            instance.LogInstagramUnlink();
        }

        public static void LogAppleLink(string userId)
        {
            if (instance == null) return;
            instance.LogAppleLink(userId);
        }

        public static void LogAppleUnlink()
        {
            if (instance == null) return;
            instance.LogAppleUnlink();
        }

        public static void LogAppleGameCenterLink(string gamePlayerId)
        {
            if (instance == null) return;
            instance.LogAppleGameCenterLink(gamePlayerId);
        }

        public static void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            if (instance == null) return;
            instance.LogAppleGameCenterTeamLink(teamPlayerId);
        }

        public static void LogAppleGameCenterUnlink()
        {
            if (instance == null) return;
            instance.LogAppleGameCenterUnlink();
        }

        public static void LogGoogleLink(string userId)
        {
            if (instance == null) return;
            instance.LogGoogleLink(userId);
        }

        public static void LogGoogleUnlink()
        {
            if (instance == null) return;
            instance.LogGoogleUnlink();
        }

        public static void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesLink(gamePlayerId);
        }

        public static void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesDeveloperLink(developerPlayerKey);
        }

        public static void LogGooglePlayGameServicesUnlink()
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesUnlink();
        }

        public static void LogLinkedInLink(string personId)
        {
            if (instance == null) return;
            instance.LogLinkedInLink(personId);
        }

        public static void LogLinkedInUnlink()
        {
            if (instance == null) return;
            instance.LogLinkedInUnlink();
        }

        public static void LogMeetupLink(string userId)
        {
            if (instance == null) return;
            instance.LogMeetupLink(userId);
        }

        public static void LogMeetupUnlink()
        {
            if (instance == null) return;
            instance.LogMeetupUnlink();
        }

        public static void LogGitHubLink(string userId)
        {
            if (instance == null) return;
            instance.LogGitHubLink(userId);
        }

        public static void LogGitHubUnlink()
        {
            if (instance == null) return;
            instance.LogGitHubUnlink();
        }

        public static void LogDiscordLink(string userId)
        {
            if (instance == null) return;
            instance.LogDiscordLink(userId);
        }

        public static void LogDiscordUnlink()
        {
            if (instance == null) return;
            instance.LogDiscordUnlink();
        }

        public static void LogTwitterLink(string userId)
        {
            if (instance == null) return;
            instance.LogTwitterLink(userId);
        }

        public static void LogTwitterUnlink()
        {
            if (instance == null) return;
            instance.LogTwitterUnlink();
        }

        public static void LogSpotifyLink(string userId)
        {
            if (instance == null) return;
            instance.LogSpotifyLink(userId);
        }

        public static void LogSpotifyUnlink()
        {
            if (instance == null) return;
            instance.LogSpotifyUnlink();
        }

        public static void LogMicrosoftLink(string userId)
        {
            if (instance == null) return;
            instance.LogMicrosoftLink(userId);
        }

        public static void LogMicrosoftUnlink()
        {
            if (instance == null) return;
            instance.LogMicrosoftUnlink();
        }

        public static void LogLINELink(string userId)
        {
            if (instance == null) return;
            instance.LogLINELink(userId);
        }

        public static void LogLINEUnlink()
        {
            if (instance == null) return;
            instance.LogLINEUnlink();
        }

        public static void LogVKLink(string userId)
        {
            if (instance == null) return;
            instance.LogVKLink(userId);
        }

        public static void LogVKUnlink()
        {
            if (instance == null) return;
            instance.LogVKUnlink();
        }

        public static void LogQQLink(string openId)
        {
            if (instance == null) return;
            instance.LogQQLink(openId);
        }

        public static void LogQQUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogQQUnionLink(unionId);
        }

        public static void LogQQUnlink()
        {
            if (instance == null) return;
            instance.LogQQUnlink();
        }

        public static void LogWeChatLink(string openId)
        {
            if (instance == null) return;
            instance.LogWeChatLink(openId);
        }

        public static void LogWeChatUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogWeChatUnionLink(unionId);
        }

        public static void LogWeChatUnlink()
        {
            if (instance == null) return;
            instance.LogWeChatUnlink();
        }

        public static void LogTikTokLink(string openId)
        {
            if (instance == null) return;
            instance.LogTikTokLink(openId);
        }

        public static void LogTikTokUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogTikTokUnionLink(unionId);
        }

        public static void LogTikTokUnlink()
        {
            if (instance == null) return;
            instance.LogTikTokUnlink();
        }

        public static void LogWeiboLink(string userId)
        {
            if (instance == null) return;
            instance.LogWeiboLink(userId);
        }

        public static void LogWeiboUnlink()
        {
            if (instance == null) return;
            instance.LogWeiboUnlink();
        }

        public static void LogAccountLink(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type);
        }

        public static void LogAccountLink(string id, string type, string scopeId)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type, scopeId);
        }

        public static void LogAccountLink(string id, string type, string scopeId, string scopeType)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type, scopeId, scopeType);
        }

        public static void LogAccountUnlink(string type)
        {
            if (instance == null) return;
            instance.LogAccountUnlink(type);
        }

        public static void AddUserPhoneNumber(int countryCode, string number)
        {
            if (instance == null) return;
            instance.AddUserPhoneNumber(countryCode, number);
        }

        public static void RemoveUserPhoneNumber(int countryCode, string number)
        {
            if (instance == null) return;
            instance.RemoveUserPhoneNumber(countryCode, number);
        }

        public static void AddUserEmail(string email)
        {
            if (instance == null) return;
            instance.AddUserEmail(email);
        }

        public static void RemoveUserEmail(string email)
        {
            if (instance == null) return;
            instance.RemoveUserEmail(email);
        }

        public static void SetUserName(string firstName, string lastName)
        {
            if (instance == null) return;
            instance.SetUserName(firstName, lastName);
        }

        public static void SetUserFirstName(string firstName)
        {
            if (instance == null) return;
            instance.SetUserFirstName(firstName);
        }

        public static void SetUserLastName(string lastName)
        {
            if (instance == null) return;
            instance.SetUserLastName(lastName);
        }

        public static void SetUserCity(string city)
        {
            if (instance == null) return;
            instance.SetUserCity(city);
        }

        public static void SetUserState(string state)
        {
            if (instance == null) return;
            instance.SetUserState(state);
        }

        public static void SetUserCountry(string country)
        {
            if (instance == null) return;
            instance.SetUserCountry(country);
        }

        public static void SetUserZipCode(string zipCode)
        {
            if (instance == null) return;
            instance.SetUserZipCode(zipCode);
        }

        public static void SetUserDateOfBirth(int day, int month, int year)
        {
            if (instance == null) return;
            instance.SetUserDateOfBirth(day, month, year);
        }

        public static void SetUserDateOfBirth(int day, int month)
        {
            if (instance == null) return;
            instance.SetUserDateOfBirth(day, month);
        }

        public static void SetUserYearOfBirth(int year)
        {
            if (instance == null) return;
            instance.SetUserYearOfBirth(year);
        }

        public static void SetUserGender(UserGender gender)
        {
            if (instance == null) return;
            instance.SetUserGender(gender);
        }

        public static void LogWalletLink(string wallet, string type = "ethereum")
        {
            if (instance == null) return;
            instance.LogWalletLink(wallet, type);
        }

        public static void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            if (instance == null) return;
            instance.LogWalletUnlink(wallet, type);
        }

        public static void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            if (instance == null) return;
            instance.LogCryptoPayment(pack, price, amount, currency, chain);
        }

        public static void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            if (instance == null) return;
            instance.LogAdRevenue(source, revenue, currency, network, networkData);
        }

        //iOS only
        public static void RequestTrackingAuthorization(RequestAuthorizationTrackingCompleteHandler callback = null)
        {
            if (instance == null)
            {
                callback?.Invoke(AuthorizationTrackingStatus.NOT_DETERMINED);
                return;
            }

            instance.RequestTrackingAuthorization(callback);
        }

        //iOS only
        public static AuthorizationTrackingStatus GetTrackingAuthorizationStatus()
        {
            if (instance == null) return AuthorizationTrackingStatus.NOT_DETERMINED;
            return instance.GetTrackingAuthorizationStatus();
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue);
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue, coarseValue);
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue, coarseValue, lockWindow);
        }
    }
}