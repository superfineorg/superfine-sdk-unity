using System.Collections.Generic;
using System.Text;
namespace Superfine.Unity
{
    public abstract class SuperfineSDKManagerBase
    {
        protected abstract void InitializeNative(SuperfineSDKSettings settings, List<string> moduleNameList);
        
        public abstract bool IsInitialized();

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

        protected static SimpleJSON.JSONObject CreateInitializationJSONObject(SuperfineSDKSettings settings, List<string> moduleNameList)
        {
            SimpleJSON.JSONObject jsonObject = new SimpleJSON.JSONObject
            {
                { "appId", settings.appId },
                { "appSecret", settings.appSecret },
                { "flushQueueSize", settings.flushQueueSize },
                { "flushInterval", settings.flushInterval },
                { "wrapper", SuperfineSDK.WRAPPER },
                { "wrapperVersion", SuperfineSDK.WRAPPER_VERSION }
            };

            AddString(jsonObject, "configId", settings.configId);
            jsonObject.Add("waitConfigId", settings.waitConfigId);
            AddString(jsonObject, "customUserId", settings.customUserId);

            AddString(jsonObject, "host", settings.host);
            AddString(jsonObject, "configUrl", settings.configUrl);

            jsonObject.Add("autoStart", settings.autoStart);
            jsonObject.Add("offline", settings.offline);
            jsonObject.Add("sendInBackground", settings.sendInBackground);

            jsonObject.Add("disableOnlineSdkConfig", settings.disableOnlineSdkConfig);

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

            AddString(jsonObject, "snapAppId", settings.snapAppId);

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

            List<SuperfineSDKAndroidInitializationActionSettings> initializationActions = settings.androidInitializationActions;
            if (initializationActions != null)
            {
                int numActions = initializationActions.Count;
                if (numActions > 0)
                {
                    SimpleJSON.JSONArray initializationActionArray = new SimpleJSON.JSONArray();

                    for (int i = 0; i < numActions; i++)
                    {
                        initializationActionArray.Add(initializationActions[i].CreateJSONObject());
                    }

                    jsonObject.Add("initializationActions", initializationActionArray);
                }
            }

            AddStringArray(jsonObject, "topicsAPIAdsSdkNames", settings.topicsAPIAdsSdkNames);

            jsonObject.Add("captureInAppPurchases", settings.androidCaptureInAppPurchases);
#elif UNITY_IOS
            jsonObject.Add("storeType", settings.iosStoreType.ToString());
            jsonObject.Add("debug", settings.debug);

            List<SuperfineSDKIosInitializationActionSettings> initializationActions = settings.iosInitializationActions;
            if (initializationActions != null)
            {
                int numActions = initializationActions.Count;
                if (numActions > 0)
                {
                    SimpleJSON.JSONArray initializationActionArray = new SimpleJSON.JSONArray();

                    for (int i = 0; i < numActions; i++)
                    {
                        initializationActionArray.Add(initializationActions[i].CreateJSONObject());
                    }

                    jsonObject.Add("initializationActions", initializationActionArray);
                }
            }

            jsonObject.Add("captureDeepLinks", false);

            jsonObject.Add("captureInAppPurchases", settings.iosCaptureInAppPurchases);
            jsonObject.Add("useSkanConversionSchema", settings.useSkanConversionSchema);
#endif
#endif

            if (moduleNameList != null)
            {
                SimpleJSON.JSONArray excludedModuleNames = new SimpleJSON.JSONArray();

                int numModules = moduleNameList.Count;
                for (int i = 0; i < numModules; i++)
                {
                    excludedModuleNames.Add(moduleNameList[i]);
                }

                jsonObject.Add("excludedModuleNames", excludedModuleNames);
            }

            List<SuperfineSDKNativeModuleSettings> moduleSettingList = settings.nativeModules;
            if (moduleSettingList != null)
            {
                SimpleJSON.JSONArray moduleArray = new SimpleJSON.JSONArray();

                HashSet<string> classPathSet = new HashSet<string>();

                SuperfineSDKSettings.PlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

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

            return jsonObject;
        }

        public abstract void Start();
        public abstract void Stop();

        public abstract void SetOffline(bool value);

        public abstract string GetVersion();

        public virtual void Execute(string eventName, object param = null) {}

        public abstract void SetConfigId(string configId);
        public abstract void SetCustomUserId(string userId);

        public abstract string GetAppId();

        public abstract string GetUserId();

        public abstract string GetSessionId();

        public abstract string GetHost();
        public abstract string GetConfigUrl();

        public abstract string GetSdkConfig();

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
        public abstract string GetSnapAppId();

        public abstract void FetchRemoteConfig();

        public abstract void GdprForgetMe();
        public abstract void DisableThirdPartySharing();
        public abstract void EnableThirdPartySharing();
        public abstract void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings);

        public abstract void Log(string eventName, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE);
        public abstract void Log(SuperfineSDKEvent eventData);

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
    }
}
