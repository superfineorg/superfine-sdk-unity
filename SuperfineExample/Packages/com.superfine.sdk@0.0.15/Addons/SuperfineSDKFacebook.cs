using Facebook.Unity;

using System;
using System.Collections.Generic;

namespace Superfine.Unity
{
    public static class SuperfineSDKFacebook
    {
        public static bool enableIAPPuchaseLog = true;
        public static bool enableAdRevenueLog = true;

        private static bool hasRegisteredSendEvent = false;
        private static bool isFacebookInitialized = false;

        private const string IAP_SUCCESS_PAYMENT = "gjsdk_iap_success_payment";
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private struct PurchaseCacheData
        {
            public decimal logPurchase;
            public string currency;
            public Dictionary<string, object> parameters;
        }

        private static List<PurchaseCacheData> purchaseCacheList = null;

        private static void AddPurchaseCache(decimal logPurchase, string currency, Dictionary<string, object> parameters)
        {
            if (purchaseCacheList == null)
            {
                purchaseCacheList = new List<PurchaseCacheData>();
            }

            purchaseCacheList.Add(new PurchaseCacheData {
                logPurchase = logPurchase,
                currency = currency,
                parameters = parameters 
            });
        }

        private struct EventCacheData
        {
            public string logEvent;
            public Dictionary<string, object> paramters;
        }

        private static List<EventCacheData> eventCacheList = null;

        private static void AddEventCache(string logEvent, Dictionary<string, object> parameters)
        {
            if (eventCacheList == null)
            {
                eventCacheList = new List<EventCacheData>();
            }

            eventCacheList.Add(new EventCacheData {
                logEvent = logEvent,
                paramters = parameters
            });
        }

        private static void ProcessCache()
        {
            if (purchaseCacheList != null)
            {
                foreach (PurchaseCacheData data in purchaseCacheList)
                {
                    LogPurchase(data.logPurchase, data.currency, data.parameters);
                }

                purchaseCacheList = null;
            }

            if (eventCacheList != null)
            {
                foreach (EventCacheData data in eventCacheList)
                {
                    LogAppEvent(data.logEvent, data.paramters);
                }

                eventCacheList = null;
            }
        }

        public static void RegisterSendEvent()
        {
            if (hasRegisteredSendEvent) return;
            hasRegisteredSendEvent = true;

            isFacebookInitialized = FB.IsInitialized;

            SuperfineSDK.AddSendEventCallback(OnSendEvent);
        }

        public static void UnregisterSendEvent()
        {
            if (!hasRegisteredSendEvent) return;
            hasRegisteredSendEvent = false;

            SuperfineSDK.RemoveSendEventCallback(OnSendEvent);
        }

        public static void OnFacebookInitialized()
        {
            if (isFacebookInitialized) return;

            isFacebookInitialized = true;
            ProcessCache();
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

        private static Dictionary<string, object> GetParameters(SimpleJSON.JSONObject obj)
        {
            if (obj == null) return null;
            
            if (obj.Count == 0) return null;

            Dictionary<string, object> ret = new Dictionary<string, object>();
            AddParameters(ret, obj);

            return ret;
        }

        private static void AddParameters(Dictionary<string, object> dict, SimpleJSON.JSONObject obj)
        {
            if (obj == null || obj.Count == 0) return;

            foreach (var pair in obj)
            {
                string value = GetString(pair.Value);
                if (string.IsNullOrEmpty(value)) continue;

                dict.Add(pair.Key, value);
            }
        }

        private static void SendIAPEvent(SimpleJSON.JSONObject obj)
        {
            if (obj == null) return;

            SimpleJSON.JSONNode node;

            if (!obj.TryGetValue("pack", out node)) return;
            if (!node.IsString) return;
            string pack = node.Value;

            if (!obj.TryGetValue("price", out node)) return;
            if (!node.IsNumber) return;
            double price = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!obj.TryGetValue("amount", out node)) return;
            if (!node.IsNumber) return;
            int amount = ((SimpleJSON.JSONNumber)node).AsInt;

            if (!obj.TryGetValue("currency", out node)) return;
            if (!node.IsString) return;
            string currency = node.Value;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "eventName", IAP_SUCCESS_PAYMENT },
                { "pack", pack },
                { "price", price },
                { "amount", amount }
            };

            obj.Remove("pack");
            obj.Remove("price");
            obj.Remove("amount");
            obj.Remove("currency");

            AddParameters(parameters, obj);                     

            LogPurchase(Convert.ToDecimal(price * amount), currency, parameters);
        }

        private static void SendAdRevenueEvent(SimpleJSON.JSONObject obj)
        {
            if (obj == null) return;

            SimpleJSON.JSONNode node;

            if (!obj.TryGetValue("source", out node)) return;
            if (!node.IsString) return;
            string source = node.Value;

            if (!obj.TryGetValue("revenue", out node)) return;
            if (!node.IsNumber) return;
            double revenue = ((SimpleJSON.JSONNumber)node).AsDouble;

            if (!obj.TryGetValue("currency", out node)) return;
            if (!node.IsString) return;
            string currency = node.Value;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "eventName", AD_REVENUE },
                { "source", source }
            };

            if (obj.TryGetValue("network", out node))
            {
                if (node.IsString)
                {
                    string network = node.Value;
                    if (!string.IsNullOrEmpty(network))
                    {
                        parameters.Add("network", network);
                    }
                }
            }

            if (obj.TryGetValue("network_data", out node))
            {
                if (node.IsObject)
                {
                    SimpleJSON.JSONObject networkData = (SimpleJSON.JSONObject)node;
                    AddParameters(parameters, networkData);
                }
            }

            LogPurchase(Convert.ToDecimal(revenue), currency, parameters);
        }

        private static void OnSendEvent(string eventName, string eventData)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            SimpleJSON.JSONObject obj = null;

            if (!string.IsNullOrEmpty(eventData))
            {
                try
                {
                    SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(eventData);
                    if (node != null && node.IsObject)
                    {
                        obj = (SimpleJSON.JSONObject)node;
                    }
                }
                catch (Exception)
                {
                }
            }

            if (eventName == IAP_SUCCESS_PAYMENT)
            {
                if (!enableIAPPuchaseLog) return;

                SendIAPEvent(obj);
                return;
            }
            else if (eventName == AD_REVENUE)
            {
                if (!enableAdRevenueLog) return;

                SendAdRevenueEvent(obj);
                return;
            }

            LogAppEvent(eventName, GetParameters(obj));
        }

        private static void LogPurchase(decimal logPurchase, string currency, Dictionary<string, object> parameters)
        {
            if (!isFacebookInitialized)
            {
                AddPurchaseCache(logPurchase, currency, parameters);
            }
            else
            {
                FB.LogPurchase(logPurchase, currency, parameters);
            }
        }

        private static void LogAppEvent(string logEvent, Dictionary<string, object> parameters)
        {
            if (!isFacebookInitialized)
            {
                AddEventCache(logEvent, parameters);
            }
            else
            {
                FB.LogAppEvent(logEvent, null, parameters);
            }
        }
    }
}
