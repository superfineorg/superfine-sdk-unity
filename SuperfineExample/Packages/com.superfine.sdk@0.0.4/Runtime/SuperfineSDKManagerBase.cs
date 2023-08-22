using System;
using System.Collections.Generic;
using System.Text;

namespace Superfine.Unity
{
    public abstract class SuperfineSDKManagerBase
    {
        protected SuperfineSDKManagerBase(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) { }

        protected static string GetMapString(Dictionary<string, string> data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            bool isFirst = true;

            StringBuilder sb = new StringBuilder();

            foreach (var pair in data)
            {
                string key = pair.Key;
                if (string.IsNullOrEmpty(key)) continue;

                string value = pair.Value;
                if (string.IsNullOrEmpty(value)) continue;

                if (!isFirst) sb.Append(',');

                sb.Append(key).Append(':').Append(value);
            }

            return sb.ToString();
        }

        protected static string GetString(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null)
        {
            SimpleJSON.JSONObject jsonObject = new SimpleJSON.JSONObject
            {
                { "appId", appId },
                { "appSecret", appSecret }
            };

            if (options == null) return jsonObject;

            jsonObject.Add("flushQueueSize", options.flushQueueSize);
            jsonObject.Add("flushInterval", options.flushInterval);

            if (!string.IsNullOrEmpty(options.configId))
            {
                jsonObject.Add("configId", options.configId);
            }
            jsonObject.Add("waitConfigId", options.waitConfigId);

            if (!string.IsNullOrEmpty(options.customUserId))
            {
                jsonObject.Add("customUserId", options.customUserId);
            }

            if (!string.IsNullOrEmpty(host))
            {
                jsonObject.Add("host", host);
            }

            jsonObject.Add("autoStart", options.autoStart);

            jsonObject.Add("storeType", options.storeType.ToString());

#if !UNITY_EDITOR
#if UNITY_ANDROID
            jsonObject.Add("captureDeepLinks", false);

            jsonObject.Add("logLevel", options.logLevel.ToString());

            jsonObject.Add("enableImei", options.enableImei);
            jsonObject.Add("enableOaid", options.enableOaid);
#elif UNITY_IOS
            jsonObject.Add("debug", options.debug);
            jsonObject.Add("captureInAppPurchases", options.captureInAppPurchases);
#endif
#endif

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            SuperfineSDKTenjinInitOptions tenjinOptions = options.tenjin;
            if (tenjinOptions != null)
            {
                SimpleJSON.JSONObject tenjinJsonObject = new SimpleJSON.JSONObject
                {
                    { "sdkKey", tenjinOptions.sdkKey },
                };

                if (tenjinOptions.optInParams != null)
                {
                    tenjinJsonObject.Add("optInParams", tenjinOptions.optInParams);
                }

                if (tenjinOptions.optOutParams != null)
                {
                    tenjinJsonObject.Add("optOutParams", tenjinOptions.optOutParams);
                }

                tenjinJsonObject.Add("appSubversion", tenjinOptions.appSubversion);
                tenjinJsonObject.Add("cacheEventSetting", tenjinOptions.cacheEventSetting);

                jsonObject.Add("tenjin", tenjinJsonObject);
            }
#endif

            return jsonObject.ToString();
        }

        public abstract void Start();
        public abstract void Stop();

        public abstract string GetVersion();

        public virtual void Execute(string eventName, object param = null) { }

        public abstract void SetConfigId(string configId);
        public abstract void SetCustomUserId(string userId);
        public abstract void SetAdvertisingId(string advertisingId);

        public abstract string GetUserId();

        protected static Action<string, string> onSendEvent = null;

        public void AddSendEventCallback(Action<string, string> callback)
        {
            bool isNew = (onSendEvent == null);
            onSendEvent += callback;

            if (isNew)
            {
                RegisterNativeSendEventCallback();
            }
        }

        public void RemoveSendEventCallback(Action<string, string> callback)
        {
            if (onSendEvent == null) return;

            onSendEvent -= callback;
            if (onSendEvent == null)
            {
                UnregisterNativeSendEventCallback();
            }
        }

        protected abstract void RegisterNativeSendEventCallback();
        protected abstract void UnregisterNativeSendEventCallback();

        public abstract void Log(string eventName, int data);
        public abstract void Log(string eventName, string data);
        public abstract void Log(string eventName, Dictionary<string, string> data = null);
        public abstract void Log(string eventName, SimpleJSON.JSONObject data = null);

        public abstract void LogBootStart();
        public abstract void LogBootEnd();

        public abstract void LogLevelStart(int id, string name);
        public abstract void LogLevelEnd(int id, string name, bool isSuccess);

        public abstract void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);

        public abstract void LogIAPInitialization(bool isSuccess);
        public abstract void LogIAPRestorePurchase();
        public abstract void LogIAPBuyStart(string pack, double price, int amount, string currency);
        public abstract void LogIAPBuyEnd(string pack, double price, int amount, string currency, bool isSuccess);

        public virtual void LogAppleIAPBuyEnd(string pack, double price, int amount, string currency, string transactionId, string receipt, bool isSuccess)
        {
            LogIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        public virtual void LogGoogleIAPBuyEnd(string pack, double price, int amount, string currency, string data, string signature, bool isSuccess)
        {
            LogIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        public abstract void LogFacebookLogin(string facebookId);
        public abstract void LogFacebookLogout(string facebookId);

        public abstract void LogUpdateGame(string newVersion);
        public abstract void LogRateGame();

        public abstract void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status);

        public abstract void LogAccountLogin(string id, string type);
        public abstract void LogAccountLogout(string id, string type);
        public abstract void LogAccountLink(string id, string type);
        public abstract void LogAccountUnlink(string id, string type);

        public abstract void LogWalletLink(string wallet, string type = "ethereum");
        public abstract void LogWalletUnlink(string wallet, string type = "ethereum");

        public abstract void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum");

        public abstract void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null);

        //iOS only
        public virtual void RequestTrackingAuthorization(RequestAuthorizationTrackingCompleteHandler callback = null)
        {
            throw new System.Exception("This feature is for iOS only");
            //callback?.Invoke(AuthorizationTrackingStatus.NOT_DETERMINED);
        }

        //iOS only
        public virtual AuthorizationTrackingStatus GetTrackingAuthorizationStatus()
        {
            throw new System.Exception("This feature is for iOS only");
            //return AuthorizationTrackingStatus.NOT_DETERMINED;
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        //iOS only
        public virtual void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            throw new System.Exception("This feature is for iOS only");
        }

        public abstract void OpenURL(string url);
        public abstract void SetPushToken(string pushToken);
    }
}
