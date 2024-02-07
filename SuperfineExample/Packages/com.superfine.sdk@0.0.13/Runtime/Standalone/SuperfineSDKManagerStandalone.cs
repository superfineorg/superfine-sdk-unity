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
        private delegate void StartDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(StartDelegate))]
        private static void InvokeStartCallback(int requestCode)
        {
            if (onStart != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStart());
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void StopDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(StopDelegate))]
        private static void InvokeStopCallback(int requestCode)
        {
            if (onStop != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStop());
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PauseDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(PauseDelegate))]
        private static void InvokePauseCallback(int requestCode)
        {
            if (onPause != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onPause());
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ResumeDelegate(int requestCode);
        [MonoPInvokeCallback(typeof(ResumeDelegate))]
        private static void InvokeResumeCallback(int requestCode)
        {
            if (onResume != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onResume());
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DeepLinkDelegate(string url, int requestCode);
        [MonoPInvokeCallback(typeof(DeepLinkDelegate))]
        private static void InvokeDeepLinkCallback(string url, int requestCode)
        {
            if (onSetDeepLink != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetDeepLink(url));
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void PushTokenDelegate(string token, int requestCode);
        [MonoPInvokeCallback(typeof(PushTokenDelegate))]
        private static void InvokePushTokenCallback(string token, int requestCode)
        {
            if (onSetPushToken != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetPushToken(token));
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SendEventDelegate(string eventName, string eventData, int requestCode);
        [MonoPInvokeCallback(typeof(SendEventDelegate))]
        private static void InvokeSendEventCallback(string eventName, string eventData, int requestCode)
        {
            if (onSendEvent != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSendEvent(eventName, eventData));
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void RemoteConfigDelegate(string data);
        [MonoPInvokeCallback(typeof(RemoteConfigDelegate))]
        private static void InvokeRemoteConfigCallback(string data)
        {
             UnityMainThreadDispatcher.Instance().Enqueue(() => onReceiveRemoteConfig(data));
        }

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

            return 1;
        }

        private static readonly SteamDRMCheckDelegate steamDRMCheckHandler = SteamDRMCheckWrapper;
        private static readonly IntPtr steamDRMCheckPointer = Marshal.GetFunctionPointerForDelegate(steamDRMCheckHandler);

        #region Interface
        [DllImport(pluginName)]
        private static extern IntPtr InitializationData_Create();
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
        private static extern IntPtr SuperfineSDKManager_Create(string appId, string appSecret, IntPtr initializationData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Destroy(IntPtr handle);
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
        private static extern void SuperfineSDKManager_AddStartCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveStartCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddStopCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveStopCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddPauseCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemovePauseCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddResumeCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveResumeCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddDeepLinkCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddDeepLinkCallback2(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode, int autoCall);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveDeepLinkCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddPushTokenCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddPushTokenCallback2(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode, int autoCall);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemovePushTokenCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_AddSendEventCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RemoveSendEventCallback(IntPtr handle, int requestCode);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_EnableSteamDRMCheck(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_DisableSteamDRMCheck(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetAppId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetUserId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetSessionId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetHost(IntPtr handle);
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
        private static extern string SuperfineSDKManager_GetDeepLinkUrl(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetPushToken(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetCustomUserId(IntPtr handle, string customUserId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetAdvertisingId(IntPtr handle, string advertisingId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_RegisterModuleFactory(string name, IntPtr factory);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_UnregisterModuleFactory(string name, int autoDestroy);
        [DllImport(pluginName)]
        private static extern IntPtr SuperfineSDKManager_GetModule(IntPtr handle, string name);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_FetchRemoteConfig(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
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
        private static extern void SuperfineSDKManager_LogAuthorizationTrackingStatus(IntPtr handle, AuthorizationTrackingStatus status);
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
        private static extern void SuperfineSDKManager_OpenURL(IntPtr handle, string url, out int error);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetPushToken(IntPtr handle, string token);
#endregion

        private SuperfineSDKMonoBehaviour unityBehaviour = null;

        private IntPtr managerHandle = IntPtr.Zero;

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

        public override void Initialize(SuperfineSDKSettings settings)
        {
            IntPtr initializationData = InitializationData_Create();

            InitializationData_SetWrapperVersion(initializationData, SuperfineSDK.VERSION);

            if (!string.IsNullOrEmpty(settings.host))
            {
                InitializationData_SetHost(initializationData, settings.host);
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

                SuperfineSDKPlatformFlag platformFlag = SuperfineSDK.GetPlatformFlag();

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

            managerHandle = SuperfineSDKManager_Create(settings.appId, settings.appSecret, initializationData);

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

            if (managerHandle != IntPtr.Zero)
            {
                SuperfineSDKManager_Destroy(managerHandle);
                managerHandle = IntPtr.Zero;
            }
        }

        public override void Execute(string eventName, object param = null)
        {
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
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_Start(managerHandle);
        }

        public override void Stop()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_Stop(managerHandle);
        }

        public override void SetOffline(bool value)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetOffline(managerHandle, value ? 1 : 0);
        }

        public override string GetVersion()
        {
             if (managerHandle == IntPtr.Zero) return string.Empty;
             return SuperfineSDKManager_GetVersion(managerHandle);
        }

        public override void SetConfigId(string configId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetConfigId(managerHandle, configId);
        }

        public override void SetCustomUserId(string customUserId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetCustomUserId(managerHandle, customUserId);
        }

        public override void SetAdvertisingId(string advertisingId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetAdvertisingId(managerHandle, advertisingId);
        }

        public override string GetAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetAppId(managerHandle);
        }

        public override string GetUserId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetUserId(managerHandle);
        }

        public override string GetSessionId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetSessionId(managerHandle);
        }

        public override string GetHost()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetHost(managerHandle);
        }

        public override StoreType GetStoreType()
        {
            if (managerHandle == IntPtr.Zero) return StoreType.UNKNOWN;
            return SuperfineSDKManager_GetStoreType(managerHandle);
        }

        public override string GetFacebookAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetFacebookAppId(managerHandle);
        }

        public override string GetInstagramAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetInstagramAppId(managerHandle);
        }

        public override string GetAppleAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetAppleAppId(managerHandle);
        }

        public override string GetAppleSignInClientId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetAppleSignInClientId(managerHandle);
        }

        public override string GetAppleDeveloperTeamId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetAppleDeveloperTeamId(managerHandle);
        }

        public override string GetGooglePlayGameServicesProjectId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetGooglePlayGameServicesProjectId(managerHandle);
        }

        public override string GetGooglePlayDeveloperAccountId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetGooglePlayDeveloperAccountId(managerHandle);
        }

        public override string GetLinkedInAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetLinkedInAppId(managerHandle);
        }

        public override string GetQQAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetQQAppId(managerHandle);
        }

        public override string GetWeChatAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetWeChatAppId(managerHandle);
        }

        public override string GetTikTokAppId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetTikTokAppId(managerHandle);
        }

        public override string GetDeepLinkUrl()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetDeepLinkUrl(managerHandle);
        }

        public override void SetPushToken(string token)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetPushToken(managerHandle, token);
        }

        public override string GetPushToken()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetPushToken(managerHandle);
        }

        protected override void FetchNativeRemoteConfig()
        {
            if (managerHandle == IntPtr.Zero)
            {
                onReceiveRemoteConfig(null);
            }
            else
            {
                SuperfineSDKManager_FetchRemoteConfig(managerHandle, remoteConfigHandlerPointer);
            }
        }

        protected override void RegisterNativeStartCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddStartCallback(managerHandle, startHandlerPointer, 0);
        }

        protected override void UnregisterNativeStartCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveStartCallback(managerHandle, 0);
        }

        protected override void RegisterNativeStopCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddStopCallback(managerHandle, stopHandlerPointer, 0);
        }

        protected override void UnregisterNativeStopCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveStopCallback(managerHandle, 0);
        }

        protected override void RegisterNativePauseCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddPauseCallback(managerHandle, pauseHandlerPointer, 0);
        }

        protected override void UnregisterNativePauseCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemovePauseCallback(managerHandle, 0);
        }

        protected override void RegisterNativeResumeCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddResumeCallback(managerHandle, resumeHandlerPointer, 0);
        }

        protected override void UnregisterNativeResumeCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveResumeCallback(managerHandle, 0);
        }

        protected override void RegisterNativeDeepLinkCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddDeepLinkCallback(managerHandle, deepLinkHandlerPointer, 0);
        }

        protected override void UnregisterNativeDeepLinkCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveDeepLinkCallback(managerHandle, 0);
        }

        protected override void RegisterNativePushTokenCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddPushTokenCallback(managerHandle, pushTokenHandlerPointer, 0);
        }

        protected override void UnregisterNativePushTokenCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemovePushTokenCallback(managerHandle, 0);
        }

        protected override void RegisterNativeSendEventCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddSendEventCallback(managerHandle, sendEventHandlerPointer, 0);
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveSendEventCallback(managerHandle, 0);
        }

        public override void GdprForgetMe()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_GdprForgetMe(managerHandle);
        }

        public override void DisableThirdPartySharing()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_DisableThirdPartySharing(managerHandle);
        }

        public override void EnableThirdPartySharing()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_EnableThirdPartySharing(managerHandle);
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
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
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_Log2(managerHandle, eventName, eventFlag);
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWithIntValue2(managerHandle, eventName, data, eventFlag);
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWithStringValue2(managerHandle, eventName, data, eventFlag);
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
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

        public override void LogBootStart()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogBootStart(managerHandle);
        }

        public override void LogBootEnd()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogBootEnd(managerHandle);
        }

        public override void LogLevelStart(int id, string name)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLevelStart(managerHandle, id, name);
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLevelEnd(managerHandle, id, name, isSuccess ? 1 : 0);
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdLoad2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdClose2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdClick2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdImpression2(managerHandle, adUnit, adPlacementType, adPlacement);
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPInitialization(managerHandle, isSuccess ? 1 : 0);
        }

        public override void LogIAPRestorePurchase()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPRestorePurchase(managerHandle);
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPResult(managerHandle, pack, price, amount, currency, isSuccess ? 1 : 0);
        }

        public override void LogIAPReceipt_Apple(string receipt)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Apple(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Google(string data, string signature)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Google(managerHandle, data, signature);
        }

        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Amazon(managerHandle, userId, receiptId);
        }

        public override void LogIAPReceipt_Roku(string transactionId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Roku(managerHandle, transactionId);
        }

        public override void LogIAPReceipt_Windows(string receipt)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Windows(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Facebook(string receipt)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Facebook(managerHandle, receipt);
        }

        public override void LogIAPReceipt_Unity(string receipt)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_Unity(managerHandle, receipt);
        }

        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_AppStoreServer(managerHandle, transactionId);
        }

        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_GooglePlayProduct(managerHandle, productId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscription(managerHandle, subscriptionId, token);
        }

        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogIAPReceipt_GooglePlaySubscriptionv2(managerHandle, token);
        }

        public override void LogUpdateApp(string newVersion)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogUpdateApp(managerHandle, newVersion);
        }

        public override void LogRateApp()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogRateApp(managerHandle);
        }

        public override void LogLocation(double latitude, double longitude)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLocation(managerHandle, latitude, longitude);
        }

        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAuthorizationTrackingStatus(managerHandle, status);
        }

        public override void LogFacebookLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogFacebookLink(managerHandle, userId);
        }

        public override void LogFacebookUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogFacebookUnlink(managerHandle);
        }

        public override void LogInstagramLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogInstagramLink(managerHandle, userId);
        }

        public override void LogInstagramUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogInstagramUnlink(managerHandle);
        }

        public override void LogAppleLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAppleLink(managerHandle, userId);
        }

        public override void LogAppleUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAppleUnlink(managerHandle);
        }

        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAppleGameCenterLink(managerHandle, gamePlayerId);
        }

        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAppleGameCenterTeamLink(managerHandle, teamPlayerId);
        }

        public override void LogAppleGameCenterUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAppleGameCenterUnlink(managerHandle);
        }

        public override void LogGoogleLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGoogleLink(managerHandle, userId);
        }

        public override void LogGoogleUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGoogleUnlink(managerHandle);
        }

        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGooglePlayGameServicesLink(managerHandle, gamePlayerId);
        }

        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGooglePlayGameServicesDeveloperLink(managerHandle, developerPlayerKey);
        }

        public override void LogGooglePlayGameServicesUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGooglePlayGameServicesUnlink(managerHandle);
        }

        public override void LogLinkedInLink(string personId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLinkedInLink(managerHandle, personId);
        }

        public override void LogLinkedInUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLinkedInUnlink(managerHandle);
        }

        public override void LogMeetupLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogMeetupLink(managerHandle, userId);
        }

        public override void LogMeetupUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogMeetupUnlink(managerHandle);
        }

        public override void LogGitHubLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGitHubLink(managerHandle, userId);
        }

        public override void LogGitHubUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogGitHubUnlink(managerHandle);
        }

        public override void LogDiscordLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogDiscordLink(managerHandle, userId);
        }

        public override void LogDiscordUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogDiscordUnlink(managerHandle);
        }

        public override void LogTwitterLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogTwitterLink(managerHandle, userId);
        }

        public override void LogTwitterUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogTwitterUnlink(managerHandle);
        }

        public override void LogSpotifyLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogSpotifyLink(managerHandle, userId);
        }

        public override void LogSpotifyUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogSpotifyUnlink(managerHandle);
        }

        public override void LogMicrosoftLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogMicrosoftLink(managerHandle, userId);
        }

        public override void LogMicrosoftUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogMicrosoftUnlink(managerHandle);
        }

        public override void LogLINELink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLINELink(managerHandle, userId);
        }

        public override void LogLINEUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogLINEUnlink(managerHandle);
        }

        public override void LogVKLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogVKLink(managerHandle, userId);
        }

        public override void LogVKUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogVKUnlink(managerHandle);
        }

        public override void LogQQLink(string openId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogQQLink(managerHandle, openId);
        }

        public override void LogQQUnionLink(string unionId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogQQUnionLink(managerHandle, unionId);
        }

        public override void LogQQUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogQQUnlink(managerHandle);
        }

        public override void LogWeChatLink(string openId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWeChatLink(managerHandle, openId);
        }

        public override void LogWeChatUnionLink(string unionId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWeChatUnionLink(managerHandle, unionId);
        }

        public override void LogWeChatUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWeChatUnlink(managerHandle);
        }

        public override void LogTikTokLink(string openId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogTikTokLink(managerHandle, openId);
        }

        public override void LogTikTokUnionLink(string unionId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogTikTokUnionLink(managerHandle, unionId);
        }

        public override void LogTikTokUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogTikTokUnlink(managerHandle);
        }

        public override void LogWeiboLink(string userId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWeiboLink(managerHandle, userId);
        }

        public override void LogWeiboUnlink()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWeiboUnlink(managerHandle);
        }

        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
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
            if (managerHandle == IntPtr.Zero) return;
             SuperfineSDKManager_LogAccountUnlink(managerHandle, type);
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWalletLink2(managerHandle, wallet, type == null ? "ethereum" : type);
        }

        public override void AddUserPhoneNumber(int countryCode, string number)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddUserPhoneNumber(managerHandle, countryCode, number);
        }

        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveUserPhoneNumber(managerHandle, countryCode, number);
        }

        public override void AddUserEmail(string email)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_AddUserEmail(managerHandle, email);
        }

        public override void RemoveUserEmail(string email)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_RemoveUserEmail(managerHandle, email);
        }

        public override void SetUserName(string firstName, string lastName)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserName(managerHandle, firstName, lastName);
        }

        public override void SetUserFirstName(string firstName)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserFirstName(managerHandle, firstName);
        }

        public override void SetUserLastName(string lastName)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserLastName(managerHandle, lastName);
        }

        public override void SetUserCity(string city)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserCity(managerHandle, city);
        }

        public override void SetUserState(string state)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserState(managerHandle, state);
        }

        public override void SetUserCountry(string country)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserCountry(managerHandle, country);
        }

        public override void SetUserZipCode(string zipCode)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserZipCode(managerHandle, zipCode);
        }

        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserDateOfBirth(managerHandle, day, month, year);
        }

        public override void SetUserDateOfBirth(int day, int month)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserDateOfBirth2(managerHandle, day, month);
        }

        public override void SetUserYearOfBirth(int year)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserYearOfBirth(managerHandle, year);
        }

        public override void SetUserGender(UserGender gender)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetUserGender(managerHandle, gender);
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWalletUnlink2(managerHandle, wallet, type == null ? "ethereum" : type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogCryptoPayment3(managerHandle, pack, price, amount, currency, chain);
        }

        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdRevenue3(managerHandle, source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString());
        }

        public override void SetSteamDRMCheck(Func<uint, int> func)
        {
            if (managerHandle == IntPtr.Zero) return;

            steamDRMCheckFunc = func;

            if (steamDRMCheckFunc == null) 
            {
                SuperfineSDKManager_DisableSteamDRMCheck(managerHandle);
            }
            else
            {
                SuperfineSDKManager_EnableSteamDRMCheck(managerHandle, steamDRMCheckPointer);
            }
        }

        public override void OpenURL(string url)
        {
            if (managerHandle == IntPtr.Zero) return;

            int error = 0;
            SuperfineSDKManager_OpenURL(managerHandle, url, out error);

            if (error > 0)
            {
                Application.Quit();
            }
        }
    }
}
#endif