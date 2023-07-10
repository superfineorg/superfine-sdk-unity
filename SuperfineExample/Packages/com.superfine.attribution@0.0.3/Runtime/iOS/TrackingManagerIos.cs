#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Tracking.Unity
{
    public class TrackingManagerIos : TrackingManagerBase
    {
        protected TrackingManagerIos(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
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

                arguments.AddString("debug", options.debug.ToString());

                arguments.AddString("captureInAppPurchases", options.captureInAppPurchases.ToString());

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

                    if (!string.IsNullOrEmpty(tenjinOptions.deepLinkUrl))
                    {
                        tenjinArguments.Add("deepLinkUrl", tenjinOptions.deepLinkUrl);
                    }

                    tenjinArguments.Add("appSubversion", tenjinOptions.appSubversion.ToString());
                    tenjinArguments.Add("cacheEventSetting", tenjinOptions.cacheEventSetting.ToString());

                    arguments.AddDictionary("tenjin", tenjinArguments);
                }
            }

            Init(arguments);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingStop();
        ~TrackingManagerIos()
        {
            SuperfineTrackingStop();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingInit(string arguments);
        private void Init(MethodArguments arguments)
        {
            SuperfineTrackingInit(arguments.ToJsonString());
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingStart();
        public override void Start()
        {
            SuperfineTrackingStart();
        }
                
        [DllImport("__Internal")]
        private static extern string SuperfineTrackingGetVersion();

        public override string GetVersion()
        {
            return SuperfineTrackingGetVersion();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingSetConfigId(string configId);
        public override void SetConfigId(string configId)
        {
            SuperfineTrackingSetConfigId(configId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingSetUserId(string userId);
        public override void SetUserId(string userId)
        {
            SuperfineTrackingSetUserId(userId);
        }

        [DllImport("__Internal")]
        private static extern string SuperfineTrackingGetUserId();

        public override string GetUserId()
        {
            return SuperfineTrackingGetUserId();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrack(string eventName);
        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWithIntValue(string eventName, int value);
        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWithStringValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWithJsonValue(string eventName, string value);

        public override void Track(string eventName, int data)
        {
            SuperfineTrackingTrackWithIntValue(eventName, data);
        }

        public override void Track(string eventName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                SuperfineTrackingTrack(eventName);
            }
            else
            {
                SuperfineTrackingTrackWithStringValue(eventName, data);
            }
        }

        public override void Track(string eventName, TrackBaseData data = null)
        {
            if (data == null)
            {
                SuperfineTrackingTrack(eventName);
            }
            else
            {
                SuperfineTrackingTrackWithJsonValue(eventName, JsonConvert.SerializeObject(data));
            }
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackBootStart();
        public override void TrackBootStart()
        {
            SuperfineTrackingTrackBootStart();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackBootEnd();
        public override void TrackBootEnd()
        {
            SuperfineTrackingTrackBootEnd();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackLevelStart(int levelId, string name);
        public override void TrackLevelStart(int id, string name)
        {
            SuperfineTrackingTrackLevelStart(id, name);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackLevelEnd(int levelId, string name, bool isSuccess);
        public override void TrackLevelEnd(int id, string name, bool isSuccess)
        {
            SuperfineTrackingTrackLevelEnd(id, name, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAdLoad(string adUnit, int adPlacementType, int adPlacement);
        public override void TrackAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineTrackingTrackAdLoad(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAdClose(string adUnit, int adPlacementType, int adPlacement);
        public override void TrackAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineTrackingTrackAdClose(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAdClick(string adUnit, int adPlacementType, int adPlacement);
        public override void TrackAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineTrackingTrackAdClick(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAdImpression(string adUnit, int adPlacementType, int adPlacement);
        public override void TrackAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineTrackingTrackAdImpression(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPInitialization(bool isSuccess);
        public override void TrackIAPInitialization(bool isSuccess)
        {
            SuperfineTrackingTrackIAPInitialization(isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPRestorePurchase();
        public override void TrackIAPRestorePurchase()
        {
            SuperfineTrackingTrackIAPRestorePurchase();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPBuyStart(string pack, float price, int amount, string currency);
        public override void TrackIAPBuyStart(string pack, float price, int amount, string currency)
        {
            SuperfineTrackingTrackIAPBuyStart(pack, price, amount, currency);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPBuyEnd(string pack, float price, int amount, string currency, bool isSuccess);
        public override void TrackIAPBuyEnd(string pack, float price, int amount, string currency, bool isSuccess)
        {
            SuperfineTrackingTrackIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPBuyEnd3(string pack, float price, int amount, string currency, string transactionId, string receipt, bool isSuccess);
        public override void TrackAppleIAPBuyEnd(string pack, float price, int amount, string currency, string transactionId, string receipt,  bool isSuccess)
        {
            SuperfineTrackingTrackIAPBuyEnd3(pack, price, amount, currency, transactionId, receipt, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackFacebookLogin(string facebookId);
        public override void TrackFacebookLogin(string facebookId)
        {
            SuperfineTrackingTrackFacebookLogin(facebookId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackFacebookLogout(string facebookId);
        public override void TrackFacebookLogout(string facebookId)
        {
            SuperfineTrackingTrackFacebookLogout(facebookId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackUpdateGame(string newVersion);
        public override void TrackUpdateGame(string newVersion)
        {
            SuperfineTrackingTrackUpdateGame(newVersion);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackRateGame();
        public override void TrackRateGame()
        {
            SuperfineTrackingTrackRateGame();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAuthorizationTrackingStatus(int status);
        public override void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            SuperfineTrackingTrackAuthorizationTrackingStatus((int)status);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAccountLogin(string accountId, string type);
        public override void TrackAccountLogin(string id, string type)
        {
            SuperfineTrackingTrackAccountLogin(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAccountLogout(string accountId, string type);
        public override void TrackAccountLogout(string id, string type)
        {
            SuperfineTrackingTrackAccountLogout(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAccountLink(string accountId, string type);
        public override void TrackAccountLink(string id, string type)
        {
            SuperfineTrackingTrackAccountLink(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackAccountUnlink(string accountId, string type);
        public override void TrackAccountUnlink(string id, string type)
        {
            SuperfineTrackingTrackAccountUnlink(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWalletLink(string wallet, string type);
        public override void TrackWalletLink(string wallet, string type = null)
        {
            SuperfineTrackingTrackWalletLink(wallet, type == null ? "ethereum" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWalletUnlink(string wallet, string type);
        public override void TrackWalletUnlink(string wallet, string type = null)
        {
            SuperfineTrackingTrackWalletUnlink(wallet, type == null ? "ethereum" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackCryptoPayment(string pack, float price, int amount, string currency, string chain);
        public override void TrackCryptoPayment(string pack, float price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            SuperfineTrackingTrackCryptoPayment(pack, price, amount, currency == null ? "ETH" : currency, chain == null ? "ethereum" : chain);
        }
    }
}
#endif