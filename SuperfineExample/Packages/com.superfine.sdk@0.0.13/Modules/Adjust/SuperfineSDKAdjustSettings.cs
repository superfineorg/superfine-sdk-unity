using com.adjust.sdk;

using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineAdjustSettings", menuName = "Superfine/SuperfineAdjustSettings", order = 1)]
    public class SuperfineSDKAdjustSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/Adjust/SuperfineAdjustSettings.asset";

        private static SuperfineSDKAdjustSettings instance = null;

        public static SuperfineSDKAdjustSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKAdjustSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK Adjust settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKAdjustSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKAdjustSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK Adjust settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif

        [Space(10)]

        public string appToken = "ADJUST APP TOKEN";
        public AdjustEnvironment environment = AdjustEnvironment.Production;

        [Space(10)]
        public bool autoStart = true;
        public bool setCustomerId = true;

        [Space(10)]
        public SuperfineDictionary<string, string> eventTokenMap;

        [Space(10)]
        public bool sendAdRevenue = false;

        [Space(10)]
        public bool setDeepLink = true;
        public bool setPushToken = false;

        [Space(10)]
        public bool sendAttribution = true;
        public bool sendDeepLink = true;

        [BoxGroup("#App Secret")]
        [Label("App Secret", true)]
        public bool hasAppSecret = false;

        [BoxGroup("#App Secret", 1)]
        [Space(5)]
        [ShowIf("hasAppSecret")]
        [Label("Secret ID")]
        public long secretId = 0;

        [BoxGroup("#App Secret", 1)]
        [ShowIf("hasAppSecret")]
        [Label("Info 1")]
        public long secretInfo1 = 0;

        [BoxGroup("#App Secret", 1)]
        [ShowIf("hasAppSecret")]
        [Label("Info 2")]
        public long secretInfo2 = 0;

        [BoxGroup("#App Secret", 1)]
        [ShowIf("hasAppSecret")]
        [Label("Info 3")]
        public long secretInfo3 = 0;

        [BoxGroup("#App Secret", 1)]
        [ShowIf("hasAppSecret")]
        [Label("Info 4")]
        public long secretInfo4 = 0;

        [Space(10)]
        public SuperfineNullable<AdjustLogLevel> logLevel = new SuperfineNullable<AdjustLogLevel>(false, AdjustLogLevel.Info);

        [Space(10)]
        public SuperfineNullable<bool> sendInBackground = new SuperfineNullable<bool>(false, false);
        public SuperfineNullable<bool> eventBuffering = new SuperfineNullable<bool>(false, false);

        [Space(10)]
        [Label("COPPA Compliant")]
        public SuperfineNullable<bool> coppaCompliant = new SuperfineNullable<bool>(false, false);
        public SuperfineNullable<bool> playStoreKidsApp = new SuperfineNullable<bool>(false, false);

        [Space(10)]
        public bool launchDeferredDeeplink = true;

        [Space(10)]
		[Label("Delay Start (s)")]
        public SuperfineNullable<double> delayStart = new SuperfineNullable<double>(false, 0.0);
        [Label("Cost Data In Attribution Callback")]
        public SuperfineNullable<bool> needsCost = new SuperfineNullable<bool>(false, false);

        [Space(10)]
        [Label("URL Strategy")]
        public AdjustUrlStrategy urlStrategy = AdjustUrlStrategy.Default;
        public string defaultTracker = string.Empty;
        
        [Foldout("Android Settings")]
        [Space(10)]
        public string processName = string.Empty;

        [Foldout("Android Settings")]
        [Space(10)]
        public SuperfineNullable<bool> preinstallTracking = new SuperfineNullable<bool>(false, false);

        [Foldout("Android Settings")]
        public string preinstallFilePath = string.Empty;

        [Foldout("Android Settings")]
        [Space(10)]
        public SuperfineNullable<bool> finalAttribution = new SuperfineNullable<bool>(false, false);

        [Foldout("Android Settings")]
        public SuperfineNullable<bool> readDeviceInfoOnce = new SuperfineNullable<bool>(false, false);

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("LinkMe")]
        public SuperfineNullable<bool> linkMe = new SuperfineNullable<bool>(false, false);

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("AdServices Info Reading")]
        public SuperfineNullable<bool> allowAdServicesInfoReading = new SuperfineNullable<bool>(false, true);

        [Foldout("iOS Settings")]
        [Label("IDFA Info Reading")]
        public SuperfineNullable<bool> allowIdfaReading = new SuperfineNullable<bool>(false, true);

        [Foldout("iOS Settings")]
        [Label("SKAdNetwork Handling")]
        public bool skanHandling = true;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("ATT Consent Waiting Interval (s)")]
        public SuperfineNullable<int> attTimeout = new SuperfineNullable<int>(false, 0);

        public override string GetModuleName()
        {
            return "AdjustUnity";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKAdjustModule(this);
        }
    }
}
