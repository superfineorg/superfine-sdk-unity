using System;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerStub : SuperfineSDKManagerBase
    {
        private static string Version = "0.0.4-stub";

        protected SuperfineSDKManagerStub(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) : base(appId, appSecret, host, options)
        {
            if (string.IsNullOrEmpty(host)) host = string.Empty;
            Debug.Log(string.Format("Init with appId = {0}, appSecret = {1}, host = {2}", appId, appSecret, host));
        }

        ~SuperfineSDKManagerStub()
        {
            Stop();
        }

        public override void Start()
        {
            Debug.Log(string.Format("Start Superfine SDK Manager"));
        }

        public override void Stop()
        {
            Debug.Log(string.Format("Stop Superfine SDK Manager"));
        }

        public override string GetVersion()
        {
            return Version;
        }

        public override void SetConfigId(string configId)
        {
            Debug.Log(string.Format("Set Config Id: {0}", configId));
        }

        public override void SetCustomUserId(string customUserId)
        {
            Debug.Log(string.Format("Set Custom User Id: {0}", customUserId));
        }

        public override void SetAdvertisingId(string advertisingId)
        {
            Debug.Log(string.Format("Set Advertising Id: {0}", advertisingId));
        }

        public override string GetUserId()
        {
            return "USER_ID";
        }

        protected override void RegisterNativeSendEventCallback()
        {
            Debug.Log(string.Format("Register Native Send Event Callback"));
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            Debug.Log(string.Format("Unregister Native Send Event Callback"));
        }

        public override void Log(string eventName, int data)
        {
            Debug.Log(string.Format("Log Int {0}: data = {1}", eventName, data));
        }

        public override void Log(string eventName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Debug.Log(string.Format("Log {0}", eventName));
            }
            else
            {
                Debug.Log(string.Format("Log String {0}: data = {1}", eventName, data));
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data = null)
        {
            if (data == null)
            {
                Debug.Log(string.Format("Log {0}", eventName));
            }
            else
            {
                Debug.Log(string.Format("Log Map {0}: data = {1}", eventName, GetMapString(data)));
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data = null)
        {
            if (data == null)
            {
                Debug.Log(string.Format("Log {0}", eventName));
            }
            else
            {
                Debug.Log(string.Format("Log JSON {0}: data = {1}", eventName, data.ToString()));
            }
        }

        public override void LogBootStart()
        {
            Debug.Log(string.Format("Log Boot Start"));
        }

        public override void LogBootEnd()
        {
            Debug.Log(string.Format("Log Boot End"));
        }

        public override void LogLevelStart(int id, string name)
        {
            Debug.Log(string.Format("Log Level Start: id = {0}, name = {1}", id, name));
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            Debug.Log(string.Format("Log Level End: id = {0}, name = {1}", id, name));
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Load: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Close: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Click: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Impression: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            Debug.Log(string.Format("Log IAP Initialization: isSuccess = {0}", isSuccess));
        }

        public override void LogIAPRestorePurchase()
        {
            Debug.Log(string.Format("Log IAP Restore Purchase"));
        }

        public override void LogIAPBuyStart(string pack, double price, int amount, string currency)
        {
            Debug.Log(string.Format("Log IAP Buy Start: pack = {0}, price = {1}, amount = {2}, currency = {3}", pack, price, amount, currency));
        }

        public override void LogIAPBuyEnd(string pack, double price, int amount, string currency, bool isSuccess)
        {
            Debug.Log(string.Format("Log IAP Buy End: pack = {0}, price = {1}, amount = {2}, currency = {3}, isSuccess = {4}", pack, price, amount, currency, isSuccess));
        }

        public override void LogFacebookLogin(string facebookId)
        {
            Debug.Log(string.Format("Log Facebook Login: facebookId = {0}", facebookId));
        }

        public override void LogFacebookLogout(string facebookId)
        {
            Debug.Log(string.Format("Log Facebook Logout: facebookId = {0}", facebookId));
        }

        public override void LogUpdateGame(string newVersion)
        {
            Debug.Log(string.Format("Log Update Game: newVersion = {0}", newVersion));
        }

        public override void LogRateGame()
        {
            Debug.Log(string.Format("Log Rate Game"));
        }

        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            Debug.Log(string.Format("Log Authorization Tracking Status: status = {0}", status.ToString()));
        }

        public override void LogAccountLogin(string id, string type)
        {
            Debug.Log(string.Format("Log Account Login: id = {0}, type = {1}", id, type));
        }

        public override void LogAccountLogout(string id, string type)
        {
            Debug.Log(string.Format("Log Account Logout: id = {0}, type = {1}", id, type));
        }

        public override void LogAccountLink(string id, string type)
        {
            Debug.Log(string.Format("Log Account Link: id = {0}, type = {1}", id, type));
        }

        public override void LogAccountUnlink(string id, string type)
        {
            Debug.Log(string.Format("Log Account Unlink: id = {0}, type = {1}", id, type));
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            Debug.Log(string.Format("Log Wallet Link: wallet = {0}, type = {1}", wallet, type));
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            Debug.Log(string.Format("Log Wallet Unlink: wallet = {0}, type = {1}", wallet, type));
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            Debug.Log(string.Format("Log Crypto Payment: pack = {0}, price = {1}, amount = {2}, currency = {3}, chain = {4}", pack, price, amount, currency, chain));
        }

        public override void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)
        {
            Debug.Log(string.Format("Log Ad Revenue: network = {0}, revenue = {1}, currency = {2}, mediation = {3}, networkData = {4}", network, revenue, currency, mediation == null ? "" : mediation, networkData == null ? "" : networkData.ToString()));
        }

        public override void OpenURL(string url)
        {
            Debug.Log(string.Format("Open URL: {0}", url));
        }

        public override void SetPushToken(string pushToken)
        {
            Debug.Log(string.Format("Set Push Token: {0}", pushToken));
        }
    }
}