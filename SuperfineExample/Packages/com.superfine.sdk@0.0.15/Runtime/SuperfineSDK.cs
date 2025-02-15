using System.Collections.Generic;
using System;

using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

namespace Superfine.Unity
{
    public static class SuperfineSDK
    {
        private static SuperfineSDKManager instance = null;

        public const string WRAPPER = "Unity";
        public const string WRAPPER_VERSION = "0.0.15-unity";

        public static SuperfineSDKSettings.PlatformFlag GetPlatformFlag()
        {
#if UNITY_ANDROID
            return SuperfineSDKSettings.PlatformFlag.Android;
#elif UNITY_IOS
            return SuperfineSDKSettings.PlatformFlag.iOS;
#elif UNITY_STANDALONE_WIN
            return SuperfineSDKSettings.PlatformFlag.Windows;
#elif UNITY_STANDALONE_OSX
            return SuperfineSDKSettings.PlatformFlag.macOS;
#elif UNITY_STANDALONE_LINUX
            return SuperfineSDKSettings.PlatformFlag.Linux;
#else
            return SuperfineSDKSettings.PlatformFlag.None;
#endif
        }

#if UNITY_EDITOR
        public static PackageInfo GetPackageInfo(string name)
        {
            var request = Client.List();
            do { } while (!request.IsCompleted);
            if (request.Status == StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    if (package.name == name) return package;
                }
            }

            return null;
        }

        public static PackageInfo GetSelfPackageInfo()
        {
            return GetPackageInfo("com.superfine.sdk");
        }
#endif


        private static Action onStart = null;

        public static void InvokeOnStart()
        {
            if (onStart != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStart());
            }
        }

        private static Action onStop = null;

        public static void InvokeOnStop()
        {
            if (onStop != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStop());
            }
        }

        private static Action onPause = null;

        public static void InvokeOnPause()
        {
            if (onPause != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onPause());
            }
        }

        private static Action onResume = null;

        public static void InvokeOnResume()
        {
            if (onResume != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onResume());
            }
        }

        private static Action<string> onSetDeepLink = null;

        public static void InvokeOnSetDeepLink(string url)
        {
            if (onSetDeepLink != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetDeepLink(url));
            }
        }

        private static Action<string> onSetPushToken = null;

        public static void InvokeOnSetPushToken(string token)
        {
            if (onSetPushToken != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetPushToken(token));
            }
        }

        private static Action<string, string> onSendEvent = null;

        public static void InvokeOnSendEvent(string eventName, string eventData)
        {
            if (onSendEvent != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSendEvent(eventName, eventData));
            }
        }

        public static void AddStartCallback(Action callback)
        {
            bool isNew = (onStart == null);

            if (isNew)
            {
                onStart += callback;
                RegisterNativeStartCallback();
            }
            else
            {
                if (onStart.GetInvocationList().Contains(callback)) return;
                onStart += callback;
            }
        }

        public static void RemoveStartCallback(Action callback)
        {
            if (onStart == null) return;

            onStart -= callback;
            if (onStart == null)
            {
                UnregisterNativeStartCallback();
            }
        }

        public static void AddStopCallback(Action callback)
        {
            bool isNew = (onStop == null);

            if (isNew)
            {
                onStop += callback;
                RegisterNativeStopCallback();
            }
            else
            {
                if (onStop.GetInvocationList().Contains(callback)) return;
                onStop += callback;
            }
        }

        public static void RemoveStopCallback(Action callback)
        {
            if (onStop == null) return;

            onStop -= callback;
            if (onStop == null)
            {
                UnregisterNativeStopCallback();
            }
        }

        public static void AddPauseCallback(Action callback)
        {
            bool isNew = (onPause == null);

            if (isNew)
            {
                onPause += callback;
                RegisterNativePauseCallback();
            }
            else
            {
                if (onPause.GetInvocationList().Contains(callback)) return;
                onPause += callback;
            }
        }

        public static void RemovePauseCallback(Action callback)
        {
            if (onPause == null) return;

            onPause -= callback;
            if (onPause == null)
            {
                UnregisterNativePauseCallback();
            }
        }

        public static void AddResumeCallback(Action callback)
        {
            bool isNew = (onResume == null);

            if (isNew)
            {
                onResume += callback;
                RegisterNativeResumeCallback();
            }
            else
            {
                if (onResume.GetInvocationList().Contains(callback)) return;
                onResume += callback;
            }
        }

        public static void RemoveResumeCallback(Action callback)
        {
            if (onResume == null) return;

            onResume -= callback;
            if (onPause == null)
            {
                UnregisterNativeResumeCallback();
            }
        }

        public static void AddDeepLinkCallback(Action<string> callback)
        {
            bool isNew = (onSetDeepLink == null);

            if (isNew)
            {
                onSetDeepLink += callback;
                RegisterNativeDeepLinkCallback();
            }
            else
            {
                if (onSetDeepLink.GetInvocationList().Contains(callback)) return;
                onSetDeepLink += callback;
            }
        }

        public static void RemoveDeepLinkCallback(Action<string> callback)
        {
            if (onSetDeepLink == null) return;

            onSetDeepLink -= callback;
            if (onSetDeepLink == null)
            {
                UnregisterNativeDeepLinkCallback();
            }
        }

        public static void AddPushTokenCallback(Action<string> callback)
        {
            bool isNew = (onSetPushToken == null);

            if (isNew)
            {
                onSetPushToken += callback;
                RegisterNativePushTokenCallback();
            }
            else
            {
                if (onSetPushToken.GetInvocationList().Contains(callback)) return;
                onSetPushToken += callback;
            }
        }

        public static void RemovePushTokenCallback(Action<string> callback)
        {
            if (onSetPushToken == null) return;

            onSetPushToken -= callback;
            if (onSetPushToken == null)
            {
                UnregisterNativePushTokenCallback();
            }
        }

        public static void AddSendEventCallback(Action<string, string> callback)
        {
            bool isNew = (onSendEvent == null);

            if (isNew)
            {
                onSendEvent += callback;
                RegisterNativeSendEventCallback();
            }
            else
            {
                if (onSendEvent.GetInvocationList().Contains(callback)) return;
                onSendEvent += callback;
            }
        }

        public static void RemoveSendEventCallback(Action<string, string> callback)
        {
            if (onSendEvent == null) return;

            onSendEvent -= callback;
            if (onSendEvent == null)
            {
                UnregisterNativeSendEventCallback();
            }
        }

        private static void RegisterNativeStartCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativeStartCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativeStartCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativeStartCallback();
