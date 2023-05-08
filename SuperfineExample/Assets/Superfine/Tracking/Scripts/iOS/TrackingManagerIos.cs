#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;

namespace Superfine.Tracking
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
            }

            Init(arguments);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingInit(string arguments);
        private void Init(MethodArguments arguments)
        {
            SuperfineTrackingInit(arguments.ToJsonString());
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
        private static extern void SuperfineTrackingTrack(string eventName);
        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWithValue(string eventName, string value);
        public override void Track(string eventName, TrackBaseData data = null)
        {
            if (data == null)
            {
                SuperfineTrackingTrack(eventName);
            }
            else
            {
                SuperfineTrackingTrackWithValue(eventName, JsonConvert.SerializeObject(data));
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
        private static extern void SuperfineTrackingTrackIAPBuyStart(string pack, string price, float amount, string currency);
        public override void TrackIAPBuyStart(string pack, string price, float amount, string currency)
        {
            SuperfineTrackingTrackIAPBuyStart(pack, price, amount, currency);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackIAPBuyEnd(string pack, string price, float amount, string currency, bool isSuccess);
        public override void TrackIAPBuyEnd(string pack, string price, float amount, string currency, bool isSuccess)
        {
            SuperfineTrackingTrackIAPBuyEnd(pack, price, amount, currency, isSuccess);
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
        private static extern void SuperfineTrackingTrackRateGame(int storeType);
        public override void TrackRateGame(StoreType storeType)
        {
            SuperfineTrackingTrackRateGame((int)storeType);
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
            SuperfineTrackingTrackWalletLink(wallet, type == null ? "eth" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackWalletUnlink(string wallet, string type);
        public override void TrackWalletUnlink(string wallet, string type = null)
        {
            SuperfineTrackingTrackWalletUnlink(wallet, type == null ? "eth" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineTrackingTrackCryptoPayment(string pack, string price, float amount, string currency, string type, int count);
        public override void TrackCryptoPayment(string pack, string price, float amount, string currency, string type = "web3", int count = 1)
        {
            SuperfineTrackingTrackCryptoPayment(pack, price, amount, currency, type == null ? "web3" : type, count);
        }
    }
}
#endif