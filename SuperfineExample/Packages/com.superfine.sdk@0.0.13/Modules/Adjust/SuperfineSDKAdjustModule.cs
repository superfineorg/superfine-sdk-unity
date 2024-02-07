using com.adjust.sdk;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKAdjustModule : SuperfineSDKModule
    {
        private const string ADJUST_ATTRIBUTION = "adjust_attribution";
        private const string ADJUST_DEEPLINK = "adjust_deeplink";

        private const string IAP_SUCCESS_PAYMENT = "gjsdk_iap_success_payment";
        private const string CRYPTO_PAYMENT = "gjsdk_crypto_payment";
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private static string[] MEDIATION_NETWORK_NAMES = {
          "ironsource", "applovin", "mopub",
          "admob", "admost", "unity",
          "chartboost", "topon", "adx"
        };

        private static string[] ADJUST_MEDIATION_NETWORK_NAMES =
        {
            AdjustConfig.AdjustAdRevenueSourceIronSource, AdjustConfig.AdjustAdRevenueSourceAppLovinMAX, AdjustConfig.AdjustAdRevenueSourceMopub,
            AdjustConfig.AdjustAdRevenueSourceAdMob, AdjustConfig.AdjustAdRevenueSourceAdmost, AdjustConfig.AdjustAdRevenueSourceUnity,
            AdjustConfig.AdjustAdRevenueSourceHeliumChartboost, AdjustConfig.AdjustAdRevenueSourceTopOn, AdjustConfig.AdjustAdRevenueSourceAdx
        };

        private static string GetAdjustAdRevenueSource(string source)
        {
            int pos = Array.IndexOf(MEDIATION_NETWORK_NAMES, source.ToLower());
            if (pos < 0) return AdjustConfig.AdjustAdRevenueSourcePublisher;

            return ADJUST_MEDIATION_NETWORK_NAMES[pos];
        }

        private bool autoStart = true;

        private bool sendLog = false;
        private Dictionary<string, string> eventTokenMap = null;

        private bool sendAdRevenue = false;

        private bool setDeepLink = true;
        private bool setPushToken = false;

        private Adjust adjust = null;
        private GameObject adjustObject = null;

        public SuperfineSDKAdjustModule(SuperfineSDKModuleSettings settings) : base(settings)
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

        private static void PutDouble(SimpleJSON.JSONObject paramJSONObject, string paramString, double paramDouble)
        {
            paramJSONObject.Add(paramString, paramDouble);
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

        protected override void Initialize(SuperfineSDKModuleSettings baseSettings)
        {
            base.Initialize(baseSettings);

            SuperfineSDKAdjustSettings settings = baseSettings as SuperfineSDKAdjustSettings;

            autoStart = settings ? settings.autoStart : true;
            bool setCustomerId = settings ? settings.setCustomerId : true;

            eventTokenMap = settings ? settings.eventTokenMap.GetDictionary() : null;
            if (eventTokenMap.Count == 0) eventTokenMap = null;

            sendAdRevenue = settings ? settings.sendAdRevenue : false;

            sendLog = (eventTokenMap != null) || sendAdRevenue;

            setDeepLink = settings ? settings.setDeepLink : true;
            setPushToken = settings ? settings.setPushToken : false;

            bool sendAttribution = settings ? settings.sendAttribution : true;
            bool sendDeepLink = settings ? settings.sendDeepLink : true;

            SuperfineSDK.AddStartCallback(OnStart);
            SuperfineSDK.AddResumeCallback(OnResume);

            if (sendLog)
            {
                SuperfineSDK.AddSendEventCallback(OnSendEvent);
            }

            adjust = GameObject.FindFirstObjectByType<Adjust>();
            if (adjust == null)
            {
                adjust = CreateMonoBehaviour<Adjust>(true);
                adjustObject = adjust.gameObject;

                adjust.startManually = true;
                adjustObject.SetActive(true);
            }

            if (autoStart)
            {
                string appToken = settings ? settings.appToken : string.Empty;
                AdjustEnvironment environment = settings ? settings.environment : AdjustEnvironment.Production;

                AdjustLogLevel? logLevel = settings ? settings.logLevel : null;

                bool? sendInBackground = settings ? settings.sendInBackground : null;
                bool? eventBuffering = settings ? settings.eventBuffering : null;

                bool? coppaCompliant = settings ? settings.coppaCompliant : null;
                bool? playStoreKidsApp = settings ? settings.playStoreKidsApp : null;

                bool launchDeferredDeeplink = settings ? settings.launchDeferredDeeplink : true;

                double? delayStart = settings ? settings.delayStart : null;
                bool? needsCost = settings ? settings.needsCost : null;

                AdjustUrlStrategy urlStrategy = settings ? settings.urlStrategy : AdjustUrlStrategy.Default;

                string defaultTracker = settings ? settings.defaultTracker : string.Empty;

                string processName = settings ? settings.processName : string.Empty;
                bool? preinstallTracking = settings ? settings.preinstallTracking : null;
                string preinstallFilePath = settings ? settings.preinstallFilePath : string.Empty;
                bool? finalAttribution = settings ? settings.finalAttribution : null;
                bool? readDeviceInfoOnce = settings ? settings.readDeviceInfoOnce : null;

                bool? linkMe = settings ? settings.linkMe : null;
                bool? allowAdServicesInfoReading = settings ? settings.allowAdServicesInfoReading : null;
                bool? allowIdfaReading = settings ? settings.allowIdfaReading : null;
                bool skanHandling = settings ? settings.skanHandling : true;
                int? attTimeout = settings ? settings.attTimeout : null;

                bool hasAppSecret = settings ? settings.hasAppSecret : false;

                AdjustConfig adjustConfig = new AdjustConfig(appToken, environment, (logLevel == AdjustLogLevel.Suppress));

                SuperfineSDKManager manager = SuperfineSDK.GetInstance();
                if (manager != null)
                {
                    string facebookAppId = manager.GetFacebookAppId();
                    if (!string.IsNullOrEmpty(facebookAppId))
                    {
                        adjustConfig.setFbAppId(facebookAppId);
                    }
                }

                if (logLevel != null)
                {
                    adjustConfig.setLogLevel(logLevel.Value);
                }

                if (sendInBackground != null)
                {
                    adjustConfig.setSendInBackground(sendInBackground.Value);
                }

                if (eventBuffering != null)
                {
                    adjustConfig.setEventBufferingEnabled(eventBuffering.Value);
                }

                if (coppaCompliant != null)
                {
                    adjustConfig.setCoppaCompliantEnabled(coppaCompliant.Value);
                }

                if (playStoreKidsApp != null)
                {
                    adjustConfig.setPlayStoreKidsAppEnabled(playStoreKidsApp.Value);
                }

                adjustConfig.setLaunchDeferredDeeplink(launchDeferredDeeplink);

                if (delayStart != null)
                {
                    adjustConfig.setDelayStart(delayStart.Value);
                }

                if (needsCost != null)
                {
                    adjustConfig.setNeedsCost(needsCost.Value);
                }

                if (hasAppSecret)
                {
                    adjustConfig.setAppSecret(settings.secretId, settings.secretInfo1, settings.secretInfo2, settings.secretInfo3, settings.secretInfo4);
                }

                adjustConfig.setUrlStrategy(urlStrategy.ToLowerCaseString());

                if (!string.IsNullOrEmpty(defaultTracker))
                {
                    adjustConfig.setDefaultTracker(defaultTracker);
                }

                if (!string.IsNullOrEmpty(processName))
                {
                    adjustConfig.setProcessName(processName);
                }

                if (preinstallTracking != null)
                {
                    adjustConfig.setPreinstallTrackingEnabled(preinstallTracking.Value);
                }

                if (!string.IsNullOrEmpty(preinstallFilePath))
                {
                    adjustConfig.setPreinstallFilePath(preinstallFilePath);
                }

                if (finalAttribution != null)
                {
                    adjustConfig.setFinalAndroidAttributionEnabled(finalAttribution.Value);
                }

                if (readDeviceInfoOnce != null)
                {
                    adjustConfig.setReadDeviceInfoOnceEnabled(readDeviceInfoOnce.Value);
                }

                if (linkMe != null)
                {
                    adjustConfig.setLinkMeEnabled(linkMe.Value);
                }

                if (allowAdServicesInfoReading != null)
                {
                    adjustConfig.setAllowAdServicesInfoReading(allowAdServicesInfoReading.Value);
                }

                if (allowIdfaReading != null)
                {
                    adjustConfig.setAllowIdfaReading(allowIdfaReading.Value);
                }

                if (skanHandling)
                {
                    adjustConfig.deactivateSKAdNetworkHandling();
                }

                if (attTimeout != null)
                {
                    adjustConfig.setAttConsentWaitingInterval(attTimeout.Value);
                }

                if (sendAttribution)
                {
                    adjustConfig.setAttributionChangedDelegate(OnAttributionChanged, adjust.gameObject.name);
                }

                if (sendDeepLink)
                {
                    adjustConfig.setDeferredDeeplinkDelegate(OnDeferredDeeplink, adjust.gameObject.name);
                }

                if (setCustomerId)
                {
                    Adjust.addSessionCallbackParameter("userId", SuperfineSDK.GetUserId());
                }

                Adjust.start(adjustConfig);
            }
            else
            {
                if (setCustomerId)
                {
                    Adjust.addSessionCallbackParameter("userId", SuperfineSDK.GetUserId());
                }
            }

            if (setDeepLink)
            {
                string deepLinkUrl = SuperfineSDK.GetDeepLinkUrl();
                if (!string.IsNullOrEmpty(deepLinkUrl))
                {
                    Adjust.appWillOpenUrl(deepLinkUrl);
                }

                SuperfineSDK.AddDeepLinkCallback(OnSetDeepLink);
            }

            if (setPushToken)
            {
                string pushToken = SuperfineSDK.GetPushToken();
                if (!string.IsNullOrEmpty(pushToken))
                {
                    Adjust.setDeviceToken(pushToken);
                }

                SuperfineSDK.AddPushTokenCallback(OnSetPushToken);
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

            if (setDeepLink)
            {
                SuperfineSDK.RemoveDeepLinkCallback(OnSetDeepLink);
            }

            if (setPushToken)
            {
                SuperfineSDK.RemovePushTokenCallback(OnSetPushToken);
            }

            if (adjustObject != null)
            {
                GameObject.Destroy(adjustObject.gameObject);
                adjustObject = null;
            }

            adjust = null;
        }

        private void OnStart()
        {
            if (autoStart)
            {
                Adjust.addSessionCallbackParameter("sessionId", SuperfineSDK.GetSessionId());
            }
        }

        private void OnResume()
        {
            if (autoStart)
            {
                Adjust.addSessionCallbackParameter("sessionId", SuperfineSDK.GetSessionId());
            }
        }

        private SimpleJSON.JSONObject CreateAttributionData(AdjustAttribution attribution)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();

            PutString(ret, "trackerToken", attribution.trackerToken);
            PutString(ret, "trackerName", attribution.trackerName);
            PutString(ret, "network", attribution.network);
            PutString(ret, "campaign", attribution.campaign);
            PutString(ret, "adgroup", attribution.adgroup);
            PutString(ret, "creative", attribution.creative);
            PutString(ret, "clickLabel", attribution.clickLabel);
            PutString(ret, "adid", attribution.adid);
            PutString(ret, "costType", attribution.costType);
            if (attribution.costAmount != null)
            {
                PutDouble(ret, "costAmount", attribution.costAmount.Value);
            }
            PutString(ret, "costCurrency", attribution.costCurrency);
            PutString(ret, "fbInstallReferrer", attribution.fbInstallReferrer);

            return ret;
        }

        public void SendAttribution(AdjustAttribution attribution)
        {
            if (attribution == null) return;
            SuperfineSDK.Log(ADJUST_ATTRIBUTION, CreateAttributionData(attribution), EventFlag.WAIT_OPEN_EVENT);
        }

        private void OnAttributionChanged(AdjustAttribution attribution)
        {
            SendAttribution(attribution);
        }

        private SimpleJSON.JSONObject CreateDeepLinkData(string url)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();
            PutString(ret, "url", url);

            return ret;
        }

        public void SendDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            SuperfineSDK.Log(ADJUST_DEEPLINK, CreateDeepLinkData(url), EventFlag.WAIT_OPEN_EVENT);
        }

        private void OnDeferredDeeplink(string url)
        {
            SendDeepLink(url);   
        }

        private void OnSetDeepLink(string url)
        {
            Adjust.appWillOpenUrl(url);
        }

        private void OnSetPushToken(string token)
        {
            Adjust.setDeviceToken(token);
        }

        private string GetEventToken(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || eventTokenMap == null) return null;

            if (!eventTokenMap.TryGetValue(eventName, out string token)) return null;
            return token;
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

        private string ExtractAdUnit(string source, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return null;

            switch (source)
            {
                case "AppLovin":
                    return OptString(networkData, "adUnitId");

                case "AdMob":
                    return OptString(networkData, "adUnitId");

                case "IronSource":
                    return null;

                case "Appodeal":
                    return OptString(networkData, "adUnitName");

                default:
                    return null;
            }
        }

        private string ExtractAdPlacement(string source, SimpleJSON.JSONObject networkData)
        {
            if (networkData == null) return null;

            switch (source)
            {
                case "AppLovin":
                    return OptString(networkData, "placement");

                case "AdMob":
                    return OptString(networkData, "placement");

                case "IronSource":
                    return OptString(networkData, "placement");

                case "Appodeal":
                    return OptString(networkData, "placement");

                default:
                    return null;
            }
        }

        private bool ExtractPurchase(string eventName, SimpleJSON.JSONObject data, ref double revenue, ref string currency, ref string productId)
        {
            if (data == null)
            {
                return false;
            }

            switch (eventName)
            {
                case IAP_SUCCESS_PAYMENT:
                case CRYPTO_PAYMENT:
                    {
                        SimpleJSON.JSONNode node;

                        if (!data.TryGetValue("price", out node)) return false;
                        if (!node.IsNumber) return false;
                        double price = node.AsDouble;

                        if (!data.TryGetValue("amount", out node)) return false;
                        if (!node.IsNumber) return false;
                        int amount = node.AsInt;

                        if (!data.TryGetValue("currency", out node)) return false;
                        if (!node.IsString) return false;

                        revenue = price * amount;
                        currency = node.Value;

                        if (data.TryGetValue("pack", out node) && node.IsString)
                        {
                            productId = node.Value;
                        }

                        return true;
                    }

                default:
                    return false;
            }
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

            string adjustSource = GetAdjustAdRevenueSource(source);

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

            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(adjustSource);
            adjustAdRevenue.setRevenue(revenue, currency);
            adjustAdRevenue.setAdImpressionsCount(1);

            if (!string.IsNullOrEmpty(network))
            {
                adjustAdRevenue.setAdRevenueNetwork(network);
            }

            string adUnit = ExtractAdUnit(source, networkData);
            string adPlacement = ExtractAdPlacement(source, networkData);

            if (!string.IsNullOrEmpty(adUnit))
            {
                adjustAdRevenue.setAdRevenueUnit(adUnit);
            }

            if (!string.IsNullOrEmpty(adPlacement))
            {
                adjustAdRevenue.setAdRevenuePlacement(adPlacement);
            }

            if (networkData != null)
            {
                foreach (var pair in networkData)
                {
                    string key = pair.Key;
                    if (string.IsNullOrEmpty(key)) continue;

                    node = pair.Value;
                    if (node == null) continue;

                    string value = GetString(node);
                    if (string.IsNullOrEmpty(value)) continue;

                    adjustAdRevenue.addCallbackParameter(key, value);
                }
            }

            Adjust.trackAdRevenue(adjustAdRevenue);
        }

        private void OnSendEvent(string eventName, string eventData)
        {
            if (eventName == ADJUST_ATTRIBUTION || eventName == ADJUST_DEEPLINK) //prevent sending duplicated event
            {
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

            string token = GetEventToken(eventName);
            if (string.IsNullOrEmpty(token)) return;

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

            AdjustEvent adjustEvent = new AdjustEvent(token);

            double revenue = 0.0;
            string currency = string.Empty;
            string productId = string.Empty;

            if (ExtractPurchase(eventName, jsonObject, ref revenue, ref currency, ref productId))
            {
                adjustEvent.setRevenue(revenue, currency);

                if (!string.IsNullOrEmpty(productId))
                {
                    adjustEvent.setProductId(productId);
                }
            }

            if (jsonObject != null)
            {
                foreach (var pair in jsonObject)
                {
                    string key = pair.Key;
                    if (string.IsNullOrEmpty(key)) continue;

                    SimpleJSON.JSONNode node = pair.Value;
                    if (node == null) continue;

                    string value = GetString(node);
                    if (string.IsNullOrEmpty(value)) continue;

                    adjustEvent.addCallbackParameter(key, value);
                }
            }

            Adjust.trackEvent(adjustEvent);
        }
    }
}
