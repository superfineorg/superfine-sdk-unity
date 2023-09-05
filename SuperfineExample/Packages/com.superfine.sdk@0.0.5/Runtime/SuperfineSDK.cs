using System.Collections.Generic;
using UnityEngine;
using System.Runtime.ExceptionServices;
using UnityEditor;
using System;

#if UNITY_EDITOR
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

namespace Superfine.Unity
{
    public static class SuperfineSDK
    {
        private static SuperfineSDKManager instance = null;

        private const string version = "0.0.6-unity";

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

        public static SuperfineSDKSettings GetSettings()
        {
            return Resources.Load<SuperfineSDKSettings>("SuperfineSettings");
        }

        public static SuperfineSDKManager CreateInstance(SuperfineSDKInitOptions options = null)
        {
            if (instance != null) return instance;

            SuperfineSDKSettings settings = GetSettings();
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
                    options = new SuperfineSDKInitOptions();
                }

                options.SetTenjinSdkKey(tenjinSdkKey);
            }
#endif

            instance = SuperfineSDKManager.CreateInstance(settings.appId, settings.appSecret, settings.host, options);

            if (options != null && options.captureDeepLinks)
            {
#if !UNITY_EDITOR
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
                RegisterDeepLink();
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
                CheckDeepLinkArguments(settings.uriSchemes, settings.associatedDomains);
#endif
#endif
            }

            return instance;
        }

        private static bool IsDeepLinkURL(string url, string[] uriSchemes, string[] associatedDomains)
        {
            if (uriSchemes != null)
            {
                int numUriSchemes = uriSchemes.Length;
                for (int i = 0; i < numUriSchemes; ++i)
                {
                    string prefix = uriSchemes[i] + "://";
                    if (url.StartsWith(prefix)) return true;
                }
            }

            if (associatedDomains != null)
            {
                int numAssociatedDomains = associatedDomains.Length;
                for (int i = 0; i < numAssociatedDomains; ++i)
                {
                    string httpsPrefix = "https://" + associatedDomains[i];
                    if (url.StartsWith(httpsPrefix)) return true;

                    string httpPrefix = "http://" + associatedDomains[i];
                    if (url.StartsWith(httpPrefix)) return true;
                }
            }

            return false;
        }

        private static void CheckDeepLinkArguments(string[] uriSchemes, string[] associatedDomains)
        {
            var args = System.Environment.GetCommandLineArgs();
            if (args != null)
            {
                int numArgs = args.Length;
                if (numArgs > 1)
                {
                    string arg = args[1];
                    if (IsDeepLinkURL(arg, uriSchemes, associatedDomains))
                    {
                        OnDeepLinkActivated(arg);
                    }
                }
            }
        }

        private static bool hasRegisteredDeepLink = false;

        private static void RegisterDeepLink()
        {
            if (hasRegisteredDeepLink) return;
            hasRegisteredDeepLink = true;

            Application.deepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
        }

        private static void UnregisterDeepLink()
        {
            if (!hasRegisteredDeepLink) return;
            hasRegisteredDeepLink = false;

            Application.deepLinkActivated -= OnDeepLinkActivated;
        }

        private static void OnDeepLinkActivated(string url)
        {
            if (instance == null || string.IsNullOrEmpty(url)) return;
            instance.OpenURL(url);
        }

        public static void Start()
        {
            if (instance == null) return;
            instance.Start();
        }

        public static void Stop()
        {
            if (instance == null) return;

            UnregisterDeepLink();

            instance.Stop();
            instance = null;
        }

        public static string GetVersion()
        {
            if (instance == null) return version;
            return version + "/" + instance.GetVersion();
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

        public static string GetUserId()
        {
            if (instance == null) return string.Empty;
            return instance.GetUserId();
        }

        public static void AddSendEventCallback(Action<string, string> callback)
        {
            if (instance == null) return;
            instance.AddSendEventCallback(callback);
        }

        public static void RemoveSendEventCallback(Action<string, string> callback)
        {
            if (instance == null) return;
            instance.RemoveSendEventCallback(callback);
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

        public static void Log(string eventName, int data)
        {
            if (instance == null) return;
            instance.Log(eventName, data);
        }

        public static void Log(string eventName, string data)
        {
            if (instance == null) return;
            instance.Log(eventName, data);
        }

        public static void Log(string eventName, Dictionary<string, string> data = null)
        {
            if (instance == null) return;
            instance.Log(eventName, data);
        }

        public static void Log(string eventName, SimpleJSON.JSONObject data = null)
        {
            if (instance == null) return;
            instance.Log(eventName, data);
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

        public static void LogIosIAPBuy(string pack, double price, int amount, string currency, string transactionId, string receipt)
        {
            if (instance == null) return;
            instance.LogIosIAPBuy(pack, price, amount, currency, transactionId, receipt);
        }

        public static void LogAndroidIAPBuy(string pack, double price, int amount, string currency, string data, string signature)
        {
            if (instance == null) return;
            instance.LogAndroidIAPBuy(pack, price, amount, currency, data, signature);
        }

        public static void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            if (instance == null) return;
            instance.LogIAPResult(pack, price, amount, currency, isSuccess);
        }

        public static void LogFacebookLogin(string facebookId)
        {
            if (instance == null) return;
            instance.LogFacebookLogin(facebookId);
        }

        public static void LogFacebookLogout(string facebookId)
        {
            if (instance == null) return;
            instance.LogFacebookLogout(facebookId);
        }

        public static void LogUpdateGame(string newVersion)
        {
            if (instance == null) return;
            instance.LogUpdateGame(newVersion);
        }

        public static void LogRateGame()
        {
            if (instance == null) return;
            instance.LogRateGame();
        }

        public static void LogLocation(double latitude, double longitude)
        {
            if (instance == null) return;
            instance.LogLocation(latitude, longitude);
        }

        public static void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            if (instance == null) return;
            instance.LogAuthorizationTrackingStatus(status);
        }

        public static void LogAccountLogin(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountLogin(id, type);
        }

        public static void LogAccountLogout(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountLogout(id, type);
        }

        public static void LogAccountLink(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountLink(id, type);
        }

        public static void LogAccountUnlink(string id, string type)
        {
            if (instance == null) return;
            instance.LogAccountUnlink(id, type);
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

        public static void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)
        {
            if (instance == null) return;
            instance.LogAdRevenue(network, revenue, currency, mediation, networkData);
        }

        //iOS only
        public static void RequestTrackingAuthorization(RequestAuthorizationTrackingCompleteHandler callback = null)
        {
            if (instance == null)
            {
                callback?.Invoke(AuthorizationTrackingStatus.NOT_DETERMINED);
                return;
            }

            instance.RequestTrackingAuthorization(callback);
        }

        //iOS only
        public static AuthorizationTrackingStatus GetTrackingAuthorizationStatus()
        {
            if (instance == null) return AuthorizationTrackingStatus.NOT_DETERMINED;
            return instance.GetTrackingAuthorizationStatus();
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue);
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue, coarseValue);
        }

        //iOS only
        public static void UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow)
        {
            if (instance == null) return;
            instance.UpdatePostbackConversionValue(conversionValue, coarseValue, lockWindow);
        }
    }
}