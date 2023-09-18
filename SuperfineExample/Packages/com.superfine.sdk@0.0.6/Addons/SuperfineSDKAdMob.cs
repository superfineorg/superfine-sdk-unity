using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using static MaxSdkCallbacks;

namespace Superfine.Unity
{
    public static class SuperfineSDKAdMob
    {
        private static Dictionary< KeyValuePair<object, string>, Action<AdValue> > onAdPaidCallbackMap = new Dictionary< KeyValuePair<object, string>, Action<AdValue> >();

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        private static void PutLong(SimpleJSON.JSONObject paramJSONObject, string paramString, long paramLong)
        {
            paramJSONObject.Add(paramString, paramLong);
        }

        private static void PutInt(SimpleJSON.JSONObject paramJSONObject, string paramString, int paramInt)
        {
            paramJSONObject.Add(paramString, paramInt);
        }

        private static void PutJSONNode(SimpleJSON.JSONObject paramJSONObject, string paramString, SimpleJSON.JSONNode paramJSONNode)
        {
            if (paramJSONNode == null) return;
            paramJSONObject.Add(paramString, paramJSONNode);
        }

        private static void PutJSONArray(SimpleJSON.JSONObject paramJSONObject, string paramString, SimpleJSON.JSONArray paramJSONArray)
        {
            if (paramJSONArray == null) return;
            paramJSONObject.Add(paramString, paramJSONArray);
        }

        public static void RegisterBannerViewPaidEvent(BannerView bannerView, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(bannerView, adUnitId);
            if (onAdPaidCallbackMap.ContainsKey(key)) return;

            Action<AdValue> callback = (adValue) => OnAdPaid(bannerView.GetResponseInfo(), adUnitId, adValue);
            onAdPaidCallbackMap.Add(key, callback);

            bannerView.OnAdPaid += callback;
        }

        public static void UnregisterBannerViewPaidEvent(BannerView bannerView, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(bannerView, adUnitId);
            onAdPaidCallbackMap.Remove(key);
        }

        public static void RegisterInterstitialAdPaidEvent(InterstitialAd interstitialAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(interstitialAd, adUnitId);
            if (onAdPaidCallbackMap.ContainsKey(key)) return;

            Action<AdValue> callback = (adValue) => OnAdPaid(interstitialAd.GetResponseInfo(), adUnitId, adValue);
            onAdPaidCallbackMap.Add(key, callback);

            interstitialAd.OnAdPaid += callback;
        }

        public static void UnregisterInterstitialAdPaidEvent(InterstitialAd interstitialAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(interstitialAd, adUnitId);
            onAdPaidCallbackMap.Remove(key);
        }

        public static void RegisterRewardedAdPaidEvent(RewardedAd rewardedAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(rewardedAd, adUnitId);
            if (onAdPaidCallbackMap.ContainsKey(key)) return;

            Action<AdValue> callback = (adValue) => OnAdPaid(rewardedAd.GetResponseInfo(), adUnitId, adValue);
            onAdPaidCallbackMap.Add(key, callback);

            rewardedAd.OnAdPaid += callback;
        }

        public static void UnregisterRewardedAdPaidEvent(RewardedAd rewardedAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(rewardedAd, adUnitId);
            onAdPaidCallbackMap.Remove(key);
        }

        public static void RegisterRewardedInterstitialAdPaidEvent(RewardedInterstitialAd rewardedInterstitialAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(rewardedInterstitialAd, adUnitId);
            if (onAdPaidCallbackMap.ContainsKey(key)) return;

            Action<AdValue> callback = (adValue) => OnAdPaid(rewardedInterstitialAd.GetResponseInfo(), adUnitId, adValue);
            onAdPaidCallbackMap.Add(key, callback);

            rewardedInterstitialAd.OnAdPaid += callback;
        }

        public static void UnregisterRewardedInterstitialAdPaidEvent(RewardedInterstitialAd rewardedInterstitialAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(rewardedInterstitialAd, adUnitId);
            onAdPaidCallbackMap.Remove(key);
        }

        public static void RegisterAppOpenAdPaidEvent(AppOpenAd appOpenAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(appOpenAd, adUnitId);
            if (onAdPaidCallbackMap.ContainsKey(key)) return;

            Action<AdValue> callback = (adValue) => OnAdPaid(appOpenAd.GetResponseInfo(), adUnitId, adValue);
            onAdPaidCallbackMap.Add(key, callback);

            appOpenAd.OnAdPaid += callback;
        }