#endif
#endif
        }

        private static void UnregisterNativeStartCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativeStartCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativeStartCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativeStartCallback();
#endif
#endif
        }

        private static void RegisterNativeStopCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativeStopCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativeStopCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativeStopCallback();
#endif
#endif
        }

        private static void UnregisterNativeStopCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativeStopCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativeStopCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativeStopCallback();
#endif
#endif
        }

        private static void RegisterNativePauseCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativePauseCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativePauseCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativePauseCallback();
#endif
#endif
        }

        private static void UnregisterNativePauseCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativePauseCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativePauseCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativePauseCallback();
#endif
#endif
        }

        private static void RegisterNativeResumeCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativeResumeCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativeResumeCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativeResumeCallback();
#endif
#endif
        }

        private static void UnregisterNativeResumeCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativeResumeCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativeResumeCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativeResumeCallback();
#endif
#endif
        }

        private static void RegisterNativeDeepLinkCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativeDeepLinkCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativeDeepLinkCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativeDeepLinkCallback();
#endif
#endif
        }

        private static void UnregisterNativeDeepLinkCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativeDeepLinkCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativeDeepLinkCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativeDeepLinkCallback();
#endif
#endif
        }

        private static void RegisterNativePushTokenCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativePushTokenCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativePushTokenCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativePushTokenCallback();
#endif
#endif
        }

        private static void UnregisterNativePushTokenCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativePushTokenCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativePushTokenCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativePushTokenCallback();
#endif
#endif
        }

        private static void RegisterNativeSendEventCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.RegisterNativeSendEventCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.RegisterNativeSendEventCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.RegisterNativeSendEventCallback();
#endif
#endif
        }

        private static void UnregisterNativeSendEventCallback()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.UnregisterNativeSendEventCallback();
