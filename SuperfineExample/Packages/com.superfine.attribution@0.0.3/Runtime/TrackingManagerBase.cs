namespace Superfine.Tracking.Unity
{
    public abstract class TrackingManagerBase
    {
        protected TrackingManagerBase(string appId, string appSecret, TrackingManagerInitOptions options = null) { }

        public abstract void Start();

        public abstract string GetVersion();

        public virtual void Execute(string eventName, object param = null) { }

        public abstract void SetConfigId(string configId);
        public abstract void SetUserId(string userId);
        public abstract string GetUserId();

        public abstract void Track(string eventName, int data);
        public abstract void Track(string eventName, string data);
        public abstract void Track(string eventName, TrackBaseData data = null);

        protected virtual void TrackOpen() { }

        public abstract void TrackBootStart();
        public abstract void TrackBootEnd();

        public abstract void TrackLevelStart(int id, string name);
        public abstract void TrackLevelEnd(int id, string name, bool isSuccess);

        public abstract void TrackAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void TrackAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void TrackAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);
        public abstract void TrackAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);

        public abstract void TrackIAPInitialization(bool isSuccess);
        public abstract void TrackIAPRestorePurchase();
        public abstract void TrackIAPBuyStart(string pack, float price, int amount, string currency);
        public abstract void TrackIAPBuyEnd(string pack, float price, int amount, string currency, bool isSuccess);

        public virtual void TrackAppleIAPBuyEnd(string pack, float price, int amount, string currency, string transactionId, string receipt,  bool isSuccess)
        {
            TrackIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        public virtual void TrackAndroidIAPBuyEnd(string pack, float price, int amount, string currency, string data, string signature, bool isSuccess)
        {
            TrackIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        public abstract void TrackFacebookLogin(string facebookId);
        public abstract void TrackFacebookLogout(string facebookId);

        public abstract void TrackUpdateGame(string newVersion);
        public abstract void TrackRateGame();

        public abstract void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status);

        public abstract void TrackAccountLogin(string id, string type);
        public abstract void TrackAccountLogout(string id, string type);
        public abstract void TrackAccountLink(string id, string type);
        public abstract void TrackAccountUnlink(string id, string type);

        public abstract void TrackWalletLink(string wallet, string type = "ethereum");
        public abstract void TrackWalletUnlink(string wallet, string type = "ethereum");

        public abstract void TrackCryptoPayment(string pack, float price, int amount, string currency = "ETH", string chain = "ethereum");
    }
}
