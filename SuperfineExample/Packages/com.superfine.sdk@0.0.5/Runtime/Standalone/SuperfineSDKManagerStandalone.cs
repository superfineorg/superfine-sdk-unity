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
        private delegate void SendEventDelegate(string eventName, string eventData, int requestCode);

        [MonoPInvokeCallback(typeof(SendEventDelegate))]
        private static void InvokeSendEventCallback(string eventName, string eventData, int requestCode)
        {
            if (onSendEvent != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSendEvent(eventName, eventData));
            }
        }

        private static readonly SendEventDelegate sendEventHandler = InvokeSendEventCallback;
        private static readonly IntPtr sendEventHandlerPointer = Marshal.GetFunctionPointerForDelegate(sendEventHandler);

        #region Interface
        [DllImport(pluginName)]
        private static extern IntPtr InitializationData_Create();
        [DllImport(pluginName)]
        private static extern void InitializationData_Destroy(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSavePath(IntPtr handle, string savePath);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetHost(IntPtr handle, string host);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetLogLevel(IntPtr handle, int level);
        [DllImport(pluginName)]
        private static extern void InitializationData_AddURIScheme(IntPtr handle, string uriScheme);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetRegisterURIScheme(IntPtr handle, int registerURIScheme);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomUserId(IntPtr handle, string customUserId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetConfigId(IntPtr handle, string configId);
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
        private static extern void InitializationData_SetEnableSave(IntPtr handle, int enableSave);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAutoStart(IntPtr handle, int autoStart);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetEnableCoppa(IntPtr handle, int enableCoppa);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomUserId(IntPtr handle, int customUserId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomConfigId(IntPtr handle, int customConfigId);

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
        private static extern void SuperfineSDKManager_Start(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Stop(IntPtr handle);
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
        private static extern string SuperfineSDKManager_GetVersion(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSendProxy(IntPtr handle, string proxy);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSendProxyCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSendCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSSLVerify(IntPtr handle, int enable);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetCustomUserId(IntPtr handle, string customUserId);
        [DllImport(pluginName)]
        private static extern string SuperfineSDKManager_GetUserId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetAdvertisingId(IntPtr handle, string advertisingId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_SetSendEventCallback(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);
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
        private static extern void SuperfineSDKManager_LogFacebookLogin(IntPtr handle, string facebookId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogFacebookLogout(IntPtr handle, string facebookId);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogUpdateGame(IntPtr handle, string newVersion);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogRateGame(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogLocation(IntPtr handle, double latitude, double longitude);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAuthorizationTrackingStatus(IntPtr handle, AuthorizationTrackingStatus status);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLogin(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLogout(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountLink(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAccountUnlink(IntPtr handle, string id, string type);
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
        private static extern void SuperfineSDKManager_LogAdRevenue(IntPtr handle, string network, double revenue, string currency);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdRevenue2(IntPtr handle, string network, double revenue, string currency, string mediation);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogAdRevenue3(IntPtr handle, string network, double revenue, string currency, string mediation, string networkData);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_Log(IntPtr handle, string eventName);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithIntValue(IntPtr handle, string eventName, int value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithStringValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithMapValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_LogWithJsonValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void SuperfineSDKManager_OpenURL(IntPtr handle, string url);
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

        protected SuperfineSDKManagerStandalone(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) : base(appId, appSecret, host, options)
        {
            IntPtr initializationData = InitializationData_Create();

            if (!string.IsNullOrEmpty(host))
            {
                InitializationData_SetHost(initializationData, host);
            }

            if (options == null)
            {
                options = new SuperfineSDKInitOptions();
            }

            InitializationData_SetLogLevel(initializationData, GetStandaloneLogLevel(options.logLevel));

            if (!string.IsNullOrEmpty(options.configId))
            {
                InitializationData_SetConfigId(initializationData, options.configId);
            }

            InitializationData_SetCustomConfigId(initializationData, options.waitConfigId ? 1 : 0);

            if (!string.IsNullOrEmpty(options.customUserId))
            {
                InitializationData_SetCustomUserId(initializationData, options.customUserId);
            }

            InitializationData_SetSavePath(initializationData, Application.persistentDataPath);

            InitializationData_SetSendDelay(initializationData, (int)options.flushInterval);
            InitializationData_SetFlushQueueSize(initializationData, options.flushQueueSize);

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            InitializationData_SetAppName(initializationData, Application.productName);
            InitializationData_SetAppVersion(initializationData, Application.version);
            InitializationData_SetBundleId(initializationData, options.bundleId);
            InitializationData_SetAppBuildNumber(initializationData, options.buildNumber);
#endif

#if UNITY_STANDALONE_LINUX
            InitializationData_SetDesktopTemplatePath(initializationData, Application.streamingAssetsPath + "/default.desktop.in");
            InitializationData_SetDesktopPath(initializationData, options.desktopPath);

            string desktopFilename = Application.productName.ToLower().Replace(' ', '-') + ".desktop";
            InitializationData_SetDesktopFilename(initializationData, desktopFilename);

            InitializationData_SetUpdateDesktopDatabase(initializationData, options.updateDesktopDatabase);
#endif

            InitializationData_SetRegisterURIScheme(initializationData, options.registerURIScheme ? 1 : 0);

            SuperfineSDKSettings settings = SuperfineSDK.GetSettings();
            if (settings != null && settings.uriSchemes != null)
            {
                int numUriSchemes = settings.uriSchemes.Length;
                if (numUriSchemes > 0)
                {
                    for (int i = 0; i < numUriSchemes; ++i)
                    {
                        InitializationData_AddURIScheme(initializationData, settings.uriSchemes[i]);
                    }
                }
            }

            InitializationData_SetStoreType(initializationData, options.storeType);

            InitializationData_SetAutoStart(initializationData, options.autoStart ? 1 : 0);

            InitializationData_SetEnableCoppa(initializationData, options.enableCoppa ? 1 : 0);

            managerHandle = SuperfineSDKManager_Create(appId, appSecret, initializationData);

            InitializationData_Destroy(initializationData);
            initializationData = IntPtr.Zero;

            if (!string.IsNullOrEmpty(options.proxy))
            {
                SuperfineSDKManager_SetSendProxy(managerHandle, options.proxy);
            }

            if (!string.IsNullOrEmpty(options.caPath))
            {
                SuperfineSDKManager_SetSendCertificateAuthority(managerHandle, options.caPath);
                SuperfineSDKManager_SetSendProxyCertificateAuthority(managerHandle, options.caPath);
            }

            if (!options.sslVerify)
            {
                SuperfineSDKManager_SetSSLVerify(managerHandle, 0);
            }

            GameObject go = new GameObject("SuperfineSDK");
            unityBehaviour = go.AddComponent<SuperfineSDKMonoBehaviour>();
            unityBehaviour.SetManager(this);
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
                Stop();
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
            managerHandle = IntPtr.Zero;
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

        public override string GetUserId()
        {
            if (managerHandle == IntPtr.Zero) return string.Empty;
            return SuperfineSDKManager_GetUserId(managerHandle);
        }

        protected override void RegisterNativeSendEventCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetSendEventCallback(managerHandle, sendEventHandlerPointer);
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetSendEventCallback(managerHandle, (IntPtr)0);
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

        public override void Log(string eventName, int data)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWithIntValue(managerHandle, eventName, data);
        }

        public override void Log(string eventName, string data)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWithStringValue(managerHandle, eventName, data);
        }

        public override void Log(string eventName, Dictionary<string, string> data = null)
        {
            if (data == null)
            {
                SuperfineSDKManager_Log(managerHandle, eventName);
            }
            else
            {
                SuperfineSDKManager_LogWithMapValue(managerHandle, eventName, GetMapString(data));
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data = null)
        {
            if (managerHandle == IntPtr.Zero) return;
            if (data == null)
            {
                SuperfineSDKManager_Log(managerHandle, eventName);
            }
            else
            {
                SuperfineSDKManager_LogWithJsonValue(managerHandle, eventName, data.ToString());
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

        public override void LogFacebookLogin(string facebookId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogFacebookLogin(managerHandle, facebookId);
        }

        public override void LogFacebookLogout(string facebookId)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogFacebookLogout(managerHandle, facebookId);
        }

        public override void LogUpdateGame(string newVersion)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogUpdateGame(managerHandle, newVersion);
        }

        public override void LogRateGame()
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogRateGame(managerHandle);
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

        public override void LogAccountLogin(string id, string type)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAccountLogin(managerHandle, id, type);
        }

        public override void LogAccountLogout(string id, string type)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAccountLogout(managerHandle, id, type);
        }

        public override void LogAccountLink(string id, string type)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAccountLink(managerHandle, id, type);
        }

        public override void LogAccountUnlink(string id, string type)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAccountUnlink(managerHandle, id, type);
        }

        public override void LogWalletLink(string wallet, string type = null)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWalletLink2(managerHandle, wallet, type);
        }

        public override void LogWalletUnlink(string wallet, string type = null)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogWalletUnlink2(managerHandle, wallet, type);
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogCryptoPayment3(managerHandle, pack, price, amount, currency, chain);
        }

        public override void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_LogAdRevenue3(managerHandle, network, revenue, currency, mediation == null ? "" : mediation, networkData == null ? "" : networkData.ToString());
        }

        public override void OpenURL(string url)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_OpenURL(managerHandle, url);
        }

        public override void SetPushToken(string pushToken)
        {
            if (managerHandle == IntPtr.Zero) return;
            SuperfineSDKManager_SetPushToken(managerHandle, pushToken);
        }
    }
}
#endif