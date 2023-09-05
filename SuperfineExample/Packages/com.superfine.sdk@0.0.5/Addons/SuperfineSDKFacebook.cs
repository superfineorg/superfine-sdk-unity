using Facebook.Unity;

using System;
using System.Collections.Generic;
using Superfine.Unity.SimpleJSON;

namespace Superfine.Unity
{
    public static class SuperfineSDKFacebook
    {
        public static void RegisterSendEvent()
        {
            SuperfineSDK.AddSendEventCallback(OnSendEvent);
        }

        public static void UnregisterSendEvent()
        {
            SuperfineSDK.RemoveSendEventCallback(OnSendEvent);
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

                FB.LogPurchase(Convert.ToDecimal(price * amount), currency, GetParameters(obj));
                return;
            }       

            FB.LogAppEvent(eventName, null, GetParameters(obj));
        }
    }
}
