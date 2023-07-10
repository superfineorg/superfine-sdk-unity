using UnityEngine;

namespace Superfine.Tracking.Unity
{
    public class TrackingManager :
#if UNITY_EDITOR
        TrackingManagerStub
#elif UNITY_ANDROID
        TrackingManagerAndroid
#elif UNITY_IPHONE || UNITY_IOS
        TrackingManagerIos
#elif UNITY_STANDALONE
        TrackingManagerStandalone
#else
        TrackingManagerStub
#endif
    {
        private static TrackingManager _instance = null;

        public static TrackingManager GetInstance()
        {
            return _instance;
        }

        public static TrackingManager CreateInstance(TrackingManagerInitOptions options = null)
        {
            SuperfineSettings settings = Resources.Load<SuperfineSettings>("SuperfineSettings");
            if (settings == null) return null;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            string tenjinSdkKey = null;

#if UNITY_ANDROID
            tenjinSdkKey = settings.tenjinAPIKeyAndroid.Trim();
#elif UNITY_IOS
            tenjinSdkKey = settings.tenjinAPIKeyIOS.Trim();
#endif

            if (!string.IsNullOrEmpty(tenjinSdkKey))
            {
                if (options == null)
                {
                    options = new TrackingManagerInitOptions();
                }

                options.SetTenjinSdkKey(tenjinSdkKey);
            }
#endif

            return CreateInstance(settings.appId, settings.appSecret, options);
        }

        public static TrackingManager CreateInstance(string appId, string appSecret, TrackingManagerInitOptions options = null)
        {
            if (_instance == null)
            {
                _instance = new TrackingManager(appId, appSecret, options);
            }

            return _instance;
        }

        protected TrackingManager(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
        }
    }
}
