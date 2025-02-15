using com.adjust.sdk;

using UnityEngine;
using System.IO;
using System.Collections.Generic;


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
            return "Adjust";
        }

        public override void MergeSdkConfig(SimpleJSON.JSONObject sdkConfig)
        {
            if (TryGetSdkConfigString(sdkConfig, "appToken", out string serverAppToken))
            {
                appToken = serverAppToken;
            }

            if (TryGetSdkConfigString(sdkConfig, "environment", out string serverEnvironment))
            {
                serverEnvironment = serverEnvironment.ToLower();
                if (serverEnvironment == "production") environment = AdjustEnvironment.Production;
                else if (serverEnvironment == "sandbox") environment = AdjustEnvironment.Sandbox;
            }

            if (TryGetSdkConfigBool(sdkConfig, "autoStart", out bool serverAutoStart))
            {
                autoStart = serverAutoStart;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setCustomerId", out bool serverSetCustomerId))
            {
                setCustomerId = serverSetCustomerId;
            }

            if (TryGetSdkConfigStringMap(sdkConfig, "eventTokenMap", out Dictionary<string, string> serverEventTokenMap))
            {
                if (eventTokenMap == null) eventTokenMap = new SuperfineDictionary<string, string>();

                foreach (var pair in serverEventTokenMap)
                {
                    eventTokenMap[pair.Key] = pair.Value;
                }
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendAdRevenue", out bool serverSendAdRevenue))
            {
                sendAdRevenue = serverSendAdRevenue;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setDeepLink", out bool serverSetDeepLink))
            {
                setDeepLink = serverSetDeepLink;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setPushToken", out bool serverSetPushToken))
            {
                setPushToken = serverSetPushToken;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendAttribution", out bool serverSendAttribution))
            {
                sendAttribution = serverSendAttribution;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendDeepLink", out bool serverSendDeepLink))
            {
                sendDeepLink = serverSendDeepLink;
            }

            if (TryGetSdkConfigObject(sdkConfig, "appSecret", out SimpleJSON.JSONObject serverAppSecret))
            {
                if (!hasAppSecret) hasAppSecret = true;

                if (TryGetSdkConfigLong(serverAppSecret, "secretId", out long serverSecretId))
                {
                    secretId = serverSecretId;
                }

                if (TryGetSdkConfigLong(serverAppSecret, "info1", out long serverSecretInfo1))
                {
                    secretInfo1 = serverSecretInfo1;
                }

                if (TryGetSdkConfigLong(serverAppSecret, "info2", out long serverSecretInfo2))
                {
                    secretInfo2 = serverSecretInfo2;
                }

                if (TryGetSdkConfigLong(serverAppSecret, "info3", out long serverSecretInfo3))
                {
                    secretInfo3 = serverSecretInfo3;
                }

                if (TryGetSdkConfigLong(serverAppSecret, "info4", out long serverSecretInfo4))
                {
                    secretInfo4 = serverSecretInfo4;
                }
            }

            if (TryGetSdkConfigString(sdkConfig, "logLevel", out string serverLogLevel))
            {
                serverLogLevel = serverLogLevel.ToLower();
                if (serverLogLevel == "verbose") logLevel.SetValue(AdjustLogLevel.Verbose);
                else if (serverLogLevel == "debug") logLevel.SetValue(AdjustLogLevel.Debug);
                else if (serverLogLevel == "info") logLevel.SetValue(AdjustLogLevel.Info);
                else if (serverLogLevel == "warn") logLevel.SetValue(AdjustLogLevel.Warn);
                else if (serverLogLevel == "error") logLevel.SetValue(AdjustLogLevel.Error);
                else if (serverLogLevel == "assert") logLevel.SetValue(AdjustLogLevel.Assert);
                else if (serverLogLevel == "suppress") logLevel.SetValue(AdjustLogLevel.Suppress);
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendInBackground", out bool serverSendInBackground))
            {
                sendInBackground.SetValue(serverSendInBackground);
            }

            if (TryGetSdkConfigBool(sdkConfig, "eventBuffering", out bool serverEventBuffering))
            {
                eventBuffering.SetValue(serverEventBuffering);
            }

            if (TryGetSdkConfigBool(sdkConfig, "coppaCompliant", out bool serverCoppaCompliant))
            {
                coppaCompliant.SetValue(serverCoppaCompliant);
            }

            if (TryGetSdkConfigBool(sdkConfig, "playStoreKidsApp", out bool serverPlayStoreKidsApp))
            {
                playStoreKidsApp.SetValue(serverPlayStoreKidsApp);
            }

            if (TryGetSdkConfigBool(sdkConfig, "launchDeferredDeeplink", out bool serverLaunchDeferredDeeplink))
            {
                launchDeferredDeeplink = serverLaunchDeferredDeeplink;
            }

            if (TryGetSdkConfigDouble(sdkConfig, "delayStart", out double serverDelayStart))
            {
                delayStart.SetValue(serverDelayStart);
            }

            if (TryGetSdkConfigBool(sdkConfig, "needsCost", out bool serverNeedsCost))
            {
                needsCost.SetValue(serverNeedsCost);
            }

            if (TryGetSdkConfigString(sdkConfig, "urlStrategy", out string serverUrlStrategy))
            {
                serverUrlStrategy = serverUrlStrategy.ToLower();
                if (serverUrlStrategy == "url_strategy_india") urlStrategy = AdjustUrlStrategy.India;
                else if (serverUrlStrategy == "url_strategy_china") urlStrategy = AdjustUrlStrategy.China;
                else if (serverUrlStrategy == "data_residency_eu") urlStrategy = AdjustUrlStrategy.DataResidencyEU;
                else if (serverUrlStrategy == "data_residency_tr") urlStrategy = AdjustUrlStrategy.DataResidencyTK;
                else if (serverUrlStrategy == "data_residency_us") urlStrategy = AdjustUrlStrategy.DataResidencyUS;
                else urlStrategy = AdjustUrlStrategy.Default;
            }

            if (TryGetSdkConfigString(sdkConfig, "defaultTracker", out string serverDefaultTracker))
            {
                defaultTracker = serverDefaultTracker;
            }

            if (TryGetSdkConfigString(sdkConfig, "processName", out string serverProcessName))
            {
                processName = serverProcessName;
            }

            if (TryGetSdkConfigBool(sdkConfig, "preinstallTracking", out bool serverPreinstallTracking))
            {
                preinstallTracking.SetValue(serverPreinstallTracking);
            }

            if (TryGetSdkConfigBool(sdkConfig, "finalAttribution", out bool serverFinalAttribution))
            {
                finalAttribution.SetValue(serverFinalAttribution);
            }

            if (TryGetSdkConfigBool(sdkConfig, "readDeviceInfoOnce", out bool serverReadDeviceInfoOnce))
            {
                readDeviceInfoOnce.SetValue(serverReadDeviceInfoOnce);
            }

            if (TryGetSdkConfigBool(sdkConfig, "linkMe", out bool serverLinkMe))
            {
                linkMe.SetValue(serverLinkMe);
            }

            if (TryGetSdkConfigBool(sdkConfig, "allowAdServicesInfoReading", out bool serverAllowAdServicesInfoReading))
            {
                allowAdServicesInfoReading.SetValue(serverAllowAdServicesInfoReading);
            }

            if (TryGetSdkConfigBool(sdkConfig, "allowIdfaReading", out bool serverAllowIdfaReading))
            {
                allowIdfaReading.SetValue(serverAllowIdfaReading);
            }

            if (TryGetSdkConfigBool(sdkConfig, "skanHandling", out bool serverSkanHandling))
            {
                skanHandling = serverSkanHandling;
            }

            if (TryGetSdkConfigInt(sdkConfig, "attTimeout", out int serverAttTimeout))
            {
                attTimeout.SetValue(serverAttTimeout);
            }
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKAdjustModule(this);
        }
    }
}
