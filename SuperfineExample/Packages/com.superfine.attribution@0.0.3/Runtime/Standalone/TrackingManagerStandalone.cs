#if UNITY_STANDALONE && !UNITY_EDITOR
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Superfine.Tracking.Unity
{
    public class TrackingManagerStandalone : TrackingManagerBase
    {
#if (UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR
        private const string pluginName = "__Internal";
#else
        private const string pluginName = "superfine-attribution-cpp";
#endif

        #region Interface
        [DllImport(pluginName)]
        private static extern IntPtr InitializationData_Create();
        [DllImport(pluginName)]
        private static extern void InitializationData_Destroy(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetSavePath(IntPtr handle, string savePath);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetUserId(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppVersion(IntPtr handle, string appVersion);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetAppBuildNumber(IntPtr handle, string appBuildNumber);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetBundleId(IntPtr handle, string bundleId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCountry(IntPtr handle, string country);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetStoreType(IntPtr handle, StoreType storeType);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetDeviceModel(IntPtr handle, string deviceModel);
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
        private static extern void InitializationData_SetCustomUserId(IntPtr handle, int customUserId);
        [DllImport(pluginName)]
        private static extern void InitializationData_SetCustomConfigId(IntPtr handle, int customConfigId);
        [DllImport(pluginName)]
        private static extern IntPtr TrackingManager_Create(string appId, string appSecret, IntPtr initializationData);
        [DllImport(pluginName)]
        private static extern void TrackingManager_Destroy(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_Start(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_Stop(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_OnPause(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_OnResume(IntPtr handle);
        [DllImport(pluginName)]
        private static extern string TrackingManager_GetVersion(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetSendVerbose(IntPtr handle, int enable);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetSendProxy(IntPtr handle, string proxy);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetSendProxyCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetSendCertificateAuthority(IntPtr handle, string path);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetUserId(IntPtr handle, string userId);
        [DllImport(pluginName)]
        private static extern string TrackingManager_GetUserId(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_SetConfigId(IntPtr handle, string configId);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackBootStart(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackBootEnd(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackLevelStart(IntPtr handle, int id, string name);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackLevelEnd(IntPtr handle, int id, string name, int success);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdLoad(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdLoad2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdClose(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdClose2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdClick(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdClick2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdImpression(IntPtr handle, string adUnit, AdPlacementType adPlacementType);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAdImpression2(IntPtr handle, string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackIAPInitialization(IntPtr handle, int success);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackIAPRestorePurchase(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackIAPBuyStart(IntPtr handle, string pack, float price, int amount, string currency);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackIAPBuyEnd(IntPtr handle, string pack, float price, int amount, string currency, int success);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackFacebookLogin(IntPtr handle, string facebookId);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackFacebookLogout(IntPtr handle, string facebookId);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackUpdateGame(IntPtr handle, string newVersion);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackRateGame(IntPtr handle);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAuthorizationTrackingStatus(IntPtr handle, AuthorizationTrackingStatus status);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAccountLogin(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAccountLogout(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAccountLink(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackAccountUnlink(IntPtr handle, string id, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWalletLink(IntPtr handle, string wallet);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWalletLink2(IntPtr handle, string wallet, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWalletUnlink(IntPtr handle, string wallet);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWalletUnlink2(IntPtr handle, string wallet, string type);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackCryptoPayment(IntPtr handle, string pack, float price, int amount);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackCryptoPayment2(IntPtr handle, string pack, float price, int amount, string currency);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackCryptoPayment3(IntPtr handle, string pack, float price, int amount, string currency, string chain);
        [DllImport(pluginName)]
        private static extern void TrackingManager_Track(IntPtr handle, string eventName);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWithIntValue(IntPtr handle, string eventName, int value);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWithStringValue(IntPtr handle, string eventName, string value);
        [DllImport(pluginName)]
        private static extern void TrackingManager_TrackWithJsonValue(IntPtr handle, string eventName, string value);
        #endregion

        private TrackingManagerStandaloneMonoBehaviour unityBehaviour = null;

        private IntPtr trackingManager = IntPtr.Zero;

        protected TrackingManagerStandalone(string appId, string appSecret, TrackingManagerInitOptions options = null) : base(appId, appSecret, options)
        {
            IntPtr initializationData = InitializationData_Create();

            if (options == null)
            {
                options = new TrackingManagerInitOptions();
            }

            InitializationData_SetCustomUserId(initializationData, options.customUserId ? 1 : 0);
            InitializationData_SetCustomConfigId(initializationData, options.waitConfigId ? 1 : 0);

            InitializationData_SetUserId(initializationData, options.userId);

            InitializationData_SetSavePath(initializationData, Application.persistentDataPath);

            InitializationData_SetSendDelay(initializationData, (int)options.flushInterval);
            InitializationData_SetFlushQueueSize(initializationData, options.flushQueueSize);

            InitializationData_SetAppVersion(initializationData, Application.version);
            //InitializationData_SetAppBuildNumber(initializationData, "1");

            InitializationData_SetBundleId(initializationData, Application.identifier);
            //InitializationData_SetCountry(initializationData, "unknown");

            InitializationData_SetDeviceModel(initializationData, SystemInfo.deviceModel);

            InitializationData_SetStoreType(initializationData, options.storeType);

            trackingManager = TrackingManager_Create(appId, appSecret, initializationData);

            InitializationData_Destroy(initializationData);
            initializationData = IntPtr.Zero;

            if (!string.IsNullOrEmpty(options.proxy))
            {
                TrackingManager_SetSendProxy(trackingManager, options.proxy);
            }

            if (!string.IsNullOrEmpty(options.caPath))
            {
                TrackingManager_SetSendCertificateAuthority(trackingManager, options.caPath);
                TrackingManager_SetSendProxyCertificateAuthority(trackingManager, options.caPath);
            }

            GameObject go = new GameObject("TrackingManagerMonoBehaviour");
            unityBehaviour = go.AddComponent<TrackingManagerStandaloneMonoBehaviour>();
            unityBehaviour.SetManager(this);

            if (options.autoStart)
            {
                Start();
            }
        }

        public override void Execute(string eventName, object param = null)
        {
            if (trackingManager == IntPtr.Zero) return;

            if (eventName == "pause")
            {
                TrackingManager_OnPause(trackingManager);
            }
            else if (eventName == "resume")
            {
                TrackingManager_OnResume(trackingManager);
            }
            else if (eventName == "destroy")
            {
                TrackingManager_Stop(trackingManager);
                trackingManager = IntPtr.Zero;
            }
        }

        public override void Start()
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_Start(trackingManager);
        }

        public override string GetVersion()
        {
             if (trackingManager == IntPtr.Zero) return string.Empty;
             return TrackingManager_GetVersion(trackingManager);
        }

        public override void SetConfigId(string configId)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_SetConfigId(trackingManager, configId);
        }

        public override void SetUserId(string userId)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_SetUserId(trackingManager, userId);
        }

        public override string GetUserId()
        {
            if (trackingManager == IntPtr.Zero) return string.Empty;
            return TrackingManager_GetUserId(trackingManager);
        }

        public override void Track(string eventName, int data)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackWithIntValue(trackingManager, eventName, data);
        }

        public override void Track(string eventName, string data)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackWithStringValue(trackingManager, eventName, data);
        }

        public override void Track(string eventName, TrackBaseData data = null)
        {
            if (trackingManager == IntPtr.Zero) return;
            if (data == null)
            {
                TrackingManager_Track(trackingManager, eventName);
            }
            else
            {
                TrackingManager_TrackWithJsonValue(trackingManager, eventName, JsonConvert.SerializeObject(data));
            }
        }

        public override void TrackBootStart()
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackBootStart(trackingManager);
        }

        public override void TrackBootEnd()
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackBootEnd(trackingManager);
        }

        public override void TrackLevelStart(int id, string name)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackLevelStart(trackingManager, id, name);
        }

        public override void TrackLevelEnd(int id, string name, bool isSuccess)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackLevelEnd(trackingManager, id, name, isSuccess ? 1 : 0);
        }

        public override void TrackAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAdLoad2(trackingManager, adUnit, adPlacementType, adPlacement);
        }

        public override void TrackAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAdClose2(trackingManager, adUnit, adPlacementType, adPlacement);
        }

        public override void TrackAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAdClick2(trackingManager, adUnit, adPlacementType, adPlacement);
        }

        public override void TrackAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAdImpression2(trackingManager, adUnit, adPlacementType, adPlacement);
        }

        public override void TrackIAPInitialization(bool isSuccess)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackIAPInitialization(trackingManager, isSuccess ? 1 : 0);
        }

        public override void TrackIAPRestorePurchase()
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackIAPRestorePurchase(trackingManager);
        }

        public override void TrackIAPBuyStart(string pack, float price, int amount, string currency)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackIAPBuyStart(trackingManager, pack, price, amount, currency);
        }

        public override void TrackIAPBuyEnd(string pack, float price, int amount, string currency, bool isSuccess)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackIAPBuyEnd(trackingManager, pack, price, amount, currency, isSuccess ? 1 : 0);
        }

        public override void TrackFacebookLogin(string facebookId)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackFacebookLogin(trackingManager, facebookId);
        }

        public override void TrackFacebookLogout(string facebookId)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackFacebookLogout(trackingManager, facebookId);
        }

        public override void TrackUpdateGame(string newVersion)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackUpdateGame(trackingManager, newVersion);
        }

        public override void TrackRateGame()
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackRateGame(trackingManager);
        }

        public override void TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAuthorizationTrackingStatus(trackingManager, status);
        }

        public override void TrackAccountLogin(string id, string type)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAccountLogin(trackingManager, id, type);
        }

        public override void TrackAccountLogout(string id, string type)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAccountLogout(trackingManager, id, type);
        }

        public override void TrackAccountLink(string id, string type)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAccountLink(trackingManager, id, type);
        }

        public override void TrackAccountUnlink(string id, string type)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackAccountUnlink(trackingManager, id, type);
        }

        public override void TrackWalletLink(string wallet, string type = null)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackWalletLink2(trackingManager, wallet, type);
        }

        public override void TrackWalletUnlink(string wallet, string type = null)
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackWalletUnlink2(trackingManager, wallet, type);
        }

        public override void TrackCryptoPayment(string pack, float price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            if (trackingManager == IntPtr.Zero) return;
            TrackingManager_TrackCryptoPayment3(trackingManager, pack, price, amount, currency, chain);
        }
    }
}
#endif