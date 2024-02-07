namespace Superfine.Unity
{
    public static class SuperfineSDKIronSource
    {
        private static bool hasRegisteredIronSource = false;

        private static void PutString(SimpleJSON.JSONObject paramJSONObject, string paramString, string paramString2)
        {
            if (string.IsNullOrEmpty(paramString2)) return;
            paramJSONObject.Add(paramString, paramString2);
        }

        private static void PutDouble(SimpleJSON.JSONObject paramJSONObject, string paramString, double paramDouble)
        {
            paramJSONObject.Add(paramString, paramDouble);
        }

        private static void PutInt(SimpleJSON.JSONObject paramJSONObject, string paramString, int paramInt)
        {
            paramJSONObject.Add(paramString, paramInt);
        }

        public static void RegisterPaidEvent()
        {
            if (hasRegisteredIronSource) return;
            IronSourceEvents.onImpressionDataReadyEvent += OnImpressionDataReadyEvent;
            hasRegisteredIronSource = true;
        }

        public static void UnregisterPaidEvent()
        {
            if (!hasRegisteredIronSource) return;
            IronSourceEvents.onImpressionDataReadyEvent -= OnImpressionDataReadyEvent;
            hasRegisteredIronSource = false;
        }

        private static void OnImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
			string network = impressionData.adNetwork;
			
            double revenueValue = 0.0;

            SimpleJSON.JSONObject networkData = new SimpleJSON.JSONObject();

            PutString(networkData, "auctionId", impressionData.auctionId);
            PutString(networkData, "adUnit", impressionData.adUnit);
            PutString(networkData, "country", impressionData.country);
            PutString(networkData, "ab", impressionData.ab);
            PutString(networkData, "segmentName", impressionData.segmentName);
            PutString(networkData, "placement", impressionData.placement);
            PutString(networkData, "adNetwork", impressionData.adNetwork);
            PutString(networkData, "instanceName", impressionData.instanceName);
            PutString(networkData, "instanceId", impressionData.instanceId);
            PutString(networkData, "precision", impressionData.precision);
            PutString(networkData, "encryptedCPM", impressionData.encryptedCPM);

            double? revenue = impressionData.revenue;
            if (revenue != null)
            {
                revenueValue = revenue.Value;
                PutDouble(networkData, "revenue", revenueValue);
            }

            double? lifetimeRevenue = impressionData.lifetimeRevenue;
            if (lifetimeRevenue != null)
            {
                PutDouble(networkData, "lifetimeRevenue", lifetimeRevenue.Value);
            }

            int? conversionValue = impressionData.conversionValue;
            if (conversionValue != null)
            {
                PutInt(networkData, "conversionValue", conversionValue.Value);
            }

            SuperfineSDK.LogAdRevenue("IronSource", revenueValue, "USD", network, networkData);
        }
    }
}