        public static void UnregisterAppOpenAdPaidEvent(AppOpenAd appOpenAd, string adUnitId)
        {
            KeyValuePair<object, string> key = new KeyValuePair<object, string>(appOpenAd, adUnitId);
            onAdPaidCallbackMap.Remove(key);
        }

        private static SimpleJSON.JSONObject CreateAdError(AdError adError)
        {
            SimpleJSON.JSONObject adErrorObject = new SimpleJSON.JSONObject();

            PutInt(adErrorObject, "code", adError.GetCode());
            PutString(adErrorObject, "message", adError.GetMessage());
            PutString(adErrorObject, "domain", adError.GetDomain());

            AdError cause = adError.GetCause();
            if (cause != null)
            {
                PutJSONNode(adErrorObject, "cause", CreateAdError(cause));
            }

            return adErrorObject;
        }

        private static SimpleJSON.JSONObject CreateAdapterResponseInfo(AdapterResponseInfo adapterResponseInfo)
        {
            SimpleJSON.JSONObject adapterResponseInfoObject = new SimpleJSON.JSONObject();

            PutString(adapterResponseInfoObject, "adapter", adapterResponseInfo.AdapterClassName);
            PutLong(adapterResponseInfoObject, "latency", adapterResponseInfo.LatencyMillis);
            PutString(adapterResponseInfoObject, "adSourceName", adapterResponseInfo.AdSourceName);
            PutString(adapterResponseInfoObject, "adSourceId", adapterResponseInfo.AdSourceId);
            PutString(adapterResponseInfoObject, "adSourceInstanceName", adapterResponseInfo.AdSourceInstanceName);
            PutString(adapterResponseInfoObject, "adSourceInstanceId", adapterResponseInfo.AdSourceInstanceId);

            Dictionary<string, string> adUnitMapping = adapterResponseInfo.AdUnitMapping;
            if (adUnitMapping != null)
            {
                if (adUnitMapping.Count > 0)
                {
                    PutJSONNode(adapterResponseInfoObject, "adUnitMapping", SimpleJSON.JSON.ToJSONNode(adUnitMapping));
                }
            }

            AdError adError = adapterResponseInfo.AdError;
            if (adError != null)
            {
                PutJSONNode(adapterResponseInfoObject, "adError", CreateAdError(adError));
            }

            return adapterResponseInfoObject;
        }

        private static SimpleJSON.JSONObject CreateResponseInfo(ResponseInfo responseInfo)
        {
            SimpleJSON.JSONObject responseInfoObject = new SimpleJSON.JSONObject();

            PutString(responseInfoObject, "responseId", responseInfo.GetResponseId());
            PutString(responseInfoObject, "mediationAdapterClassName", responseInfo.GetMediationAdapterClassName());

            List<AdapterResponseInfo> adapterResponses = responseInfo.GetAdapterResponses();
            if (adapterResponses != null)
            {
                int numAdapterResponses = adapterResponses.Count;
                if (numAdapterResponses > 0)
                {
                    SimpleJSON.JSONArray adapterResponsesArray = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numAdapterResponses; ++i)
                    {
                        adapterResponsesArray.Add(null, CreateAdapterResponseInfo(adapterResponses[i]));
                    }

                    PutJSONArray(responseInfoObject, "adapterResponses", adapterResponsesArray);
                }
            }

            Dictionary<string, string> responseExtras = responseInfo.GetResponseExtras();
            if (responseExtras != null)
            {
                PutJSONNode(responseInfoObject, "responseExtras", SimpleJSON.JSON.ToJSONNode(responseExtras));
            }

            return responseInfoObject;
        }

        private static void OnAdPaid(ResponseInfo responseInfo, string adUnitId, AdValue adValue)
        {
            SimpleJSON.JSONObject networkData;

            if (responseInfo != null)
            {
                networkData = CreateResponseInfo(responseInfo);
            }
            else
            {
                networkData = new SimpleJSON.JSONObject();
            }

            PutString(networkData, "adUnitId", adUnitId);
            PutString(networkData, "precisionType", adValue.Precision.ToString());

            double revenue = adValue.Value / 1000000.0;
            string mediation = responseInfo.GetMediationAdapterClassName();
            SuperfineSDK.LogAdRevenue("AdMob", revenue, adValue.CurrencyCode, mediation, networkData);
        }
    }
}