#elif UNITY_IOS
            SuperfineSDKManagerIos.UnregisterNativeSendEventCallback();
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.UnregisterNativeSendEventCallback();
#endif
#endif
        }

        public static Action OnCreateInstance = null;
        public static Action OnDestroyInstance = null;
        public static Action OnInitializeInstance = null;

        public static SuperfineSDKManager GetInstance()
        {
            return instance;
        }

        public static void Initialize(Action callback)
        {
            SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();
            Initialize(settings, callback);
        }

        private static bool isInitializing = false;
        private static Action onInitialized = null;

        public static void InvokeOnInitialized()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                if (instance != null)
                {
                    instance.OnInitialized(onInitialized);
                }

                onInitialized = null;
                isInitializing = false;
            });
        }

        public static void Initialize(SuperfineSDKSettings settings, Action callback)
        {
            if (instance != null)
            {
                Debug.LogError("Superfine SDK Manager already exists");
                return;
            }

            if (settings == null)
            {
                Debug.LogError("Superfine SDK Settings isn't available");
                return;
            }

            if (isInitializing)
            {
                Debug.LogError("Superfine SDK is initializing");
                return;
            }

            //Force create dispatcher
            UnityMainThreadDispatcher.Instance();

            instance = SuperfineSDKManager.Create();

            OnCreateInstance?.Invoke();

            isInitializing = true;
            onInitialized = callback;

            instance.Initialize(settings);

            OnInitializeInstance?.Invoke();
        }

        public static bool IsInitialized()
        {
            if (instance == null) return false;
            return instance.IsInitialized();
        }

        public static void Destroy()
        {
            if (instance != null)
            {
                OnDestroyInstance?.Invoke();

                instance.Destroy();
                instance = null;
            }
        }

        public static void Start()
        {
            if (instance == null) return;
            instance.Start();
        }

        public static void Stop()
        {
            if (instance == null) return;
            instance.Stop();
        }

        public static void SetOffline(bool value)
        {
            if (instance == null) return;
            instance.SetOffline(value);
        }

        public static string GetVersion()
        {
            if (instance == null) return WRAPPER_VERSION;
            return instance.GetVersion();
        }

        public static void SetConfigId(string configId)
        {
            if (instance == null) return;
            instance.SetConfigId(configId);
        }

        public static void SetCustomUserId(string customUserId)
        {
            if (instance == null) return;
            instance.SetCustomUserId(customUserId);
        }

        public static string GetAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppId();
        }

        public static string GetUserId()
        {
            if (instance == null) return string.Empty;
            return instance.GetUserId();
        }

        public static string GetSessionId()
        {
            if (instance == null) return string.Empty;
            return instance.GetSessionId();
        }

        public static string GetHost()
        {
            if (instance == null) return string.Empty;
            return instance.GetHost();
        }

        public static string GetConfigUrl()
        {
            if (instance == null) return string.Empty;
            return instance.GetConfigUrl();
        }

        public static string GetSdkConfig()
        {
            if (instance == null) return string.Empty;
            return instance.GetSdkConfig();
        }

        public static StoreType GetStoreType()
        {
            if (instance == null) return StoreType.UNKNOWN;
            return instance.GetStoreType();
        }

        public static string GetFacebookAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetFacebookAppId();
        }

        public static string GetInstagramAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetInstagramAppId();
        }

        public static string GetAppleAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleAppId();
        }

        public static string GetAppleSignInClientId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleSignInClientId();
        }

        public static string GetAppleDeveloperTeamId()
        {
            if (instance == null) return string.Empty;
            return instance.GetAppleDeveloperTeamId();
        }

        public static string GetGooglePlayGameServicesProjectId()
        {
            if (instance == null) return string.Empty;
            return instance.GetGooglePlayGameServicesProjectId();
        }

        public static string GetGooglePlayDeveloperAccountId()
        {
            if (instance == null) return string.Empty;
            return instance.GetGooglePlayDeveloperAccountId();
        }

        public static string GetLinkedInAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetLinkedInAppId();
        }

        public static string GetQQAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetQQAppId();
        }

        public static string GetWeChatAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetWeChatAppId();
        }

        public static string GetTikTokAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetTikTokAppId();
        }

        public static string GetSnapAppId()
        {
            if (instance == null) return string.Empty;
            return instance.GetSnapAppId();
        }

        public static void OpenURL(string url)
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.OpenURL(url);
#elif UNITY_IOS
            SuperfineSDKManagerIos.OpenURL(url);
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.OpenURL(url);
#endif
#endif
        }

        public static string GetDeepLinkUrl()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            return SuperfineSDKManagerAndroid.GetDeepLinkUrl();
#elif UNITY_IOS
            return SuperfineSDKManagerIos.GetDeepLinkUrl();
#elif UNITY_STANDALONE
            return SuperfineSDKManagerStandalone.GetDeepLinkUrl();
#endif
#else
            return string.Empty;
#endif
        }

        public static void SetPushToken(string pushToken)
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.SetPushToken(pushToken);
#elif UNITY_IOS
            SuperfineSDKManagerIos.SetPushToken(pushToken);
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.SetPushToken(pushToken);
#endif
#endif
        }

        public static string GetPushToken()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            return SuperfineSDKManagerAndroid.GetPushToken();
