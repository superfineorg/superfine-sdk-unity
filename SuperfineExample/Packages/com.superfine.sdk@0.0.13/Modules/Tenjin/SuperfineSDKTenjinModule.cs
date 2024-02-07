using System;
using System.Collections.Generic;
using System.Linq;

namespace Superfine.Unity
{
    public class SuperfineSDKTenjinModule : SuperfineSDKModule
    {
        private const string TENJIN_ATTRIBUTION = "tenjin_attribution";
        private const string TENJIN_DEEPLINK = "tenjin_deeplink";

        private const string IAP_SUCCESS_PAYMENT = "gjsdk_iap_success_payment";
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private string sdkKey = string.Empty;

        private bool autoStart = true;
        private bool setCustomerId = true;

        private bool sendLog = false;

        private bool sendIAP = false;

        private bool sendAttribution = true;
        private bool sendDeepLink = true;

        private string[] logFilters = null;
        private string[] logNotFilters = null;

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        public SuperfineSDKTenjinModule(SuperfineSDKModuleSettings settings) : base(settings)
        {
        }

        protected override void Initialize(SuperfineSDKModuleSettings baseSettings)
        {
            base.Initialize(baseSettings);

            SuperfineSDKTenjinSettings settings = baseSettings as SuperfineSDKTenjinSettings;

            sdkKey = settings ? settings.sdkKey : string.Empty;

            autoStart = settings ? settings.autoStart : true;
            setCustomerId = settings ? settings.setCustomerId : true;

            sendLog = settings ? settings.sendLog : false;

            logFilters = settings ? settings.logFilters : null;
            if (logFilters != null && logFilters.Length == 0) logFilters = null;

            logNotFilters = settings ? settings.logNotFilters : null;
            if (logNotFilters != null && logNotFilters.Length == 0) logNotFilters = null;

            sendIAP = settings ? settings.sendIAP : false;

            sendAttribution = settings ? settings.sendAttribution : true;
            sendDeepLink = settings ? settings.sendDeepLink : true;

            int subversion = settings ? settings.subversion : 0;
            bool cacheEvent = settings ? settings.cacheEvent : false;

            bool? optIn = settings ? settings.optIn : null;
            string[] optInParams = settings ? settings.optInParams : null;
            string[] optOutParams = settings ? settings.optOutParams : null;

            bool debug = settings ? settings.debug : false;

            BaseTenjin baseTenjin = Tenjin.getInstance(sdkKey);

            SuperfineSDK.AddStartCallback(OnStart);
            SuperfineSDK.AddResumeCallback(OnResume);

            if (sendLog)
            {
                SuperfineSDK.AddSendEventCallback(OnSendEvent);
            }

            if (autoStart)
            {
                if (baseTenjin != null)
                {
                    AppStoreType appStoreType = GetTenjinAppStoreType();
                    if (appStoreType != AppStoreType.unspecified)
                    {
                        baseTenjin.SetAppStoreType(appStoreType);
                    }

                    if (optIn != null)
                    {
                        bool optInValue = optIn.Value;
                        if (optInValue)
                        {
                            baseTenjin.OptIn();
                        }
                        else
                        {
                            baseTenjin.OptOut();
                        }
                    }

                    if (optInParams != null && optInParams.Length > 0)
                    {
                        baseTenjin.OptInParams(optInParams.ToList());
                    }

                    if (optOutParams != null && optOutParams.Length > 0)
                    {
                        baseTenjin.OptOutParams(optOutParams.ToList());
                    }

                    if (subversion != 0)
                    {
                        baseTenjin.AppendAppSubversion(subversion);
                    }

                    baseTenjin.SetCacheEventSetting(cacheEvent);

                    if (debug)
                    {
                        baseTenjin.DebugLogs();
                    }
                }
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
        }

        private AppStoreType GetTenjinAppStoreType()
        {
            StoreType storeType = SuperfineSDK.GetStoreType();

            switch (storeType)
            {
                case StoreType.APP_STORE:
                    return AppStoreType.unspecified;

                case StoreType.GOOGLE_PLAY:
                    return AppStoreType.googleplay;

                case StoreType.AMAZON_STORE: 
                    return AppStoreType.amazon;

                default:
                    return AppStoreType.other;
            }
        }

        private BaseTenjin GetTenjinSDK()
        {
            return Tenjin.getInstance(sdkKey);
        }

        private void ConnectTenjin(BaseTenjin baseTenjin)
        {
            if (baseTenjin == null) return;

            string deepLinkUrl = SuperfineSDK.GetDeepLinkUrl();

            if (!string.IsNullOrEmpty(deepLinkUrl)) baseTenjin.Connect(deepLinkUrl);
            else baseTenjin.Connect();
        }

        private static SimpleJSON.JSONObject CreateAttributionData(string analyticsInstallationId, Dictionary<string, string> data)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();
            PutString(ret, "analyticsInstallationId", analyticsInstallationId);

            if (data != null)
            {
                foreach (KeyValuePair<string, string> entry in data)
                {
                    string key = entry.Key;
                    if (string.IsNullOrEmpty(key)) continue;

                    PutString(ret, key, entry.Value);
                }
            }

            return ret;
        }

