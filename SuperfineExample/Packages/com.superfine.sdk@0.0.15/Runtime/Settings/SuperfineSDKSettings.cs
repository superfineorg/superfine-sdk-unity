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
        [Flags]
        public enum PlatformFlag
        {
            None = 0,
            Android = 1 << 0,
            iOS = 1 << 1,
            Windows = 1 << 2,
            macOS = 1 << 3,
            Linux = 1 << 4,
            All = ~0
        }

        public enum ApsEnvironment
        {
            [InspectorName("Development")]
            DEVELOPMENT = 0,

            [InspectorName("Production")]
            PRODUCTION = 1
        }

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

        public void AddNativeModuleSettings(string classPath, PlatformFlag platform, string data = null)
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

            PlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

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

        [Label("Config URL")]
        public string configUrl = string.Empty;

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
        [Label("Disable Online SDK Config")]
        public bool disableOnlineSdkConfig = false;

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

        [Space(5)]
        [Foldout("Third-party Settings")]
        [Label("Snap App Id")]
        public string snapAppId = string.Empty;

        [Foldout("Deep Links")]
        [Space(10)]
        public bool captureDeepLinks = true;

        [Foldout("Deep Links")]
        [Space(5)]
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
        [Label("Initialization Actions")]
        public List<SuperfineSDKAndroidInitializationActionSettings> androidInitializationActions;

        [Foldout("Android Settings")]
        [Space(10)]
        [Label("Capture In-App Purchases")]
        public bool androidCaptureInAppPurchases = false;

        [Foldout("Android Settings")]
        [Space(10)]
        [Label("Topics API Ads SDK Names")]
        public string[] topicsAPIAdsSdkNames = null;

        [Foldout("Android Settings")]
        [Space(10)]
        [Label("Query Packages")]
        public string[] androidQueryPackages = null;

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
        [Label("BLUETOOTH_CONNECT")]
        public bool androidBluetoothConnect = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("GET_ACCOUNTS")]
        public bool androidGetAccounts = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("VIBRATE")]
        public bool androidPermissionVibrate = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("QUERY_ALL_PACKAGES")]
        public bool androidPermissionQueryAllPackages = false;

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
        [Label("BIND_JOB_SERVICE")]
        public bool androidPermissionJobService = false;

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

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("POST_NOTIFICATIONS")]
        public bool androidPostNotifications = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("SCHEDULE_EXACT_ALARM")]
        public bool androidScheduleExactAlarm = false;

        [Foldout("Android Settings")]
        [BoxGroup("Android Permissions")]
        [Label("REQUEST_IGNORE_BATTERY_OPTIMIZATIONS")]
        public bool androidRequestIgnoreBatteryOptimizations = false;

        [Foldout("iOS Settings")]
        [Space(5)]
        [Label("Store Type")]
        public StoreType iosStoreType = StoreType.APP_STORE;

        [Foldout("iOS Settings")]
        public bool debug = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("Initialization Actions")]
        public List<SuperfineSDKIosInitializationActionSettings> iosInitializationActions;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("Capture In-App Purchases")]
        public bool iosCaptureInAppPurchases = false;

        [Foldout("iOS Settings")]
        [Label("Use SKAdNetwork Conversion Schema")]
        public bool useSkanConversionSchema = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool setAdvertisingAttributionReportEndpoint = false;

        [Foldout("iOS Settings")]
        [ShowIf("setAdvertisingAttributionReportEndpoint")]
        [Label("Advertising Attribution Report Endpoint")]
        public string advertisingAttributionReportEndpoint = string.Empty;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool setUserTrackingUsageDescription = false;

        [Foldout("iOS Settings")]
        [ShowIf("setUserTrackingUsageDescription")]
        [Label("User Tracking Usage Description")]
        public string userTrackingUsageDescription = string.Empty;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("Disable Default SKAdNetworkID File")]
        public bool disableDefaultSKAdNetworkIdFile = false;

        [Foldout("iOS Settings")]
        [Label("Extra SKAdNetworkID Files")]
        public TextAsset[] extraSKAdNetworkIdFiles = null;

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
        [Label("CoreLocation")]
        public bool iosFrameworkCoreLocation = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("CoreTelephony")]
        public bool iosFrameworkCoreTelephony = true;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("Security")]
        public bool iosFrameworkSecurity = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("Zlib")]
        public bool iosLibZ = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Frameworks")]
        [Label("SQLite")]
        public bool iosLibSqlite3 = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Space(5)]
        [Label("Push Notifications")]
        public SuperfineNullable<ApsEnvironment> iosCapabilityPushNotifications = new SuperfineNullable<ApsEnvironment>(false, ApsEnvironment.DEVELOPMENT);

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Silent Notifications")]
        public bool iosCapabilitySilentNotifications = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Time Sensitive Notifications")]
        public bool iosCapabilityTimeSensitiveNotifications = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("In-App Purchase")]
        public bool iosCapabilityInAppPurchase = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Sign In with Apple")]
        public bool iosCapabilitySignInWithApple = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Game Center")]
        public bool iosCapabilityGameCenter = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("HomeKit")]
        public bool iosCapabilityHomeKit = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Data Protection")]
        public bool iosCapabilityDataProtection = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("HealthKit")]
        public bool iosCapabilityHealthKit = false;

        [Foldout("iOS Settings")]
        [BoxGroup("iOS Capabilities")]
        [Label("Siri")]
        public bool iosCapabilitySiri = false;

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
        [Label("Bundle Id (Windows and Linux)")]
        public string bundleId = string.Empty;

        [Foldout("Standalone Settings")]
        [Label("Build Number (Windows and Linux)")]
        public string buildNumber = "1";

        [Foldout("Standalone Settings")]
        [Space(10)]
        [Label("Desktop Path (Linux)")]
        public string desktopPath = "~/.local/share/applications";

        [Foldout("Standalone Settings")]
        [Label("Update Desktop Entries Command (Linux)")]
        public string updateDesktopDatabase = "update-desktop-database";

        [Foldout("Standalone Settings")]
        [Space(10)]
        [Label("App Data Usage Description (macOS)")]
        public string appDataUsageDescription = string.Empty;

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

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Space(5)]
        [Label("App Sandbox")]
        public bool macosCapabilityAppSandbox = true;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities", 1)]
        [ShowIf("macosCapabilityAppSandbox")]
        [ExtractChildren]
        public SuperfineSDKAppSandboxSettings macosAppSandboxSettings;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]

        [Label("Push Notifications")]
        [Space(5)]
        public SuperfineNullable<ApsEnvironment> macosCapabilityPushNotifications = new SuperfineNullable<ApsEnvironment>(false, ApsEnvironment.DEVELOPMENT);

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Label("Silent Notifications")]
        public bool macosCapabilitySilentNotifications = false;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Label("Time Sensitive Notifications")]
        public bool macosCapabilityTimeSensitiveNotifications = false;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Label("In-App Purchase")]
        public bool macosCapabilityInAppPurchase = false;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Label("Sign In with Apple")]
        public bool macosCapabilitySignInWithApple = false;

        [Foldout("Standalone Settings")]
        [BoxGroup("macOS Capabilities")]
        [Label("Game Center")]
        public bool macosCapabilityGameCenter = false;
    }
}