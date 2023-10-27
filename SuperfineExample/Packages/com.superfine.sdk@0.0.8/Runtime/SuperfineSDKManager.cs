using UnityEngine;

namespace Superfine.Unity
{
    public delegate void RequestAuthorizationTrackingCompleteHandler(AuthorizationTrackingStatus status);

    public class SuperfineSDKManager :
#if UNITY_EDITOR
        SuperfineSDKManagerStub
#elif UNITY_ANDROID
        SuperfineSDKManagerAndroid
#elif UNITY_IPHONE || UNITY_IOS
        SuperfineSDKManagerIos
#elif UNITY_STANDALONE
        SuperfineSDKManagerStandalone
#else
        SuperfineSDKManagerStub
#endif
    {
        public static readonly string VERSION = "0.0.8-unity";

        public static SuperfineSDKManager CreateInstance(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null)
        {
            return new SuperfineSDKManager(appId, appSecret, host, options);
        }

        protected SuperfineSDKManager(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) : base(appId, appSecret, host, options)
        {
            //Force create dispatcher
            UnityMainThreadDispatcher.Instance();
        }
    }
}
