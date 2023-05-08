namespace Superfine.Tracking
{
    public abstract class TrackingManagerBase
    {
        protected TrackingManagerBase(string appId, string appSecret, TrackingManagerInitOptions options = null) {}

        public abstract string GetVersion();

        public virtual void Execute(string eventName, object param = null) {}

        public abstract void SetConfigId(string configId);
        public abstract void SetUserId(string userId);

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
        public abstract void TrackIAPBuyStart(string pack, string price, float amount, string currency);
        public abstract void TrackIAPBuyEnd(string pack, string price, float amount, string currency, bool isSuccess);

        public abstract void TrackFacebookLogin(string facebookId);
        public abstract void TrackFacebookLogout(string facebookId);

        public abstract void TrackUpdateGame(string newVersion);
        public abstract void TrackRateGame(StoreType storeType);

        protected virtual void TrackSoundMode() { }
        public abstract void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status);

        public abstract void TrackAccountLogin(string id, string type);
        public abstract void TrackAccountLogout(string id, string type);
        public abstract void TrackAccountLink(string id, string type);
        public abstract void TrackAccountUnlink(string id, string type);

        public abstract void TrackWalletLink(string wallet, string type = null);
        public abstract void TrackWalletUnlink(string wallet, string type = null);

        public abstract void TrackCryptoPayment(string pack, string price, float amount, string currency, string type = "web3", int count = 1);
    }
}
