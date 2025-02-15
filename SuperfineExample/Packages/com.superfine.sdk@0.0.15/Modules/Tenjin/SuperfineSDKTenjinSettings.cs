using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineTenjinSettings", menuName = "Superfine/SuperfineTenjinSettings", order = 1)]
    public class SuperfineSDKTenjinSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/Tenjin/SuperfineTenjinSettings.asset";

        private static SuperfineSDKTenjinSettings instance = null;

        public static SuperfineSDKTenjinSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKTenjinSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK Tenjin settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKTenjinSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKTenjinSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK Tenjin settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif

        [Space(10)]

        public string sdkKey = "TENJIN SDK KEY";

        [Space(10)]

        public bool autoStart = true;
        public bool setCustomerId = true;

        [Space(10)]
        public bool sendLog = false;
        public string[] logFilters;
        public string[] logNotFilters;

        [Space(10)]
        public bool sendIAP = false;
        [HideInInspector]
        public bool sendAdRevenue = false;

        [Space(10)]
        public bool sendAttribution = true;
        public bool sendDeepLink = true;

        [Space(10)]
        public int subversion = 0;
        public bool cacheEvent = false;

        [Space(10)]
        [Label("Opt In")]
        public SuperfineNullable<bool> optIn = new SuperfineNullable<bool>(false, true);
        public string[] optInParams;
        public string[] optOutParams;

        [Space(10)]
        [InfoBox("iOS only", EInfoBoxType.Normal)]
        public bool debug = false;

        public override string GetModuleName()
        {
            return "Tenjin";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKTenjinModule(this);
        }

        public override void MergeSdkConfig(SimpleJSON.JSONObject sdkConfig)
        {
            if (TryGetSdkConfigString(sdkConfig, "sdkKey", out string serverSdkKey))
            {
                sdkKey = serverSdkKey;
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

            if (TryGetSdkConfigBool(sdkConfig, "sendDeepLink", out bool serverSendDeepLink))
            {
                sendDeepLink = serverSendDeepLink;
            }

            if (TryGetSdkConfigInt(sdkConfig, "subversion", out int serverSubversion))
            {
                subversion = serverSubversion;
            }

            if (TryGetSdkConfigBool(sdkConfig, "cacheEvent", out bool serverCacheEvent))
            {
                cacheEvent = serverCacheEvent;
            }

            if (TryGetSdkConfigBool(sdkConfig, "optIn", out bool serverOptIn))
            {
                optIn.SetValue(serverOptIn);
            }

            if (TryGetSdkConfigStringList(sdkConfig, "optInParams", out List<string> serverOptInParams))
            {
                MergeStringArray(ref optInParams, serverOptInParams);
            }

            if (TryGetSdkConfigStringList(sdkConfig, "optOutParams", out List<string> serverOptOutParams))
            {
                MergeStringArray(ref optOutParams, serverOptOutParams);
            }

            if (TryGetSdkConfigBool(sdkConfig, "debug", out bool serverDebug))
            {
                debug = serverDebug;
            }
        }
    }
}
