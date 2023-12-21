using Facebook.Unity;

using System;
using System.Collections.Generic;
using Superfine.Unity.SimpleJSON;

namespace Superfine.Unity
{
    public static class SuperfineSDKFacebook
    {
        public static bool enableIAPPuchaseLog = true;

        private static bool hasRegisteredSendEvent = false;
        private static bool isFacebookInitialized = false;

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

        private static Dictionary<string, object> GetParameters(SimpleJSON.JSONObject obj)
        {
            if (obj == null) return null;
            
            if (obj.Count == 0) return null;

            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (var pair in obj)
            {
                ret.Add(pair.Key, pair.Value.ToString());
            }

            return ret;
        }

        private static void OnSendEvent(string eventName, string eventData)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            JSONObject obj = null;

            if (!string.IsNullOrEmpty(eventData))
            {
                try
                {
                    JSONNode node = JSONNode.Parse(eventData);
                    if (node != null && node.IsObject)
                    {
                        obj = (JSONObject)node;
                    }
                }
                catch (Exception)
                {
                }
            }

            if (eventName.Equals("gjsdk_iap_success_payment")) //IAP success event
            {
                if (!enableIAPPuchaseLog) return;

                double price = 0.0;
                int amount = 1;

                string currency = null;

                if (obj != null)
                {
                    JSONNumber priceNode = obj.GetValueOrDefault<JSONNumber>("price", null);

                    if (priceNode != null)
                    {
                        price = priceNode.AsDouble;
                        obj.Remove("price");

                        JSONNumber amountNode = obj.GetValueOrDefault<JSONNumber>("amount", null);
                        if (amountNode != null)
                        {
                            try
                            {
                                amount = (int)amountNode.AsLong;
                            }
                            catch
                            {
                                amount = 1;
                            }

                            obj.Remove("amount");
                        }
                    }
                    
                    JSONString currencyNode = obj.GetValueOrDefault<JSONString>("currency", null);

                    if (currencyNode != null)
                    {
                        currency = currencyNode.Value;
                        obj.Remove("currency");
                    }
                }

                LogPurchase(Convert.ToDecimal(price * amount), currency, GetParameters(obj));
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
