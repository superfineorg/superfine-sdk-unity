using Newtonsoft.Json;
using UnityEngine;

namespace Superfine.Tracking
{
    public class TrackingManagerStub : TrackingManagerBase
    {
        private static string Version = "0.0.1-stub";

        protected TrackingManagerStub(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
            Debug.Log(string.Format("Init with appId = {0}, appSecret = {1}", appId, appSecret));
        }

        public override string GetVersion()
        {
            return Version;
        }

        public override void SetConfigId(string configId)
        {
            Debug.Log(string.Format("Set Config Id: {0}", configId));
        }

        public override void SetUserId(string userId)
        {
            Debug.Log(string.Format("Set User Id: {0}", userId));
        }
        public override string GetUserId()
        {
            return "USREID";
        }

        public override void Track(string eventName, TrackBaseData data = null)
        {
            if (data == null)
            {
                Debug.Log(string.Format("Track {0}", eventName));
            }
            else
            {
                Debug.Log(string.Format("Track {0}: data = {1}", eventName, JsonConvert.SerializeObject(data)));
            }
        }

        public override void TrackBootStart()
        {
            Debug.Log(string.Format("Track Boot Start"));
        }

        public override void TrackBootEnd()
        {
            Debug.Log(string.Format("Track Boot End"));
        }

        public override void TrackLevelStart(int id, string name)
        {
            Debug.Log(string.Format("Track Level Start: id = {0}, name = {1}", id, name));
        }

        public override void TrackLevelEnd(int id, string name, bool isSuccess)
        {
            Debug.Log(string.Format("Track Level End: id = {0}, name = {1}", id, name));
        }

        public override void TrackAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Track Ad Load: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void TrackAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Track Ad Close: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void TrackAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Track Ad Click: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void TrackAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Track Ad Impression: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void TrackIAPInitialization(bool isSuccess)
        {
            Debug.Log(string.Format("Track IAP Initialization: isSuccess = {0}", isSuccess));
        }

        public override void TrackIAPRestorePurchase()
        {
            Debug.Log(string.Format("Track IAP Restore Purchase"));
        }

        public override void TrackIAPBuyStart(string pack, string price, float amount, string currency)
        {
            Debug.Log(string.Format("Track IAP Buy Start: pack = {0}, price = {1], amount = {2}, currency = {3}", pack, price, amount, currency));
        }

        public override void TrackIAPBuyEnd(string pack, string price, float amount, string currency, bool isSuccess)
        {
            Debug.Log(string.Format("Track IAP Buy End: pack = {0}, price = {1], amount = {2}, currency = {3}, isSuccess = {4}", pack, price, amount, currency, isSuccess));
        }

        public override void TrackFacebookLogin(string facebookId)
        {
            Debug.Log(string.Format("Track Facebook Login: facebookId = {0}", facebookId));
        }

        public override void TrackFacebookLogout(string facebookId)
        {
            Debug.Log(string.Format("Track Facebook Logout: facebookId = {0}", facebookId));
        }

        public override void TrackUpdateGame(string newVersion)
        {
            Debug.Log(string.Format("Track Update Game: newVersion = {0}", newVersion));
        }

        public override void TrackRateGame(StoreType storeType)
        {
            Debug.Log(string.Format("Track Rate Game: storeType = {0}", storeType.ToString()));
        }

        public override void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            Debug.Log(string.Format("Track Authorization Tracking Status: status = {0}", status.ToString()));
        }

        public override void TrackAccountLogin(string id, string type)
        {
            Debug.Log(string.Format("Track Account Login: id = {0}, type = {1}", id, type));
        }

        public override void TrackAccountLogout(string id, string type)
        {
            Debug.Log(string.Format("Track Account Logout: id = {0}, type = {1}", id, type));
        }

        public override void TrackAccountLink(string id, string type)
        {
            Debug.Log(string.Format("Track Account Link: id = {0}, type = {1}", id, type));
        }

        public override void TrackAccountUnlink(string id, string type)
        {
            Debug.Log(string.Format("Track Account Unlink: id = {0}, type = {1}", id, type));
        }

        public override void TrackWalletLink(string wallet, string type = null)
        {
            Debug.Log(string.Format("Track Wallet Link: wallet = {0}, type = {1}", wallet, type));
        }

        public override void TrackWalletUnlink(string wallet, string type = null)
        {
            Debug.Log(string.Format("Track Wallet Unlink: wallet = {0}, type = {1}", wallet, type));
        }

        public override void TrackCryptoPayment(string pack, string price, float amount, string currency, string type = "web3", int count = 1)
        {
            Debug.Log(string.Format("Track Crypto Payment: pack = {0}, price = {1], amount = {2}, currency = {3}, type = {4}, count = {5}", pack, price, amount, currency, type, count));
        }
    }
}