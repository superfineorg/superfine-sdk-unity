using UnityEngine;

namespace Superfine.Tracking
{
    public class TrackingManager :
#if UNITY_EDITOR
        TrackingManagerStub
#elif UNITY_ANDROID
        TrackingManagerAndroid
#elif UNITY_IPHONE || UNITY_IOS
        TrackingManagerIos
#else
        TrackingManagerStub
#endif
    {
        private static TrackingManager _instance = null;

        //Tenjin API KEY
#if UNITY_IPHONE
        private string tenjinAPIKey = "TENJIN API KEY FOR IOS";
#else
        private string tenjinAPIKey = "TENJIN API KEY FOR ANDROID";
#endif

        private bool hasConnectTenjin = false;
        private bool hasUpdateCustomerID = false;

        public static TrackingManager GetInstance()
        {
            return _instance;
        }

        public static TrackingManager CreateInstance(string appId, string appSecret, TrackingManagerInitOptions options = null)
        {
            if (_instance == null)
            {
                _instance = new TrackingManager(appId, appSecret, options);
            }

            //Check if connect tenjin
            if (!_instance.hasConnectTenjin)
            {
                _instance.hasConnectTenjin = true;

                _instance.ConnectTenjin();
            }

            try
            {
                string spid = TrackingManager.GetInstance().GetUserId();
                if (_instance.getTenjinInstance() != null && !string.IsNullOrEmpty(spid))
                {
                    _instance.getTenjinInstance().SetCustomerUserId(spid);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("SUPERFINE exception" + e.Message);
            }

            return _instance;
        }

        public BaseTenjin getTenjinInstance()
        {
            return Tenjin.getInstance(tenjinAPIKey);
        }
        private void ConnectTenjin()
        {
#if !UNITY_EDITOR
            getTenjinInstance().Connect();
#endif
        }

        protected TrackingManager(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
        }
    }
}
