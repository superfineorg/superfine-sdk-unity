using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Superfine.Unity
{
    public abstract class SuperfineSDKManagerBase
    {
        public abstract void Initialize(SuperfineSDKSettings settings);
        public abstract void Destroy();

        protected static string GetMapString(Dictionary<string, string> data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            bool isFirst = true;

            StringBuilder sb = new StringBuilder();

            foreach (var pair in data)
            {
                string key = pair.Key;
                if (string.IsNullOrEmpty(key)) continue;

                string value = pair.Value;
                if (string.IsNullOrEmpty(value)) continue;

                if (!isFirst) sb.Append(',');

                sb.Append(key).Append(':').Append(value);
            }

            return sb.ToString();
        }

        protected static string GetString(SuperfineSDKThirdPartySharingSettings settings)
        {
            SimpleJSON.JSONObject jsonObject = new SimpleJSON.JSONObject();

            var values = settings.GetValues();
            if (values.Count > 0)
            {
                jsonObject.Add("values", SimpleJSON.JSON.ToJSONNode(values));
            }

            var flags = settings.GetFlags();
            if (flags.Count > 0)
            {
                jsonObject.Add("flags", SimpleJSON.JSON.ToJSONNode(flags));
            }

            return jsonObject.ToString();
        }

        private static void AddString(SimpleJSON.JSONObject jsonObject, string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            jsonObject.Add(key, value);
        }

        private static void AddStringArray(SimpleJSON.JSONObject jsonObject, string key, string[] values)
        {
            if (values == null) return;

            int numValues = values.Length;
            if (numValues == 0) return;

            SimpleJSON.JSONArray jsonArray = new SimpleJSON.JSONArray();
            for (int i = 0; i < numValues; ++i)
            {
                string value = values[i];
                if (string.IsNullOrEmpty(value)) continue;

                jsonArray.Add(value);
            }

            if (jsonArray.Count > 0)
            {
                jsonObject.Add(key, values);
            }
        }

        protected static string GetString(SuperfineSDKSettings settings)
        {
            if (settings == null) return string.Empty;

            SimpleJSON.JSONObject jsonObject = new SimpleJSON.JSONObject
            {
                { "appId", settings.appId },
                { "appSecret", settings.appSecret }
            };
                        
            jsonObject.Add("flushQueueSize", settings.flushQueueSize);
            jsonObject.Add("flushInterval", settings.flushInterval);

            jsonObject.Add("wrapperVersion", SuperfineSDK.VERSION);

            if (!string.IsNullOrEmpty(settings.configId))
            {
                jsonObject.Add("configId", settings.configId);
            }
            jsonObject.Add("waitConfigId", settings.waitConfigId);

            if (!string.IsNullOrEmpty(settings.customUserId))
            {
                jsonObject.Add("customUserId", settings.customUserId);
            }

            if (!string.IsNullOrEmpty(settings.host))
            {
                jsonObject.Add("host", settings.host);
            }

            jsonObject.Add("autoStart", settings.autoStart);
            jsonObject.Add("offline", settings.offline);
            jsonObject.Add("sendInBackground", settings.sendInBackground);

            jsonObject.Add("enableCoppa", settings.enableCoppa);

            AddString(jsonObject, "facebookAppId", settings.facebookAppId);
            AddString(jsonObject, "instagramAppId", settings.instagramAppId);

            AddString(jsonObject, "appleAppId", settings.appleAppId);
            AddString(jsonObject, "appleSignInClientId", settings.appleSignInClientId);
            AddString(jsonObject, "appleDeveloperTeamId", settings.appleDeveloperTeamId);

            AddString(jsonObject, "googlePlayGameServicesProjectId", settings.googlePlayGameServicesProjectId);
            AddString(jsonObject, "googlePlayDeveloperAccountId", settings.googlePlayDeveloperAccountId);

            AddString(jsonObject, "linkedInAppId", settings.linkedInAppId);

            AddString(jsonObject, "qqAppId", settings.qqAppId);
            AddString(jsonObject, "weChatAppId", settings.weChatAppId);
            AddString(jsonObject, "tikTokAppId", settings.tikTokAppId);

#if !UNITY_EDITOR
#if UNITY_ANDROID
            jsonObject.Add("storeType", settings.androidStoreType.ToString());
            jsonObject.Add("logLevel", settings.androidLogLevel.ToString());

            jsonObject.Add("captureDeepLinks", false);

            jsonObject.Add("enableImei", settings.enableImei);
            jsonObject.Add("enableOaid", settings.enableOaid);

            jsonObject.Add("enableFireAdvertisingId", settings.enableFireAdvertisingId);

            jsonObject.Add("enablePlayStoreKidsApp", settings.enablePlayStoreKidsApp);

            jsonObject.Add("enableReferrerSamsung", settings.enableReferrerSamsung);
            jsonObject.Add("enableReferrerXiaomi", settings.enableReferrerXiaomi);
            jsonObject.Add("enableReferrerVivo", settings.enableReferrerVivo);
            jsonObject.Add("enableReferrerHuawei", settings.enableReferrerHuawei);
            jsonObject.Add("enableReferrerMeta", settings.enableReferrerMeta);

            AddStringArray(jsonObject, "topicsAPIAdsSdkNames", settings.topicsAPIAdsSdkNames);

            jsonObject.Add("captureInAppPurchases", settings.androidCaptureInAppPurchases);
#elif UNITY_IOS
            jsonObject.Add("storeType", settings.iosStoreType.ToString());
            jsonObject.Add("debug", settings.debug);

            jsonObject.Add("captureDeepLinks", false);

            jsonObject.Add("captureInAppPurchases", settings.iosCaptureInAppPurchases);
            jsonObject.Add("useSkanConversionSchema", settings.useSkanConversionSchema);
#endif
#endif

            List<SuperfineSDKNativeModuleSettings> moduleSettingList = settings.nativeModules;
            if (moduleSettingList != null)
            {
                SimpleJSON.JSONArray moduleArray = new SimpleJSON.JSONArray();

                HashSet<string> classPathSet = new HashSet<string>();

                SuperfineSDKPlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

                int numModules = moduleSettingList.Count;
                for (int i = 0; i < numModules; ++i)
                {
                    SuperfineSDKNativeModuleSettings moduleSettings = moduleSettingList[i];

                    string classPath = moduleSettings.classPath;
                    if (string.IsNullOrEmpty(classPath)) continue;

                    if (!moduleSettings.platform.HasFlag(platformFlag)) continue;

                    if (classPathSet.Contains(classPath)) continue;
                    classPathSet.Add(classPath);

                    SimpleJSON.JSONObject moduleObject = new SimpleJSON.JSONObject
                    {
                        { "classPath", classPath }
                    };

                    if (moduleSettings.data != null)
                    {
                        SimpleJSON.JSONObject dataObject = moduleSettings.data.GetJsonObject();
                        if (dataObject != null)
                        {
                            moduleObject.Add("data", dataObject);
                        }
                    }

                    moduleArray.Add(moduleObject);
                }

                if (moduleArray.Count > 0)
                {
                    jsonObject.Add("modules", moduleArray);
                }
            }

            return jsonObject.ToString();
        }

        public abstract void Start();
        public abstract void Stop();

        public abstract void SetOffline(bool value);

        public abstract string GetVersion();

        public virtual void Execute(string eventName, object param = null) { }

        public abstract void SetConfigId(string configId);
        public abstract void SetCustomUserId(string userId);
        public abstract void SetAdvertisingId(string advertisingId);

        public abstract string GetAppId();

        public abstract string GetUserId();

        public abstract string GetSessionId();

        public abstract string GetHost();

        public abstract StoreType GetStoreType();

        public abstract string GetFacebookAppId();
        public abstract string GetInstagramAppId();
        public abstract string GetAppleAppId();
        public abstract string GetAppleSignInClientId();
        public abstract string GetAppleDeveloperTeamId();
        public abstract string GetGooglePlayGameServicesProjectId();
        public abstract string GetGooglePlayDeveloperAccountId();
        public abstract string GetLinkedInAppId();
        public abstract string GetQQAppId();
        public abstract string GetWeChatAppId();
        public abstract string GetTikTokAppId();

        public abstract string GetDeepLinkUrl();

        public abstract void SetPushToken(string token);

        public abstract string GetPushToken();

        protected static Action onStart = null;

        public void AddStartCallback(Action callback)
        {
            bool isNew = (onStart == null);

            if (isNew)
            {
                onStart += callback;
                RegisterNativeStartCallback();
            }
            else
            {
                if (onStart.GetInvocationList().Contains(callback)) return;
                onStart += callback;
            }
        }

        public void RemoveStartCallback(Action callback)
        {
            if (onStart == null) return;

            onStart -= callback;
            if (onStart == null)
            {
                UnregisterNativeStartCallback();
            }
        }

        protected static Action onStop = null;

        public void AddStopCallback(Action callback)
        {
            bool isNew = (onStop == null);

            if (isNew)
            {
                onStop += callback;
                RegisterNativeStopCallback();
            }
            else
            {
                if (onStop.GetInvocationList().Contains(callback)) return;
                onStop += callback;
            }
        }

        public void RemoveStopCallback(Action callback)
        {
            if (onStop == null) return;

            onStop -= callback;
            if (onStop == null)
            {
                UnregisterNativeStopCallback();
            }
        }

        protected static Action onPause = null;

        public void AddPauseCallback(Action callback)
        {
            bool isNew = (onPause == null);

            if (isNew)
            {
                onPause += callback;
                RegisterNativePauseCallback();
            }
            else
            {
                if (onPause.GetInvocationList().Contains(callback)) return;
                onPause += callback;
            }
        }

        public void RemovePauseCallback(Action callback)
        {
            if (onPause == null) return;

            onPause -= callback;
            if (onPause == null)
            {
                UnregisterNativePauseCallback();
            }
        }

        protected static Action onResume = null;

        public void AddResumeCallback(Action callback)
        {
            bool isNew = (onResume == null);

            if (isNew)
            {
                onResume += callback;
                RegisterNativeResumeCallback();
            }
            else
            {
                if (onResume.GetInvocationList().Contains(callback)) return;
                onResume += callback;
            }
        }

        public void RemoveResumeCallback(Action callback)
        {
            if (onResume == null) return;

            onResume -= callback;
            if (onPause == null)
            {
                UnregisterNativeResumeCallback();
            }
        }

        protected static Action<string> onSetDeepLink = null;

        public void AddDeepLinkCallback(Action<string> callback, bool autoCall)
        {
            bool isNew = (onSetDeepLink == null);

            if (isNew)
            {
                onSetDeepLink += callback;
                RegisterNativeDeepLinkCallback();
            }
            else
            {
                if (onSetDeepLink.GetInvocationList().Contains(callback)) return;
                onSetDeepLink += callback;
            }

            if (autoCall)
            {
                string deepLinkUrl = GetDeepLinkUrl();
                if (!string.IsNullOrEmpty(deepLinkUrl)) callback(deepLinkUrl);
            }
        }

        public void RemoveDeepLinkCallback(Action<string> callback)
        {
            if (onSetDeepLink == null) return;

            onSetDeepLink -= callback;
            if (onSetDeepLink == null)
            {
                UnregisterNativeDeepLinkCallback();
            }
        }

        protected static Action<string> onSetPushToken = null;

        public void AddPushTokenCallback(Action<string> callback, bool autoCall)
        {
            bool isNew = (onSetPushToken == null);

            if (isNew)
            {
                onSetPushToken += callback;
                RegisterNativePushTokenCallback();
            }
            else
            {
                if (onSetPushToken.GetInvocationList().Contains(callback)) return;
                onSetPushToken += callback;
            }

            if (autoCall)
            {
                string pushToken = GetPushToken();
                if (!string.IsNullOrEmpty(pushToken)) callback(pushToken);
            }
        }

        public void RemovePushTokenCallback(Action<string> callback)
        {
            if (onSetPushToken == null) return;

            onSetPushToken -= callback;
            if (onSetPushToken == null)
            {
                UnregisterNativePushTokenCallback();
            }
        }

        protected static Action<string, string> onSendEvent = null;

        public void AddSendEventCallback(Action<string, string> callback)
        {
            bool isNew = (onSendEvent == null);

            if (isNew)
            {
                onSendEvent += callback;
                RegisterNativeSendEventCallback();
            }
            else
            {
                if (onSendEvent.GetInvocationList().Contains(callback)) return;
                onSendEvent += callback;
            }
        }

        public void RemoveSendEventCallback(Action<string, string> callback)
        {
            if (onSendEvent == null) return;

            onSendEvent -= callback;
            if (onSendEvent == null)
            {
                UnregisterNativeSendEventCallback();
            }
        }

        protected static bool isFetchingRemoteConfig = false;
        protected static Action<SimpleJSON.JSONObject> remoteConfigCallback = null;

        protected static SimpleJSON.JSONObject defaultRemoteConfig = null;

        protected static void onReceiveRemoteConfig(string data)
        {
            if (remoteConfigCallback != null)
            {
                SimpleJSON.JSONObject remoteConfig = defaultRemoteConfig;

                if (!string.IsNullOrEmpty(data))
                {
                    if (SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node) && node.IsObject)
                    {
                        remoteConfig = (SimpleJSON.JSONObject)node;
                    }
                }

                remoteConfigCallback.Invoke(remoteConfig);
                remoteConfigCallback = null;
            }

            isFetchingRemoteConfig = false;
        }

        public void SetDefaultRemoteConfig(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node) && node.IsObject)
                {
                    SetDefaultRemoteConfig((SimpleJSON.JSONObject)node);
                }
            }
        }

        public void SetDefaultRemoteConfig(SimpleJSON.JSONObject remoteConfig)
        {
            defaultRemoteConfig = remoteConfig;
        }

        public void FetchRemoteConfig(Action<SimpleJSON.JSONObject> callback)
        {
            if (isFetchingRemoteConfig) return;

            isFetchingRemoteConfig = true;
            remoteConfigCallback = callback;

            FetchNativeRemoteConfig();
        }

        protected abstract void FetchNativeRemoteConfig();

        protected abstract void RegisterNativeStartCallback();
        protected abstract void UnregisterNativeStartCallback();

        protected abstract void RegisterNativeStopCallback();
        protected abstract void UnregisterNativeStopCallback();

        protected abstract void RegisterNativePauseCallback();
        protected abstract void UnregisterNativePauseCallback();

        protected abstract void RegisterNativeResumeCallback();
        protected abstract void UnregisterNativeResumeCallback();

        protected abstract void RegisterNativeDeepLinkCallback();
        protected abstract void UnregisterNativeDeepLinkCallback();

        protected abstract void RegisterNativePushTokenCallback();
        protected abstract void UnregisterNativePushTokenCallback();

        protected abstract void RegisterNativeSendEventCallback();
        protected abstract void UnregisterNativeSendEventCallback();

        public abstract void GdprForgetMe();
        public abstract void DisableThirdPartySharing();
        public abstract void EnableThirdPartySharing();
        public abstract void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings);

        public abstract void Log(string eventName, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE);

        public abstract void LogBootStart();
        public abstract void LogBootEnd();

        public abstract void LogLevelStart(int id, string name);
        public abstract void LogLevelEnd(int id, string name, bool isSuccess);

        public abstract void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);

        public abstract void LogIAPInitialization(bool isSuccess);
        public abstract void LogIAPRestorePurchase();
        public abstract void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess);

        public abstract void LogIAPReceipt_Apple(string receipt);
        public abstract void LogIAPReceipt_Google(string data, string signature);
        public abstract void LogIAPReceipt_Amazon(string userId, string receiptId);
        public abstract void LogIAPReceipt_Roku(string transactionId);
        public abstract void LogIAPReceipt_Windows(string receipt);
        public abstract void LogIAPReceipt_Facebook(string receipt);
        public abstract void LogIAPReceipt_Unity(string receipt);

        public abstract void LogIAPReceipt_AppStoreServer(string transactionId);
        public abstract void LogIAPReceipt_GooglePlayProduct(string productId, string token);
        public abstract void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token);
        public abstract void LogIAPReceipt_GooglePlaySubscriptionv2(string token);

        public abstract void LogUpdateApp(string newVersion);
        public abstract void LogRateApp();

        public abstract void LogLocation(double latitude, double longitude);

        public abstract void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status);

        public abstract void LogFacebookLink(string userId);
        public abstract void LogFacebookUnlink();
        public abstract void LogInstagramLink(string userId);
        public abstract void LogInstagramUnlink();
        public abstract void LogAppleLink(string userId);
        public abstract void LogAppleUnlink();
        public abstract void LogAppleGameCenterLink(string gamePlayerId);
        public abstract void LogAppleGameCenterTeamLink(string teamPlayerId);
        public abstract void LogAppleGameCenterUnlink();
        public abstract void LogGoogleLink(string userId);
        public abstract void LogGoogleUnlink();
        public abstract void LogGooglePlayGameServicesLink(string gamePlayerId);
        public abstract void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey);
        public abstract void LogGooglePlayGameServicesUnlink();
        public abstract void LogLinkedInLink(string personId);
        public abstract void LogLinkedInUnlink();
        public abstract void LogMeetupLink(string userId);
        public abstract void LogMeetupUnlink();
        public abstract void LogGitHubLink(string userId);
        public abstract void LogGitHubUnlink();
        public abstract void LogDiscordLink(string userId);
        public abstract void LogDiscordUnlink();
        public abstract void LogTwitterLink(string userId);
        public abstract void LogTwitterUnlink();
        public abstract void LogSpotifyLink(string userId);
        public abstract void LogSpotifyUnlink();
        public abstract void LogMicrosoftLink(string userId);
        public abstract void LogMicrosoftUnlink();
        public abstract void LogLINELink(string userId);
        public abstract void LogLINEUnlink();
        public abstract void LogVKLink(string userId);
        public abstract void LogVKUnlink();
        public abstract void LogQQLink(string openId);
        public abstract void LogQQUnionLink(string unionId);
        public abstract void LogQQUnlink();
        public abstract void LogWeChatLink(string openId);
        public abstract void LogWeChatUnionLink(string unionId);
        public abstract void LogWeChatUnlink();
        public abstract void LogTikTokLink(string openId);
        public abstract void LogTikTokUnionLink(string unionId);
        public abstract void LogTikTokUnlink();
        public abstract void LogWeiboLink(string userId);
        public abstract void LogWeiboUnlink();

        public abstract void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "");
        public abstract void LogAccountUnlink(string type);

        public abstract void AddUserPhoneNumber(int countryCode, string number);
        public abstract void RemoveUserPhoneNumber(int countryCode, string number);
        public abstract void AddUserEmail(string email);
        public abstract void RemoveUserEmail(string email);
        public abstract void SetUserName(string firstName, string lastName);
        public abstract void SetUserFirstName(string firstName);
        public abstract void SetUserLastName(string lastName);
        public abstract void SetUserCity(string city);
        public abstract void SetUserState(string state);
        public abstract void SetUserCountry(string country);
        public abstract void SetUserZipCode(string zipCode);
        public abstract void SetUserDateOfBirth(int day, int month, int year);
        public abstract void SetUserDateOfBirth(int day, int month);
        public abstract void SetUserYearOfBirth(int year);
        public abstract void SetUserGender(UserGender gender);

        public abstract void LogWalletLink(string wallet, string type = "ethereum");
        public abstract void LogWalletUnlink(string wallet, string type = "ethereum");

        public abstract void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum");

        public abstract void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null);

        //iOS only
        public virtual void RequestTrackingAuthorization(RequestAuthorizationTrackingCompleteHandler callback = null)
        {
            throw new System.Exception("This feature is for iOS only");
            //callback?.Invoke(AuthorizationTrackingStatus.NOT_DETERMINED);
        }

        //iOS only
        public virtual AuthorizationTrackingStatus GetTrackingAuthorizationStatus()
        {
            throw new System.Exception("This feature is for iOS only");
            //return AuthorizationTrackingStatus.NOT_DETERMINED;
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        //Standalone only
        public virtual void SetSteamDRMCheck(Func<uint, int> func)
        {
            throw new System.Exception("This feature is for Standalone only");
        }

        public abstract void OpenURL(string url);
    }
}
