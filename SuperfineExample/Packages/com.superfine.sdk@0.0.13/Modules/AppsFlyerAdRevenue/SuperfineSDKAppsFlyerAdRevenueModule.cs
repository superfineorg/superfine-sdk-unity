using AppsFlyerSDK;

using System.Collections.Generic;
using System;

namespace Superfine.Unity
{
    public class SuperfineSDKAppsFlyerAdRevenueModule : SuperfineSDKModule
    {
        private const string AD_REVENUE = "gjsdk_ad_revenue";

        private static readonly string[] MEDIATION_NETWORK_NAMES = new string[]
        {
            "admob", "ironsource", "applovin", "fyber", "appodeal", "admost", "topon", "tradplus", "yandex", "chartboost", "unity"
        };

        private static readonly AppsFlyerAdRevenueMediationNetworkType[] MEDIATION_NETWORKS = new AppsFlyerAdRevenueMediationNetworkType[]
        {
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeFyber,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeAppodeal,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeAdmost,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeTopon,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeTradplus,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeYandex,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeChartBoost,
            AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeUnity
        };

        private static Dictionary<string, string> ConvertObjectToMap(SimpleJSON.JSONObject jsonObject)
        {
            if (jsonObject == null) return null;

            Dictionary<string, string> map = new Dictionary<string, string>();

            foreach (KeyValuePair<string, SimpleJSON.JSONNode> pair in jsonObject)
            {
                map.Add(pair.Key, pair.Value.ToString());
            }

            return map;
        }

        private static AppsFlyerAdRevenueMediationNetworkType GetMediationNetworkFromSource(string source)
        {
            int pos = Array.IndexOf(MEDIATION_NETWORK_NAMES, source.ToLower());

            if (pos == -1) return AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeCustomMediation;
            else return MEDIATION_NETWORKS[pos];
        }

        public SuperfineSDKAppsFlyerAdRevenueModule(SuperfineSDKModuleSettings settings) : base(settings)
        {
        }

        protected override void Initialize(SuperfineSDKModuleSettings baseSettings)
        {
            base.Initialize(baseSettings);

            SuperfineSDKAppsFlyerAdRevenueSettings settings = baseSettings as SuperfineSDKAppsFlyerAdRevenueSettings;

            bool autoStart = settings ? settings.autoStart : true;
            bool debug = settings ? settings.debug : false;

            SuperfineSDK.AddSendEventCallback(OnSendEvent);

            if (autoStart)
            {
                AppsFlyerAdRevenue.start();
                AppsFlyerAdRevenue.setIsDebug(debug);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            SuperfineSDK.RemoveSendEventCallback(OnSendEvent);
        }

        private void OnSendEvent(string eventName, string eventData)
        {
            if (eventName != AD_REVENUE)
            {
                return;
            }

            if (!SimpleJSON.JSON.TryParse(eventData, out SimpleJSON.JSONNode node)) return;
            if (!node.IsObject) return;

            SimpleJSON.JSONObject adRevenueData = (SimpleJSON.JSONObject)node;

            if (!adRevenueData.TryGetValue("source", out node)) return;
            if (!node.IsString) return;
            string source = node.Value;

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

            AppsFlyerAdRevenueMediationNetworkType mediationNetwork = AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeCustomMediation;

            if (string.IsNullOrEmpty(network))
            {
                network = source;
                mediationNetwork = AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypedirectMonetization;
            }
            else
            {
                mediationNetwork = GetMediationNetworkFromSource(source);
            }

            AppsFlyerAdRevenue.logAdRevenue(network, mediationNetwork, revenue, currency, ConvertObjectToMap(networkData));
        }
    }
}
