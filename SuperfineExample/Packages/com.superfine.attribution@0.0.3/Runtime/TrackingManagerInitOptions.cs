namespace Superfine.Tracking.Unity
{
    public class TrackingManagerInitOptions
    {
        public long flushInterval = 15 * 1000; //ms
        public int flushQueueSize = 5;

        public bool customUserId = false;
        public string userId = string.Empty;

        public bool waitConfigId = false;

        public bool autoStart = true;

#if UNITY_EDITOR
        public StoreType storeType = StoreType.UNKNOWN;
#elif UNITY_ANDROID
        public StoreType storeType = StoreType.GOOGLE_PLAY;
#elif UNITY_IOS
        public StoreType storeType = StoreType.APP_STORE;
#else
        public StoreType storeType = StoreType.UNKNOWN;
#endif

#if !UNITY_EDITOR
#if UNITY_ANDROID
        public LogLevel logLevel = LogLevel.NONE;
#elif UNITY_IOS
        public bool debug = false;
        public bool captureInAppPurchases = false;
#elif UNITY_STANDALONE
        public string proxy = string.Empty;
        public string caPath = string.Empty;
#endif
#endif

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        public TrackingManagerTenjinInitOptions tenjin = null;
        
        public void SetTenjinSdkKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (tenjin == null)
                {
                    tenjin = new TrackingManagerTenjinInitOptions();
                }

                tenjin.sdkKey = key;
            }
        }
#endif
    }

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
    public class TrackingManagerTenjinInitOptions
    {
        public string sdkKey = string.Empty;
        public string[] optInParams = null;
        public string[] optOutParams = null;

#if UNITY_IOS
        public string deepLinkUrl = string.Empty;
#endif

        public int appSubversion = 0;

        public bool cacheEventSetting = false;
    }
#endif
}
