using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineSingularSettings", menuName = "Superfine/SuperfineSingularSettings", order = 1)]
    public class SuperfineSDKSingularSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/Singular/SuperfineSingularSettings.asset";

        private static SuperfineSDKSingularSettings instance = null;

        public static SuperfineSDKSingularSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKSingularSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK Singular settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKSingularSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKSingularSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK Singular settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif
        public enum LogLevel
        {
            VERBOSE = 2,
            DEBUG = 3,
            INFO = 4,
            WARN = 5,
            ERROR = 6,
            ASSERT = 7
        }

        [Space(10)]
        [Label("API Key")]
        public string sdkKey = "SINGULAR SDK KEY";
        [Label("API Secret")]
        public string sdkSecret = "SINGULAR SDK SECRET";

        [Space(10)]
        public bool autoStart = true;
        public bool setCustomerId = true;

        [Space(10)]
        public bool sendLog = false;
        public string[] logFilters;
        public string[] logNotFilters;

        [Space(10)]
        public bool sendIAP = false;
        public bool sendAdRevenue = false;

        [Space(10)]
        public bool sendAttribution = true;

        [Space(10)]
        public bool setDeepLink = true;
        public bool setPushToken = false;

        [Space(10)]
        public SuperfineNullable<bool> limitDataSharing = new SuperfineNullable<bool>(false, false);

        [Space(10)]
        [Label("Session Timeout (s)")]
        public SuperfineNullable<long> sessionTimeout = new SuperfineNullable<long>(false, 0);
        [Label("Short Link Resolve Timeout (s)")]
        public SuperfineNullable<long> shortLinkTimeout = new SuperfineNullable<long>(false, 0);
        [Label("Deferred Deep Link Timeout (s)")]
        public SuperfineNullable<long> deferredDeepLinkTimeout = new SuperfineNullable<long>(false, 0);

        [Foldout("Android Settings")]
        [Space(10)]
        public bool enableLogging = true;
        [Foldout("Android Settings")]
        public SuperfineNullable<LogLevel> logLevel = new SuperfineNullable<LogLevel>(false, LogLevel.DEBUG);

        [Foldout("Android Settings")]
        [Space(10)]
        public bool collectOAID = false;

        [Foldout("Android Settings")]
        public bool limitedIdentifiers = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        public bool clipboardAttribution = false;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("SKAdNetwork Enabled")]
        public bool enableSkan = true;

        [Foldout("iOS Settings")]
        [Label("Manual SKAN Conversion Management")]
        public bool manualSkanConversion = true;

        [Foldout("iOS Settings")]
        [Space(10)]
        [Label("ATT Waiting Interval (s)")]
        public SuperfineNullable<int> attTimeout = new SuperfineNullable<int>(false, 0);

        public override string GetModuleName()
        {
            return "Singular";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKSingularModule(this);
        }

        public override void MergeSdkConfig(SimpleJSON.JSONObject sdkConfig)
        {
            if (TryGetSdkConfigString(sdkConfig, "sdkKey", out string serverSdkKey))
            {
                sdkKey = serverSdkKey;
            }

            if (TryGetSdkConfigString(sdkConfig, "sdkSecret", out string serverSdkSecret))
            {
                sdkSecret = serverSdkSecret;
            }

            if (TryGetSdkConfigBool(sdkConfig, "autoStart", out bool serverAutoStart))
            {
                autoStart = serverAutoStart;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setCustomerId", out bool serverSetCustomerId))
            {
                setCustomerId = serverSetCustomerId;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendLog", out bool serverSendLog))
            {
                sendLog = serverSendLog;
            }

            if (TryGetSdkConfigStringList(sdkConfig, "logFilters", out List<string> serverLogFilters))
            {
                MergeStringArray(ref logFilters, serverLogFilters);
            }

            if (TryGetSdkConfigStringList(sdkConfig, "logNotFilters", out List<string> serverLogNotFilters))
            {
                MergeStringArray(ref logNotFilters, serverLogNotFilters);
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendIAP", out bool serverSendIAP))
            {
                sendIAP = serverSendIAP;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendAdRevenue", out bool serverSendAdRevenue))
            {
                sendAdRevenue = serverSendAdRevenue;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendAttribution", out bool serverSendAttribution))
            {
                sendAttribution = serverSendAttribution;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setDeepLink", out bool serverSetDeepLink))
            {
                setDeepLink = serverSetDeepLink;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setPushToken", out bool serverSetPushToken))
            {
                setPushToken = serverSetPushToken;
            }

            if (TryGetSdkConfigBool(sdkConfig, "limitDataSharing", out bool serverLimitDataSharing))
            {
                limitDataSharing.SetValue(serverLimitDataSharing);
            }

            if (TryGetSdkConfigLong(sdkConfig, "sessionTimeout", out long serverSessionTimeout))
            {
                sessionTimeout.SetValue(serverSessionTimeout);
            }

            if (TryGetSdkConfigLong(sdkConfig, "shortLinkTimeout", out long serverShortLinkTimeout))
            {
                shortLinkTimeout.SetValue(serverShortLinkTimeout);
            }

            if (TryGetSdkConfigLong(sdkConfig, "deferredDeepLinkTimeout", out long serverDeferredDeepLinkTimeout))
            {
                deferredDeepLinkTimeout.SetValue(serverDeferredDeepLinkTimeout);
            }

            if (TryGetSdkConfigBool(sdkConfig, "enableLogging", out bool serverEnableLogging))
            {
                enableLogging = serverEnableLogging;
            }

            if (TryGetSdkConfigString(sdkConfig, "logLevel", out string serverLogLevel))
            {
                serverLogLevel = serverLogLevel.ToLower();
                if (serverLogLevel == "verbose") logLevel.SetValue(LogLevel.VERBOSE);
                else if (serverLogLevel == "debug") logLevel.SetValue(LogLevel.DEBUG);
                else if (serverLogLevel == "info") logLevel.SetValue(LogLevel.INFO);
                else if (serverLogLevel == "warn") logLevel.SetValue(LogLevel.WARN);
                else if (serverLogLevel == "error") logLevel.SetValue(LogLevel.ERROR);
                else if (serverLogLevel == "assert") logLevel.SetValue(LogLevel.ASSERT);
            }

            if (TryGetSdkConfigBool(sdkConfig, "collectOAID", out bool serverCollectOAID))
            {
                collectOAID = serverCollectOAID;
            }

            if (TryGetSdkConfigBool(sdkConfig, "limitedIdentifiers", out bool serverLimitedIdentifiers))
            {
                limitedIdentifiers = serverLimitedIdentifiers;
            }

            if (TryGetSdkConfigBool(sdkConfig, "clipboardAttribution", out bool serverClipboardAttribution))
            {
                clipboardAttribution = serverClipboardAttribution;
            }

            if (TryGetSdkConfigBool(sdkConfig, "enableSkan", out bool serverEnableSkan))
            {
                enableSkan = serverEnableSkan;
            }

            if (TryGetSdkConfigBool(sdkConfig, "manualSkanConversion", out bool serverManualSkanConversion))
            {
                manualSkanConversion = serverManualSkanConversion;
            }

            if (TryGetSdkConfigInt(sdkConfig, "attTimeout", out int serverAttTimeout))
            {
                attTimeout.SetValue(serverAttTimeout);
            }
        }
    }
}
