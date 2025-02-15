using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

        [Label("Deep Link Timeout (ms)")]
        public SuperfineNullable<long> deepLinkTimeout = new SuperfineNullable<long>(false, 3000L);

        [Space(5)]
        public SuperfineNullable<bool> enableTCFDataCollection = new SuperfineNullable<bool>(false, false);

        [BoxGroup("#Consent Data")]
        [Space(5)]
        [Label("Consent Data", true)]
        public bool setConsentData = false;

        [BoxGroup("#Consent Data", 1)]
        [ShowIf("setConsentData")]
        [Label("GDPR User")]
        public bool consentGDPRUser = false;

        [BoxGroup("#Consent Data", 2)]
        [ShowIf("consentGDPRUser")]
        [Label("Consent For Data Usage")]
        public bool consentDataUsage = false;

        [BoxGroup("#Consent Data", 2)]
        [ShowIf("consentGDPRUser")]
        [Label("Consent For Ads Personalization")]
        public bool consentAdsPersonalization = false;

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
            return "AppsFlyer";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKAppsFlyerModule(this);
        }

        public override void MergeSdkConfig(SimpleJSON.JSONObject sdkConfig)
        {
            if (TryGetSdkConfigString(sdkConfig, "devKey", out string serverDevKey))
            {
                devKey = serverDevKey;
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

            if (TryGetSdkConfigBool(sdkConfig, "sendLocation", out bool serverSendLocation))
            {
                sendLocation = serverSendLocation;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendAttribution", out bool serverSendAttribution))
            {
                sendAttribution = serverSendAttribution;
            }

            if (TryGetSdkConfigBool(sdkConfig, "sendDeepLink", out bool serverSendDeepLink))
            {
                sendDeepLink = serverSendDeepLink;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setPushToken", out bool serverSetPushToken))
            {
                setPushToken = serverSetPushToken;
            }

            if (TryGetSdkConfigBool(sdkConfig, "debug", out bool serverDebug))
            {
                debug = serverDebug;
            }

            if (TryGetSdkConfigStringList(sdkConfig, "oneLinkCustomDomains", out List<string> serverOneLinkCustomDomains))
            {
                MergeStringArray(ref oneLinkCustomDomains, serverOneLinkCustomDomains);
            }

            if (TryGetSdkConfigStringList(sdkConfig, "resolveDeepLinkUrls", out List<string> serverResolveDeepLinkUrls))
            {
                MergeStringArray(ref resolveDeepLinkUrls, serverResolveDeepLinkUrls);
            }

            if (TryGetSdkConfigInt(sdkConfig, "minTimeBetweenSessions", out int serverMinTimeBetweenSessions))
            {
                minTimeBetweenSessions.SetValue(serverMinTimeBetweenSessions);
            }

            if (TryGetSdkConfigLong(sdkConfig, "deepLinkTimeout", out long serverDeepLinkTimeout))
            {
                deepLinkTimeout.SetValue(serverDeepLinkTimeout);
            }

            if (TryGetSdkConfigBool(sdkConfig, "enableTCFDataCollection", out bool serverEnableTCFDataCollection))
            {
                enableTCFDataCollection.SetValue(serverEnableTCFDataCollection);
            }

            if (TryGetSdkConfigObject(sdkConfig, "consentData", out SimpleJSON.JSONObject consentDataObject))
            {
                setConsentData = true;

                if (TryGetSdkConfigBool(consentDataObject, "consentGDPRUser", out bool serverConsentGDPRUser))
                {
                    consentGDPRUser = serverConsentGDPRUser;
                }

                if (TryGetSdkConfigBool(consentDataObject, "consentDataUsage", out bool serverConsentDataUsage))
                {
                    consentDataUsage = serverConsentDataUsage;
                }

                if (TryGetSdkConfigBool(consentDataObject, "consentAdsPersonalization", out bool serverConsentAdsPersonalization))
                {
                    consentAdsPersonalization = serverConsentAdsPersonalization;
                }
            }

            if (TryGetSdkConfigBool(sdkConfig, "collectIMEI", out bool serverCollectIMEI))
            {
                collectIMEI = serverCollectIMEI;
            }

            if (TryGetSdkConfigBool(sdkConfig, "collectOAID", out bool serverCollectOAID))
            {
                collectOAID = serverCollectOAID;
            }

            if (TryGetSdkConfigBool(sdkConfig, "collectAndroidId", out bool serverCollectAndroidId))
            {
                collectAndroidId = serverCollectAndroidId;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableAdvertisingIdentifiers", out bool serverDisableAdvertisingIdentifiers))
            {
                disableAdvertisingIdentifiers = serverDisableAdvertisingIdentifiers;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableNetworkData", out bool serverDisableNetworkData))
            {
                disableNetworkData = serverDisableNetworkData;
            }

            if (TryGetSdkConfigBool(sdkConfig, "enableFacebookDeferredApplinks", out bool serverEnableFacebookDeferredApplinks))
            {
                enableFacebookDeferredApplinks = serverEnableFacebookDeferredApplinks;
            }

            if (TryGetSdkConfigString(sdkConfig, "outOfStore", out string serverOutOfStore))
            {
                outOfStore = serverOutOfStore;
            }

            if (TryGetSdkConfigBool(sdkConfig, "setDeepLink", out bool serverSetDeepLink))
            {
                setDeepLink = serverSetDeepLink;
            }

            if (TryGetSdkConfigBool(sdkConfig, "collectDeviceName", out bool serverCollectDeviceName))
            {
                collectDeviceName = serverCollectDeviceName;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableSkan", out bool serverDisableSkan))
            {
                disableSkan = serverDisableSkan;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableIDFVCollection", out bool serverDisableIDFVCollection))
            {
                disableIDFVCollection = serverDisableIDFVCollection;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableCollectAppleAdSupport", out bool serverDisableCollectAppleAdSupport))
            {
                disableCollectAppleAdSupport = serverDisableCollectAppleAdSupport;
            }

            if (TryGetSdkConfigBool(sdkConfig, "disableCollectIAd", out bool serverDisableCollectIAd))
            {
                disableCollectIAd = serverDisableCollectIAd;
            }

            if (TryGetSdkConfigInt(sdkConfig, "attTimeout", out int serverAttTimeout))
            {
                attTimeout.SetValue(serverAttTimeout);
            }
        }
    }
}