#elif UNITY_IOS
            return SuperfineSDKManagerIos.GetPushToken();
#elif UNITY_STANDALONE
            return SuperfineSDKManagerStandalone.GetPushToken();
#endif
#else
            return string.Empty;
#endif
        }

        private static bool isFetchingRemoteConfig = false;
        private static Action<SimpleJSON.JSONObject> onReceiveRemoteConfig = null;

        private static SimpleJSON.JSONObject defaultRemoteConfig = null;

        public static void SetDefaultRemoteConfig(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                if (SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node) && node.IsObject)
                {
                    SetDefaultRemoteConfig((SimpleJSON.JSONObject)node);
                }
            }
        }

        public static void SetDefaultRemoteConfig(SimpleJSON.JSONObject remoteConfig)
        {
            defaultRemoteConfig = remoteConfig;
        }

        public static void InvokeOnReceiveRemoteConfig(string data)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                if (onReceiveRemoteConfig != null)
                {
                    SimpleJSON.JSONObject remoteConfig = defaultRemoteConfig;
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (SimpleJSON.JSON.TryParse(data, out SimpleJSON.JSONNode node) && node.IsObject)
                        {
                            remoteConfig = (SimpleJSON.JSONObject)node;
                        }
                    }

                    onReceiveRemoteConfig.Invoke(remoteConfig);
                    onReceiveRemoteConfig = null;
                }

                isFetchingRemoteConfig = false;
            });
        }

        public static void FetchRemoteConfig(Action<SimpleJSON.JSONObject> callback)
        {
            if (instance == null || isFetchingRemoteConfig) return;

            isFetchingRemoteConfig = true;
            onReceiveRemoteConfig = callback;

            instance.FetchRemoteConfig();
        }

        public static void GdprForgetMe()
        {
            if (instance == null) return;
            instance.GdprForgetMe();
        }

        public static void DisableThirdPartySharing()
        {
            if (instance == null) return;
            instance.DisableThirdPartySharing();
        }

        public static void EnableThirdPartySharing()
        {
            if (instance == null) return;
            instance.EnableThirdPartySharing();
        }

        public static void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            if (instance == null) return;
            instance.LogThirdPartySharingSettings(settings);
        }

        public static void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, eventFlag);
        }

        public static void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (instance == null) return;
            instance.Log(eventName, data, eventFlag);
        }

        public static void Log(SuperfineSDKEvent eventData)
        {
            if (instance == null) return;
            instance.Log(eventData);
        }

        public static void LogCache(SuperfineSDKEvent eventData)
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
            SuperfineSDKManagerAndroid.LogCache(eventData);
#elif UNITY_IOS
            SuperfineSDKManagerIos.LogCache(eventData);
#elif UNITY_STANDALONE
            SuperfineSDKManagerStandalone.LogCache(eventData);
