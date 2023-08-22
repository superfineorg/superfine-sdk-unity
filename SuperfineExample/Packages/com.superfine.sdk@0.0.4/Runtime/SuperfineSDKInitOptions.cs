﻿namespace Superfine.Unity
{
    public class SuperfineSDKInitOptions
    {
        public int flushQueueSize = 5;
        public long flushInterval = 15 * 1000; //ms

        public string configId = string.Empty;
        public bool waitConfigId = false;

        public string customUserId = string.Empty;

        public bool autoStart = true;

        public bool captureDeepLinks = true;

#if UNITY_EDITOR
        public StoreType storeType = StoreType.UNKNOWN;
#elif UNITY_ANDROID
        public StoreType storeType = StoreType.GOOGLE_PLAY;
#elif UNITY_IOS
        public StoreType storeType = StoreType.APP_STORE;
#else
        public StoreType storeType = StoreType.UNKNOWN;
#endif

#if UNITY_ANDROID
        public LogLevel logLevel = LogLevel.NONE;

        public bool enableImei = false;
        public bool enableOaid = false;
#elif UNITY_IOS
        public bool debug = false;
        public bool captureInAppPurchases = false;
#elif UNITY_STANDALONE
        public LogLevel logLevel = LogLevel.NONE;

        public bool registerURIScheme = false;

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        public string bundleId = string.Empty;
        public string buildNumber = "1";
#endif

#if UNITY_STANDALONE_LINUX
        public string desktopPath = "~/.local/share/applications";
        public string updateDesktopDatabase = "update-desktop-database";
#endif

        public string proxy = string.Empty;
        public string caPath = string.Empty;
        public bool sslVerify = true;
#endif

#if (UNITY_ANDROID || UNITY_IOS)
        public SuperfineSDKTenjinInitOptions tenjin = null;
        
        public void SetTenjinSdkKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (tenjin == null)
                {
                    tenjin = new SuperfineSDKTenjinInitOptions();
                }

                tenjin.sdkKey = key;
            }
        }
#endif
    }

#if (UNITY_ANDROID || UNITY_IOS)
    public class SuperfineSDKTenjinInitOptions
    {
        public string sdkKey = string.Empty;
        public string[] optInParams = null;
        public string[] optOutParams = null;

        public int appSubversion = 0;

        public bool cacheEventSetting = false;
    }
#endif
}