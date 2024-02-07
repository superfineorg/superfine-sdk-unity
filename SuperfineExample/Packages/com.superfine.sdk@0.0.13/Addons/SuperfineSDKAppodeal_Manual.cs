using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

namespace Superfine.Unity
{
    public static class SuperfineSDKAppodeal
    {
        private class SuperfineSDKAppodealAdRevenueListener : IAdRevenueListener
        {
            public void onAdRevenueReceived(AppodealAdRevenue ad)
            {
                string network = ad.NetworkName;
                SimpleJSON.JSONObject networkData = new SimpleJSON.JSONObject();

                PutString(networkData, "adType", ad.AdType);
                PutString(networkData, "adUnitName", ad.AdUnitName);
                PutString(networkData, "demandSource", ad.DemandSource);
                PutString(networkData, "placement", ad.Placement);
                PutString(networkData, "revenuePrecision", ad.RevenuePrecision);

                SuperfineSDK.LogAdRevenue("Appodeal", ad.Revenue, ad.Currency, network, networkData);
            }
        }

        private static bool hasRegisteredAppodeal = false;
        private static IAdRevenueListener listener = null;

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        public static void RegisterPaidEvent()
        {
            if (hasRegisteredAppodeal) return;

            listener = new SuperfineSDKAppodealAdRevenueListener();
            Appodeal.setAdRevenueCallback(listener);

            hasRegisteredAppodeal = true;
        }

        public static void UnregisterPaidEvent()
        {
            if (!hasRegisteredAppodeal) return;

            Appodeal.setAdRevenueCallback(null);
            listener = null;

            hasRegisteredAppodeal = false;
        }
    }
}
