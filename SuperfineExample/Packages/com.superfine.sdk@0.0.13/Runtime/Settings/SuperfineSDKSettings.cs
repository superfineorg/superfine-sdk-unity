using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    public class SuperfineSDKSettings : ScriptableObject
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Resources/SuperfineSettings.asset";

        private static SuperfineSDKSettings instance = null;

        public static SuperfineSDKSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = Create();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }


        [MenuItem("Superfine/Edit Settings")]
        private static void Edit()
        {
            Selection.activeObject = Instance;
        }
#endif

        public static SuperfineSDKSettings Create()
        {
            return CreateInstance<SuperfineSDKSettings>();
        }

        public SuperfineSDKSettings Clone()
        {
            return Instantiate(this);
        }

        public static SuperfineSDKSettings LoadFromResources()
        {
            return Resources.Load<SuperfineSDKSettings>("SuperfineSettings");
        }

        public void AddModuleSettings<T>() where T : SuperfineSDKModuleSettings
        {
            modules.Add(CreateInstance<T>());
        }

        public void AddNativeModuleSettings(string classPath, SuperfineSDKPlatformFlag platform, string data = null)
        {
            SuperfineSDKNativeModuleSettings settings = new SuperfineSDKNativeModuleSettings()
            {
                classPath = classPath,
                platform = platform,
                data = new SuperfineSDKJSONData(data)
            };

            nativeModules.Add(settings);
        }

        public List<string> GetCustomSchemes()
        {
            if (customSchemes == null) return null;

            List<string> ret = null;

            SuperfineSDKPlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

            int numSchemes = customSchemes.Length;
            for (int i = 0; i < numSchemes; i++)
            {
                SuperfineSDKCustomSchemeSettings customSchemeSettings = customSchemes[i];
                if ((customSchemeSettings.platform & platformFlag) == 0) continue;

                if (ret == null)
                {
                    ret = new List<string>();
                }

                ret.Add(customSchemeSettings.scheme);
            }

            return ret;
        }

        public List<Tuple<string, bool>> GetAppLinks()
        {
#if UNITY_ANDROID
            List<Tuple<string, bool>> ret = null;

            int numAppLinks = appLinks.Length;
            for (int i = 0; i < numAppLinks; i++)
            {
                SuperfineSDKAppLinkSettings appLinkSettings = appLinks[i];

                if (ret == null)
                {
                    ret = new List<Tuple<string, bool>>();
                }

                ret.Add(new Tuple<string, bool>(appLinkSettings.host, !appLinkSettings.disableVerify));
            }

            return ret;
#else
            return null;
#endif
        }

        public List<string> GetAssociatedDomains()
        {
#if UNITY_IOS || UNITY_STANDALONE_OSX
            if (associatedDomains == null) return null;

            List<string> ret = null;

            int numAssociatedDomains = associatedDomains.Length;
            for (int i = 0; i < numAssociatedDomains; i++)
            {
                SuperfineSDKAssociatedDomainSettings associatedDomainSettings = associatedDomains[i];
#if UNITY_IOS
                if (!associatedDomainSettings.ios) continue;
#else //UNITY_STANDALONE_OSX
                if (!associatedDomainSettings.macos) continue;
#endif
                if (ret == null)
                {
                    ret = new List<string>();
                }

                ret.Add(associatedDomainSettings.host);
            }

            return ret;
#else
            return null;
#endif
        }

        public string appId = "YOUR APP ID";
        public string appSecret = "YOUR APP SECRET";

        [Space(10)]

        public string host = string.Empty;

        [Space(10)]

        public int flushQueueSize = 5;
        public long flushInterval = 15 * 1000;

        [Space(10)]
        public bool waitConfigId = false;
        public string configId = string.Empty;
        public string customUserId = string.Empty;

        [Space(10)]
        public bool autoStart = true;
        public bool offline = false;
        public bool sendInBackground = true;

        [Space(10)]
        public bool captureDeepLinks = true;

        [HideInInspector]
        public bool enableCoppa = false;

        [Space(10)]
        [Foldout("Modules")]
        [Label("Unity Modules")]
        public List<SuperfineSDKModuleSettings> modules;

        [Space(5)]
        [Foldout("Modules")]
        public List<SuperfineSDKNativeModuleSettings> nativeModules;

        [Space(10)]
        [Foldout("Third-party Settings")]
        public string facebookAppId = string.Empty;

        [Foldout("Third-party Settings")]
        public string instagramAppId = string.Empty;

        [Space(5)]
        [Foldout("Third-party Settings")]
        public string appleAppId = string.Empty;

        [Foldout("Third-party Settings")]
        public string appleSignInClientId = string.Empty;

        [Foldout("Third-party Settings")]
        public string appleDeveloperTeamId = string.Empty;

        [Space(5)]
        [Foldout("Third-party Settings")]
        public string googlePlayGameServicesProjectId = string.Empty;

        [Foldout("Third-party Settings")]
        public string googlePlayDeveloperAccountId = string.Empty;

        [Space(5)]
        [Foldout("Third-party Settings")]
        [Label("LinkedIn App Id")]
        public string linkedInAppId = string.Empty;

        [Space(5)]
        [Foldout("Third-party Settings")]
        [Label("QQ App Id")]
        public string qqAppId = string.Empty;

        [Foldout("Third-party Settings")]
        [Label("WeChat App Id")]
        public string weChatAppId = string.Empty;

        [Foldout("Third-party Settings")]
        [Label("TikTok App Id")]
        public string tikTokAppId = string.Empty;

        [Foldout("Deep Links")]
        [Space(10)]
        [Label("Custom URI Schemes")]
        public SuperfineSDKCustomSchemeSettings[] customSchemes = null;

        [Foldout("Deep Links")]
        [Space(5)]
        [Label("App Links (Android)")]
        public SuperfineSDKAppLinkSettings[] appLinks = null;

        [Foldout("Deep Links")]
        [Space(5)]
        [Label("Associated Domains (iOS and macOS)")]
        public SuperfineSDKAssociatedDomainSettings[] associatedDomains = null;

        [Foldout("Android Settings")]
        [Space(5)]
        [Label("Store Type")]
        public StoreType androidStoreType = StoreType.GOOGLE_PLAY;

        [Foldout("Android Settings")]
        [Label("Log Level")]
        public LogLevel androidLogLevel = LogLevel.NONE;

        [HideInInspector]
        public bool enableOaid = false;
        [HideInInspector]
        public bool enableFireAdvertisingId = true;
        [HideInInspector]
        public bool enablePlayStoreKidsApp = false;
        [HideInInspector]
        public bool enableReferrerSamsung = false;
        [HideInInspector]
        public bool enableReferrerXiaomi = false;
        [HideInInspector]
        public bool enableReferrerVivo = false;
        [HideInInspector]
        public bool enableReferrerHuawei = false;

        [Foldout("Android Settings")]
        [Space(10)]
        public bool enableReferrerMeta = true;

        [Foldout("Android Settings")]
        [Label("Enable IMEI")]
        public bool enableImei = false;

        [Foldout("Android Settings")]
        [Space(10)]
        [Label("Topics API Ads SDK Names")]
        public string[] topicsAPIAdsSdkNames = null;

        [Foldout("Android Settings")]
        [Space(10)]
        [Label("Capture In-App Purchases")]
        public bool androidCaptureInAppPurchases = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Space(5)]
        [Label("INTERNET")]
        public bool androidPermissionInternet = true;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("ACCESS_NETWORK_STATE")]
        public bool androidPermissionAccessNetworkState = true;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("WRITE_EXTERNAL_STORAGE")]
        public bool androidPermissionWriteExternalStorage = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("ACCESS_WIFI_STATE")]
        public bool androidPermissionAccessWifiState = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("VIBRATE")]
        public bool androidPermissionVibrate = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("AD_ID")]
        public bool androidPermissionAdId = true;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("BIND_GET_INSTALL_REFERRER_SERVICE")]
        public bool androidPermissionInstallReferrerService = true;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("READ_PHONE_STATE")]
        public bool androidPermissionReadPhoneState = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("READ_PRIVILEGED_PHONE_STATE")]
        public bool androidPermissionReadPrivilegedPhoneState = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("WAKE_LOCK")]
        public bool androidPermissionWakeLock = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("ACCESS_COARSE_LOCATION")]
        public bool androidPermissionAccessCoarseLocation = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("ACCESS_FINE_LOCATION")]
        public bool androidPermissionAccessFineLocation = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("CHECK_LICENSE")]
        public bool androidPermissionCheckLicense = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("ACCESS_ADSERVICES_ATTRIBUTION")]
        public bool androidPermissionAccessAdservicesAttribution = true;

        [Foldout("iOS Settings")]
        [Space(5)]
        [Label("Store Type")]
        public StoreType iosStoreType = StoreType.APP_STORE;

        [Foldout("iOS Settings")]
        public bool debug = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("Capture In-App Purchases")]
        public bool iosCaptureInAppPurchases = false;

        [Foldout("iOS Settings")]
        [Label("Use SKAdNetwork Conversion Schema")]
        public bool useSkanConversionSchema = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        public string advertisingAttributionReportEndpoint = string.Empty;

        [Foldout("iOS Settings")]
        [Space(10)]
        public string userTrackingUsageDescription = string.Empty;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Space(5)]
        [Label("AdSupport")]
        public bool iosFrameworkAdSupport = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("AdServices")]
        public bool iosFrameworkAdServices = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("StoreKit")]
        public bool iosFrameworkStoreKit = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("AppTrackingTransparency")]
        public bool iosFrameworkAppTrackingTransparency = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("AuthenticationServices")]
        public bool iosFrameworkAuthenticationServices = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("UserNotifications")]
        public bool iosFrameworkUserNotifications = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("CoreTelephony")]
        public bool iosFrameworkCoreTelephony = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("Security")]
        public bool iosFrameworkSecurity = false;

        [Foldout("Standalone Settings")]
        [Space(5)]
        [Label("Store Type")]
        public StoreType standaloneStoreType = StoreType.UNKNOWN;

        [Foldout("Standalone Settings")]
        [Label("Log Level")]
        public LogLevel standaloneLogLevel = LogLevel.NONE;

        [Foldout("Standalone Settings")]
        [Space(10)]
        public bool registerURIScheme = false;

        [Foldout("Standalone Settings")]
        [Space(10)]
        [InfoBox("Win and Linux only", EInfoBoxType.Normal)]
        public string bundleId = string.Empty;

        [Foldout("Standalone Settings")]
        public string buildNumber = "1";

        [Foldout("Standalone Settings")]
        [Space(10)]
        [InfoBox("Linux only", EInfoBoxType.Normal)]
        public string desktopPath = "~/.local/share/applications";

        [Foldout("Standalone Settings")]
        public string updateDesktopDatabase = "update-desktop-database";

        [Foldout("Standalone Settings")]
        [Space(10)]
        public string proxy = string.Empty;

        [Foldout("Standalone Settings")]
        public string caPath = string.Empty;

        [Foldout("Standalone Settings")]
        public bool sslVerify = true;

        [Foldout("Standalone Settings")]
        [Space(10)]
        public bool steamBuild = false;

        [Foldout("Standalone Settings")]
        [ShowIf("steamBuild")]
        public uint steamAppId = 0;
    }
}