#endif
#endif
        }

        public static void LogBootStart()
        {
            if (instance == null) return;
            instance.LogBootStart();
        }

        public static void LogBootEnd()
        {
            if (instance == null) return;
            instance.LogBootEnd();
        }

        public static void LogLevelStart(int id, string name)
        {
            if (instance == null) return;
            instance.LogLevelStart(id, name);
        }

        public static void LogLevelEnd(int id, string name, bool isSuccess)
        {
            if (instance == null) return;
            instance.LogLevelEnd(id, name, isSuccess);
        }

        public static void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdLoad(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdClose(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdClick(adUnit, adPlacementType, adPlacement);
        }

        public static void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (instance == null) return;
            instance.LogAdImpression(adUnit, adPlacementType, adPlacement);
        }

        public static void LogIAPInitialization(bool isSuccess)
        {
            if (instance == null) return;
            instance.LogIAPInitialization(isSuccess);
        }

        public static void LogIAPRestorePurchase()
        {
            if (instance == null) return;
            instance.LogIAPRestorePurchase();
        }

        public static void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            if (instance == null) return;
            instance.LogIAPResult(pack, price, amount, currency, isSuccess);
        }

        public static void LogIAPReceipt_Apple(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Apple(receipt);
        }

        public static void LogIAPReceipt_Google(string data, string signature)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Google(data, signature);
        }

        public static void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Amazon(userId, receiptId);
        }

        public static void LogIAPReceipt_Roku(string transactionId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Roku(transactionId);
        }

        public static void LogIAPReceipt_Windows(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Windows(receipt);
        }

        public static void LogIAPReceipt_Facebook(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Facebook(receipt);
        }

        public static void LogIAPReceipt_Unity(string receipt)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_Unity(receipt);
        }

        public static void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_AppStoreServer(transactionId);
        }

        public static void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlayProduct(productId, token);
        }

        public static void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlaySubscription(subscriptionId, token);
        }

        public static void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            if (instance == null) return;
            instance.LogIAPReceipt_GooglePlaySubscriptionv2(token);
        }

        public static void LogUpdateApp(string newVersion)
        {
            if (instance == null) return;
            instance.LogUpdateApp(newVersion);
        }

        public static void LogRateApp()
        {
            if (instance == null) return;
            instance.LogRateApp();
        }

        public static void LogLocation(double latitude, double longitude)
        {
            if (instance == null) return;
            instance.LogLocation(latitude, longitude);
        }

        public static void LogFacebookLink(string userId)
        {
            if (instance == null) return;
            instance.LogFacebookLink(userId);
        }

        public static void LogFacebookUnlink()
        {
            if (instance == null) return;
            instance.LogFacebookUnlink();
        }

        public static void LogInstagramLink(string userId)
        {
            if (instance == null) return;
            instance.LogInstagramLink(userId);
        }

        public static void LogInstagramUnlink()
        {
            if (instance == null) return;
            instance.LogInstagramUnlink();
        }

        public static void LogAppleLink(string userId)
        {
            if (instance == null) return;
            instance.LogAppleLink(userId);
        }

        public static void LogAppleUnlink()
        {
            if (instance == null) return;
            instance.LogAppleUnlink();
        }

        public static void LogAppleGameCenterLink(string gamePlayerId)
        {
            if (instance == null) return;
            instance.LogAppleGameCenterLink(gamePlayerId);
        }

        public static void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            if (instance == null) return;
            instance.LogAppleGameCenterTeamLink(teamPlayerId);
        }

        public static void LogAppleGameCenterUnlink()
        {
            if (instance == null) return;
            instance.LogAppleGameCenterUnlink();
        }

        public static void LogGoogleLink(string userId)
        {
            if (instance == null) return;
            instance.LogGoogleLink(userId);
        }

        public static void LogGoogleUnlink()
        {
            if (instance == null) return;
            instance.LogGoogleUnlink();
        }

        public static void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesLink(gamePlayerId);
        }

        public static void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesDeveloperLink(developerPlayerKey);
        }

        public static void LogGooglePlayGameServicesUnlink()
        {
            if (instance == null) return;
            instance.LogGooglePlayGameServicesUnlink();
        }

        public static void LogLinkedInLink(string personId)
        {
            if (instance == null) return;
            instance.LogLinkedInLink(personId);
        }

        public static void LogLinkedInUnlink()
        {
            if (instance == null) return;
            instance.LogLinkedInUnlink();
        }

        public static void LogMeetupLink(string userId)
        {
            if (instance == null) return;
            instance.LogMeetupLink(userId);
        }

        public static void LogMeetupUnlink()
        {
            if (instance == null) return;
            instance.LogMeetupUnlink();
        }

        public static void LogGitHubLink(string userId)
        {
            if (instance == null) return;
            instance.LogGitHubLink(userId);
        }

        public static void LogGitHubUnlink()
        {
            if (instance == null) return;
            instance.LogGitHubUnlink();
        }

        public static void LogDiscordLink(string userId)
        {
            if (instance == null) return;
            instance.LogDiscordLink(userId);
        }

        public static void LogDiscordUnlink()
        {
            if (instance == null) return;
            instance.LogDiscordUnlink();
        }

        public static void LogTwitterLink(string userId)
        {
            if (instance == null) return;
            instance.LogTwitterLink(userId);
        }

        public static void LogTwitterUnlink()
        {
            if (instance == null) return;
            instance.LogTwitterUnlink();
        }

        public static void LogSpotifyLink(string userId)
        {
            if (instance == null) return;
            instance.LogSpotifyLink(userId);
        }

        public static void LogSpotifyUnlink()
        {
            if (instance == null) return;
            instance.LogSpotifyUnlink();
        }

        public static void LogMicrosoftLink(string userId)
        {
            if (instance == null) return;
            instance.LogMicrosoftLink(userId);
        }

        public static void LogMicrosoftUnlink()
        {
            if (instance == null) return;
            instance.LogMicrosoftUnlink();
        }

        public static void LogLINELink(string userId)
        {
            if (instance == null) return;
            instance.LogLINELink(userId);
        }

        public static void LogLINEUnlink()
        {
            if (instance == null) return;
            instance.LogLINEUnlink();
        }

        public static void LogVKLink(string userId)
        {
            if (instance == null) return;
            instance.LogVKLink(userId);
        }

        public static void LogVKUnlink()
        {
            if (instance == null) return;
            instance.LogVKUnlink();
        }

        public static void LogQQLink(string openId)
        {
            if (instance == null) return;
            instance.LogQQLink(openId);
        }

        public static void LogQQUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogQQUnionLink(unionId);
        }

        public static void LogQQUnlink()
        {
            if (instance == null) return;
            instance.LogQQUnlink();
        }

        public static void LogWeChatLink(string openId)
        {
            if (instance == null) return;
            instance.LogWeChatLink(openId);
        }

        public static void LogWeChatUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogWeChatUnionLink(unionId);
        }

        public static void LogWeChatUnlink()
        {
            if (instance == null) return;
            instance.LogWeChatUnlink();
        }

        public static void LogTikTokLink(string openId)
        {
            if (instance == null) return;
            instance.LogTikTokLink(openId);
        }

        public static void LogTikTokUnionLink(string unionId)
        {
            if (instance == null) return;
            instance.LogTikTokUnionLink(unionId);
        }

        public static void LogTikTokUnlink()
        {
            if (instance == null) return;
            instance.LogTikTokUnlink();
        }

        public static void LogWeiboLink(string userId)
        {
            if (instance == null) return;
            instance.LogWeiboLink(userId);
        }

        public static void LogWeiboUnlink()
        {
            if (instance == null) return;
            instance.LogWeiboUnlink();
        }

        public static void LogAccountLink(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type);
        }

        public static void LogAccountLink(string id, string type, string scopeId)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type, scopeId);
        }

        public static void LogAccountLink(string id, string type, string scopeId, string scopeType)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type, scopeId, scopeType);
        }

        public static void LogAccountUnlink(string type)
        {
            if (instance == null) return;
            instance.LogAccountUnlink(type);
        }

        public static void AddUserPhoneNumber(int countryCode, string number)
        {
            if (instance == null) return;
            instance.AddUserPhoneNumber(countryCode, number);
        }

        public static void RemoveUserPhoneNumber(int countryCode, string number)
        {
            if (instance == null) return;
            instance.RemoveUserPhoneNumber(countryCode, number);
        }

        public static void AddUserEmail(string email)
        {
            if (instance == null) return;
            instance.AddUserEmail(email);
        }

        public static void RemoveUserEmail(string email)
        {
            if (instance == null) return;
            instance.RemoveUserEmail(email);
        }

        public static void SetUserName(string firstName, string lastName)
        {
            if (instance == null) return;
            instance.SetUserName(firstName, lastName);
        }

        public static void SetUserFirstName(string firstName)
        {
            if (instance == null) return;
            instance.SetUserFirstName(firstName);
        }

        public static void SetUserLastName(string lastName)
        {
            if (instance == null) return;
            instance.SetUserLastName(lastName);
        }

        public static void SetUserCity(string city)
        {
            if (instance == null) return;
            instance.SetUserCity(city);
        }

        public static void SetUserState(string state)
        {
            if (instance == null) return;
            instance.SetUserState(state);
        }

        public static void SetUserCountry(string country)
        {
            if (instance == null) return;
            instance.SetUserCountry(country);
        }

        public static void SetUserZipCode(string zipCode)
        {
            if (instance == null) return;
            instance.SetUserZipCode(zipCode);
        }

        public static void SetUserDateOfBirth(int day, int month, int year)
        {
            if (instance == null) return;
            instance.SetUserDateOfBirth(day, month, year);
        }

        public static void SetUserDateOfBirth(int day, int month)
        {
            if (instance == null) return;
            instance.SetUserDateOfBirth(day, month);
        }

        public static void SetUserYearOfBirth(int year)
        {
            if (instance == null) return;
            instance.SetUserYearOfBirth(year);
        }

        public static void SetUserGender(UserGender gender)
        {
            if (instance == null) return;
            instance.SetUserGender(gender);
        }

        public static void LogWalletLink(string wallet, string type = "ethereum")
        {
            if (instance == null) return;
            instance.LogWalletLink(wallet, type);
        }

        public static void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            if (instance == null) return;
            instance.LogWalletUnlink(wallet, type);
        }

        public static void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            if (instance == null) return;
            instance.LogCryptoPayment(pack, price, amount, currency, chain);
        }

        public static void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            if (instance == null) return;
            instance.LogAdRevenue(source, revenue, currency, network, networkData);
        }

        public static class Android
        {
            public static string GetIMEI()
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                if (instance == null) return string.Empty;
                return instance.GetIMEI();
#else
                return string.Empty;
#endif
            }

