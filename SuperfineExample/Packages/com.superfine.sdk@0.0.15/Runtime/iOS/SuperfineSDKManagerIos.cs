#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerIos : SuperfineSDKManagerBase
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InitializationDelegate();
        [MonoPInvokeCallback(typeof(InitializationDelegate))]
        private static void InvokeInitializationCallback()
        {
            SuperfineSDK.InvokeOnInitialized();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void StartDelegate();
        [MonoPInvokeCallback(typeof(StartDelegate))]
        private static void InvokeStartCallback()
        {
            SuperfineSDK.InvokeOnStart();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void StopDelegate();
        [MonoPInvokeCallback(typeof(StopDelegate))]
        private static void InvokeStopCallback()
        {
            SuperfineSDK.InvokeOnStop();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PauseDelegate();
        [MonoPInvokeCallback(typeof(PauseDelegate))]
        private static void InvokePauseCallback()
        {
            SuperfineSDK.InvokeOnPause();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ResumeDelegate();
        [MonoPInvokeCallback(typeof(ResumeDelegate))]
        private static void InvokeResumeCallback()
        {
            SuperfineSDK.InvokeOnResume();
        }
                
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DeepLinkDelegate(string url);
        [MonoPInvokeCallback(typeof(DeepLinkDelegate))]
        private static void InvokeDeepLinkCallback(string url)
        {
            SuperfineSDK.InvokeOnSetDeepLink(url);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PushTokenDelegate(string token);
        [MonoPInvokeCallback(typeof(PushTokenDelegate))]
        private static void InvokePushTokenCallback(string token)
        {
            SuperfineSDK.InvokeOnSetPushToken(token);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SendEventDelegate(string eventName, string eventData);
        [MonoPInvokeCallback(typeof(SendEventDelegate))]
        private static void InvokeSendEventCallback(string eventName, string eventData)
        {
            SuperfineSDK.InvokeOnSendEvent(eventName, eventData);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RemoteConfigDelegate(string data);
        [MonoPInvokeCallback(typeof(RemoteConfigDelegate))]
        private static void InvokeRemoteConfigCallback(string data)
        {
            SuperfineSDK.InvokeOnReceiveRemoteConfig(data);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RequestTrackingAuthorizationDelegate(int status);
        [MonoPInvokeCallback(typeof(RequestTrackingAuthorizationDelegate))]
        private static void InvokeRequestTrackingAuthorizationCallback(int status)
        {
            SuperfineSDK.iOS.InvokeOnResponseTrackingAuthorization((IosTrackingAuthorizationStatus)status);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RequestNotificationAuthorizationDelegate(bool granted, string error);
        [MonoPInvokeCallback(typeof(RequestNotificationAuthorizationDelegate))]
        private static void InvokeRequestNotificationAuthorizationCallback(bool granted, string error)
        {
            SuperfineSDK.iOS.InvokeOnResponseNotificationAuthorization(granted, error);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RegisterForRemoteNotificationsDelegate(string error);
        [MonoPInvokeCallback(typeof(RegisterForRemoteNotificationsDelegate))]
        private static void InvokeRegisterForRemoteNotificationsCallback(string error)
        {
            SuperfineSDK.iOS.InvokeOnRegisterForRemoteNotifications(error);
        }

        private static readonly InitializationDelegate initializationHandler = InvokeInitializationCallback;
        private static readonly IntPtr initializationHandlerPointer = Marshal.GetFunctionPointerForDelegate(initializationHandler);

        private static readonly StartDelegate startHandler = InvokeStartCallback;
        private static readonly IntPtr startHandlerPointer = Marshal.GetFunctionPointerForDelegate(startHandler);

        private static readonly StopDelegate stopHandler = InvokeStopCallback;
        private static readonly IntPtr stopHandlerPointer = Marshal.GetFunctionPointerForDelegate(stopHandler);

        private static readonly PauseDelegate pauseHandler = InvokePauseCallback;
        private static readonly IntPtr pauseHandlerPointer = Marshal.GetFunctionPointerForDelegate(pauseHandler);

        private static readonly ResumeDelegate resumeHandler = InvokeResumeCallback;
        private static readonly IntPtr resumeHandlerPointer = Marshal.GetFunctionPointerForDelegate(resumeHandler);

        private static readonly DeepLinkDelegate deepLinkHandler = InvokeDeepLinkCallback;
        private static readonly IntPtr deepLinkHandlerPointer = Marshal.GetFunctionPointerForDelegate(deepLinkHandler);

        private static readonly PushTokenDelegate pushTokenHandler = InvokePushTokenCallback;
        private static readonly IntPtr pushTokenHandlerPointer = Marshal.GetFunctionPointerForDelegate(pushTokenHandler);

        private static readonly SendEventDelegate sendEventHandler = InvokeSendEventCallback;
        private static readonly IntPtr sendEventHandlerPointer = Marshal.GetFunctionPointerForDelegate(sendEventHandler);

        private static readonly RemoteConfigDelegate remoteConfigHandler = InvokeRemoteConfigCallback;
        private static readonly IntPtr remoteConfigHandlerPointer = Marshal.GetFunctionPointerForDelegate(remoteConfigHandler);

        private static readonly RequestTrackingAuthorizationDelegate requestTrackingAuthorizationHandler = InvokeRequestTrackingAuthorizationCallback;
        private static readonly IntPtr requestTrackingAuthorizationHandlerPointer = Marshal.GetFunctionPointerForDelegate(requestTrackingAuthorizationHandler);

        private static readonly RequestNotificationAuthorizationDelegate requestNotificationAuthorizationHandler = InvokeRequestNotificationAuthorizationCallback;
        private static readonly IntPtr requestNotificationAuthorizationHandlerPointer = Marshal.GetFunctionPointerForDelegate(requestNotificationAuthorizationHandler);

        private static readonly RegisterForRemoteNotificationsDelegate registerForRemoteNotificationsHandler = InvokeRegisterForRemoteNotificationsCallback;
        private static readonly IntPtr registerForRemoteNotificationsHandlerPointer = Marshal.GetFunctionPointerForDelegate(registerForRemoteNotificationsHandler);

        [DllImport("__Internal")]
        private static extern void SuperfineSDKInitialize(string arguments);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKInitialize2(string arguments, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        protected override void InitializeNative(SuperfineSDKSettings settings, List<string> moduleNameList)
        {
            SuperfineSDKInitialize2(CreateInitializationJSONObject(settings, moduleNameList).ToString(), initializationHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKShutdown();
        public override void Destroy()
        {
            SuperfineSDKShutdown();
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKIsInitialized();
        public override bool IsInitialized()
        {
            return SuperfineSDKIsInitialized() != 0;
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKStart();
        public override void Start()
        {
            SuperfineSDKStart();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKStop();
        public override void Stop()
        {
            SuperfineSDKStop();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetOffline(bool value);
        public override void SetOffline(bool value)
        {
            SuperfineSDKSetOffline(value);
        }
                
        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetVersion();
        public override string GetVersion()
        {
            return SuperfineSDKGetVersion();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetConfigId(string configId);
        public override void SetConfigId(string configId)
        {
            SuperfineSDKSetConfigId(configId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetCustomUserId(string userId);
        public override void SetCustomUserId(string customUserId)
        {
            SuperfineSDKSetCustomUserId(customUserId);
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetAppId();
        public override string GetAppId()
        {
            return SuperfineSDKGetAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetUserId();
        public override string GetUserId()
        {
            return SuperfineSDKGetUserId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetSessionId();
        public override string GetSessionId()
        {
            return SuperfineSDKGetSessionId();
        }
        
        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetHost();
        public override string GetHost()
        {
            return SuperfineSDKGetHost();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetConfigUrl();
        public override string GetConfigUrl()
        {
            return SuperfineSDKGetConfigUrl();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetSdkConfig();
        public override string GetSdkConfig()
        {
            return SuperfineSDKGetSdkConfig();
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKGetStoreType();
        public override StoreType GetStoreType()
        {
            return (StoreType)SuperfineSDKGetStoreType();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineGetFacebookAppId();
        public override string GetFacebookAppId()
        {
            return SuperfineGetFacebookAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineGetInstagramAppId();
        public override string GetInstagramAppId()
        {
            return SuperfineGetInstagramAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetAppleAppId();
        public override string GetAppleAppId()
        {
            return SuperfineSDKGetAppleAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetAppleSignInClientId();
        public override string GetAppleSignInClientId()
        {
            return SuperfineSDKGetAppleSignInClientId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetAppleDeveloperTeamId();
        public override string GetAppleDeveloperTeamId()
        {
            return SuperfineSDKGetAppleDeveloperTeamId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetGooglePlayGameServicesProjectId();
        public override string GetGooglePlayGameServicesProjectId()
        {
            return SuperfineSDKGetGooglePlayGameServicesProjectId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetGooglePlayDeveloperAccountId();
        public override string GetGooglePlayDeveloperAccountId()
        {
            return SuperfineSDKGetGooglePlayDeveloperAccountId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetLinkedInAppId();
        public override string GetLinkedInAppId()
        {
            return SuperfineSDKGetLinkedInAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetQQAppId();
        public override string GetQQAppId()
        {
            return SuperfineSDKGetQQAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetWeChatAppId();
        public override string GetWeChatAppId()
        {
            return SuperfineSDKGetWeChatAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetTikTokAppId();
        public override string GetTikTokAppId()
        {
            return SuperfineSDKGetTikTokAppId();
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetSnapAppId();
        public override string GetSnapAppId()
        {
            return SuperfineSDKGetSnapAppId();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKOpenURL(string url);
        public static void OpenURL(string url)
        {
            SuperfineSDKOpenURL(url);
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetDeepLinkUrl();
        public static string GetDeepLinkUrl()
        {
            return SuperfineSDKGetDeepLinkUrl();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetDeviceToken(string token);
        public static void SetPushToken(string token)
        {
            SuperfineSDKSetDeviceToken(token);
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetDeviceToken();
        public static string GetPushToken()
        {
            return SuperfineSDKGetDeviceToken();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKFetchRemoteConfig([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public override void FetchRemoteConfig()
        {
           SuperfineSDKFetchRemoteConfig(remoteConfigHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddStartCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativeStartCallback()
        {
            SuperfineSDKAddStartCallback(startHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveStartCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativeStartCallback()
        {
            SuperfineSDKRemoveStartCallback(startHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddStopCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativeStopCallback()
        {
            SuperfineSDKAddStopCallback(stopHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveStopCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativeStopCallback()
        {
            SuperfineSDKRemoveStopCallback(stopHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddPauseCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativePauseCallback()
        {
            SuperfineSDKAddPauseCallback(pauseHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemovePauseCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativePauseCallback()
        {
            SuperfineSDKRemovePauseCallback(pauseHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddResumeCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativeResumeCallback()
        {
            SuperfineSDKAddResumeCallback(resumeHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveResumeCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativeResumeCallback()
        {
            SuperfineSDKRemoveResumeCallback(resumeHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddDeepLinkCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativeDeepLinkCallback()
        {
            SuperfineSDKAddDeepLinkCallback(deepLinkHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveDeepLinkCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativeDeepLinkCallback()
        {
            SuperfineSDKRemoveDeepLinkCallback(deepLinkHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddDeviceTokenCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativePushTokenCallback()
        {
            SuperfineSDKAddDeviceTokenCallback(pushTokenHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveDeviceTokenCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativePushTokenCallback()
        {
            SuperfineSDKRemoveDeviceTokenCallback(pushTokenHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddSendEventCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterNativeSendEventCallback()
        {
            SuperfineSDKAddSendEventCallback(sendEventHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveSendEventCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void UnregisterNativeSendEventCallback()
        {
            SuperfineSDKRemoveSendEventCallback(sendEventHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKGdprForgetMe();
        public override void GdprForgetMe()
        {
            SuperfineSDKGdprForgetMe();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKDisableThirdPartySharing();
        public override void DisableThirdPartySharing()
        {
            SuperfineSDKDisableThirdPartySharing();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKEnableThirdPartySharing();
        public override void EnableThirdPartySharing()
        {
            SuperfineSDKEnableThirdPartySharing();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogThirdPartySharing(string arguments);
        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            SuperfineSDKLogThirdPartySharing(GetString(settings));
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLog(string eventName);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithFlag(string eventName, int flag);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithIntValue(string eventName, int value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithIntValueAndFlag(string eventName, int value, int flag);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithStringValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithStringValueAndFlag(string eventName, string value, int flag);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithMapValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithMapValueAndFlag(string eventName, string value, int flag);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithJsonValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithJsonValueAndFlag(string eventName, string value, int flag);

        public override void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            SuperfineSDKLogWithFlag(eventName, (int)eventFlag);
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            SuperfineSDKLogWithIntValueAndFlag(eventName, data, (int)eventFlag);
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (string.IsNullOrEmpty(data))
            {
                SuperfineSDKLogWithFlag(eventName, (int)eventFlag);
            }
            else
            {
                SuperfineSDKLogWithStringValueAndFlag(eventName, data, (int)eventFlag);
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (data == null)
            {
                SuperfineSDKLogWithFlag(eventName, (int)eventFlag);
            }
            else
            {
                SuperfineSDKLogWithMapValueAndFlag(eventName, GetMapString(data), (int)eventFlag);
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            SuperfineSDKLogWithJsonValueAndFlag(eventName, data == null ? null : data.ToString(), (int)eventFlag);
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKBeginLogEvent(string eventName);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventIntValue(int eventRef, int value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventStringValue(int eventRef, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventMapValue(int eventRef, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventJsonValue(int eventRef, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventFlag(int eventRef, int flag);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetLogEventRevenue(int eventRef, double revenue, string currency);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKEndLogEvent(int eventRef, bool cache);

        private static int CreateNativeLogEventRef(SuperfineSDKEvent eventData)
        {
            int eventRef = SuperfineSDKBeginLogEvent(eventData.eventName);
            if (eventRef <= 0) return 0;

            SuperfineSDKEvent.ValueType valueType = eventData.valueType;
            switch (valueType)
            {
                case SuperfineSDKEvent.ValueType.INT:
                    {
                        int data = (int)eventData.value;
                        SuperfineSDKSetLogEventIntValue(eventRef, data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.STRING:
                    {
                        string data = (string)eventData.value;
                        SuperfineSDKSetLogEventStringValue(eventRef, data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.MAP:
                    {
                        Dictionary<string, string> data = (Dictionary<string, string>)eventData.value;
                        SuperfineSDKSetLogEventMapValue(eventRef, GetMapString(data));
                    }
                    break;

                case SuperfineSDKEvent.ValueType.JSON:
                    {
                        SimpleJSON.JSONObject data = (SimpleJSON.JSONObject)eventData.value;
                        SuperfineSDKSetLogEventJsonValue(eventRef, data.ToString());
                    }
                    break;

                default:
                    break;
            }

            EventFlag eventFlag = eventData.eventFlag;
            if (eventFlag != EventFlag.NONE)
            {
                SuperfineSDKSetLogEventFlag(eventRef, (int)eventFlag);
            }

            if (eventData.HasRevenue())
            {
                SuperfineSDKSetLogEventRevenue(eventRef, eventData.revenue, eventData.currency);
            }

            return eventRef;
        }

        public override void Log(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;

            int eventRef = CreateNativeLogEventRef(eventData);
            if (eventRef <= 0) return;

            SuperfineSDKEndLogEvent(eventRef, false);
        }

        public static void LogCache(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;
            
            int eventRef = CreateNativeLogEventRef(eventData);
            if (eventRef <= 0) return;
              
            SuperfineSDKEndLogEvent(eventRef, true);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogBootStart();
        public override void LogBootStart()
        {
            SuperfineSDKLogBootStart();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogBootEnd();
        public override void LogBootEnd()
        {
            SuperfineSDKLogBootEnd();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLevelStart(int levelId, string name);
        public override void LogLevelStart(int id, string name)
        {
            SuperfineSDKLogLevelStart(id, name);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLevelEnd(int levelId, string name, bool isSuccess);
        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            SuperfineSDKLogLevelEnd(id, name, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAdLoad(string adUnit, int adPlacementType, int adPlacement);
        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineSDKLogAdLoad(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAdClose(string adUnit, int adPlacementType, int adPlacement);
        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineSDKLogAdClose(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAdClick(string adUnit, int adPlacementType, int adPlacement);
        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineSDKLogAdClick(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAdImpression(string adUnit, int adPlacementType, int adPlacement);
        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            SuperfineSDKLogAdImpression(adUnit, (int)adPlacementType, (int)adPlacement);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPInitialization(bool isSuccess);
        public override void LogIAPInitialization(bool isSuccess)
        {
            SuperfineSDKLogIAPInitialization(isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPRestorePurchase();
        public override void LogIAPRestorePurchase()
        {
            SuperfineSDKLogIAPRestorePurchase();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPResult(string pack, double price, int amount, string currency, bool isSuccess);
        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            SuperfineSDKLogIAPResult(pack, price, amount, currency, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Apple(string receipt);
        public override void LogIAPReceipt_Apple(string receipt)
        {
            SuperfineSDKLogIAPReceipt_Apple(receipt);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Google(string data, string signature);
        public override void LogIAPReceipt_Google(string data, string signature)
        {
            SuperfineSDKLogIAPReceipt_Google(data, signature);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Amazon(string userId, string receiptId);
        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            SuperfineSDKLogIAPReceipt_Amazon(userId, receiptId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Roku(string transactionId);
        public override void LogIAPReceipt_Roku(string transactionId)
        {
            SuperfineSDKLogIAPReceipt_Roku(transactionId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Windows(string receipt);
        public override void LogIAPReceipt_Windows(string receipt)
        {
            SuperfineSDKLogIAPReceipt_Windows(receipt);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Facebook(string receipt);
        public override void LogIAPReceipt_Facebook(string receipt)
        {
            SuperfineSDKLogIAPReceipt_Facebook(receipt);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_Unity(string receipt);
        public override void LogIAPReceipt_Unity(string receipt)
        {
            SuperfineSDKLogIAPReceipt_Unity(receipt);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_AppStoreServer(string transactionId);
        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            SuperfineSDKLogIAPReceipt_AppStoreServer(transactionId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_GooglePlayProduct(string productId, string token);
        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            SuperfineSDKLogIAPReceipt_GooglePlayProduct(productId, token);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token);
        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            SuperfineSDKLogIAPReceipt_GooglePlaySubscription(subscriptionId, token);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPReceipt_GooglePlaySubscriptionv2(string token);
        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            SuperfineSDKLogIAPReceipt_GooglePlaySubscriptionv2(token);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogUpdateApp(string newVersion);
        public override void LogUpdateApp(string newVersion)
        {
            SuperfineSDKLogUpdateApp(newVersion);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogRateApp();
        public override void LogRateApp()
        {
            SuperfineSDKLogRateApp();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLocation(double latitude, double longitude);
        public override void LogLocation(double latitude, double longitude)
        {
            SuperfineSDKLogLocation(latitude, longitude);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogFacebookLink(string userId);
        public override void LogFacebookLink(string userId)
        {
            SuperfineSDKLogFacebookLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogFacebookUnlink();
        public override void LogFacebookUnlink()
        {
            SuperfineSDKLogFacebookUnlink();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogInstagramLink(string userId);
        public override void LogInstagramLink(string userId)
        {
            SuperfineSDKLogInstagramLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogInstagramUnlink();
        public override void LogInstagramUnlink()
        {
            SuperfineSDKLogInstagramUnlink();
        }
        
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAppleLink(string userId);
        public override void LogAppleLink(string userId)
        {
            SuperfineSDKLogAppleLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAppleUnlink();
        public override void LogAppleUnlink()
        {
            SuperfineSDKLogAppleUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAppleGameCenterLink(string gamePlayerId);
        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            SuperfineSDKLogAppleGameCenterLink(gamePlayerId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAppleGameCenterTeamLink(string teamPlayerId);
        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            SuperfineSDKLogAppleGameCenterTeamLink(teamPlayerId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAppleGameCenterUnlink();
        public override void LogAppleGameCenterUnlink()
        {
            SuperfineSDKLogAppleGameCenterUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGoogleLink(string userId);
        public override void LogGoogleLink(string userId)
        {
            SuperfineSDKLogGoogleLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGoogleUnlink();
        public override void LogGoogleUnlink()
        {
            SuperfineSDKLogGoogleUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGooglePlayGameServicesLink(string gamePlayerId);
        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            SuperfineSDKLogGooglePlayGameServicesLink(gamePlayerId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGooglePlayGameServicesDeveloperLink(string developerPlayerKey);
        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            SuperfineSDKLogGooglePlayGameServicesDeveloperLink(developerPlayerKey);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGooglePlayGameServicesUnlink();
        public override void LogGooglePlayGameServicesUnlink()
        {
            SuperfineSDKLogGooglePlayGameServicesUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLinkedInLink(string personId);
        public override void LogLinkedInLink(string personId)
        {
            SuperfineSDKLogLinkedInLink(personId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLinkedInUnlink();
        public override void LogLinkedInUnlink()
        {
            SuperfineSDKLogLinkedInUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogMeetupLink(string userId);
        public override void LogMeetupLink(string userId)
        {
            SuperfineSDKLogMeetupLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogMeetupUnlink();
        public override void LogMeetupUnlink()
        {
            SuperfineSDKLogMeetupUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGitHubLink(string userId);
        public override void LogGitHubLink(string userId)
        {
            SuperfineSDKLogGitHubLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogGitHubUnlink();
        public override void LogGitHubUnlink()
        {
            SuperfineSDKLogGitHubUnlink();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogDiscordLink(string userId);
        public override void LogDiscordLink(string userId)
        {
            SuperfineSDKLogDiscordLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogDiscordUnlink();
        public override void LogDiscordUnlink()
        {
            SuperfineSDKLogDiscordUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTwitterLink(string userId);
        public override void LogTwitterLink(string userId)
        {
            SuperfineSDKLogTwitterLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTwitterUnlink();
        public override void LogTwitterUnlink()
        {
            SuperfineSDKLogTwitterUnlink();
        }
        
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogSpotifyLink(string userId);
        public override void LogSpotifyLink(string userId)
        {
            SuperfineSDKLogSpotifyLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogSpotifyUnlink();
        public override void LogSpotifyUnlink()
        {
            SuperfineSDKLogSpotifyUnlink();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogMicrosoftLink(string userId);
        public override void LogMicrosoftLink(string userId)
        {
            SuperfineSDKLogMicrosoftLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogMicrosoftUnlink();
        public override void LogMicrosoftUnlink()
        {
            SuperfineSDKLogMicrosoftUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLINELink(string userId);
        public override void LogLINELink(string userId)
        {
            SuperfineSDKLogLINELink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogLINEUnlink();
        public override void LogLINEUnlink()
        {
            SuperfineSDKLogLINEUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogVKLink(string userId);
        public override void LogVKLink(string userId)
        {
            SuperfineSDKLogVKLink(userId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogVKUnlink();
        public override void LogVKUnlink()
        {
            SuperfineSDKLogVKUnlink();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogQQLink(string openId);
        public override void LogQQLink(string openId)
        {
            SuperfineSDKLogQQLink(openId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogQQUnionLink(string unionId);
        public override void LogQQUnionLink(string unionId)
        {
            SuperfineSDKLogQQUnionLink(unionId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogQQUnlink();
        public override void LogQQUnlink()
        {
            SuperfineSDKLogQQUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWeChatLink(string openId);
        public override void LogWeChatLink(string openId)
        {
            SuperfineSDKLogWeChatLink(openId);
        }
        
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWeChatUnionLink(string unionId);
        public override void LogWeChatUnionLink(string unionId)
        {
            SuperfineSDKLogWeChatUnionLink(unionId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWeChatUnlink();
        public override void LogWeChatUnlink()
        {
            SuperfineSDKLogWeChatUnlink();
        }
                
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTikTokLink(string openId);
        public override void LogTikTokLink(string openId)
        {
            SuperfineSDKLogTikTokLink(openId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTikTokUnionLink(string unionId);
        public override void LogTikTokUnionLink(string unionId)
        {
            LogTikTokUnionLink(unionId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTikTokUnlink();
        public override void LogTikTokUnlink()
        {
            SuperfineSDKLogTikTokUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWeiboLink(string userId);
        public override void LogWeiboLink(string userId)
        {
            SuperfineSDKLogWeiboLink(userId);
        }
        
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWeiboUnlink();
        public override void LogWeiboUnlink()
        {
            SuperfineSDKLogWeiboUnlink();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLink(string accountId, string type);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLink2(string accountId, string type, string scopeId);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLink3(string accountId, string type, string scopeId, string scopeType);
        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
            if (string.IsNullOrEmpty(scopeId))
            {
                SuperfineSDKLogAccountLink(id, type);
            }
            else if (string.IsNullOrEmpty(scopeType))
            {
                SuperfineSDKLogAccountLink2(id, type, scopeId);
            }
            else
            {
                SuperfineSDKLogAccountLink3(id, type, scopeId, scopeType);
            }
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountUnlink(string type);
        public override void LogAccountUnlink(string type)
        {
            SuperfineSDKLogAccountUnlink(type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddUserPhoneNumber(int countryCode, string number);
        public override void AddUserPhoneNumber(int countryCode, string number)
        {
            SuperfineSDKAddUserPhoneNumber(countryCode, number);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveUserPhoneNumber(int countryCode, string number);
        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            SuperfineSDKRemoveUserPhoneNumber(countryCode, number);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKAddUserEmail(string email);
        public override void AddUserEmail(string email)
        {
            SuperfineSDKAddUserEmail(email);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRemoveUserEmail(string email);
        public override void RemoveUserEmail(string email)
        {
            SuperfineSDKRemoveUserEmail(email);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserName(string firstName, string lastName);
        public override void SetUserName(string firstName, string lastName)
        {
            SuperfineSDKSetUserName(firstName, lastName);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserFirstName(string firstName);
        public override void SetUserFirstName(string firstName)
        {
            SuperfineSDKSetUserFirstName(firstName);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserLastName(string lastName);
        public override void SetUserLastName(string lastName)
        {
            SuperfineSDKSetUserLastName(lastName);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserCity(string city);
        public override void SetUserCity(string city)
        {
            SuperfineSDKSetUserCity(city);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserState(string state);
        public override void SetUserState(string state)
        {
            SuperfineSDKSetUserState(state);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserCountry(string country);
        public override void SetUserCountry(string country)
        {
            SuperfineSDKSetUserCountry(country);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserZipCode(string zipCode);
        public override void SetUserZipCode(string zipCode)
        {
            SuperfineSDKSetUserZipCode(zipCode);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserDateOfBirth(int day, int month, int year);
        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            SuperfineSDKSetUserDateOfBirth(day, month, year);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserDateOfBirth2(int day, int month);
        public override void SetUserDateOfBirth(int day, int month)
        {
            SuperfineSDKSetUserDateOfBirth2(day, month);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserYearOfBirth(int year);
        public override void SetUserYearOfBirth(int year)
        {
            SuperfineSDKSetUserYearOfBirth(year);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetUserGender(int gender);
        public override void SetUserGender(UserGender gender)
        {
            SuperfineSDKSetUserGender((int)gender);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWalletLink(string wallet, string type);
        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            SuperfineSDKLogWalletLink(wallet, type == null ? "ethereum" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWalletUnlink(string wallet, string type);
        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            SuperfineSDKLogWalletUnlink(wallet, type == null ? "ethereum" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogCryptoPayment(string pack, double price, int amount, string currency, string chain);
        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            SuperfineSDKLogCryptoPayment(pack, price, amount, currency == null ? "ETH" : currency, chain == null ? "ethereum" : chain);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAdRevenue3(string source, double revenue, string currency, string network, string networkData);
        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            SuperfineSDKLogAdRevenue3(source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString());
        }
        
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogTrackingAuthorizationStatus(int status);
        public void LogTrackingAuthorizationStatus(IosTrackingAuthorizationStatus status)
        {
            SuperfineSDKLogTrackingAuthorizationStatus((int)status);
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKRequestTrackingAuthorization([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RequestTrackingAuthorization()
        {
            SuperfineSDKRequestTrackingAuthorization(requestTrackingAuthorizationHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKGetTrackingAuthorizationStatus();
        public static IosTrackingAuthorizationStatus GetTrackingAuthorizationStatus()
        {
            return (IosTrackingAuthorizationStatus)SuperfineSDKGetTrackingAuthorizationStatus();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRequestNotificationAuthorization([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int options);
        public static void RequestNotificationAuthorization(IosNotificationAuthorizationOptions options)
        {
            SuperfineSDKRequestNotificationAuthorization(requestNotificationAuthorizationHandlerPointer, (int)options);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRequestNotificationAuthorization2([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int options, bool registerRemote);
        public static void RequestNotificationAuthorization(IosNotificationAuthorizationOptions options, bool registerRemote)
        {
            SuperfineSDKRequestNotificationAuthorization2(requestNotificationAuthorizationHandlerPointer, (int)options, registerRemote);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRegisterForRemoteNotifications([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        public static void RegisterForRemoteNotifications()
        {
            SuperfineSDKRegisterForRemoteNotifications(registerForRemoteNotificationsHandlerPointer);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUnregisterForRemoteNotifications();
        public static void UnregisterForRemoteNotifications()
        {
            SuperfineSDKUnregisterForRemoteNotifications();
        }

        [DllImport("__Internal")]
        private static extern bool SuperfineSDKIsRegisteredForRemoteNotifications();
        public static bool IsRegisteredForRemoteNotifications()
        {
            return SuperfineSDKIsRegisteredForRemoteNotifications();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKRegisterAppForAdNetworkAttribution();
        public static void RegisterAppForAdNetworkAttribution()
        {
            SuperfineSDKRegisterAppForAdNetworkAttribution();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue(int conversionValue);
        public static void UpdatePostbackConversionValue(int conversionValue)
        {
            SuperfineSDKUpdatePostbackConversionValue(conversionValue);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue2(int conversionValue, string coarseValue);
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            SuperfineSDKUpdatePostbackConversionValue2(conversionValue, coarseValue);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue3(int conversionValue, string coarseValue, bool lockWindow);
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            SuperfineSDKUpdatePostbackConversionValue3(conversionValue, coarseValue, lockWindow);
        }
    }
}
#endif