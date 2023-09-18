#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerAndroid : SuperfineSDKManagerBase
    {
        private const string JavaClassName = "com.superfine.sdk.unity.SuperfineSDKUnityPlugin";

        class SendEventCallback : AndroidJavaProxy
        {
            public SendEventCallback() : base("com.superfine.sdk.SendEventCallback") { }

            public void onExecute(string eventName, string eventData)
            {
                if (onSendEvent != null)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => onSendEvent(eventName, eventData));
                }
            }
        }

        private SendEventCallback sendEventCallback = null;

        private AndroidJavaClass javaClass = null; 

        protected SuperfineSDKManagerAndroid(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) : base(appId, appSecret, host, options)
        {
            javaClass = new AndroidJavaClass(JavaClassName);
            Init(GetString(appId, appSecret, host, options));

            sendEventCallback = new SendEventCallback();
        }

        ~SuperfineSDKManagerAndroid()
        {
            Stop();
        }

        private void Init(string arguments)
        {
            CallStatic("Init", arguments);
        }

        private T CallStatic<T>(string methodName)
        {
            return javaClass.CallStatic<T>(methodName);
        }

        private void CallStatic(string methodName, params object[] args)
        {
            javaClass.CallStatic(methodName, args);
        }

        public override void Start()
        {
             javaClass.CallStatic("Start");
        }

        public override void Stop()
        {
             javaClass.CallStatic("Stop");
        }

        public override string GetVersion()
        {
            return javaClass.CallStatic<string>("GetVersion");
        }

        public override void SetConfigId(string configId)
        {
            javaClass.CallStatic("SetConfigId", configId);
        }

        public override void SetCustomUserId(string customUserId)
        {
            javaClass.CallStatic("SetCustomUserId", customUserId);
        }

        public override void SetAdvertisingId(string advertisingId)
        {
            throw new System.Exception("SDK doesn't support setting advertising id");
        }

        public override string GetUserId()
        {
            return javaClass.CallStatic<string>("GetUserId");
        }

        protected override void RegisterNativeSendEventCallback()
        {
            javaClass.CallStatic("SetSendEventCallback", sendEventCallback);
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            javaClass.CallStatic("SetSendEventCallback", null);
        }

        public override void GdprForgetMe()
        {
            javaClass.CallStatic("GdprForgetMe");
        }

        public override void DisableThirdPartySharing()
        {
            javaClass.CallStatic("DisableThirdPartySharing");
        }

        public override void EnableThirdPartySharing()
        {
            javaClass.CallStatic("EnableThirdPartySharing");
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            javaClass.CallStatic("LogThirdPartySharing", GetString(settings));
        }

        public override void Log(string eventName, int data)
        {
            javaClass.CallStatic("LogWithIntValue", eventName, data);
        }

        public override void Log(string eventName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                javaClass.CallStatic("Log", eventName);
            }
            else
            {
                javaClass.CallStatic("LogWithStringValue", eventName, data);
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data = null)
        {
            if (data == null)
            {
                javaClass.CallStatic("Log", eventName);
            }
            else
            {
                javaClass.CallStatic("LogWithMapValue", eventName, GetMapString(data));
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data = null)
        {
            if (data == null)
            {
                javaClass.CallStatic("Log", eventName);
            }
            else
            {
                javaClass.CallStatic("LogWithJsonValue", eventName, data.ToString());
            }
        }

        public override void LogBootStart()
        {
            javaClass.CallStatic("LogBootStart");
        }

        public override void LogBootEnd()
        {
            javaClass.CallStatic("LogBootEnd");
        }

        public override void LogLevelStart(int id, string name)
        {
            javaClass.CallStatic("LogLevelStart", id, name);
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            javaClass.CallStatic("LogLevelEnd", id, name, isSuccess);
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdLoad", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdClose", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdClick", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("LogAdImpression", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            javaClass.CallStatic("LogIAPInitialization", isSuccess);
        }

        public override void LogIAPRestorePurchase()
        {
            javaClass.CallStatic("LogIAPRestorePurchase");
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            javaClass.CallStatic("LogIAPResult", pack, price, amount, currency, isSuccess);
        }

        public override void LogAndroidIAPBuy(string pack, double price, int amount, string currency, string data, string signature)
        {
            javaClass.CallStatic("LogIAPBuy", pack, price, amount, currency, data, signature);
        }

        public override void LogFacebookLogin(string facebookId)
        {
            javaClass.CallStatic("LogFacebookLogin", facebookId);
        }

        public override void LogFacebookLogout(string facebookId)
        {
            javaClass.CallStatic("LogFacebookLogout", facebookId);
        }

        public override void LogUpdateGame(string newVersion)
        {
            javaClass.CallStatic("LogUpdateGame", newVersion);
        }

        public override void LogRateGame()
        {
            javaClass.CallStatic("LogRateGame");
        }

        public override void LogLocation(double latitude, double longitude)
        {
            javaClass.CallStatic("LogLocation", latitude, longitude);
        }

        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            javaClass.CallStatic("LogAuthorizationTrackingStatus", status.ToString());
        }

        public override void LogAccountLogin(string id, string type)
        {
            javaClass.CallStatic("LogAccountLogin", id, type);
        }

        public override void LogAccountLogout(string id, string type)
        {
            javaClass.CallStatic("LogAccountLogout", id, type);
        }

        public override void LogAccountLink(string id, string type)
        {
            javaClass.CallStatic("LogAccountLink", id, type);
        }

        public override void LogAccountUnlink(string id, string type)
        {
            javaClass.CallStatic("LogAccountUnlink", id, type);
        }

        public override void LogWalletLink(string wallet, string type = null)
        {
            javaClass.CallStatic("LogWalletLink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogWalletUnlink(string wallet, string type = null)
        {
            javaClass.CallStatic("LogWalletUnlink", wallet, type == null ? "ethereum" : type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            javaClass.CallStatic("LogCryptoPayment", pack, price, amount, currency, chain == null ? "ethereum" : chain);
        }

        public override void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)
        {
            javaClass.CallStatic("LogAdRevenue", network, revenue, currency, mediation == null ? "" : mediation, networkData == null ? "" : networkData.ToString());
        }

        public override void OpenURL(string url)
        {
            javaClass.CallStatic("OpenURL", url);
        }

        public override void SetPushToken(string pushToken)
        {
            javaClass.CallStatic("SetPushToken", pushToken);
        }
    }
}
#endif