#if !UNITY_EDITOR && UNITY_ANDROID
            private static bool isRequestingPermissions = false;
            private static Action<AndroidPermissionResult[]> onPermissionRequestResults = null;
#endif

            public static void InvokeOnPermissionRequestResults(AndroidPermissionResult[] results)
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (onPermissionRequestResults != null)
                    {
                        onPermissionRequestResults.Invoke(results);
                        onPermissionRequestResults = null;
                    }

                    isRequestingPermissions = false;
                });
#endif
            }

            public static void RequestPermission(string permission, Action<AndroidPermissionResult> callback = null)
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                if (isRequestingPermissions) return;

                SuperfineSDKAndroidPermissionRequest request = new SuperfineSDKAndroidPermissionRequest(permission);

                Action<AndroidPermissionResult[]> finalCallback = null;

                if (callback != null)
                {
                    finalCallback = (AndroidPermissionResult[] results) =>
                    {
                        callback.Invoke(results[0]);
                    };
                }

                RequestPermissions(request, finalCallback);
#endif
            }

            public static void RequestPermissions(string[] permissions, Action<AndroidPermissionResult[]> callback = null)
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                if (isRequestingPermissions) return;

                if (permissions == null || permissions.Length == 0) return;

                SuperfineSDKAndroidPermissionRequest request = new SuperfineSDKAndroidPermissionRequest(permissions);

                RequestPermissions(request, callback);