        private static SimpleJSON.JSONObject CreateDeepLinkData(Dictionary<string, string> data)
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();

            if (data != null)
            {
                foreach (KeyValuePair<string, string> entry in data)
                {
                    string key = entry.Key;
                    if (string.IsNullOrEmpty(key)) continue;

                    PutString(ret, key, entry.Value);
                }
            }

            return ret;
        }

        public void SendAttributionEvent(Dictionary<string, string> attributionInfoData)
        {
            BaseTenjin baseTenjin = GetTenjinSDK();
            SimpleJSON.JSONObject data = CreateAttributionData(baseTenjin != null ? baseTenjin.GetAnalyticsInstallationId() : string.Empty, attributionInfoData);

            if (data == null || data.Count == 0) SuperfineSDK.Log(TENJIN_ATTRIBUTION, (SimpleJSON.JSONObject)null, EventFlag.WAIT_OPEN_EVENT);
            else SuperfineSDK.Log(TENJIN_ATTRIBUTION, data, EventFlag.WAIT_OPEN_EVENT);
        }

        private void OnReceiveAttributionInfo(Dictionary<string, string> attributionInfoData)
        {
            SendAttributionEvent(attributionInfoData);
        }

        public void SendDeepLinkEvent(Dictionary<string, string> deferredLinkData)
        {
            SimpleJSON.JSONObject data = CreateDeepLinkData(deferredLinkData);

            if (data == null || data.Count == 0) SuperfineSDK.Log(TENJIN_DEEPLINK, (SimpleJSON.JSONObject)null, EventFlag.WAIT_OPEN_EVENT);
            else SuperfineSDK.Log(TENJIN_DEEPLINK, data, EventFlag.WAIT_OPEN_EVENT);
        }

        private void OnReceiveDeepLink(Dictionary<string, string> deferredLinkData)
        {
            SendDeepLinkEvent(deferredLinkData);
        }

