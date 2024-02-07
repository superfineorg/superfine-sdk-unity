using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKManagerStub : SuperfineSDKManagerBase
    {
        private static string Version = "0.0.13-stub";

        public override void Initialize(SuperfineSDKSettings settings)
        {
            Debug.Log(string.Format("Init with appId = {0}, appSecret = {1}, host = {2}", settings.appId, settings.appSecret, settings.host == null ? string.Empty : settings.host));
        }

        public override void Destroy()
        {
            Debug.Log(string.Format("Destroy Superfine SDK Manager"));
        }

        public override void Start()
        {
            Debug.Log(string.Format("Start Superfine SDK Manager"));

            if (onStart != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStart());
            }
        }

        public override void Stop()
        {
            Debug.Log(string.Format("Stop Superfine SDK Manager"));

            if (onStop != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onStop());
            }
        }

        public override void SetOffline(bool value)
        {
            Debug.Log(string.Format("Set Offline: {0}", value.ToString()));
        }

        public override string GetVersion()
        {
            return Version;
        }

        public override void SetConfigId(string configId)
        {
            Debug.Log(string.Format("Set Config Id: {0}", configId));
        }

        public override void SetCustomUserId(string customUserId)
        {
            Debug.Log(string.Format("Set Custom User Id: {0}", customUserId));
        }

        public override void SetAdvertisingId(string advertisingId)
        {
            Debug.Log(string.Format("Set Advertising Id: {0}", advertisingId));
        }

        public override string GetAppId()
        {
            return "APP_ID";
        }

        public override string GetUserId()
        {
            return "USER_ID";
        }

        public override string GetSessionId()
        {
            return "SESSION_ID";
        }

        public override string GetHost()
        {
            return string.Empty;
        }

        public override StoreType GetStoreType()
        {
            return StoreType.UNKNOWN;
        }

        public override string GetFacebookAppId()
        {
            return "FACEBOOK_APP_ID";
        }

        public override string GetInstagramAppId()
        {
            return "INSTAGRAM_APP_ID";
        }

        public override string GetAppleAppId()
        {
            return "APPLE_APP_ID";
        }

        public override string GetAppleSignInClientId()
        {
            return "APPLE_SIGNIN_CLIENT_ID";
        }

        public override string GetAppleDeveloperTeamId()
        {
            return "APPLE_DEVELOPER_TEAM_ID";
        }

        public override string GetGooglePlayGameServicesProjectId()
        {
            return "GOOGLE_PLAY_GAME_SERVICES_PROJECT_ID";
        }

        public override string GetGooglePlayDeveloperAccountId()
        {
            return "GOOGLE_PLAY_DEVELOPER_ACCOUNT_ID";
        }

        public override string GetLinkedInAppId()
        {
            return "LINKEDIN_APP_ID";
        }

        public override string GetQQAppId()
        {
            return "QQ_APP_ID";
        }

        public override string GetWeChatAppId()
        {
            return "WECHAT_APP_ID";
        }

        public override string GetTikTokAppId()
        {
            return "TIKTOK_APP_ID";
        }

        public override string GetDeepLinkUrl()
        {
            return string.Empty;
        }

        public override void SetPushToken(string token)
        {
            Debug.Log(string.Format("Set Push Token {0}", token));

            if (onSetPushToken != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetPushToken(token));
            }
        }

        public override string GetPushToken()
        {
            return string.Empty;
        }

        protected override void FetchNativeRemoteConfig()
        {
            onReceiveRemoteConfig(defaultRemoteConfig);
        }

        protected override void RegisterNativeStartCallback()
        {
            Debug.Log(string.Format("Register Native Start Callback"));
        }

        protected override void UnregisterNativeStartCallback()
        {
            Debug.Log(string.Format("Unregister Native Start Callback"));
        }

        protected override void RegisterNativeStopCallback()
        {
            Debug.Log(string.Format("Register Native Stop Callback"));
        }

        protected override void UnregisterNativeStopCallback()
        {
            Debug.Log(string.Format("Unregister Native Stop Callback"));
        }

        protected override void RegisterNativePauseCallback()
        {
            Debug.Log(string.Format("Register Native Pause Callback"));
        }

        protected override void UnregisterNativePauseCallback()
        {
            Debug.Log(string.Format("Unregister Native Pause Callback"));
        }

        protected override void RegisterNativeResumeCallback()
        {
            Debug.Log(string.Format("Register Native Resume Callback"));
        }

        protected override void UnregisterNativeResumeCallback()
        {
            Debug.Log(string.Format("Unregister Native Resume Callback"));
        }

        protected override void RegisterNativeDeepLinkCallback()
        {
            Debug.Log(string.Format("Register Native Deep Link Callback"));
        }

        protected override void UnregisterNativeDeepLinkCallback()
        {
            Debug.Log(string.Format("Unregister Native Deep Link Callback"));
        }

        protected override void RegisterNativePushTokenCallback()
        {
            Debug.Log(string.Format("Register Native Push Token Callback"));
        }

        protected override void UnregisterNativePushTokenCallback()
        {
            Debug.Log(string.Format("Unregister Native Push Token Callback"));
        }

        protected override void RegisterNativeSendEventCallback()
        {
            Debug.Log(string.Format("Register Native Send Event Callback"));
        }

        protected override void UnregisterNativeSendEventCallback()
        {
            Debug.Log(string.Format("Unregister Native Send Event Callback"));
        }

        public override void GdprForgetMe()
        {
            Debug.Log(string.Format("GDPR Forget Me"));
        }

        public override void DisableThirdPartySharing()
        {
            Debug.Log(string.Format("Disable 3rd-Party Sharing"));
        }

        public override void EnableThirdPartySharing()
        {
            Debug.Log(string.Format("Enable 3rd-Party Sharing"));
        }

        public override void LogThirdPartySharingSettings(SuperfineSDKThirdPartySharingSettings settings)
        {
            Debug.Log(string.Format("Log 3rd-Party Sharing Settings"));
        }

        public override void Log(string eventName, EventFlag eventFlag = EventFlag.NONE)
        {
            Debug.Log(string.Format("Log {0}: eventFlag = {1}", eventName, eventFlag));
        }

        public override void Log(string eventName, int data, EventFlag eventFlag = EventFlag.NONE)
        {
            Debug.Log(string.Format("Log Int {0}: data = {1}, eventFlag = {2}", eventName, data, eventFlag));
        }

        public override void Log(string eventName, string data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (string.IsNullOrEmpty(data))
            {
                Debug.Log(string.Format("Log {0}: eventFlag = {1}", eventName, eventFlag));
            }
            else
            {
                Debug.Log(string.Format("Log String {0}: data = {1}, eventFlag = {2}", eventName, data, eventFlag));
            }
        }

        public override void Log(string eventName, Dictionary<string, string> data, EventFlag eventFlag = EventFlag.NONE)
        {
            if (data == null)
            {
                Debug.Log(string.Format("Log {0}: eventFlag = {1}", eventName, eventFlag));
            }
            else
            {
                Debug.Log(string.Format("Log Map {0}: data = {1}, eventFlag = {2}", eventName, GetMapString(data), eventFlag));
            }
        }

        public override void Log(string eventName, SimpleJSON.JSONObject data, EventFlag eventFlag)
        {
            if (data == null)
            {
                Debug.Log(string.Format("Log {0}: eventFlag = {1}", eventName, eventFlag));
            }
            else
            {
                Debug.Log(string.Format("Log JSON {0}: data = {1}, eventFlag = {2}", eventName, data.ToString(), eventFlag));
            }
        }

        public override void LogBootStart()
        {
            Debug.Log(string.Format("Log Boot Start"));
        }

        public override void LogBootEnd()
        {
            Debug.Log(string.Format("Log Boot End"));
        }

        public override void LogLevelStart(int id, string name)
        {
            Debug.Log(string.Format("Log Level Start: id = {0}, name = {1}", id, name));
        }

        public override void LogLevelEnd(int id, string name, bool isSuccess)
        {
            Debug.Log(string.Format("Log Level End: id = {0}, name = {1}", id, name));
        }

        public override void LogAdLoad(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Load: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdClose(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Close: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Click: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN)
        {
            Debug.Log(string.Format("Log Ad Impression: adUnit = {0}, adPlacementType = {1}, adPlacement = {2}", adUnit, adPlacementType.ToString(), adPlacement.ToString()));
        }

        public override void LogIAPInitialization(bool isSuccess)
        {
            Debug.Log(string.Format("Log IAP Initialization: isSuccess = {0}", isSuccess));
        }

        public override void LogIAPRestorePurchase()
        {
            Debug.Log(string.Format("Log IAP Restore Purchase"));
        }

        public override void LogIAPResult(string pack, double price, int amount, string currency, bool isSuccess)
        {
            Debug.Log(string.Format("Log IAP Result: pack = {0}, price = {1}, amount = {2}, currency = {3}, isSuccess = {4}", pack, price, amount, currency, isSuccess));
        }

        public override void LogIAPReceipt_Apple(string receipt)
        {
            Debug.Log(string.Format("Log IAP Receipt Apple: receipt = {0}", receipt));
        }

        public override void LogIAPReceipt_Google(string data, string signature)
        {
            Debug.Log(string.Format("Log IAP Receipt Google: data = {0}, signature = {1}", data, signature));
        }

        public override void LogIAPReceipt_Amazon(string userId, string receiptId)
        {
            Debug.Log(string.Format("Log IAP Receipt Amazon: userId = {0}, receiptId = {1}", userId, receiptId));
        }

        public override void LogIAPReceipt_Roku(string transactionId)
        {
            Debug.Log(string.Format("Log IAP Receipt Roku: transactionId = {0}", transactionId));
        }

        public override void LogIAPReceipt_Windows(string receipt)
        {
            Debug.Log(string.Format("Log IAP Receipt Windows: receipt = {0}", receipt));
        }

        public override void LogIAPReceipt_Facebook(string receipt)
        {
            Debug.Log(string.Format("Log IAP Receipt Facebook: receipt = {0}", receipt));
        }

        public override void LogIAPReceipt_Unity(string receipt)
        {
            Debug.Log(string.Format("Log IAP Receipt Unity: receipt = {0}", receipt));
        }

        public override void LogIAPReceipt_AppStoreServer(string transactionId)
        {
            Debug.Log(string.Format("Log IAP Receipt AppStoreServer: transactionId = {0}", transactionId));
        }

        public override void LogIAPReceipt_GooglePlayProduct(string productId, string token)
        {
            Debug.Log(string.Format("Log IAP Receipt GooglePlayProduct: productId = {0}, token = {1}", productId, token));
        }

        public override void LogIAPReceipt_GooglePlaySubscription(string subscriptionId, string token)
        {
            Debug.Log(string.Format("Log IAP Receipt GooglePlaySubscription: subscriptionId = {0}, token = {1}", subscriptionId, token));
        }

        public override void LogIAPReceipt_GooglePlaySubscriptionv2(string token)
        {
            Debug.Log(string.Format("Log IAP Receipt GooglePlaySubscriptionv2: token = {0}", token));
        }

        public override void LogUpdateApp(string newVersion)
        {
            Debug.Log(string.Format("Log Update Game: newVersion = {0}", newVersion));
        }

        public override void LogRateApp()
        {
            Debug.Log(string.Format("Log Rate Game"));
        }

        public override void LogLocation(double latitude, double longitude)
        {
            Debug.Log(string.Format("Log Location: latitude = {0}, longitude = {1}", latitude, longitude));
        }

        public override void LogAuthorizationTrackingStatus(AuthorizationTrackingStatus status)
        {
            Debug.Log(string.Format("Log Authorization Tracking Status: status = {0}", status.ToString()));
        }

        public override void LogFacebookLink(string userId)
        {
            Debug.Log(string.Format("Log Facebook Link: userId = {0}", userId));
        }

        public override void LogFacebookUnlink()
        {
            Debug.Log(string.Format("Log Facebook Unlink"));
        }

        public override void LogInstagramLink(string userId)
        {
            Debug.Log(string.Format("Log Instagram Link: userId = {0}", userId));
        }

        public override void LogInstagramUnlink()
        {
            Debug.Log(string.Format("Log Instagram Unlink"));
        }

        public override void LogAppleLink(string userId)
        {
            Debug.Log(string.Format("Log Apple Link: userId = {0}", userId));
        }

        public override void LogAppleUnlink()
        {
            Debug.Log(string.Format("Log Apple Unlink"));
        }

        public override void LogAppleGameCenterLink(string gamePlayerId)
        {
            Debug.Log(string.Format("Log Apple Game Center Link: gamePlayerId = {0}", gamePlayerId));
        }

        public override void LogAppleGameCenterTeamLink(string teamPlayerId)
        {
            Debug.Log(string.Format("Log Apple Game Center Team Link: teamPlayerId = {0}", teamPlayerId));
        }

        public override void LogAppleGameCenterUnlink()
        {
            Debug.Log(string.Format("Log Apple Game Center Unlink"));
        }

        public override void LogGoogleLink(string userId)
        {
            Debug.Log(string.Format("Log Google Link: userId = {0}", userId));
        }

        public override void LogGoogleUnlink()
        {
            Debug.Log(string.Format("Log Google Unlink"));
        }

        public override void LogGooglePlayGameServicesLink(string gamePlayerId)
        {
            Debug.Log(string.Format("Log Google Play Game Services Link: gamePlayerId = {0}", gamePlayerId));
        }

        public override void LogGooglePlayGameServicesDeveloperLink(string developerPlayerKey)
        {
            Debug.Log(string.Format("Log Google Play Game Services Developer Link: developerPlayerKey = {0}", developerPlayerKey));
        }

        public override void LogGooglePlayGameServicesUnlink()
        {
            Debug.Log(string.Format("Log Google Play Game Services Unlink"));
        }

        public override void LogLinkedInLink(string personId)
        {
            Debug.Log(string.Format("Log LinkedIn Link: personId = {0}", personId));
        }

        public override void LogLinkedInUnlink()
        {
            Debug.Log(string.Format("Log LinkedIn Unlink"));
        }

        public override void LogMeetupLink(string userId)
        {
            Debug.Log(string.Format("Log Meetup Link: userId = {0}", userId));
        }

        public override void LogMeetupUnlink()
        {
            Debug.Log(string.Format("Log Meetup Unlink"));
        }

        public override void LogGitHubLink(string userId)
        {
            Debug.Log(string.Format("Log GitHub Link: userId = {0}", userId));
        }

        public override void LogGitHubUnlink()
        {
            Debug.Log(string.Format("Log GitHub Unlink"));
        }

        public override void LogDiscordLink(string userId)
        {
            Debug.Log(string.Format("Log Discord Link: userId = {0}", userId));
        }

        public override void LogDiscordUnlink()
        {
            Debug.Log(string.Format("Log Discord Unlink"));
        }

        public override void LogTwitterLink(string userId)
        {
            Debug.Log(string.Format("Log Twitter Link: userId = {0}", userId));
        }

        public override void LogTwitterUnlink()
        {
            Debug.Log(string.Format("Log Twitter Unlink"));
        }

        public override void LogSpotifyLink(string userId)
        {
            Debug.Log(string.Format("Log Spotify Link: userId = {0}", userId));
        }

        public override void LogSpotifyUnlink()
        {
            Debug.Log(string.Format("Log Spotify Unlink"));
        }

        public override void LogMicrosoftLink(string userId)
        {
            Debug.Log(string.Format("Log Microsoft Link: userId = {0}", userId));
        }

        public override void LogMicrosoftUnlink()
        {
            Debug.Log(string.Format("Log Microsoft Unlink"));
        }

        public override void LogLINELink(string userId)
        {
            Debug.Log(string.Format("Log LINE Link: userId = {0}", userId));
        }

        public override void LogLINEUnlink()
        {
            Debug.Log(string.Format("Log LINE Unlink"));
        }

        public override void LogVKLink(string userId)
        {
            Debug.Log(string.Format("Log VK Link: userId = {0}", userId));
        }

        public override void LogVKUnlink()
        {
            Debug.Log(string.Format("Log VK Unlink"));
        }

        public override void LogQQLink(string openId)
        {
            Debug.Log(string.Format("Log QQ Link: openId = {0}", openId));
        }

        public override void LogQQUnionLink(string unionId)
        {
            Debug.Log(string.Format("Log QQ Union Link: unionId = {0}", unionId));
        }

        public override void LogQQUnlink()
        {
            Debug.Log(string.Format("Log QQ Unlink"));
        }

        public override void LogWeChatLink(string openId)
        {
            Debug.Log(string.Format("Log WeChat Link: openId = {0}", openId));
        }

        public override void LogWeChatUnionLink(string unionId)
        {
            Debug.Log(string.Format("Log WeChat Union Link: unionId = {0}", unionId));
        }

        public override void LogWeChatUnlink()
        {
            Debug.Log(string.Format("Log WeChat Unlink"));
        }

        public override void LogTikTokLink(string openId)
        {
            Debug.Log(string.Format("Log TikTok Link: openId = {0}", openId));
        }

        public override void LogTikTokUnionLink(string unionId)
        {
            Debug.Log(string.Format("Log TikTok Union Link: unionId = {0}", unionId));
        }

        public override void LogTikTokUnlink()
        {
            Debug.Log(string.Format("Log TikTok Unlink"));
        }

        public override void LogWeiboLink(string userId)
        {
            Debug.Log(string.Format("Log Weibo Link: userId = {0}", userId));
        }

        public override void LogWeiboUnlink()
        {
            Debug.Log(string.Format("Log Weibo Unlink"));
        }

        public override void LogAccountLink(string id, string type, string scopeId = "", string scopeType = "")
        {
            if (string.IsNullOrEmpty(scopeId))
            {
                Debug.Log(string.Format("Log Account Link: id = {0}, type = {1}", id, type));
            }
            else if (string.IsNullOrEmpty(scopeType))
            {
                Debug.Log(string.Format("Log Account Link: id = {0}, type = {1}, scopeId = {2}", id, type, scopeId));
            }
            else
            {
                Debug.Log(string.Format("Log Account Link: id = {0}, type = {1}, scopeId = {2}, scopeType = {3}", id, type, scopeId, scopeType));
            }
        }

        public override void LogAccountUnlink(string type)
        {
            Debug.Log(string.Format("Log Account Unlink: type = {0}", type));
        }

        public override void AddUserPhoneNumber(int countryCode, string number)
        {
            Debug.Log(string.Format("Add User Phone Number: +{0} {1}", countryCode.ToString(), number));
        }

        public override void RemoveUserPhoneNumber(int countryCode, string number)
        {
            Debug.Log(string.Format("Remove User Phone Number: +{0} {1}", countryCode.ToString(), number));
        }

        public override void AddUserEmail(string email)
        {
            Debug.Log(string.Format("Add User Email: {0}", email));
        }

        public override void RemoveUserEmail(string email)
        {
            Debug.Log(string.Format("Remove User Email: {0}", email));
        }

        public override void SetUserName(string firstName, string lastName)
        {
            Debug.Log(string.Format("Set User Name: {0} {1}", firstName, lastName));
        }

        public override void SetUserFirstName(string firstName)
        {
            Debug.Log(string.Format("Set User First Name: {0}", firstName));
        }

        public override void SetUserLastName(string lastName)
        {
            Debug.Log(string.Format("Set User Last Name: {0}", lastName));
        }

        public override void SetUserCity(string city)
        {
            Debug.Log(string.Format("Set User City: {0}", city));
        }

        public override void SetUserState(string state)
        {
            Debug.Log(string.Format("Set User State: {0}", state));
        }

        public override void SetUserCountry(string country)
        {
            Debug.Log(string.Format("Set User Country: {0}", country));
        }

        public override void SetUserZipCode(string zipCode)
        {
            Debug.Log(string.Format("Set User ZIP Code: {0}", zipCode));
        }

        public override void SetUserDateOfBirth(int day, int month, int year)
        {
            Debug.Log(string.Format("Set User Date of Birth: {0:D2}/{1:D2}/{2:D4}", day, month, year));
        }

        public override void SetUserDateOfBirth(int day, int month)
        {
            Debug.Log(string.Format("Set User Date of Birth: {0:D2}/{1:D2}", day, month));
        }

        public override void SetUserYearOfBirth(int year)
        {
            Debug.Log(string.Format("Set User Year of Birth: {0:D4}", year));
        }

        public override void SetUserGender(UserGender gender)
        {
            Debug.Log(string.Format("Set User Gender: {0}", gender.ToString()));
        }

        public override void LogWalletLink(string wallet, string type = "ethereum")
        {
            Debug.Log(string.Format("Log Wallet Link: wallet = {0}, type = {1}", wallet, type));
        }

        public override void LogWalletUnlink(string wallet, string type = "ethereum")
        {
            Debug.Log(string.Format("Log Wallet Unlink: wallet = {0}, type = {1}", wallet, type));
        }

        public override void LogCryptoPayment(string pack, double price, int amount, string currency = "ETH", string chain = "ethereum")
        {
            Debug.Log(string.Format("Log Crypto Payment: pack = {0}, price = {1}, amount = {2}, currency = {3}, chain = {4}", pack, price, amount, currency, chain));
        }

        public override void LogAdRevenue(string source, double revenue, string currency, string network = "", SimpleJSON.JSONObject networkData = null)
        {
            Debug.Log(string.Format("Log Ad Revenue: source = {0}, revenue = {1}, currency = {2}, network = {3}, networkData = {4}", source, revenue, currency, network == null ? "" : network, networkData == null ? "" : networkData.ToString()));
        }

        public override void OpenURL(string url)
        {
            Debug.Log(string.Format("Open URL: {0}", url));

            if (onSetDeepLink != null)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => onSetDeepLink(url));
            }
        }
    }
}