#endif
            }

            public static void RequestPermissions(SuperfineSDKAndroidPermissionRequest request, Action<AndroidPermissionResult[]> callback = null)
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                if (isRequestingPermissions) return;

                if (request == null || request.permissions == null || request.permissions.Length == 0) return;

                isRequestingPermissions = true;
                onPermissionRequestResults = callback;

                SuperfineSDKManagerAndroid.RequestPermissions(request);
#endif
            }
        }

        public static class iOS
        {
            public static void LogTrackingAuthorizationStatus(IosTrackingAuthorizationStatus status)
            {
#if !UNITY_EDITOR && UNITY_IOS
                if (instance == null) return;
                instance.LogTrackingAuthorizationStatus(status);
#endif
            }

#if !UNITY_EDITOR && UNITY_IOS
            private static bool isRequestingTrackingAuthorization = false;
            private static Action<IosTrackingAuthorizationStatus> onResponseTrackingAuthorization = null;

            private static bool isRequestingNotificationAuthorization = false;
            private static Action<bool, SuperfineSDKIosError> onResponseNotificationAuthorization = null;

            private static bool isRegisteringForRemoteNotifications = false;
            private static Action<SuperfineSDKIosError> onRegisterForRemoteNotifications = null;
#endif

            public static void InvokeOnResponseTrackingAuthorization(IosTrackingAuthorizationStatus status)
            {
#if !UNITY_EDITOR && UNITY_IOS
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (onResponseTrackingAuthorization != null)
                    {
                        onResponseTrackingAuthorization.Invoke(status);
                        onResponseTrackingAuthorization = null;
                    }

                    isRequestingTrackingAuthorization = false;
                });
#endif
            }
            public static void InvokeOnResponseNotificationAuthorization(bool granted, string errorString)
            {
#if !UNITY_EDITOR && UNITY_IOS
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SuperfineSDKIosError error = null;
                    if (!string.IsNullOrEmpty(errorString))
                    {
                        if (SimpleJSON.JSON.TryParse(errorString, out SimpleJSON.JSONNode node) && node.IsObject)
                        {
                            error = SuperfineSDKIosError.Create((SimpleJSON.JSONObject)node);
                        }
                    }

                    if (onResponseNotificationAuthorization != null)
                    {
                        onResponseNotificationAuthorization.Invoke(granted, error);
                        onResponseNotificationAuthorization = null;
                    }

                    isRequestingNotificationAuthorization = false;
                    isRegisteringForRemoteNotifications = false;
                });
#endif
            }

            public static void InvokeOnRegisterForRemoteNotifications(string errorString)
            {
#if !UNITY_EDITOR && UNITY_IOS
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SuperfineSDKIosError error = null;
                    if (!string.IsNullOrEmpty(errorString))
                    {
                        if (SimpleJSON.JSON.TryParse(errorString, out SimpleJSON.JSONNode node) && node.IsObject)
                        {
                            error = SuperfineSDKIosError.Create((SimpleJSON.JSONObject)node);
                        }
                    }

                    if (onRegisterForRemoteNotifications != null)
                    {
                        onRegisterForRemoteNotifications.Invoke(error);
                        onRegisterForRemoteNotifications = null;
                    }

                    isRegisteringForRemoteNotifications = false;
                });