        private void OnStart()
        {
            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin != null)
            {
                if (setCustomerId)
                {
                    baseTenjin.SetCustomerUserId(SuperfineSDK.GetUserId());
                }

                if (autoStart)
                {
                    ConnectTenjin(baseTenjin);
                }

                if (sendAttribution)
                {
                    if (baseTenjin != null)
                    {
                        baseTenjin.GetAttributionInfo(OnReceiveAttributionInfo);
                    }
                }

                if (sendDeepLink)
                {
                    if (baseTenjin != null)
                    {
                        baseTenjin.GetDeeplink(OnReceiveDeepLink);
                    }
                }
            }
        }

        private void OnResume()
        {
            if (autoStart)
            {
                BaseTenjin baseTenjin = GetTenjinSDK();
                if (baseTenjin != null)
                {
                    ConnectTenjin(baseTenjin);
                }
            }
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

        public void SendAppStoreTransaction(string productId, string currencyCode, int quantity, double unitPrice)
        {
            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin == null)
            {
                return;
            }

            baseTenjin.Transaction(productId, currencyCode, quantity, unitPrice, string.Empty, string.Empty, string.Empty);
        }

        public void SendGooglePlayTransaction(string productId, string currencyCode, int quantity, double unitPrice)
        {
            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin == null)
            {
                return;
            }

            baseTenjin.Transaction(productId, currencyCode, quantity, unitPrice, string.Empty, string.Empty, string.Empty);
        }

        public void SendAmazonTransaction(string productId, string currencyCode, int quantity, double unitPrice)
        {
            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin == null)
            {
                return;
            }

            baseTenjin.TransactionAmazon(productId, currencyCode, quantity, unitPrice, string.Empty, string.Empty);
        }

        public static StoreType GetStoreType(string storeTypeString)
        {
            switch (storeTypeString)
            {
                case "app_store":
                    return StoreType.APP_STORE;

                case "google_play":
                    return StoreType.GOOGLE_PLAY;

                case "amazon_store":
                    return StoreType.AMAZON_STORE;

                case "microsoft_store":
                    return StoreType.MICROSOFT_STORE;

                default:
                    return StoreType.UNKNOWN;
            }
        }

        private void SendIAPEvent(string eventData)
        {
            if (string.IsNullOrEmpty(eventData))
            {
                return;
            }

            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin == null)
            {
                return;
            }

            if (!SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node)) return;
            if (!node.IsObject) return;

            SimpleJSON.JSONObject iapData = (SimpleJSON.JSONObject)node;

            if (!iapData.TryGetValue("store_type", out node)) return;
            if (!node.IsString) return;
            string storeTypeString = node.Value;

            StoreType storeType = GetStoreType(storeTypeString);
            if (storeType != StoreType.APP_STORE && storeType != StoreType.GOOGLE_PLAY && storeType != StoreType.AMAZON_STORE)
            {
                return;
            }

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

            if (storeType == StoreType.APP_STORE)
            {
                SendAppStoreTransaction(pack, currency, amount, price);
            }
            else if (storeType == StoreType.GOOGLE_PLAY)
            {
                SendGooglePlayTransaction(pack, currency, amount, price);
            }
            else if (storeType == StoreType.AMAZON_STORE)
            {
                SendAmazonTransaction(pack, currency, amount, price);
            }
        }

        private void OnSendEvent(string eventName, string eventData)
        {
            if (eventName == TENJIN_ATTRIBUTION || eventName == TENJIN_DEEPLINK) //prevent sending duplicated event
            {
                return;
            }

            if (eventName == IAP_SUCCESS_PAYMENT)
            {
                if (sendIAP)
                {
                    SendIAPEvent(eventData);
                }

                return;
            }

            if (eventName == AD_REVENUE)
            {
                return;
            }

            if (!IsValidEventName(eventName))
            {
                return;
            }

            BaseTenjin baseTenjin = GetTenjinSDK();
            if (baseTenjin == null)
            {
                return;
            }

            bool isIntData = false;
            int eventDataInt = 0;

            if (!string.IsNullOrEmpty(eventData))
            {
                if (SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node))
                {
                    if (node.IsObject)
                    {
                        SimpleJSON.JSONObject jsonObject = (SimpleJSON.JSONObject)node;
                        if (jsonObject.Count == 1)
                        {
                            if (jsonObject.TryGetValue("value", out node))
                            {
                                if (node.IsNumber)
                                {
                                    double valueDouble = ((SimpleJSON.JSONNumber)node).AsDouble;
                                    int valueInt = (int)Math.Round(valueDouble);

                                    if (Math.Abs(valueDouble - valueInt) < 1e-5)
                                    {
                                        isIntData = true;
                                        eventDataInt = valueInt;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (isIntData)
            {
                baseTenjin.SendEvent(eventName, eventDataInt.ToString());
            }
            else
            {
                baseTenjin.SendEvent(eventName);
            }
        }
    }
}
