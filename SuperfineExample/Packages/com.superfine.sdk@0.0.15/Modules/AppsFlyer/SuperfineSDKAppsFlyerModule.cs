using AppsFlyerSDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKAppsFlyerModule : SuperfineSDKModule
    {
        private const string APPSFLYER_CONVERSION = "appsflyer_conversion";
        private const string APPSFLYER_DEEPLINK = "appsflyer_deeplink";

        private const string IAP_SUCCESS_PAYMENT = "gjsdk_iap_success_payment";
        private const string CRYPTO_PAYMENT = "gjsdk_crypto_payment";
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private const string LOCATION = "gjsdk_location";

        private const string REVENUE_DATA_FIELD_NAME = "__revenue__";

        private bool autoStart = true;
        private bool setCustomerId = true;

        private bool sendLog = false;

        private string[] logFilters = null;
        private string[] logNotFilters = null;

        private bool sendLocation = false;

        private bool setDeepLink = true;
        private bool setPushToken = false;

        private SuperfineSDKAppsFlyerMonoBehaviour monoBehaviour = null;
        private class SuperfineSDKAppsFlyerMonoBehaviour : MonoBehaviour, IAppsFlyerConversionData
        {
            private SuperfineSDKAppsFlyerModule module = null;

            public void SetModule(SuperfineSDKAppsFlyerModule module)
            {
                this.module = module;
            }

            public void onConversionDataFail(string error)
            {
                if (module != null)
                {
                    module.OnConversionDataFail(error);
                }
            }

            public void onConversionDataSuccess(string conversionData)
            {
                if (module != null)
                {
                    module.OnConversionDataSuccess(conversionData);
                }
            }

            public void onAppOpenAttribution(string attributionData)
            {
                //NOT USED
            }

            public void onAppOpenAttributionFailure(string error)
            {
                //NOT USED
            }
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

        private static SimpleJSON.JSONObject ParseJSONObject(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            if (!SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node)) return null;
            if (!node.IsObject) return null;

            return (SimpleJSON.JSONObject)node;
        }

        private static Dictionary<string, string> ConvertStringToMap(string data)
        {
            SimpleJSON.JSONObject jsonObject = ParseJSONObject(data);
            if (jsonObject == null) return null;

            return ConvertObjectToMap(jsonObject);
        }

        private static string GetString(SimpleJSON.JSONNode node)
        {
            if (node == null) return null;

            if (node.IsString)
            {
                return node.Value;
            }
            else
            {
                return node.ToString();
            }
        }

        private static Dictionary<string, string> ConvertObjectToMap(SimpleJSON.JSONObject jsonObject)
        {
            if (jsonObject == null) return null;

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (KeyValuePair<string, SimpleJSON.JSONNode> pair in jsonObject)
            {
                string key = pair.Key;
                if (string.IsNullOrEmpty(key)) continue;

                SimpleJSON.JSONNode node = pair.Value;
                if (node == null) continue;

                string value = GetString(node);
                if (string.IsNullOrEmpty(value)) continue;

                map.Add(key, value);
            }

            if (map.Count == 0) return null;

            return map;
        }

        public SuperfineSDKAppsFlyerModule(SuperfineSDKModuleSettings settings) : base(settings)
        {
        }

        protected override void Initialize(SuperfineSDKModuleSettings baseSettings)
        {
            base.Initialize(baseSettings);

#if UNITY_STANDALONE_OSX
            return;
#else
            SuperfineSDKAppsFlyerSettings settings = baseSettings as SuperfineSDKAppsFlyerSettings;

            string devKey = settings ? settings.devKey : string.Empty;

            string appId = SuperfineSDK.GetAppleAppId();

            sendLog = settings ? settings.sendLog : false;

            logFilters = settings ? settings.logFilters : null;
            if (logFilters != null && logFilters.Length == 0) logFilters = null;

            logNotFilters = settings ? settings.logNotFilters : null;
            if (logNotFilters != null && logNotFilters.Length == 0) logNotFilters = null;

            sendLocation = settings ? settings.sendLocation : false;

            bool sendAttribution = settings ? settings.sendAttribution : true;
            bool sendDeepLink = settings ? settings.sendDeepLink : true;

            setDeepLink = settings ? settings.setDeepLink : true;
            setPushToken = settings ? settings.setPushToken : false;

            bool debug = settings ? settings.debug : false;

            autoStart = settings ? settings.autoStart : true;
            setCustomerId = settings ? settings.setCustomerId : true;

            SuperfineSDK.AddStartCallback(OnStart);

            monoBehaviour = CreateMonoBehaviour<SuperfineSDKAppsFlyerMonoBehaviour>();
            monoBehaviour.SetModule(this);

            monoBehaviour.gameObject.AddComponent<AppsFlyer>();

            if (sendLog)
            {
                SuperfineSDK.AddSendEventCallback(OnSendEvent);
            }

            if (setDeepLink)
            {
                SuperfineSDK.AddDeepLinkCallback(OnSetDeepLink);
            }

            if (setPushToken)
            {
                string token = SuperfineSDK.GetPushToken();
                if (!string.IsNullOrEmpty(token))
                {
                    SetPushToken(token);
                }

                SuperfineSDK.AddPushTokenCallback(OnSetPushToken);
            }

            if (autoStart)
            {
                AppsFlyer.setIsDebug(debug);

                AppsFlyer.initSDK(devKey, appId, sendAttribution ? monoBehaviour : null);

                string[] oneLinkCustomDomains = settings ? settings.oneLinkCustomDomains : null;
                if (oneLinkCustomDomains != null && oneLinkCustomDomains.Length > 0)
                {
                    AppsFlyer.setOneLinkCustomDomain(oneLinkCustomDomains);
                }

                string[] resolveDeepLinkUrls = settings ? settings.resolveDeepLinkUrls : null;
                if (resolveDeepLinkUrls != null && resolveDeepLinkUrls.Length > 0)
                {
                    AppsFlyer.setResolveDeepLinkURLs(resolveDeepLinkUrls);
                }

                int? minTimeBetweenSessions = settings ? settings.minTimeBetweenSessions : null;
                if (minTimeBetweenSessions != null)
                {
                    AppsFlyer.setMinTimeBetweenSessions(minTimeBetweenSessions.Value);
                }

                long? deepLinkTimeout = settings ? settings.deepLinkTimeout : null;
                if (deepLinkTimeout != null)
                {
                    AppsFlyer.setDeepLinkTimeout(deepLinkTimeout.Value);
                }

                bool collectIMEI = settings ? settings.collectIMEI : true;
                AppsFlyer.setCollectIMEI(collectIMEI);

                bool collectOAID = settings ? settings.collectOAID : false;
                AppsFlyer.setCollectOaid(collectOAID);

                bool collectAndroidId = settings ? settings.collectAndroidId : true;
                AppsFlyer.setCollectAndroidID(collectAndroidId);

                bool disableAdvertisingIdentifiers = settings ? settings.disableAdvertisingIdentifiers : false;
                AppsFlyer.setDisableAdvertisingIdentifiers(disableAdvertisingIdentifiers);

                bool disableNetworkData = settings ? settings.disableNetworkData : false;
                AppsFlyer.setDisableNetworkData(disableNetworkData);

#if UNITY_ANDROID
                bool enableFacebookDeferredApplinks = settings ? settings.enableFacebookDeferredApplinks : false;
                if (AppsFlyer.instance != null && AppsFlyer.instance is AppsFlyerAndroid)
                {
                    AppsFlyerAndroid appsFlyerAndroidInstance = (AppsFlyerAndroid)AppsFlyer.instance;

                    //Need to have facebook_app_id and facebook_client_token in strings.xml 
                    appsFlyerAndroidInstance.enableFacebookDeferredApplinks(enableFacebookDeferredApplinks);
                }
#endif

                string outOfStore = settings ? settings.outOfStore : string.Empty;
                if (!string.IsNullOrEmpty(outOfStore))
                {
                    AppsFlyer.setOutOfStore(outOfStore);
                }

                bool collectDeviceName = settings ? settings.collectDeviceName : true;
                AppsFlyer.setShouldCollectDeviceName(collectDeviceName);

                bool disableSkan = settings ? settings.disableSkan : false;
                AppsFlyer.disableSKAdNetwork(disableSkan);

                bool disableIDFVCollection = settings ? settings.disableIDFVCollection : false;
                AppsFlyer.disableIDFVCollection(disableIDFVCollection);

                bool disableCollectAppleAdSupport = settings ? settings.disableCollectAppleAdSupport : false;
                AppsFlyer.setDisableCollectAppleAdSupport(disableCollectAppleAdSupport);

                bool disableCollectIAd = settings ? settings.disableCollectIAd : false;
                AppsFlyer.setDisableCollectIAd(disableCollectIAd);

                int? attTimeout = settings ? settings.attTimeout : null;
                if (attTimeout != null)
                {
                    AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(attTimeout.Value);
                }

                bool? enableTCFDataCollection = settings ? settings.enableTCFDataCollection : null;
                if (enableTCFDataCollection != null)
                {
                    AppsFlyer.enableTCFDataCollection(enableTCFDataCollection.Value);
                }

                bool setConsentData = settings ? settings.setConsentData : false;
                if (setConsentData)
                {
                    bool consentGDPRUser = settings ? settings.consentGDPRUser : false;

                    if (consentGDPRUser)
                    {
                        bool consentDataUsage = settings ? settings.consentDataUsage : false;
                        bool consentAdsPersonalization = settings ? settings.consentAdsPersonalization : false;

                        AppsFlyer.setConsentData(AppsFlyerConsent.ForGDPRUser(consentDataUsage, consentAdsPersonalization));
                    }
                    else
                    {
                        AppsFlyer.setConsentData(AppsFlyerConsent.ForNonGDPRUser());
                    }
                }
            }
            else
            {
                if (sendAttribution)
                {
                    AppsFlyer.getConversionData(monoBehaviour.name);
                }
            }

            if (sendDeepLink)
            {
                AppsFlyer.OnDeepLinkReceived += OnDeepLink;

            }

            if (setCustomerId)
            {
                AppsFlyer.setCustomerUserId(SuperfineSDK.GetUserId());
            }
#endif
        }

        public override void Destroy()
        {
            base.Destroy();

#if UNITY_STANDALONE_OSX
            return;
#else
            SuperfineSDK.RemoveStartCallback(OnStart);

            if (sendLog)
            {
                SuperfineSDK.RemoveSendEventCallback(OnSendEvent);
            }

            if (setDeepLink)
            {
                SuperfineSDK.RemoveDeepLinkCallback(OnSetDeepLink);
            }

            if (setPushToken)
            {
                SuperfineSDK.RemovePushTokenCallback(OnSetPushToken);
            }

            if (monoBehaviour != null)
            {
                GameObject.Destroy(monoBehaviour.gameObject);
                monoBehaviour = null;
            }
#endif
        }

        private void OnStart()
        {
            if (autoStart)
            {
                try
                {
                    AppsFlyer.startSDK();
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnSetDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            AppsFlyer.handleOpenUrl(url, string.Empty, string.Empty);
        }

        private void OnSetPushToken(string token)
        {
            SetPushToken(token);
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                return null;
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        private void SetPushToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

#if UNITY_ANDROID
            AppsFlyer.updateServerUninstallToken(token);
#elif UNITY_IOS
            byte[] data = ConvertHexStringToByteArray(token);
            if (data != null)
            {
                AppsFlyer.registerUninstall(data);
            }
#endif
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

        private void SendLocation(string eventData)
        {
            if (string.IsNullOrEmpty(eventData))
            {
                return;
            }

            if (!SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node)) return;
            if (!node.IsObject) return;

            SimpleJSON.JSONObject locationData = (SimpleJSON.JSONObject)node;

            if (!locationData.TryGetValue("latitude", out node)) return;
            if (!node.IsNumber) return;
            double latitude = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!locationData.TryGetValue("longitude", out node)) return;
            if (!node.IsNumber) return;
            double longitude = ((SimpleJSON.JSONNumber)node).AsDouble;

            AppsFlyer.recordLocation(latitude, longitude);
        }

        private class PurchaseData
        {
            public double revenue;
            public string currency;

            public string productId;
            public double? price;
            public int? amount;

            public PurchaseData(double revenue, string currency)
            {
                this.revenue = revenue;
                this.currency = currency;

                productId = null;
                price = null;
                amount = null;
            }
        }

        private PurchaseData ExtractPurchaseData(string eventName, SimpleJSON.JSONObject data)
        {
            if (data == null) return null;

            switch (eventName)
            {
                case IAP_SUCCESS_PAYMENT:
                case CRYPTO_PAYMENT:
                    {
                        SimpleJSON.JSONNode node;

                        if (!data.TryGetValue("price", out node) || !node.IsNumber) return null;
                        double price = ((SimpleJSON.JSONNumber)node).AsDouble;

                        if (!data.TryGetValue("amount", out node) || !node.IsNumber) return null;
                        int amount = ((SimpleJSON.JSONNumber)node).AsInt;

                        if (!data.TryGetValue("currency", out node) || !node.IsString) return null;
                        string currency = node.Value;

                        string producId = null;
                        if (data.TryGetValue("pack", out node) && node.IsString)
                        {
                            producId = node.Value;
                        }

                        PurchaseData purchaseData = new PurchaseData(price * amount, currency);
                        purchaseData.productId = producId;
                        purchaseData.price = price;
                        purchaseData.amount = amount;

                        return purchaseData;
                    }

                default:
                    {
                        SimpleJSON.JSONNode node;

                        if (!data.TryGetValue(REVENUE_DATA_FIELD_NAME, out node) || !node.IsObject) return null;
                        SimpleJSON.JSONObject revenueData = (SimpleJSON.JSONObject)node;

                        if (!revenueData.TryGetValue("revenue", out node) || !node.IsNumber) return null;
                        double revenue = ((SimpleJSON.JSONNumber)node).AsDouble;

                        if (!revenueData.TryGetValue("currency", out node) || !node.IsString) return null;
                        string currency = node.Value;

                        data.Remove(REVENUE_DATA_FIELD_NAME);

                        return new PurchaseData(revenue, currency);
                    }
            }
        }

        private void OnSendEvent(string eventName, string eventData)
        {
            if (eventName == APPSFLYER_CONVERSION || eventName == APPSFLYER_DEEPLINK) //prevent sending duplicated event
            {
                return;
            }

            if (eventName == AD_REVENUE) //this event is tracked with SuperfineAppsFlyerAdRevenueModule
            {
                return;
            }

            if (eventName == LOCATION)
            {
                if (sendLocation)
                {
                    SendLocation(eventData);
                }

                return;
            }

            if (!IsValidEventName(eventName))
            {
                return;
            }

            SimpleJSON.JSONObject jsonObject = ParseJSONObject(eventData);

            PurchaseData purchaseData = ExtractPurchaseData(eventName, jsonObject);

            if (purchaseData != null)
            {
                if (eventName.Equals(IAP_SUCCESS_PAYMENT))
                {
                    eventName = AFInAppEvents.PURCHASE;
                }

                jsonObject.Add(AFInAppEvents.CURRENCY, purchaseData.currency);
                jsonObject.Add(AFInAppEvents.REVENUE, purchaseData.revenue);

                string productId = purchaseData.productId;
                if (!string.IsNullOrEmpty(productId))
                {
                    jsonObject.Add(AFInAppEvents.CONTENT_ID, productId);
                }

                double? price = purchaseData.price;
                if (price != null)
                {
                    jsonObject.Add(AFInAppEvents.PRICE, price.Value);
                }

                int? amount = purchaseData.amount;
                if (amount != null)
                {
                    jsonObject.Add(AFInAppEvents.QUANTITY, amount.Value);
                }
            }

            AppsFlyer.sendEvent(eventName, ConvertObjectToMap(jsonObject));
        }

        public void OnDeepLink(object sender, EventArgs args)
        {
            SimpleJSON.JSONObject deepLinkResultData = new SimpleJSON.JSONObject();

            var deepLinkEventArgs = args as DeepLinkEventsArgs;

            PutString(deepLinkResultData, "status", deepLinkEventArgs.status.ToString());

            if (deepLinkEventArgs.status == DeepLinkStatus.ERROR)
            {
                PutString(deepLinkResultData, "error", deepLinkEventArgs.error.ToString());
            }

            Dictionary<string, object> deepLink = deepLinkEventArgs.deepLink;
            if (deepLink != null)
            {
                SimpleJSON.JSONObject deepLinkData = new SimpleJSON.JSONObject();

                Dictionary<string, object> clickEvent = null;
#if UNITY_IOS && !UNITY_EDITOR
                if (deepLink.ContainsKey("click_event") && deepLinkEventArgs.deepLink["click_event"] != null)
                {
                    clickEvent = deepLinkEventArgs.deepLink["click_event"] as Dictionary<string, object>;
                }
#elif UNITY_ANDROID && !UNITY_EDITOR
                clickEvent = deepLinkEventArgs.deepLink;
#endif
                if (clickEvent != null)
                {
                    SimpleJSON.JSONObject clickEventData = new SimpleJSON.JSONObject();
                    foreach (KeyValuePair<string, object> pair in clickEvent)
                    {
                        if (SimpleJSON.JSON.TryParse(pair.Value.ToString(), out SimpleJSON.JSONNode node))
                        {
                            clickEventData.Add(pair.Key, node);
                        }
                    }

                    PutString(deepLinkData, "clickEvent", clickEventData);
                }

                PutString(deepLinkData, "deeplinkValue", deepLinkEventArgs.getDeepLinkValue());
                PutString(deepLinkData, "matchType", deepLinkEventArgs.getMatchType());
                PutString(deepLinkData, "clickHTTPReferrer", deepLinkEventArgs.getClickHttpReferrer());
                PutString(deepLinkData, "mediaSource", deepLinkEventArgs.getMediaSource());

                PutString(deepLinkData, "campaign", deepLinkEventArgs.getCampaign());
                PutString(deepLinkData, "campaignId", deepLinkEventArgs.getCampaignId());

                PutString(deepLinkData, "afSub1", deepLinkEventArgs.getAfSub1());
                PutString(deepLinkData, "afSub2", deepLinkEventArgs.getAfSub2());
                PutString(deepLinkData, "afSub3", deepLinkEventArgs.getAfSub3());
                PutString(deepLinkData, "afSub4", deepLinkEventArgs.getAfSub4());
                PutString(deepLinkData, "afSub5", deepLinkEventArgs.getAfSub5());

                if (deepLink != null && deepLink.ContainsKey("is_deferred"))
                {
                    PutBoolean(deepLinkData, "isDeferred", deepLinkEventArgs.isDeferred());
                }

                PutJSONObject(deepLinkResultData, "deepLink", deepLinkData);
            }

            SuperfineSDK.Log(APPSFLYER_DEEPLINK, deepLinkResultData, EventFlag.WAIT_OPEN_EVENT);
        }

        public void OnConversionDataFail(string error)
        {
            SimpleJSON.JSONObject conversionData = new SimpleJSON.JSONObject();
            PutString(conversionData, "error", error);

            SuperfineSDK.Log(APPSFLYER_CONVERSION, conversionData, EventFlag.WAIT_OPEN_EVENT);
        }

        public void OnConversionDataSuccess(string data)
        {
            if (SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node) && node.IsObject)
            {
                SuperfineSDK.Log(APPSFLYER_CONVERSION, (SimpleJSON.JSONObject)node, EventFlag.WAIT_OPEN_EVENT);
            }
            else
            {
                SuperfineSDK.Log(APPSFLYER_CONVERSION, (SimpleJSON.JSONObject)null, EventFlag.WAIT_OPEN_EVENT);
            }
        }
    }
}