#endif
            }

            public static void RequestTrackingAuthorization(Action<IosTrackingAuthorizationStatus> callback = null)
            {
#if !UNITY_EDITOR && UNITY_IOS
                if (isRequestingTrackingAuthorization) return;

                isRequestingTrackingAuthorization = true;
                onResponseTrackingAuthorization = callback;

                SuperfineSDKManagerIos.RequestTrackingAuthorization();
#endif
            }

            public static IosTrackingAuthorizationStatus GetTrackingAuthorizationStatus()
            {
#if !UNITY_EDITOR && UNITY_IOS
                return SuperfineSDKManagerIos.GetTrackingAuthorizationStatus();
#else
                return IosTrackingAuthorizationStatus.NOT_DETERMINED;
#endif
            }

            public static void RequestNotificationAuthorization(Action<bool, SuperfineSDKIosError> callback, IosNotificationAuthorizationOptions options)
            {
#if !UNITY_EDITOR && UNITY_IOS
                if (isRequestingNotificationAuthorization) return;

                isRequestingNotificationAuthorization = true;
                onResponseNotificationAuthorization = callback;

                SuperfineSDKManagerIos.RequestNotificationAuthorization(options);
#endif
            }

            public static void RequestNotificationAuthorization(Action<bool, SuperfineSDKIosError> callback, IosNotificationAuthorizationOptions options, bool registerRemote)
            {
#if !UNITY_EDITOR && UNITY_IOS
                if (isRequestingNotificationAuthorization || isRegisteringForRemoteNotifications) return;

                isRequestingNotificationAuthorization = true;
                isRegisteringForRemoteNotifications = true;

                onResponseNotificationAuthorization = callback;

                SuperfineSDKManagerIos.RequestNotificationAuthorization(options, registerRemote);
#endif
            }

            public static void RegisterForRemoteNotifications(Action<SuperfineSDKIosError> callback)
            {
#if !UNITY_EDITOR && UNITY_IOS
                if (isRegisteringForRemoteNotifications) return;

                isRegisteringForRemoteNotifications = true;
                onRegisterForRemoteNotifications = callback;

                SuperfineSDKManagerIos.RegisterForRemoteNotifications();
#endif
            }

            public static void UnregisterForRemoteNotifications()
            {
#if !UNITY_EDITOR && UNITY_IOS
                SuperfineSDKManagerIos.UnregisterForRemoteNotifications();
#endif
            }

            public static bool IsRegisteredForRemoteNotifications()
            {
#if !UNITY_EDITOR && UNITY_IOS
                return SuperfineSDKManagerIos.IsRegisteredForRemoteNotifications();
#else
                return false;
#endif
            }

            public static void RegisterAppForAdNetworkAttribution()
            {
#if !UNITY_EDITOR && UNITY_IOS
                SuperfineSDKManagerIos.RegisterAppForAdNetworkAttribution();
#endif
            }

            public static void UpdatePostbackConversionValue(int conversionValue)
            {
#if !UNITY_EDITOR && UNITY_IOS
                SuperfineSDKManagerIos.UpdatePostbackConversionValue(conversionValue);
#endif
            }

            public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
            {
#if !UNITY_EDITOR && UNITY_IOS
                SuperfineSDKManagerIos.UpdatePostbackConversionValue(conversionValue, coarseValue);
#endif
            }

            public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
            {
#if !UNITY_EDITOR && UNITY_IOS
                SuperfineSDKManagerIos.UpdatePostbackConversionValue(conversionValue, coarseValue, lockWindow);
#endif
            }
        }

        public static class Standalone
        {
            public static void SetAdvertisingId(string advertisingId)
            {
#if !UNITY_EDITOR && UNITY_STANDALONE
                if (instance == null) return;
                instance.SetAdvertisingId(advertisingId);
#endif
            }

            public static void SetSteamDRMCheck(Func<uint, int> func)
            {
#if !UNITY_EDITOR && UNITY_STANDALONE
                SuperfineSDKManagerStandalone.SetSteamDRMCheck(func);
#endif
            }

            public static int GetSteamDRMError()
            {
#if !UNITY_EDITOR && UNITY_STANDALONE
                return SuperfineSDKManagerStandalone.GetSteamDRMError();
#else
                return 0;
#endif
            }

            public static void SetSaveSteamActivationLink(bool value)
            {
#if !UNITY_EDITOR && UNITY_STANDALONE
                SuperfineSDKManagerStandalone.SetSaveSteamActivationLink(value);
#endif
            }
        }
    }
}