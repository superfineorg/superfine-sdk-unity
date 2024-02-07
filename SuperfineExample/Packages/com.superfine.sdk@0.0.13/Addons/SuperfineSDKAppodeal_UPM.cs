using AppodealStack.Monetization.Common;

namespace Superfine.Unity
{
    public static class SuperfineSDKAppodeal
    {
        private static bool hasRegisteredAppodeal = false;

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        private static void OnAdRevenueReceived(object sender, AdRevenueEventArgs e)
        {
            AppodealAdRevenue ad = e.Ad;
            if (ad == null) return;

            string network = ad.NetworkName;
            SimpleJSON.JSONObject networkData = new SimpleJSON.JSONObject();

            PutString(networkData, "adType", ad.AdType);
            PutString(networkData, "adUnitName", ad.AdUnitName);
            PutString(networkData, "demandSource", ad.DemandSource);
            PutString(networkData, "placement", ad.Placement);
            PutString(networkData, "revenuePrecision", ad.RevenuePrecision);

            SuperfineSDK.LogAdRevenue("Appodeal", ad.Revenue, ad.Currency, network, networkData);
        }

        public static void RegisterPaidEvent()
        {
            if (hasRegisteredAppodeal) return;
            AppodealCallbacks.AdRevenue.OnReceived += OnAdRevenueReceived;
            hasRegisteredAppodeal = true;
        }

        public static void UnregisterPaidEvent()
        {
            if (!hasRegisteredAppodeal) return;
            AppodealCallbacks.AdRevenue.OnReceived -= OnAdRevenueReceived;
            hasRegisteredAppodeal = false;
        }
    }
}
