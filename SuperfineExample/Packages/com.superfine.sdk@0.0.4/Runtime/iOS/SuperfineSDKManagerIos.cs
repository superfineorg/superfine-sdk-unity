#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AOT;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerIos : SuperfineSDKManagerBase
    {
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

        protected SuperfineSDKManagerIos(string appId, string appSecret, string host = null, SuperfineSDKInitOptions options = null) : base(appId, appSecret, host, options)
        {
            Init(GetString(appId, appSecret, host, options));
        }

        ~SuperfineSDKManagerIos()
        {
            Stop();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKInit(string arguments);
        private void Init(string arguments)
        {
            SuperfineSDKInit(arguments);
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

        public override void SetAdvertisingId(string advertisingId)
        {
            throw new System.Exception("SDK doesn't support setting advertising id");
        }

        [DllImport("__Internal")]
        private static extern string SuperfineSDKGetUserId();

        public override string GetUserId()
        {
            return SuperfineSDKGetUserId();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetSendEventCallback([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr callback);

        protected override void RegisterNativeSendEventCallback()
        {
            SuperfineSDKSetSendEventCallback(sendEventHandlerPointer);
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            SuperfineSDKSetSendEventCallback((IntPtr)0);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLog(string eventName);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithIntValue(string eventName, int value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithStringValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithMapValue(string eventName, string value);
        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWithJsonValue(string eventName, string value);

        public override void Log(string eventName, int data)
        {
            SuperfineSDKLogWithIntValue(eventName, data);
        }

        public override void Log(string eventName, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                SuperfineSDKLog(eventName);
            }
            else
            {
                SuperfineSDKLogWithStringValue(eventName, data);
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data = null)
        {
            if (data == null)
            {
                SuperfineSDKLog(eventName);
            }
            else
            {
                SuperfineSDKLogWithMapValue(eventName, GetMapString(data));
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data = null)
        {
            if (data == null)
            {
                SuperfineSDKLog(eventName);
            }
            else
            {
                SuperfineSDKLogWithJsonValue(eventName, data.ToString());
            }
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
        private static extern void SuperfineSDKLogIAPBuyStart(string pack, double price, int amount, string currency);
        public override void LogIAPBuyStart(string pack, double price, int amount, string currency)
        {
            SuperfineSDKLogIAPBuyStart(pack, price, amount, currency);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPBuyEnd(string pack, double price, int amount, string currency, bool isSuccess);
        public override void LogIAPBuyEnd(string pack, double price, int amount, string currency, bool isSuccess)
        {
            SuperfineSDKLogIAPBuyEnd(pack, price, amount, currency, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogIAPBuyEnd3(string pack, double price, int amount, string currency, string transactionId, string receipt, bool isSuccess);
        public override void LogAppleIAPBuyEnd(string pack, double price, int amount, string currency, string transactionId, string receipt,  bool isSuccess)
        {
            SuperfineSDKLogIAPBuyEnd3(pack, price, amount, currency, transactionId, receipt, isSuccess);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogFacebookLogin(string facebookId);
        public override void LogFacebookLogin(string facebookId)
        {
            SuperfineSDKLogFacebookLogin(facebookId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogFacebookLogout(string facebookId);
        public override void LogFacebookLogout(string facebookId)
        {
            SuperfineSDKLogFacebookLogout(facebookId);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogUpdateGame(string newVersion);
        public override void LogUpdateGame(string newVersion)
        {
            SuperfineSDKLogUpdateGame(newVersion);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogRateGame();
        public override void LogRateGame()
        {
            SuperfineSDKLogRateGame();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAuthorizationTrackingStatus(int status);
        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            SuperfineSDKLogAuthorizationTrackingStatus((int)status);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLogin(string accountId, string type);
        public override void LogAccountLogin(string id, string type)
        {
            SuperfineSDKLogAccountLogin(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLogout(string accountId, string type);
        public override void LogAccountLogout(string id, string type)
        {
            SuperfineSDKLogAccountLogout(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountLink(string accountId, string type);
        public override void LogAccountLink(string id, string type)
        {
            SuperfineSDKLogAccountLink(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogAccountUnlink(string accountId, string type);
        public override void LogAccountUnlink(string id, string type)
        {
            SuperfineSDKLogAccountUnlink(id, type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWalletLink(string wallet, string type);
        public override void LogWalletLink(string wallet, string type = null)
        {
            SuperfineSDKLogWalletLink(wallet, type == null ? "ethereum" : type);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKLogWalletUnlink(string wallet, string type);
        public override void LogWalletUnlink(string wallet, string type = null)
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
        private static extern void SuperfineSDKLogAdRevenue(string network, double revenue, string currency, string mediation, string networkData);
        public override void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)
        {
            SuperfineSDKLogAdRevenue(network, revenue, currency, mediation == null ? "" : mediation, networkData == null ? "" : networkData.ToString());
        }

        private delegate void _RequestAuthorizationTrackingCompleteNativeHandler(int status);

        [DllImport("__Internal")]
        private static extern int SuperfineSDKRequestTrackingAuthorization(_RequestAuthorizationTrackingCompleteNativeHandler callback);

        private static RequestAuthorizationTrackingCompleteHandler _requestAuthorizationTrackingCompleteCallback = null;

        [MonoPInvokeCallback(typeof(_RequestAuthorizationTrackingCompleteNativeHandler))]
        public static void AppTransparencyTrackingRequestCompleted(int status)
        {
            if (_requestAuthorizationTrackingCompleteCallback != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    _requestAuthorizationTrackingCompleteCallback((AuthorizationTrackingStatus)status);
                    _requestAuthorizationTrackingCompleteCallback = null;
                });
            }
        }

        public override void RequestTrackingAuthorization(RequestAuthorizationTrackingCompleteHandler callback = null)
        {
            _requestAuthorizationTrackingCompleteCallback = callback;
            SuperfineSDKRequestTrackingAuthorization(AppTransparencyTrackingRequestCompleted);
        }

        [DllImport("__Internal")]
        private static extern int SuperfineSDKGetTrackingAuthorizationStatus();
        public override AuthorizationTrackingStatus GetTrackingAuthorizationStatus()
        {
            return (AuthorizationTrackingStatus)SuperfineSDKGetTrackingAuthorizationStatus();
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue(int conversionValue);
        public override void UpdatePostbackConversionValue(int conversionValue)
        {
            SuperfineSDKUpdatePostbackConversionValue(conversionValue);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue2(int conversionValue, string coarseValue);
        public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            SuperfineSDKUpdatePostbackConversionValue2(conversionValue, coarseValue);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKUpdatePostbackConversionValue3(int conversionValue, string coarseValue, bool lockWindow);
        public override void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            SuperfineSDKUpdatePostbackConversionValue3(conversionValue, coarseValue, lockWindow);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKOpenURL(string url);
        public override void OpenURL(string url)
        {
            SuperfineSDKOpenURL(url);
        }

        [DllImport("__Internal")]
        private static extern void SuperfineSDKSetDeviceToken(string pushToken);
        public override void SetPushToken(string pushToken)
        {
            SuperfineSDKSetDeviceToken(pushToken);
        }
    }
}
#endif