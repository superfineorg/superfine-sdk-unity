using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKSingularModule : SuperfineSDKModule, SingularDeviceAttributionCallbackHandler, SingularLinkHandler, SingularDeferredDeepLinkHandler
    {
        private const string SINGULAR_ATTRIBUTION = "singular_attribution";

        private const string SINGULAR_SINGULARLINK = "singular_singularlink";
        private const string SINGULAR_DEEPLINK = "singular_deeplink";

        private const string IAP_SUCCESS_PAYMENT = "gjsdk_iap_success_payment";
        private const string CRYPTO_PAYMENT = "gjsdk_crypto_payment";
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private const string REVENUE_DATA_FIELD_NAME = "__revenue__";

        private bool autoStart = true;
        private bool setCustomerId = true;

        private bool sendLog = false;

        private string[] logFilters = null;
        private string[] logNotFilters = null;

        private bool sendIAP = false;
        private bool sendAdRevenue = false;

        private bool sendAttribution = true;

        private bool setDeepLink = true;
        private bool setPushToken = false;

        private SingularSDK singularSDK = null;
        private GameObject singularSDKObject = null;

        public SuperfineSDKSingularModule(SuperfineSDKModuleSettings settings) : base(settings)
        {
        }

        private static string OptString(SimpleJSON.JSONObject jsonObject, string key)
        {
            if (jsonObject == null) return null;

            if (!jsonObject.TryGetValue(key, out SimpleJSON.JSONNode value)) return null;
            if (!value.IsString) return null;

            return value.Value;
        }

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        private static void PutBoolean(SimpleJSON.JSONObject paramJSONObject, string paramString, bool paramBool)
        {
            paramJSONObject.Add(paramString, paramBool);
        }

        private static void PutJSONObject(SimpleJSON.JSONObject paramJSONObject, string paramString, SimpleJSON.JSONObject paramJSONObject2)
        {
            if (paramJSONObject2 == null) return;
            paramJSONObject.Add(paramString, paramJSONObject2);
        }

        private static ArrayList ConvertJSONArray(SimpleJSON.JSONArray jsonArray)
        {
            ArrayList ret = new ArrayList();

            int num = jsonArray.Count;
            for (int i = 0; i < num; ++i)
            {
                object value = ConvertJSONNode(jsonArray[i]);
                if (value != null) ret.Add(value);
            }

            return ret;
        }

        private static Dictionary<string, object> ConvertJSONObject(SimpleJSON.JSONObject jsonObject)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();

            foreach (var pair in jsonObject)
            {
                string key = pair.Key;
                if (string.IsNullOrEmpty(key)) continue;

                object value = ConvertJSONNode(pair.Value);

                if (value != null)
                {
                    ret.Add(key, value);
                }
            }

            return ret;
        }

        private static object ConvertJSONNode(SimpleJSON.JSONNode node)
        {
            if (node == null) return null;

            if (node.IsObject)
            {
                return ConvertJSONObject((SimpleJSON.JSONObject)node);
            }
            else if (node.IsArray)
            {
                return ConvertJSONArray((SimpleJSON.JSONArray)node);
            }
            else if (node.IsString)
            {
                return node.Value;
            }
            else if (node.IsBoolean)
            {
                return node.AsBool;
            }
            else if (node.IsNumber)
            {
                double valueDouble = ((SimpleJSON.JSONNumber)node).AsDouble;
                int valueInt = (int)Math.Round(valueDouble);

                if (Math.Abs(valueDouble - valueInt) < 1e-5)
                {
                    return valueInt;
                }

                return valueDouble;
            }
        
            return null;
        }

        protected override void Initialize(SuperfineSDKModuleSettings baseSettings)
        {
            base.Initialize(baseSettings);

            SuperfineSDKSingularSettings settings = baseSettings as SuperfineSDKSingularSettings;

            autoStart = settings ? settings.autoStart : true;
            setCustomerId = settings ? settings.setCustomerId : true;

            sendLog = settings ? settings.sendLog : false;

            logFilters = settings ? settings.logFilters : null;
            if (logFilters != null && logFilters.Length == 0) logFilters = null;

            logNotFilters = settings ? settings.logNotFilters : null;
            if (logNotFilters != null && logNotFilters.Length == 0) logNotFilters = null;

            sendIAP = settings ? settings.sendIAP : false;
            sendAdRevenue = settings ? settings.sendAdRevenue : false;

            sendAttribution = settings ? settings.sendAttribution : true;

            setDeepLink = settings ? settings.setDeepLink : true;
            setPushToken = settings ? settings.setPushToken : false;

            SuperfineSDK.AddStartCallback(OnStart);
            SuperfineSDK.AddResumeCallback(OnResume);

            if (sendLog)
            {
                SuperfineSDK.AddSendEventCallback(OnSendEvent);
            }

            singularSDK = GameObject.FindFirstObjectByType<SingularSDK>();
            if (singularSDK == null)
            {
                singularSDK = CreateMonoBehaviour<SingularSDK>(true);
                singularSDKObject = singularSDK.gameObject;
                singularSDKObject.name = "SingularSDKObject"; //To match with the game object name in bridge library

                singularSDK.InitializeOnAwake = false;
                singularSDKObject.SetActive(true);
            }

            string userId = SuperfineSDK.GetUserId();

            if (setCustomerId)
            {
                SingularSDK.SetCustomUserId(userId);
            }

            if (setPushToken)
            {
                string pushToken = SuperfineSDK.GetPushToken();
                if (!string.IsNullOrEmpty(pushToken))
                {
                    SingularSDK.RegisterTokenForUninstall(pushToken);
                }

                SuperfineSDK.AddPushTokenCallback(OnSetPushToken);
            }

            if (autoStart)
            {
                string sdkKey = settings ? settings.sdkKey : string.Empty;
                string sdkSecret = settings ? settings.sdkSecret : string.Empty;

                long? sessionTimeout = settings ? settings.sessionTimeout : null;
                long? shortLinkTimeout = settings ? settings.shortLinkTimeout : null;
                long? deferredDeepLinkTimeout = settings ? settings.deferredDeepLinkTimeout : null;

                bool enableLogging = settings ? settings.enableLogging : false;

                SuperfineSDKSingularSettings.LogLevel? logLevel = settings ? settings.logLevel : null;

                bool? limitDataSharing = settings ? settings.limitDataSharing : null;

                bool collectOAID = settings ? settings.collectOAID : false;
                bool limitedIdentifiers = settings ? settings.limitedIdentifiers : false;

                bool clipboardAttribution = settings ? settings.clipboardAttribution : false;
                bool enableSkan = settings ? settings.enableSkan : true;
                bool manualSkanConversion = settings ? settings.manualSkanConversion : true;

                int? attTimeout = settings ? settings.attTimeout : null;

                SingularSDK.enableDeferredDeepLinks = true;

                singularSDK.SingularAPIKey = sdkKey;
                singularSDK.SingularAPISecret = sdkSecret;

                singularSDK.sessionTimeoutSec = sessionTimeout ?? 0;
                singularSDK.shortlinkResolveTimeout = shortLinkTimeout ?? 0;
                singularSDK.ddlTimeoutSec = deferredDeepLinkTimeout ?? 0;

                singularSDK.enableLogging = enableLogging;

                if (logLevel != null)
                {
                    singularSDK.logLevel = (int)logLevel.Value;
                }

#if UNITY_ANDROID
                string imei = SuperfineSDK.Android.GetIMEI();
                if (!string.IsNullOrEmpty(imei))
                {
                    SingularSDK.SetIMEI(imei);
                }
                else
                {
                    SingularSDK.SetIMEI(null);
                }
#endif

                string facebookAppId = SuperfineSDK.GetFacebookAppId();
                if (!string.IsNullOrEmpty(facebookAppId))
                {
                    singularSDK.facebookAppId = facebookAppId;
                }
                else
                {
                    singularSDK.facebookAppId = null;
                }

                string deepLinkUrl = SuperfineSDK.GetDeepLinkUrl();
                if (string.IsNullOrEmpty(deepLinkUrl))
                {
                    SingularSDK.openUri = deepLinkUrl;
                }
                else
                {
                    SingularSDK.openUri = null;
                }

                if (limitDataSharing != null)
                {
                    SingularSDK.LimitDataSharing(limitDataSharing.Value);
                }

                singularSDK.collectOAID = collectOAID;
                singularSDK.limitedIdentifiersEnabled = limitedIdentifiers;

                singularSDK.clipboardAttribution = clipboardAttribution;
                singularSDK.SKANEnabled = enableSkan;
                singularSDK.manualSKANConversionManagement = manualSkanConversion;

                singularSDK.waitForTrackingAuthorizationWithTimeoutInterval = attTimeout ?? 0;

                if (sendAttribution)
                {
                    SingularSDK.SetSingularDeviceAttributionCallbackHandler(this);
                }

                if (setDeepLink)
                {
                    SingularSDK.SetSingularLinkHandler(this);
                    SingularSDK.SetDeferredDeepLinkHandler(this);

                    //SuperfineSDK.AddDeepLinkCallback(OnSetDeepLink);
                }

                SingularSDK.InitializeSingularSDK();
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            SuperfineSDK.RemoveStartCallback(OnStart);
            SuperfineSDK.RemoveResumeCallback(OnResume);

            if (sendLog)
            {
                SuperfineSDK.RemoveSendEventCallback(OnSendEvent);
			}

            if (setPushToken)
            {
                SuperfineSDK.RemovePushTokenCallback(OnSetPushToken);
            }

            if (autoStart)
            {
                if (sendAttribution)
                {
                    SingularSDK.SetSingularDeviceAttributionCallbackHandler(null);
                }

                if (setDeepLink)
                {
                    SingularSDK.SetSingularLinkHandler(null);
                    SingularSDK.SetDeferredDeepLinkHandler(null);

                    //SuperfineSDK.RemoveDeepLinkCallback(OnSetDeepLink);
                }
            }

            if (singularSDKObject != null)
            {
                GameObject.Destroy(singularSDKObject.gameObject);
                singularSDKObject = null;
            }

            singularSDK = null;
        }

        private void OnStart()
        {
            if (autoStart)
            {
                SingularSDK.SetGlobalProperty("sessionId", SuperfineSDK.GetSessionId(), true);
            }
        }

        private void OnResume()
        {
            if (autoStart)
            {
                SingularSDK.SetGlobalProperty("sessionId", SuperfineSDK.GetSessionId(), true);
            }
        }

        private void OnSetPushToken(string token)
        {
            SingularSDK.RegisterTokenForUninstall(token);
        }

        /*
        private void OnSetDeepLink(string url)
        {
            //NOTHING
        }
        */

        private SimpleJSON.JSONObject CreateSingularAttributionData(Dictionary<string, object> attributionInfo)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();

            SimpleJSON.JSONNode data = SimpleJSON.JSON.ToJSONNode(attributionInfo);
            if (data == null || !data.IsObject) return null;

            return (SimpleJSON.JSONObject)ret;
        }

        public void SendAttribution(Dictionary<string, object> attributionInfo)
        {
            if (attributionInfo == null) return;
            SuperfineSDK.Log(SINGULAR_ATTRIBUTION, CreateSingularAttributionData(attributionInfo), EventFlag.WAIT_OPEN_EVENT);
        }

        private SimpleJSON.JSONObject CreateSingularLinkData(SingularLinkParams singularLinkParams)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();

            PutString(ret, "deeplink", singularLinkParams.Deeplink);
            PutString(ret, "passthrough", singularLinkParams.Passthrough);
            PutBoolean(ret, "isDeferred", singularLinkParams.IsDeferred);

            Dictionary<string, string> urlParameters = singularLinkParams.UrlParameters;
            if (urlParameters != null && urlParameters.Count > 0)
            {
                SimpleJSON.JSONObject urlParametersObject = new SimpleJSON.JSONObject();

                foreach (var pair in urlParameters)
                {
                    string key = pair.Key;
                    string value = pair.Value;

                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        PutString(urlParametersObject, key, value);
                    }
                }

                if (urlParametersObject.Count > 0)
                {
                    PutJSONObject(ret, "urlParameters", urlParametersObject);
                }
            }

            return ret;
        }

        public void SendSingularLink(SingularLinkParams singularLinkParams)
        {
            if (singularLinkParams == null) return;
            SuperfineSDK.Log(SINGULAR_SINGULARLINK, CreateSingularLinkData(singularLinkParams), EventFlag.WAIT_OPEN_EVENT);
        }

        private SimpleJSON.JSONObject CreateDeepLinkData(string deepLink)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();
            PutString(ret, "deeplink", deepLink);

            return ret;
        }

        public void SendDeepLink(string deepLink)
        {
            if (string.IsNullOrEmpty(deepLink)) return;
            SuperfineSDK.Log(SINGULAR_DEEPLINK, CreateDeepLinkData(deepLink), EventFlag.WAIT_OPEN_EVENT);
        }

        public void OnSingularDeviceAttributionCallback(Dictionary<string, object> attributionInfo)
        {
            SendAttribution(attributionInfo);
        }

        public void OnSingularLinkResolved(SingularLinkParams linkParams)
        {
            SendSingularLink(linkParams);
        }

        public void OnDeferredDeepLink(string deepLink)
        {
            SendDeepLink(deepLink);
        }

        private void SendIAPEvent(string eventData, string eventName)
        {
            if (string.IsNullOrEmpty(eventData)) return;

            if (!SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node)) return;
            if (!node.IsObject) return;

            SimpleJSON.JSONObject iapData = (SimpleJSON.JSONObject)node;

            if (!iapData.TryGetValue("pack", out node)) return;
            if (!node.IsString) return;
            string pack = node.Value;

            if (!iapData.TryGetValue("price", out node)) return;
            if (!node.IsNumber) return;
            double price = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!iapData.TryGetValue("amount", out node)) return;
            if (!node.IsNumber) return;
            int amount = ((SimpleJSON.JSONNumber)node).AsInt;

            if (!iapData.TryGetValue("currency", out node)) return;
            if (!node.IsString) return;
            string currency = node.Value;

            if (!string.IsNullOrEmpty(eventName))
            {
                SingularSDK.CustomRevenue(eventName, currency, price * amount, pack, string.Empty, string.Empty, amount, price);
            }
            else
            {
                SingularSDK.Revenue(currency, price * amount, pack, string.Empty, string.Empty, amount, price);
            }
        }

        private void SetAdType(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdType(value);
        }

        private void SetAdGroupType(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdGroupType(value);
        }

        private void SetImpressionId(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithImpressionId(value);
        }

        private void SetAdPlacementName(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdPlacmentName(value);
        }

        private void SetAdUnitId(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdUnitId(value);
        }

        private void SetAdUnitName(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdUnitName(value);
        }

        private void SetAdGroupId(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdGroupId(value);
        }

        private void SetAdGroupName(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdGroupName(value);
        }

        private void SetAdGroupPriority(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithAdGroupPriority(value);
        }

        private void SetPrecision(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithPrecision(value);
        }

        private void SetPlacemenId(SingularAdData data, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            data.WithPlacementId(value);
        }

        private void SetupAppLovinAdData(SingularAdData data, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return;

            SetAdType(data, OptString(networkData, "adFormat"));
            SetAdPlacementName(data, OptString(networkData, "placement"));
            SetAdUnitId(data, OptString(networkData, "adUnitId"));
            SetPrecision(data, OptString(networkData, "revenuePrecision"));
            SetPlacemenId(data, OptString(networkData,"networkPlacement"));
        }

        private void SetupAdMobAdData(SingularAdData data, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return;

            SetAdPlacementName(data, OptString(networkData, "placement"));
            SetAdUnitId(data, OptString(networkData, "adUnitId"));
            SetPrecision(data, OptString(networkData, "precisionType"));
        }

        private void SetupIronSourceAdData(SingularAdData data, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return;

            SetAdType(data, OptString(networkData, "adUnit"));
            SetAdPlacementName(data, OptString(networkData, "placement"));
            SetPrecision(data, OptString(networkData, "precision"));
        }

        private void SetupAppodealAdData(SingularAdData data, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return;

            SetAdType(data, OptString(networkData, "adType"));
            SetAdPlacementName(data, OptString(networkData, "placement"));

            if (networkData.TryGetValue("placementId", out SimpleJSON.JSONNode node))
            {
                if (node.IsNumber)
                {
                    int placementId = ((SimpleJSON.JSONNumber)node).AsInt;
                    SetPlacemenId(data, placementId.ToString());
                }
            }
                        
            SetAdUnitName(data, OptString(networkData, "getAdUnitName"));
            SetPrecision(data, OptString(networkData, "revenuePrecision"));
        }

        private void SendAdRevenueEvent(string eventData)
        {
            if (string.IsNullOrEmpty(eventData)) return;

            if (!SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node)) return;
            if (!node.IsObject) return;

            SimpleJSON.JSONObject adRevenueData = (SimpleJSON.JSONObject)node;

            if (!adRevenueData.TryGetValue("source", out node)) return;
            if (!node.IsString) return;
            string source = node.Value;

            if (!adRevenueData.TryGetValue("revenue", out node)) return;
            if (!node.IsNumber) return;
            double revenue = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!adRevenueData.TryGetValue("currency", out node)) return;
            if (!node.IsString) return;
            string currency = node.Value;

            string network = null;
            if (adRevenueData.TryGetValue("network", out node))
            {
                if (node.IsString)
                {
                    network = node.Value;
                }
            }

            SimpleJSON.JSONObject networkData = null;
            if (adRevenueData.TryGetValue("network_data", out node))
            {
                if (node.IsObject)
                {
                    networkData = (SimpleJSON.JSONObject)node;
                }
            }

            SingularAdData data = new SingularAdData(source, currency, revenue);
            if (!string.IsNullOrEmpty(network))
            {
                data.WithNetworkName(network);
            }

            switch (source)
            {
                case "AppLovin":
                    SetupAppLovinAdData(data, networkData);
                    break;
                case "AdMob":
                    SetupAdMobAdData(data, networkData);
                    break;
                case "IronSource":
                    SetupIronSourceAdData(data, networkData);
                    break;
                case "Appodeal":
                    SetupAppodealAdData(data, networkData);
                    break;
            }

            SingularSDK.AdRevenue(data);
        }

        private bool IsValidEventName(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return false;
            }

            if (logFilters != null && !logFilters.Contains(eventName))
            {
                return false;
            }

            if (logNotFilters != null && logNotFilters.Contains(eventName))
            {
                return false;
            }

            return true;
        }

        private void ExtractRevenueData(SimpleJSON.JSONObject data)
        {
            if (data == null) return;

            SimpleJSON.JSONNode node;

            if (!data.TryGetValue(REVENUE_DATA_FIELD_NAME, out node) || !node.IsObject) return;
            SimpleJSON.JSONObject revenueData = (SimpleJSON.JSONObject)node;

            if (!revenueData.TryGetValue("revenue", out node) || !node.IsNumber) return;
            double revenue = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!revenueData.TryGetValue("currency", out node) || !node.IsString) return;
            string currency = node.Value;

            data.Remove(REVENUE_DATA_FIELD_NAME);

            data.Add("pcc", currency);
            data.Add("r", revenue);
            data.Add("is_revenue_event", true);
        }

        private void OnSendEvent(string eventName, string eventData)
        {
            if (eventName == SINGULAR_SINGULARLINK || eventName == SINGULAR_DEEPLINK) //prevent sending duplicated event
            {
                return;
            }

            if (eventName == IAP_SUCCESS_PAYMENT || eventName == CRYPTO_PAYMENT)
            {
                string iapEventName = eventName;
                if (eventName == IAP_SUCCESS_PAYMENT) iapEventName = null;

                if (sendIAP)
                {
                    SendIAPEvent(eventData, iapEventName);
                }

                return;
            }

            if (eventName == AD_REVENUE)
            {
                if (sendAdRevenue)
                {
                    SendAdRevenueEvent(eventData);
                }

                return;
            }

            if (!IsValidEventName(eventName))
            {
                return;
            }

            SimpleJSON.JSONObject jsonObject = null;
            if (!string.IsNullOrEmpty(eventData))
            {
                if (SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node))
                {
                    if (node.IsObject)
                    {
                        jsonObject = (SimpleJSON.JSONObject)node;
                    }
                }
            }

            if (jsonObject == null)
            {
                SingularSDK.Event(eventName);
            }
            else
            {
                ExtractRevenueData(jsonObject);
                SingularSDK.Event(ConvertJSONObject(jsonObject), eventName);
            }
        }
    }
}
