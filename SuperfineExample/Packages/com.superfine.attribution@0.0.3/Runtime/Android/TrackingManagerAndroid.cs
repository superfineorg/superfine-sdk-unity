#if UNITY_ANDROID && !UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Tracking.Unity
{
    public class TrackingManagerAndroid : TrackingManagerBase
    {
        private const string JavaClassName = "com.superfine.tracking.unity.TrackingUnityPlugin";

        private AndroidJavaClass javaClass = null; 

        protected TrackingManagerAndroid(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
            javaClass = new AndroidJavaClass(JavaClassName);

            MethodArguments arguments = new MethodArguments();
            arguments.AddString("appId", appId);
            arguments.AddString("appSecret", appSecret);

            if (options != null)
            {
                arguments.AddString("flushInterval", options.flushInterval.ToString());
                arguments.AddString("flushQueueSize", options.flushQueueSize.ToString());

                arguments.AddString("customUserId", options.customUserId.ToString());
                arguments.AddString("userId", options.userId);

                arguments.AddString("waitConfigId", options.waitConfigId.ToString());

                arguments.AddString("logLevel", options.logLevel.ToString());

                arguments.AddString("storeType", options.storeType.ToString());
                arguments.AddString("autoStart", options.autoStart.ToString());

                TrackingManagerTenjinInitOptions tenjinOptions = options.tenjin;
                if (tenjinOptions != null)
                {
                    Dictionary<string, object> tenjinArguments = new Dictionary<string, object>();
                    tenjinArguments.Add("sdkKey", tenjinOptions.sdkKey);

                    if (tenjinOptions.optInParams != null)
                    {
                        tenjinArguments.Add("optInParams", tenjinOptions.optInParams);
                    }

                    if (tenjinOptions.optOutParams != null)
                    {
                        tenjinArguments.Add("optOutParams", tenjinOptions.optOutParams);
                    }

                    tenjinArguments.Add("appSubversion", tenjinOptions.appSubversion.ToString());
                    tenjinArguments.Add("cacheEventSetting", tenjinOptions.cacheEventSetting.ToString());

                    arguments.AddDictionary("tenjin", tenjinArguments);
                }
            }

            Init(arguments);
        }

        ~TrackingManagerAndroid()
        {
            javaClass.CallStatic("Stop");
        }

        private void Init(MethodArguments arguments)
        {
            CallStatic("Init", arguments.ToJsonString());
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

        public override string GetVersion()
        {
            return javaClass.CallStatic<string>("GetVersion");
        }

        public override void SetConfigId(string configId)
        {
            javaClass.CallStatic("SetConfigId", configId);
        }

        public override void SetUserId(string userId)
        {
            javaClass.CallStatic("SetUserId", userId);
        }

        public override string GetUserId()
        {
            return javaClass.CallStatic<string>("GetUserId");
        }

        public override void Track(string eventName, int data)
        {
            javaClass.CallStatic("TrackWithIntValue", eventName, data);
        }

        public override void Track(string eventName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                javaClass.CallStatic("Track", eventName);
            }
            else
            {
                javaClass.CallStatic("TrackWithStringValue", eventName, data);
            }
        }

        public override void Track(string eventName, TrackBaseData data = null)
        {
            if (data == null)
            {
                javaClass.CallStatic("Track", eventName);
            }
            else
            {
                javaClass.CallStatic("TrackWithJsonValue", eventName, JsonConvert.SerializeObject(data));
            }
        }

        public override void TrackBootStart()
        {
            javaClass.CallStatic("TrackBootStart");
        }

        public override void TrackBootEnd()
        {
            javaClass.CallStatic("TrackBootEnd");
        }

        public override void TrackLevelStart(int id, string name)
        {
            javaClass.CallStatic("TrackLevelStart", id, name);
        }

        public override void TrackLevelEnd(int id, string name, bool isSuccess)
        {
            javaClass.CallStatic("TrackLevelEnd", id, name, isSuccess);
        }

        public override void TrackAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("TrackAdLoad", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void TrackAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("TrackAdClose", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void TrackAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("TrackAdClick", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void TrackAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            javaClass.CallStatic("TrackAdImpression", adUnit, adPlacementType.ToString(), adPlacement.ToString());
        }

        public override void TrackIAPInitialization(bool isSuccess)
        {
            javaClass.CallStatic("TrackIAPInitialization", isSuccess);
        }

        public override void TrackIAPRestorePurchase()
        {
            javaClass.CallStatic("TrackIAPRestorePurchase");
        }

        public override void TrackIAPBuyStart(string pack, float price, int amount, string currency)
        {
            javaClass.CallStatic("TrackIAPBuyStart", pack, price, amount, currency);
        }

        public override void TrackIAPBuyEnd(string pack, float price, int amount, string currency, bool isSuccess)
        {
            javaClass.CallStatic("TrackIAPBuyEnd", pack, price, amount, currency, isSuccess);
        }

         public override void TrackAndroidIAPBuyEnd(string pack, float price, int amount, string currency, string data, string signature, bool isSuccess)
        {
            javaClass.CallStatic("TrackIAPBuyEnd", pack, price, amount, currency, data, signature, isSuccess);
        }

        public override void TrackFacebookLogin(string facebookId)
        {
            javaClass.CallStatic("TrackFacebookLogin", facebookId);
        }

        public override void TrackFacebookLogout(string facebookId)
        {
            javaClass.CallStatic("TrackFacebookLogout", facebookId);
        }

        public override void TrackUpdateGame(string newVersion)
        {
            javaClass.CallStatic("TrackUpdateGame", newVersion);
        }

        public override void TrackRateGame()
        {
            javaClass.CallStatic("TrackRateGame");
        }

        public override void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            javaClass.CallStatic("TrackAuthorizationTrackingStatus", status.ToString());
        }

        public override void TrackAccountLogin(string id, string type)
        {
            javaClass.CallStatic("TrackAccountLogin", id, type);
        }

        public override void TrackAccountLogout(string id, string type)
        {
            javaClass.CallStatic("TrackAccountLogout", id, type);
        }

        public override void TrackAccountLink(string id, string type)
        {
            javaClass.CallStatic("TrackAccountLink", id, type);
        }

        public override void TrackAccountUnlink(string id, string type)
        {
            javaClass.CallStatic("TrackAccountUnlink", id, type);
        }

        public override void TrackWalletLink(string wallet, string type = null)
        {
            javaClass.CallStatic("TrackWalletLink", wallet, type == null ? "ethereum" : type);
        }

        public override void TrackWalletUnlink(string wallet, string type = null)
        {
            javaClass.CallStatic("TrackWalletUnlink", wallet, type == null ? "ethereum" : type);
        }

        public override void TrackCryptoPayment(string pack, float price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            javaClass.CallStatic("TrackCryptoPayment", pack, price, amount, currency, chain == null ? "ethereum" : chain);
        }
    }
}
#endif