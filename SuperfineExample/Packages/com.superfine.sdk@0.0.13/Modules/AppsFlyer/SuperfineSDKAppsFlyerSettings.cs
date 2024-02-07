using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineAppsFlyerSettings", menuName = "Superfine/SuperfineAppsFlyerSettings", order = 1)]
    public class SuperfineSDKAppsFlyerSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/AppsFlyer/SuperfineAppsFlyerSettings.asset";

        private static SuperfineSDKAppsFlyerSettings instance = null;

        public static SuperfineSDKAppsFlyerSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKAppsFlyerSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK AppsFlyer settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKAppsFlyerSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKAppsFlyerSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK AppsFlyer settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif

        [Space(10)]
        public string devKey = "APPSFLYER DEV KEY";

        [Space(10)]
        public bool autoStart = true;
        public bool setCustomerId = true;

        [Space(10)]
        public bool sendLog = false;
        public string[] logFilters;
        public string[] logNotFilters;

        [Space(10)]
        public bool sendLocation = false;

        [Space(10)]
        public bool sendAttribution = true;
        public bool sendDeepLink = true;

        [Space(10)]
        public bool setPushToken = false;

        [Space(10)]
        public bool debug = false;

        [Space(10)]
        [Label("OneLink Custom Domains")]
        public string[] oneLinkCustomDomains;
        [Label("Resolve Deep Link URLs")]
        public string[] resolveDeepLinkUrls;

        [Space(10)]
        [Label("Min Time Between Sessions (s)")]
        public SuperfineNullable<int> minTimeBetweenSessions = new SuperfineNullable<int>(false, 5);

        [Foldout("Android Settings")]
        [Space(10)]
        public bool collectIMEI = true;

        [Foldout("Android Settings")]
        public bool collectOAID = false;

        [Foldout("Android Settings")]
        public bool collectAndroidId = true;

        [Foldout("Android Settings")]
        [Space(10)]
        public bool disableAdvertisingIdentifiers = false;

        [Foldout("Android Settings")]
        public bool disableNetworkData = false;

        [Foldout("Android Settings")]
        public bool enableFacebookDeferredApplinks = false;

        [Foldout("Android Settings")]
        [Space(10)]
        public string outOfStore = string.Empty;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool setDeepLink = true;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool collectDeviceName = true;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool disableSkan = false;

        [Foldout("iOS Settings")]
        public bool disableIDFVCollection = false;

        [Foldout("iOS Settings")]
        public bool disableCollectAppleAdSupport = false;

        [Foldout("iOS Settings")]
        [Label("Disable Collect iAd")]
        public bool disableCollectIAd = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("Wait for ATT Timeout (s)")]
        public SuperfineNullable<int> attTimeout = new SuperfineNullable<int>(false, 0);

        public override string GetModuleName()
        {
            return "AppsFlyerUnity";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKAppsFlyerModule(this);
        }
    }
}
