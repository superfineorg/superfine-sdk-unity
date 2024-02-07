using System.Collections.Generic;

namespace Superfine.Unity
{
    public static class SuperfineSDKApplovin
    {
        private static bool hasRegisteredAppLovin = false;

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        private static void PutDouble(SimpleJSON.JSONObject paramJSONObject, string paramString, double paramDouble)
        {
            paramJSONObject.Add(paramString, paramDouble);
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

        private static void PutLong(SimpleJSON.JSONObject paramJSONObject, string paramString, long paramLong)
        {
            paramJSONObject.Add(paramString, paramLong);
        }

        private static void PutInt(SimpleJSON.JSONObject paramJSONObject, string paramString, int paramInt)
        {
            paramJSONObject.Add(paramString, paramInt);
        }

        private static void PutBoolean(SimpleJSON.JSONObject paramJSONObject, string paramString, bool paramBoolean)
        {
            paramJSONObject.Add(paramString, paramBoolean);
        }

        private static SimpleJSON.JSONObject CreateNetworkResponseInfo(MaxSdkBase.NetworkResponseInfo response)
        {
            SimpleJSON.JSONObject networkResponseObject = new SimpleJSON.JSONObject();
            PutInt(networkResponseObject, "adLoadState", (int)response.AdLoadState);

            MaxSdkBase.MediatedNetworkInfo mediatedNetworkInfo = response.MediatedNetwork;
            if (mediatedNetworkInfo != null)
            {
                SimpleJSON.JSONObject networkInfoObject = new SimpleJSON.JSONObject();
                PutString(networkInfoObject, "name", mediatedNetworkInfo.Name);
                PutString(networkInfoObject, "adapterClassName", mediatedNetworkInfo.AdapterClassName);
                PutString(networkInfoObject, "adapterVersion", mediatedNetworkInfo.AdapterVersion);
                PutString(networkInfoObject, "sdkVersion", mediatedNetworkInfo.SdkVersion);

                PutJSONNode(networkResponseObject, "mediatedNetwork", networkInfoObject);
            }

            Dictionary<string, object> credentials = response.Credentials;
            if (credentials != null)
            {
                PutJSONNode(networkResponseObject, "credentials", SimpleJSON.JSON.ToJSONNode(credentials));
            }

            PutBoolean(networkResponseObject, "isBidding", response.IsBidding);

            MaxSdkBase.ErrorInfo error = response.Error;
            if (error != null)
            {
                SimpleJSON.JSONObject errorObject = new SimpleJSON.JSONObject();

                PutString(errorObject, "errorMessage", error.Message);
                PutString(errorObject, "adLoadFailureInfo", error.AdLoadFailureInfo);
                PutInt(errorObject, "errorCode", (int)error.Code);

                PutJSONNode(networkResponseObject, "error", errorObject);
            }

            PutLong(networkResponseObject, "latencyMillis", response.LatencyMillis);

            return networkResponseObject;
        }

        private static SimpleJSON.JSONObject CreateWaterfallInfo(MaxSdkBase.WaterfallInfo waterfallInfo)
        {
            if (waterfallInfo == null) return null;

            SimpleJSON.JSONObject waterfallInfoObject = new SimpleJSON.JSONObject();
            PutString(waterfallInfoObject, "name", waterfallInfo.Name);
            PutString(waterfallInfoObject, "testName", waterfallInfo.TestName);

            List<MaxSdkBase.NetworkResponseInfo> networkResponses = waterfallInfo.NetworkResponses;
            if (networkResponses != null)
            {
                int numNetworkResponses = networkResponses.Count;
                if (numNetworkResponses > 0)
                {
                    SimpleJSON.JSONArray networkResponsesArray = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numNetworkResponses; ++i)
                    {
                        networkResponsesArray.Add(null, CreateNetworkResponseInfo(networkResponses[i]));
                    }

                    PutJSONArray(waterfallInfoObject, "networkResponses", networkResponsesArray);
                }
            }

            PutLong(waterfallInfoObject, "latencyMillis", waterfallInfo.LatencyMillis);

            return waterfallInfoObject;
        }

        public static void RegisterPaidEvent()
        {
            if (hasRegisteredAppLovin) return;

            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.CrossPromo.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;

            hasRegisteredAppLovin = true;
        }

        public static void UnregisterPaidEvent()
        {
            if (!hasRegisteredAppLovin) return;

            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.CrossPromo.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;

            hasRegisteredAppLovin = false;
        }

        private static void OnAdRevenuePaidEvent(string adUnit, MaxSdkBase.AdInfo adInfo)
        {
            string network = adInfo.NetworkName;
            SimpleJSON.JSONObject networkData = new SimpleJSON.JSONObject();

            PutString(networkData, "country", MaxSdk.GetSdkConfiguration().CountryCode);

            PutString(networkData, "adUnitId", adInfo.AdUnitIdentifier);
            PutString(networkData, "adFormat", adInfo.AdFormat);
            PutString(networkData, "networkName", adInfo.NetworkName);
            PutString(networkData, "networkPlacement", adInfo.NetworkPlacement);
            PutString(networkData, "creativeId", adInfo.CreativeIdentifier);
            PutString(networkData, "placement", adInfo.Placement);
            PutDouble(networkData, "revenue", adInfo.Revenue);
            PutString(networkData, "revenuePrecision", adInfo.RevenuePrecision);
            PutString(networkData, "dspName", adInfo.DspName);
            PutJSONNode(networkData, "waterfallInfo", CreateWaterfallInfo(adInfo.WaterfallInfo));

            SuperfineSDK.LogAdRevenue("AppLovin", adInfo.Revenue, "USD", network, networkData);
        }
    }
}
