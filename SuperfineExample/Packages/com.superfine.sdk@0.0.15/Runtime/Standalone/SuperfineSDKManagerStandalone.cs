#if UNITY_STANDALONE && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerStandalone : SuperfineSDKManagerBase
    {
#if (UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR
        private const string pluginName = "__Internal";
#else
        private const string pluginName = "superfine-sdk-cpp";
#endif

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void InitializationDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(InitializationDelegate))]
        private static void InvokeInitializationCallback(int requestCode)
        {
             SuperfineSDK.InvokeOnInitialized();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void StartDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(StartDelegate))]
        private static void InvokeStartCallback(int requestCode)
        {
            SuperfineSDK.InvokeOnStart();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void StopDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(StopDelegate))]
        private static void InvokeStopCallback(int requestCode)
        {
            SuperfineSDK.InvokeOnStop();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PauseDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(PauseDelegate))]
        private static void InvokePauseCallback(int requestCode)
        {
            SuperfineSDK.InvokeOnPause();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ResumeDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(ResumeDelegate))]
        private static void InvokeResumeCallback(int requestCode)
        {
            SuperfineSDK.InvokeOnResume();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DeepLinkDelegate(string url, int requestCode);
        [MonoPInvokeCallback(typeof(DeepLinkDelegate))]
        private static void InvokeDeepLinkCallback(string url, int requestCode)
        {
            SuperfineSDK.InvokeOnSetDeepLink(url);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PushTokenDelegate(string token, int requestCode);
        [MonoPInvokeCallback(typeof(PushTokenDelegate))]
        private static void InvokePushTokenCallback(string token, int requestCode)
        {
            SuperfineSDK.InvokeOnSetPushToken(token);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SendEventDelegate(string eventName, string eventData, int requestCode);
        [MonoPInvokeCallback(typeof(SendEventDelegate))]
        private static void InvokeSendEventCallback(string eventName, string eventData, int requestCode)
        {
            SuperfineSDK.InvokeOnSendEvent(eventName, eventData);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RemoteConfigDelegate(string data, int requestCode);
        [MonoPInvokeCallback(typeof(RemoteConfigDelegate))]
        private static void InvokeRemoteConfigCallback(string data, int requestCode)
        {
            SuperfineSDK.InvokeOnReceiveRemoteConfig(data);
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

        private static Func<uint, int> steamDRMCheckFunc = null;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int SteamDRMCheckDelegate(uint appId);

        [MonoPInvokeCallback(typeof(SteamDRMCheckDelegate))]
        private static int SteamDRMCheckWrapper(uint appId)
        {
            if (steamDRMCheckFunc != null)
            {
                return steamDRMCheckFunc.Invoke(appId);
            }

            return 0;
        }

        private static readonly SteamDRMCheckDelegate steamDRMCheckHandler = SteamDRMCheckWrapper;
        private static readonly IntPtr steamDRMCheckPointer = Marshal.GetFunctionPointerForDelegate(steamDRMCheckHandler);

        #region Interface
        [DllImport(pluginName)]
        private static extern IntPtr InitializationData_Create(string appId, string appSecret);
        [DllImport(pluginName)]
        private static extern void InitializationData_Destroy(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetLogLevel(IntPtr handle, int level);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSavePath(IntPtr handle, string savePath);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetRegisterURIScheme(IntPtr handle, int registerURIScheme);
        [DllImport(pluginName)]
        private static extern void InitializationData_AddURIScheme(IntPtr handle, string uriScheme);
        [DllImport(pluginName)]
        private static extern void InitializationData_AddModule(IntPtr handle, string moduleName, string moduleParameters);
        [DllImport(pluginName)]
        private static extern void InitializationData_AddExcludedModuleName(IntPtr handle, string moduleName);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSteamBuild(IntPtr handle, uint steamAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_UnsetSteamBuild(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomUserId(IntPtr handle, string customUserId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetHost(IntPtr handle, string host);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetConfigUrl(IntPtr handle, string configUrl);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetOffline(IntPtr handle, int offline);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendInBackground(IntPtr handle, int sendInBackground);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetFacebookAppId(IntPtr handle, string facebookAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetInstagramAppId(IntPtr handle, string instagramAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppleAppId(IntPtr handle, string appleAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppleSignInClientId(IntPtr handle, string appleSignInClientId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppleDeveloperTeamId(IntPtr handle, string appleDeveloperTeamId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetGooglePlayGameServicesProjectId(IntPtr handle, string googlePlayGameServicesProjectId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetGooglePlayDeveloperAccountId(IntPtr handle, string googlePlayDeveloperAccountId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetLinkedInAppId(IntPtr handle, string linkedInAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetQQAppId(IntPtr handle, string qqAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetWeChatAppId(IntPtr handle, string weChatAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetTikTokAppId(IntPtr handle, string tikTokAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSnapAppId(IntPtr handle, string snapAppId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetWrapper(IntPtr handle, string wrapper);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetWrapperVersion(IntPtr handle, string wrapperVersion);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppName(IntPtr handle, string appName);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppVersion(IntPtr handle, string appVersion);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppBuildNumber(IntPtr handle, string appBuildNumber);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetBundleId(IntPtr handle, string bundleId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetStoreType(IntPtr handle, StoreType storeType);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetMaxRetries(IntPtr handle, int maxRetries);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetInitialRetryInterval(IntPtr handle, int initialRetryInterval);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetFlushQueueSize(IntPtr handle, int flushQueueSize);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetFlushSendSize(IntPtr handle, int flushSendSize);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetQueueCheckInterval(IntPtr handle, int queueCheckInterval);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendDelay(IntPtr handle, int sendDelay);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAutoStart(IntPtr handle, int autoStart);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetEnableSave(IntPtr handle, int enableSave);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetDisableOnlineSdkConfig(IntPtr handle, int disableOnlineSdkConfig);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetEnableCoppa(IntPtr handle, int enableCoppa);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomUserId(IntPtr handle, int customUserId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomConfigId(IntPtr handle, int customConfigId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendProxy(IntPtr handle, string proxy);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendProxyCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSendSSLVerify(IntPtr handle, int enable);

#if UNITY_STANDALONE_LINUX
        [DllImport(pluginName)]
        private static extern void InitializationData_SetDesktopTemplatePath(IntPtr handle, string templatePath);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetDesktopPath(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetDesktopFilename(IntPtr handle, string filename);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetUpdateDesktopDatabase(IntPtr handle, string updateDesktopDatabase);
#endif

        [DllImport(pluginName)]
        private static extern IntPtr ThirdPartySharingSettings_Create();
        [DllImport(pluginName)]
        private static extern void ThirdPartySharingSettings_Destroy(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void ThirdPartySharingSettings_AddValue(IntPtr handle, string partnerName, string key, string value);
        [DllImport(pluginName)]
        private static extern void ThirdPartySharingSettings_AddFlag(IntPtr handle, string partnerName, string key, int value);

        [DllImport(pluginName)]
        private static extern IntPtr SuperfineSDKEvent_Get(string eventName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_SetIntValue(IntPtr handle, int value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_SetStringValue(IntPtr handle, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_SetMapValue(IntPtr handle, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_SetJsonValue(IntPtr handle, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_AddRevenueData(IntPtr handle, double revenue, string currency);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKEvent_SetFlag(IntPtr handle, int flag);

        [DllImport(pluginName)]
        private static extern IntPtr SuperfineSDKManager_GetInstance();
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Initialize(IntPtr initializationData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Initialize2(IntPtr initializationData, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Initialize3(IntPtr initializationData, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Destroy();
        [DllImport(pluginName)]
        private static extern int SuperfineSDKManager_IsInitialized(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RegisterModuleFactory(string name, IntPtr factory);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_UnregisterModuleFactory(string name, int autoDestroy);
        [DllImport(pluginName)]
        private static extern IntPtr SuperfineSDKManager_GetModule(IntPtr handle, string name);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetVersion(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Start(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Stop(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetOffline(IntPtr handle, int value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_GdprForgetMe(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_DisableThirdPartySharing(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_EnableThirdPartySharing(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogThirdPartySharingSettings(IntPtr handle, IntPtr settings);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_OnPause(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_OnResume(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddStartCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveStartCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddStopCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveStopCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddPauseCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemovePauseCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddResumeCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveResumeCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddDeepLinkCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveDeepLinkCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddPushTokenCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemovePushTokenCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddSendEventCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveSendEventCallback(int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_EnableSteamDRMCheck([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSaveSteamActivationLink(int value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_DisableSteamDRMCheck();
        [DllImport(pluginName)]
        private static extern int SuperfineSDKManager_GetSteamDRMError();
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetUserId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetSessionId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetHost(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetConfigUrl(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetSdkConfig(IntPtr handle);
        [DllImport(pluginName)]
        private static extern StoreType SuperfineSDKManager_GetStoreType(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetFacebookAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetInstagramAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetAppleAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetAppleSignInClientId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetAppleDeveloperTeamId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetGooglePlayGameServicesProjectId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetGooglePlayDeveloperAccountId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetLinkedInAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetQQAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetWeChatAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetTikTokAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetSnapAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetCustomUserId(IntPtr handle, string customUserId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetAdvertisingId(IntPtr handle, string advertisingId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_FetchRemoteConfig(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_FetchRemoteConfig2(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogBootStart(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogBootEnd(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLevelStart(IntPtr handle, int id, string name);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLevelEnd(IntPtr handle, int id, string name, int success);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdLoad(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdLoad2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdClose(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdClose2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdClick(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdClick2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdImpression(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdImpression2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPInitialization(IntPtr handle, int success);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPRestorePurchase(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPResult(IntPtr handle, string pack, double price, int amount, string currency, int success);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Apple(IntPtr handle, string receipt);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Google(IntPtr handle, string data, string signature);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Amazon(IntPtr handle, string userId, string receiptId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Roku(IntPtr handle, string transactionId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Windows(IntPtr handle, string receipt);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Facebook(IntPtr handle, string receipt);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_Unity(IntPtr handle, string receipt);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_AppStoreServer(IntPtr handle, string transactionId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_GooglePlayProduct(IntPtr handle, string productId, string token);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscription(IntPtr handle, string subscriptionId, string token);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscriptionv2(IntPtr handle, string token);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogUpdateApp(IntPtr handle, string newVersion);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogRateApp(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLocation(IntPtr handle, double latitude, double longitude);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogFacebookLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogFacebookUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogInstagramLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogInstagramUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAppleLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAppleUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAppleGameCenterLink(IntPtr handle, string gamePlayerId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAppleGameCenterTeamLink(IntPtr handle, string teamPlayerId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAppleGameCenterUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGoogleLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGoogleUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGooglePlayGameServicesLink(IntPtr handle, string gamePlayerId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGooglePlayGameServicesDeveloperLink(IntPtr handle, string developerPlayerKey);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGooglePlayGameServicesUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLinkedInLink(IntPtr handle, string personId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLinkedInUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogMeetupLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogMeetupUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGitHubLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogGitHubUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogDiscordLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogDiscordUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogTwitterLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogTwitterUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogSpotifyLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogSpotifyUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogMicrosoftLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogMicrosoftUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLINELink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLINEUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogVKLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogVKUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogQQLink(IntPtr handle, string openId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogQQUnionLink(IntPtr handle, string unionId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogQQUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWeChatLink(IntPtr handle, string openId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWeChatUnionLink(IntPtr handle, string unionId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWeChatUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogTikTokLink(IntPtr handle, string openId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogTikTokUnionLink(IntPtr handle, string unionId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogTikTokUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWeiboLink(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWeiboUnlink(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLink(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLink2(IntPtr handle, string id, string type, string scopeId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLink3(IntPtr handle, string id, string type, string scopeId, string scopeType);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountUnlink(IntPtr handle, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddUserPhoneNumber(IntPtr handle, int countryCode, string number);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveUserPhoneNumber(IntPtr handle, int countryCode, string number);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddUserEmail(IntPtr handle, string email);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveUserEmail(IntPtr handle, string email);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserName(IntPtr handle, string firstName, string lastName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserFirstName(IntPtr handle, string firstName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserLastName(IntPtr handle, string lastName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserCity(IntPtr handle, string city);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserState(IntPtr handle, string state);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserCountry(IntPtr handle, string country);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserZipCode(IntPtr handle, string zipCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserDateOfBirth(IntPtr handle, int day, int month, int year);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserDateOfBirth2(IntPtr handle, int day, int month);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserYearOfBirth(IntPtr handle, int year);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetUserGender(IntPtr handle, UserGender gender);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWalletLink(IntPtr handle, string wallet);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWalletLink2(IntPtr handle, string wallet, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWalletUnlink(IntPtr handle, string wallet);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWalletUnlink2(IntPtr handle, string wallet, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogCryptoPayment(IntPtr handle, string pack, double price, int amount);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogCryptoPayment2(IntPtr handle, string pack, double price, int amount, string currency);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogCryptoPayment3(IntPtr handle, string pack, double price, int amount, string currency, string chain);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdRevenue(IntPtr handle, string source, double revenue, string currency);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdRevenue2(IntPtr handle, string source, double revenue, string currency, string network);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdRevenue3(IntPtr handle, string source, double revenue, string currency, string network, string networkData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Log(IntPtr handle, string eventName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Log2(IntPtr handle, string eventName, EventFlag flag);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithIntValue(IntPtr handle, string eventName, int value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithIntValue2(IntPtr handle, string eventName, int value, EventFlag flag);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithStringValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithStringValue2(IntPtr handle, string eventName, string value, EventFlag flag);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithJsonValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithJsonValue2(IntPtr handle, string eventName, string value, EventFlag flag);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithMapValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithMapValue2(IntPtr handle, string eventName, string value, EventFlag flag);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogEvent(IntPtr handle, IntPtr eventData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogEventCache(IntPtr eventData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_OpenURL(string url);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetDeepLinkUrl();
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetPushToken(string token);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetPushToken();
#endregion

        private int GetStandaloneLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.NONE:
                    return 1;

                case LogLevel.INFO:
                    return 2;

                case LogLevel.DEBUG:
                    return 3;

                case LogLevel.VERBOSE:
                    return 4;
            }

            return 1;
        }

        private SuperfineSDKMonoBehaviour unityBehaviour = null;

        protected override void InitializeNative(SuperfineSDKSettings settings, List<string> moduleNameList)
        {
            IntPtr initializationData = InitializationData_Create(settings.appId, settings.appSecret);

            InitializationData_SetWrapper(initializationData, SuperfineSDK.WRAPPER);
            InitializationData_SetWrapperVersion(initializationData, SuperfineSDK.WRAPPER_VERSION);

            if (!string.IsNullOrEmpty(settings.host))
            {
                InitializationData_SetHost(initializationData, settings.host);
            }

            if (!string.IsNullOrEmpty(settings.configUrl))
            {
                InitializationData_SetConfigUrl(initializationData, settings.configUrl);
            }

            InitializationData_SetLogLevel(initializationData, GetStandaloneLogLevel(settings.standaloneLogLevel));

            if (!string.IsNullOrEmpty(settings.configId))
            {
                InitializationData_SetConfigId(initializationData, settings.configId);
            }

            InitializationData_SetCustomConfigId(initializationData, settings.waitConfigId ? 1 : 0);

            if (!string.IsNullOrEmpty(settings.customUserId))
            {
                InitializationData_SetCustomUserId(initializationData, settings.customUserId);
            }

            InitializationData_SetSavePath(initializationData, Application.persistentDataPath);

            InitializationData_SetSendDelay(initializationData, (int)settings.flushInterval);
            InitializationData_SetFlushQueueSize(initializationData, settings.flushQueueSize);

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            InitializationData_SetAppName(initializationData, Application.productName);
            InitializationData_SetAppVersion(initializationData, Application.version);
            InitializationData_SetBundleId(initializationData, settings.bundleId);
            InitializationData_SetAppBuildNumber(initializationData, settings.buildNumber);
#endif

#if UNITY_STANDALONE_LINUX
            InitializationData_SetDesktopTemplatePath(initializationData, Application.streamingAssetsPath + "/default.desktop.in");
            InitializationData_SetDesktopPath(initializationData, settings.desktopPath);

            string desktopFilename = Application.productName.ToLower().Replace(' ', '-') + ".desktop";
            InitializationData_SetDesktopFilename(initializationData, desktopFilename);

            InitializationData_SetUpdateDesktopDatabase(initializationData, settings.updateDesktopDatabase);
#endif

            InitializationData_SetRegisterURIScheme(initializationData, settings.registerURIScheme ? 1 : 0);

            List<string> uriSchemes = settings.GetCustomSchemes();
            if (uriSchemes != null)
            {
                int numUriSchemes = uriSchemes.Count;
                if (numUriSchemes > 0)
                {
                    for (int i = 0; i < numUriSchemes; ++i)
                    {
                        InitializationData_AddURIScheme(initializationData, uriSchemes[i]);
                    }
                }
            }

            if (moduleNameList != null)
            {
                int numModules = moduleNameList.Count;
                for (int i = 0; i < numModules; i++)
                {
                    InitializationData_AddExcludedModuleName(initializationData, moduleNameList[i]);
                }
            }

            if (settings.steamBuild && settings.steamAppId > 0)
            {
                InitializationData_SetSteamBuild(initializationData, settings.steamAppId);
            }

            InitializationData_SetStoreType(initializationData, settings.standaloneStoreType);

            InitializationData_SetAutoStart(initializationData, settings.autoStart ? 1 : 0);

            InitializationData_SetOffline(initializationData, settings.offline ? 1 : 0);
            InitializationData_SetSendInBackground(initializationData, settings.sendInBackground ? 1 : 0);

            if (!string.IsNullOrEmpty(settings.facebookAppId))
            {
                InitializationData_SetFacebookAppId(initializationData, settings.facebookAppId);
            }

            if (!string.IsNullOrEmpty(settings.instagramAppId))
            {
                InitializationData_SetInstagramAppId(initializationData, settings.instagramAppId);
            }

            if (!string.IsNullOrEmpty(settings.appleAppId))
            {
                InitializationData_SetAppleAppId(initializationData, settings.appleAppId);
            }

            if (!string.IsNullOrEmpty(settings.appleSignInClientId))
            {
                InitializationData_SetAppleSignInClientId(initializationData, settings.appleSignInClientId);
            }

            if (!string.IsNullOrEmpty(settings.appleDeveloperTeamId))
            {
                InitializationData_SetAppleDeveloperTeamId(initializationData, settings.appleDeveloperTeamId);
            }

            if (!string.IsNullOrEmpty(settings.googlePlayGameServicesProjectId))
            {
                InitializationData_SetGooglePlayGameServicesProjectId(initializationData, settings.googlePlayGameServicesProjectId);
            }

            if (!string.IsNullOrEmpty(settings.googlePlayDeveloperAccountId))
            {
                InitializationData_SetGooglePlayDeveloperAccountId(initializationData, settings.googlePlayDeveloperAccountId);
            }

            if (!string.IsNullOrEmpty(settings.linkedInAppId))
            {
                InitializationData_SetLinkedInAppId(initializationData, settings.linkedInAppId);
            }

            if (!string.IsNullOrEmpty(settings.qqAppId))
            {
                InitializationData_SetQQAppId(initializationData, settings.qqAppId);
            }

            if (!string.IsNullOrEmpty(settings.weChatAppId))
            {
                InitializationData_SetWeChatAppId(initializationData, settings.weChatAppId);
            }

            if (!string.IsNullOrEmpty(settings.tikTokAppId))
            {
                InitializationData_SetTikTokAppId(initializationData, settings.tikTokAppId);
            }

            if (!string.IsNullOrEmpty(settings.snapAppId))
            {
                InitializationData_SetSnapAppId(initializationData, settings.snapAppId);
            }

            InitializationData_SetDisableOnlineSdkConfig(initializationData, settings.disableOnlineSdkConfig ? 1 : 0);
            
            InitializationData_SetEnableCoppa(initializationData, settings.enableCoppa ? 1 : 0);
                        
            if (!string.IsNullOrEmpty(settings.proxy))
            {
                InitializationData_SetSendProxy(initializationData, settings.proxy);
            }

            if (!string.IsNullOrEmpty(settings.caPath))
            {
                InitializationData_SetSendCertificateAuthority(initializationData, settings.caPath);
                InitializationData_SetSendProxyCertificateAuthority(initializationData, settings.caPath);
            }

            if (!settings.sslVerify)
            {
                InitializationData_SetSendSSLVerify(initializationData, 0);
            }

            List<SuperfineSDKNativeModuleSettings> moduleSettingList = settings.nativeModules;
            if (moduleSettingList != null)
            {
                HashSet<string> classPathSet = new HashSet<string>();

                SuperfineSDKSettings.PlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

                int numModules = moduleSettingList.Count;
                for (int i = 0; i < numModules; ++i)
                {
                    SuperfineSDKNativeModuleSettings moduleSettings = moduleSettingList[i];

                    string classPath = moduleSettings.classPath;
                    if (string.IsNullOrEmpty(classPath)) continue;

                    if (!moduleSettings.platform.HasFlag(platformFlag)) continue;

                    if (classPathSet.Contains(classPath)) continue;
                    classPathSet.Add(classPath);

                    string moduleData = string.Empty;

                    if (moduleSettings.data != null)
                    {
                        SimpleJSON.JSONObject dataObject = moduleSettings.data.GetJsonObject();
                        if (dataObject != null)
                        {
                            moduleData = dataObject.ToString();
                        }
                    }

                    InitializationData_AddModule(initializationData, classPath, moduleData);
                }
            }

            SuperfineSDKManager_Initialize2(initializationData, initializationHandlerPointer);

            InitializationData_Destroy(initializationData);
            initializationData = IntPtr.Zero;

            GameObject go = new GameObject("SuperfineSDK");
            unityBehaviour = go.AddComponent<SuperfineSDKMonoBehaviour>();
            unityBehaviour.SetManager(this);
        }

        public override void Destroy()
        {
            if (unityBehaviour != null)
            {
                unityBehaviour.SetManager(null);

                GameObject.Destroy(unityBehaviour.gameObject);
                unityBehaviour = null;
            }

            SuperfineSDKManager_Destroy();
        }

        private IntPtr GetNativeInstance()
        {
            return SuperfineSDKManager_GetInstance();
        }

        public override bool IsInitialized()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return false;

            return SuperfineSDKManager_IsInitialized(managerHandle) != 0;
        }

        public override void Execute(string eventName, object param = null)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            if (eventName == "pause")
            {
                SuperfineSDKManager_OnPause(managerHandle);
            }
            else if (eventName == "resume")
            {
                SuperfineSDKManager_OnResume(managerHandle);
            }
            else if (eventName == "destroy")
            {
                Destroy();
            }
        }

        public override void Start()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_Start(managerHandle);
        }

        public override void Stop()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_Stop(managerHandle);
        }

        public override void SetOffline(bool value)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetOffline(managerHandle, value ? 1 : 0);
        }

        public override string GetVersion()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;             
            
            return SuperfineSDKManager_GetVersion(managerHandle);
        }

        public override void SetConfigId(string configId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetConfigId(managerHandle, configId);
        }

        public override void SetCustomUserId(string customUserId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetCustomUserId(managerHandle, customUserId);
        }

        public void SetAdvertisingId(string advertisingId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetAdvertisingId(managerHandle, advertisingId);
        }

        public override string GetAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetAppId(managerHandle);
        }

        public override string GetUserId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetUserId(managerHandle);
        }

        public override string GetSessionId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetSessionId(managerHandle);
        }

        public override string GetHost()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetHost(managerHandle);
        }

        public override string GetConfigUrl()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetConfigUrl(managerHandle);
        }

        public override string GetSdkConfig()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetSdkConfig(managerHandle);
        }

        public override StoreType GetStoreType()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return StoreType.UNKNOWN;

            return SuperfineSDKManager_GetStoreType(managerHandle);
        }

        public override string GetFacebookAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetFacebookAppId(managerHandle);
        }

        public override string GetInstagramAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetInstagramAppId(managerHandle);
        }

        public override string GetAppleAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetAppleAppId(managerHandle);
        }

        public override string GetAppleSignInClientId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetAppleSignInClientId(managerHandle);
        }

        public override string GetAppleDeveloperTeamId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetAppleDeveloperTeamId(managerHandle);
        }

        public override string GetGooglePlayGameServicesProjectId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetGooglePlayGameServicesProjectId(managerHandle);
        }

        public override string GetGooglePlayDeveloperAccountId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetGooglePlayDeveloperAccountId(managerHandle);
        }

        public override string GetLinkedInAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetLinkedInAppId(managerHandle);
        }

        public override string GetQQAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetQQAppId(managerHandle);
        }

        public override string GetWeChatAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetWeChatAppId(managerHandle);
        }

        public override string GetTikTokAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetTikTokAppId(managerHandle);
        }

        public override string GetSnapAppId()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return string.Empty;

            return SuperfineSDKManager_GetSnapAppId(managerHandle);
        }
                
        public static void OpenURL(string url)
        {
            SuperfineSDKManager_OpenURL(url);
        }

        public static string GetDeepLinkUrl()
        {
            return SuperfineSDKManager_GetDeepLinkUrl();
        }

        public static void SetPushToken(string token)
        {
            SuperfineSDKManager_SetPushToken(token);
        }

        public static string GetPushToken()
        {
            return SuperfineSDKManager_GetPushToken();
        }

        public override void FetchRemoteConfig()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) 
            {
                SuperfineSDK.InvokeOnReceiveRemoteConfig(string.Empty);
            }
            else
            {
                SuperfineSDKManager_FetchRemoteConfig(managerHandle, remoteConfigHandlerPointer);
            }
        }

        public static void RegisterNativeStartCallback()
        {
            SuperfineSDKManager_AddStartCallback(startHandlerPointer, 0);
        }

        public static void UnregisterNativeStartCallback()
        {
            SuperfineSDKManager_RemoveStartCallback(0);
        }

        public static void RegisterNativeStopCallback()
        {
            SuperfineSDKManager_AddStopCallback(stopHandlerPointer, 0);
        }

        public static void UnregisterNativeStopCallback()
        {
            SuperfineSDKManager_RemoveStopCallback(0);
        }

        public static void RegisterNativePauseCallback()
        {
            SuperfineSDKManager_AddPauseCallback(pauseHandlerPointer, 0);
        }

        public static void UnregisterNativePauseCallback()
        {
            SuperfineSDKManager_RemovePauseCallback(0);
        }

        public static void RegisterNativeResumeCallback()
        {
            SuperfineSDKManager_AddResumeCallback(resumeHandlerPointer, 0);
        }

        public static void UnregisterNativeResumeCallback()
        {
            SuperfineSDKManager_RemoveResumeCallback(0);
        }

        public static void RegisterNativeDeepLinkCallback()
        {
            SuperfineSDKManager_AddDeepLinkCallback(deepLinkHandlerPointer, 0);
        }

        public static void UnregisterNativeDeepLinkCallback()
        {
            SuperfineSDKManager_RemoveDeepLinkCallback(0);
        }

        public static void RegisterNativePushTokenCallback()
        {
            SuperfineSDKManager_AddPushTokenCallback(pushTokenHandlerPointer, 0);
        }

        public static void UnregisterNativePushTokenCallback()
        {
            SuperfineSDKManager_RemovePushTokenCallback(0);
        }

        public static void RegisterNativeSendEventCallback()
        {
            SuperfineSDKManager_AddSendEventCallback(sendEventHandlerPointer, 0);
        }

        public static void UnregisterNativeSendEventCallback()
        {
            SuperfineSDKManager_RemoveSendEventCallback(0);
        }

        public override void GdprForgetMe()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;
            
            SuperfineSDKManager_GdprForgetMe(managerHandle);
        }

        public override void DisableThirdPartySharing()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_DisableThirdPartySharing(managerHandle);
        }

        public override void EnableThirdPartySharing()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_EnableThirdPartySharing(managerHandle);
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            IntPtr settingsHandle = ThirdPartySharingSettings_Create();

            var values = settings.GetValues();
            foreach (var pair in values)
            {
                string partnerName = pair.Key;
                foreach (var partnerPair in pair.Value)
                {
                    ThirdPartySharingSettings_AddValue(settingsHandle, partnerName, partnerPair.Key, partnerPair.Value);
                }
            }

            var flags = settings.GetFlags();
            foreach (var pair in flags)
            {
                string partnerName = pair.Key;
                foreach (var partnerPair in pair.Value)
                {
                    ThirdPartySharingSettings_AddFlag(settingsHandle, partnerName, partnerPair.Key, partnerPair.Value ? 1 : 0);
                }
            }

            SuperfineSDKManager_LogThirdPartySharingSettings(managerHandle, settingsHandle);
            ThirdPartySharingSettings_Destroy(settingsHandle);
        }

        public override void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_Log2(managerHandle, eventName, eventFlag);
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWithIntValue2(managerHandle, eventName, data, eventFlag);
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWithStringValue2(managerHandle, eventName, data, eventFlag);
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            if (data == null)
            {
                SuperfineSDKManager_Log2(managerHandle, eventName, eventFlag);
            }
            else
            {
                SuperfineSDKManager_LogWithMapValue2(managerHandle, eventName, GetMapString(data), eventFlag);
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag = EventFlag.NONE)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;
            
            if (data == null)
            {
                SuperfineSDKManager_Log2(managerHandle, eventName, eventFlag);
            }
            else
            {
                SuperfineSDKManager_LogWithJsonValue2(managerHandle, eventName, data.ToString(), eventFlag);
            }
        }

        private static IntPtr CreateNativeLogEvent(SuperfineSDKEvent eventData)
        {
            IntPtr logEvent = SuperfineSDKEvent_Get(eventData.eventName);
            if (logEvent == null) return IntPtr.Zero;

            SuperfineSDKEvent.ValueType valueType = eventData.valueType;
            switch (valueType)
            {
                case SuperfineSDKEvent.ValueType.INT:
                    {
                        int data = (int)eventData.value;
                        SuperfineSDKEvent_SetIntValue(logEvent, data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.STRING:
                    {
                        string data = (string)eventData.value;
                        SuperfineSDKEvent_SetStringValue(logEvent, data);
                    }
                    break;

                case SuperfineSDKEvent.ValueType.MAP:
                    {
                        Dictionary<string, string> data = (Dictionary<string, string>)eventData.value;
                        SuperfineSDKEvent_SetMapValue(logEvent, GetMapString(data));
                    }
                    break;

                case SuperfineSDKEvent.ValueType.JSON:
                    {
                        SimpleJSON.JSONObject data = (SimpleJSON.JSONObject)eventData.value;
                        SuperfineSDKEvent_SetJsonValue(logEvent, data.ToString());
                    }
                    break;

                default:
                    break;
            }

            EventFlag eventFlag = eventData.eventFlag;
            if (eventFlag != EventFlag.NONE)
            {
                SuperfineSDKEvent_SetFlag(logEvent, (int)eventFlag);
            }

            if (eventData.HasRevenue())
            {
                SuperfineSDKEvent_AddRevenueData(logEvent, eventData.revenue, eventData.currency);
            }

            return logEvent;
        }

        public override void Log(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;
            
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            IntPtr logEvent = CreateNativeLogEvent(eventData);
            if (logEvent == null) return;
              
            SuperfineSDKManager_LogEvent(managerHandle, logEvent);
        }

        public static void LogCache(SuperfineSDKEvent eventData)
        {
            if (eventData == null) return;
            
            IntPtr logEvent = CreateNativeLogEvent(eventData);
            if (logEvent == null) return;
              
            SuperfineSDKManager_LogEventCache(logEvent);
        }

        public override void LogBootStart()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogBootStart(managerHandle);
        }

        public override void LogBootEnd()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogBootEnd(managerHandle);
        }

        public override void LogLevelStart(int id, string name)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLevelStart(managerHandle, id, name);
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLevelEnd(managerHandle, id, name, isSuccess ? 1 : 0);
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAdLoad2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAdClose2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAdClick2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAdImpression2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPInitialization(managerHandle, isSuccess ? 1 : 0);
        }

        public override void LogIAPRestorePurchase()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPRestorePurchase(managerHandle);
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPResult(managerHandle, pack, price, amount, currency, isSuccess ? 1 : 0);
        }

        public override void LogIAPReceipt_Apple(string receipt)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Apple(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Google(string data, string signature)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Google(managerHandle, data, signature);
        }

        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Amazon(managerHandle, userId, receiptId);
        }

        public override void LogIAPReceipt_Roku(string transactionId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Roku(managerHandle, transactionId);
        }

        public override void LogIAPReceipt_Windows(string receipt)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Windows(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Facebook(string receipt)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Facebook(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Unity(string receipt)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_Unity(managerHandle, receipt);
        }

        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_AppStoreServer(managerHandle, transactionId);
        }

        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_GooglePlayProduct(managerHandle, productId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscription(managerHandle, subscriptionId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscriptionv2(managerHandle, token);
        }

        public override void LogUpdateApp(string newVersion)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogUpdateApp(managerHandle, newVersion);
        }

        public override void LogRateApp()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogRateApp(managerHandle);
        }

        public override void LogLocation(double latitude, double longitude)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLocation(managerHandle, latitude, longitude);
        }

        public override void LogFacebookLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogFacebookLink(managerHandle, userId);
        }

        public override void LogFacebookUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogFacebookUnlink(managerHandle);
        }

        public override void LogInstagramLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogInstagramLink(managerHandle, userId);
        }

        public override void LogInstagramUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogInstagramUnlink(managerHandle);
        }

        public override void LogAppleLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAppleLink(managerHandle, userId);
        }

        public override void LogAppleUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAppleUnlink(managerHandle);
        }

        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAppleGameCenterLink(managerHandle, gamePlayerId);
        }

        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAppleGameCenterTeamLink(managerHandle, teamPlayerId);
        }

        public override void LogAppleGameCenterUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAppleGameCenterUnlink(managerHandle);
        }

        public override void LogGoogleLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGoogleLink(managerHandle, userId);
        }

        public override void LogGoogleUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGoogleUnlink(managerHandle);
        }

        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGooglePlayGameServicesLink(managerHandle, gamePlayerId);
        }

        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGooglePlayGameServicesDeveloperLink(managerHandle, developerPlayerKey);
        }

        public override void LogGooglePlayGameServicesUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGooglePlayGameServicesUnlink(managerHandle);
        }

        public override void LogLinkedInLink(string personId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLinkedInLink(managerHandle, personId);
        }

        public override void LogLinkedInUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLinkedInUnlink(managerHandle);
        }

        public override void LogMeetupLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogMeetupLink(managerHandle, userId);
        }

        public override void LogMeetupUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogMeetupUnlink(managerHandle);
        }

        public override void LogGitHubLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGitHubLink(managerHandle, userId);
        }

        public override void LogGitHubUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogGitHubUnlink(managerHandle);
        }

        public override void LogDiscordLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogDiscordLink(managerHandle, userId);
        }

        public override void LogDiscordUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogDiscordUnlink(managerHandle);
        }

        public override void LogTwitterLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogTwitterLink(managerHandle, userId);
        }

        public override void LogTwitterUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogTwitterUnlink(managerHandle);
        }

        public override void LogSpotifyLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogSpotifyLink(managerHandle, userId);
        }

        public override void LogSpotifyUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogSpotifyUnlink(managerHandle);
        }

        public override void LogMicrosoftLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogMicrosoftLink(managerHandle, userId);
        }

        public override void LogMicrosoftUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogMicrosoftUnlink(managerHandle);
        }

        public override void LogLINELink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLINELink(managerHandle, userId);
        }

        public override void LogLINEUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogLINEUnlink(managerHandle);
        }

        public override void LogVKLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogVKLink(managerHandle, userId);
        }

        public override void LogVKUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogVKUnlink(managerHandle);
        }

        public override void LogQQLink(string openId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogQQLink(managerHandle, openId);
        }

        public override void LogQQUnionLink(string unionId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogQQUnionLink(managerHandle, unionId);
        }

        public override void LogQQUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogQQUnlink(managerHandle);
        }

        public override void LogWeChatLink(string openId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWeChatLink(managerHandle, openId);
        }

        public override void LogWeChatUnionLink(string unionId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWeChatUnionLink(managerHandle, unionId);
        }

        public override void LogWeChatUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWeChatUnlink(managerHandle);
        }

        public override void LogTikTokLink(string openId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogTikTokLink(managerHandle, openId);
        }

        public override void LogTikTokUnionLink(string unionId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogTikTokUnionLink(managerHandle, unionId);
        }

        public override void LogTikTokUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogTikTokUnlink(managerHandle);
        }

        public override void LogWeiboLink(string userId)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWeiboLink(managerHandle, userId);
        }

        public override void LogWeiboUnlink()
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWeiboUnlink(managerHandle);
        }

        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            if (string.IsNullOrEmpty(scopeId))
            {
                SuperfineSDKManager_LogAccountLink(managerHandle, id, type);
            }
            else if (string.IsNullOrEmpty(scopeType))
            {
                SuperfineSDKManager_LogAccountLink2(managerHandle, id, type, scopeId);
            }
            else
            {
                SuperfineSDKManager_LogAccountLink3(managerHandle, id, type, scopeId, scopeType);
            }
        }

        public override void LogAccountUnlink(string type)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAccountUnlink(managerHandle, type);
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;
            
            SuperfineSDKManager_LogWalletLink2(managerHandle, wallet, type == null ? "ethereum" : type);
        }

        public override void AddUserPhoneNumber(int countryCode, string number)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_AddUserPhoneNumber(managerHandle, countryCode, number);
        }

        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_RemoveUserPhoneNumber(managerHandle, countryCode, number);
        }

        public override void AddUserEmail(string email)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_AddUserEmail(managerHandle, email);
        }

        public override void RemoveUserEmail(string email)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_RemoveUserEmail(managerHandle, email);
        }

        public override void SetUserName(string firstName, string lastName)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserName(managerHandle, firstName, lastName);
        }

        public override void SetUserFirstName(string firstName)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserFirstName(managerHandle, firstName);
        }

        public override void SetUserLastName(string lastName)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserLastName(managerHandle, lastName);
        }

        public override void SetUserCity(string city)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserCity(managerHandle, city);
        }

        public override void SetUserState(string state)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserState(managerHandle, state);
        }

        public override void SetUserCountry(string country)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserCountry(managerHandle, country);
        }

        public override void SetUserZipCode(string zipCode)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserZipCode(managerHandle, zipCode);
        }

        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserDateOfBirth(managerHandle, day, month, year);
        }

        public override void SetUserDateOfBirth(int day, int month)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserDateOfBirth2(managerHandle, day, month);
        }

        public override void SetUserYearOfBirth(int year)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserYearOfBirth(managerHandle, year);
        }

        public override void SetUserGender(UserGender gender)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_SetUserGender(managerHandle, gender);
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogWalletUnlink2(managerHandle, wallet, type == null ? "ethereum" : type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogCryptoPayment3(managerHandle, pack, price, amount, currency, chain);
        }

        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            IntPtr managerHandle = GetNativeInstance();
            if (managerHandle == IntPtr.Zero) return;

            SuperfineSDKManager_LogAdRevenue3(managerHandle, source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString());
        }

        public static void SetSteamDRMCheck(Func<uint, int> func)
        {
            steamDRMCheckFunc = func;

            if (steamDRMCheckFunc == null) 
            {
                SuperfineSDKManager_DisableSteamDRMCheck();
            }
            else
            {
                SuperfineSDKManager_EnableSteamDRMCheck(steamDRMCheckPointer);
            }
        }

        public static int GetSteamDRMError()
        {
            return SuperfineSDKManager_GetSteamDRMError();
        }

        public static void SetSaveSteamActivationLink(bool value)
        {
            SuperfineSDKManager_SetSaveSteamActivationLink(value ? 1 : 0);
        }
    }
}
#endif