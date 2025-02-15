using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Superfine.Unity;
using Facebook.Unity;
using System.Text;
using System;

public class SampleScene : MonoBehaviour
{
    public string userCity;
    public string userState;
    public string userCountry;
    public string userZipCode;
    public string userEmail;
    public int userPhoneCountryCode;
    public string userPhoneNumber;
    public string userFirstName;
    public string userLastName;
    public Vector3Int userDateOfBirth;
    public UserGender userGender;

    public int levelId;
    public string levelName;

    public string testWalletAddress;
    public string testWalletType = "ethereum";

    public string testAdUnit;

    public string testPushToken;

    public AdPlacementType testAdPlacementType = AdPlacementType.INTERSTITIAL;
    public AdPlacement testAdPlacement = AdPlacement.UNKNOWN;

    public string testIAPPackId;

    public string testIAPReceipt;
    public string testIAPData;
    public string testIAPSignature;
    public string testIAPReceiptId;
    public string testIAPStore;
    public string testIAPTransactionId;
    public string testIAPUserId;
    public string testIAPPayload;
    public string testIAPToken;

    public string testFacebookId;

    public string testAccountId;
    public string testAccountType;

    public string testCryptoPackId;

    public string testAdNetworkId;

    public Vector2 testLocation;

    public TextMeshProUGUI userIdText;
    public TextMeshProUGUI sessionIdText;

    public TextMeshProUGUI hostText;
    public TextMeshProUGUI storeTypeText;

    public TextMeshProUGUI deepLinkUrlText;

    public Toggle autoStartToggle;

    public Toggle offlineToggle;
    private bool offline;

    public TextMeshProUGUI thirdPartyIdsText;

    public GameObject startButtonObject;
    public GameObject stopButtonObject;

    public GameObject testButtonPanel;
    public GameObject testAllButtonObject;

    class SampleModuleSettings : SuperfineSDKModuleSettings
    {
        public override string GetModuleName()
        {
            return "SampleModule";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SampleModule(this);
        }
    }

    class SampleModule : SuperfineSDKModule
    {
        public SampleModule(SuperfineSDKModuleSettings settings) : base(settings)
        {
        }

        protected override void Initialize(SuperfineSDKModuleSettings settings)
        {
            base.Initialize(settings);
           SuperfineSDK.AddStartCallback(OnStart);
        }

        private void OnStart()
        {
            Debug.Log("ON START MODULE");
        }
    }

    void Awake()
    {
#if !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_LINUX
        if (!FB.IsInitialized)
        {
            FB.Init(InitFacebookCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
#endif

        SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources().Clone();
        settings.autoStart = false;

        // Initialize SuperfineSDK.
        SuperfineSDK.Initialize(settings, OnInitialized);

        // Note: Call Unity.SuperfineSDK.Start(); after MMP and Mediation SDK initialization.
        SuperfineSDK.Start();

        SuperfineSDKFacebook.RegisterSendEvent();

        SuperfineSDK.AddSendEventCallback(PrintEvent);
        SuperfineSDK.AddStartCallback(OnStartSDK);
        SuperfineSDK.AddStopCallback(OnStopSDK);
        SuperfineSDK.AddPauseCallback(OnPauseSDK);
        SuperfineSDK.AddResumeCallback(OnResumeSDK);

        SuperfineSDK.AddDeepLinkCallback(OnSetDeepLink);

        userIdText.text = StandardizeInfoText(SuperfineSDK.GetUserId(), "USER ID");

        autoStartToggle.isOn = settings.autoStart;

        UpdateStatusDisplay(false);
    }

#if !UNITY_STANDALONE_OSX && !UNITY_STANDALONE_LINUX
    private void InitFacebookCallback()
    {
        if (FB.IsInitialized)
        {
            SuperfineSDKFacebook.OnFacebookInitialized();
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
#endif

    public void OnInitialized()
    {

    }

    public void OnOfflineStatusChanged(bool value)
    {
        if (value == offline) return;

        offline = value;
        SuperfineSDK.SetOffline(value);
    }

    public void TestStart()
    {
        StartTracking();
    }

    public void TestStop()
    {
        SuperfineSDK.Stop();
    }

    private void UpdateStatusDisplay(bool started)
    {
        startButtonObject.SetActive(!started);
        stopButtonObject.SetActive(started);
    }

    private void PrintEvent(string eventName, string eventData)
    {
        Debug.LogError("SEND EVENT: " + eventName + " " + eventData);
    }

    private string StandardizeInfoText(string text, string label = null)
    {
        string content = string.IsNullOrEmpty(text) ? "EMPTY" : text;

        if (string.IsNullOrEmpty(label)) return content;
        else return label + ": " + content;
    }

    private void UpdateThirdPartyIdsText(StringBuilder builder, string key, Func<string> func)
    {
        string value = func();
        if (!string.IsNullOrEmpty(value))
        {
            if (builder.Length > 0) builder.Append(' ');
            builder.Append(key).Append(':').Append(value);
        }
    }

    private string GenerateThirdPartyIdsText()
    {
        StringBuilder builder = new StringBuilder();

        UpdateThirdPartyIdsText(builder, "facebook", SuperfineSDK.GetFacebookAppId);
        UpdateThirdPartyIdsText(builder, "instagram", SuperfineSDK.GetInstagramAppId);
        UpdateThirdPartyIdsText(builder, "apple", SuperfineSDK.GetAppleAppId);
        UpdateThirdPartyIdsText(builder, "appleSignInClient", SuperfineSDK.GetAppleSignInClientId);
        UpdateThirdPartyIdsText(builder, "appleDeveloperTeam", SuperfineSDK.GetAppleDeveloperTeamId);
        UpdateThirdPartyIdsText(builder, "googlePGSProject", SuperfineSDK.GetGooglePlayGameServicesProjectId);
        UpdateThirdPartyIdsText(builder, "googlePlayAccount", SuperfineSDK.GetGooglePlayDeveloperAccountId);
        UpdateThirdPartyIdsText(builder, "linkedin", SuperfineSDK.GetLinkedInAppId);
        UpdateThirdPartyIdsText(builder, "qq", SuperfineSDK.GetQQAppId);
        UpdateThirdPartyIdsText(builder, "wechat", SuperfineSDK.GetWeChatAppId);
        UpdateThirdPartyIdsText(builder, "tiktok", SuperfineSDK.GetTikTokAppId);

        return builder.ToString();
    }

    private void OnStartSDK()
    {
        Debug.LogError("START SDK");
        UpdateStatusDisplay(true);

        sessionIdText.text = StandardizeInfoText(SuperfineSDK.GetSessionId(), "SESSION ID");

        hostText.text = StandardizeInfoText(SuperfineSDK.GetHost(), "HOST");
        storeTypeText.text = StandardizeInfoText(SuperfineSDK.GetStoreType().ToString(), "STORE");

        thirdPartyIdsText.text = GenerateThirdPartyIdsText();
    }

    private void OnStopSDK()
    {
        Debug.LogError("STOP SDK");
        UpdateStatusDisplay(false);
        autoStartToggle.isOn = false;
    }

    private void OnPauseSDK()
    {
        Debug.LogError("PAUSE SDK");
    }

    private void OnResumeSDK()
    {
        Debug.LogError("RESUME SDK");
        sessionIdText.text = StandardizeInfoText(SuperfineSDK.GetSessionId(), "SESSION ID");
    }

    private void OnSetDeepLink(string url)
    {
        deepLinkUrlText.text = StandardizeInfoText(url, "DEEP LINK");
    }

    private void StartTracking()
    {
        SuperfineSDK.Start();

        startButtonObject.SetActive(false);
        stopButtonObject.SetActive(true);
    }

    public void TestBoot()
    {
        StartCoroutine(TestBootCoroutine());
    }

    private IEnumerator TestBootCoroutine()
    {
        SuperfineSDK.LogBootStart();
        yield return new WaitForSecondsRealtime(3.0f);
        SuperfineSDK.LogBootEnd();
    }

    public void TestLinkWallet()
    {
        SuperfineSDK.LogWalletLink(testWalletAddress, testWalletType);
    }

    public void TestUnlinkWallet()
    {
        SuperfineSDK.LogWalletUnlink(testWalletAddress, testWalletType);
    }

    public void TestLevelStart()
    {
        SuperfineSDK.LogLevelStart(levelId, levelName);
    }

    public void TestLevelCompleted()
    {
        SuperfineSDK.LogLevelEnd(levelId, levelName, true);
    }

    public void TestLevelFailed()
    {
        SuperfineSDK.LogLevelEnd(levelId, levelName, false);
    }

    public void TestAdImpression()
    {
        SuperfineSDK.LogAdImpression(testAdUnit, testAdPlacementType, testAdPlacement);
    }

    public void TestPrivacy()
    {
        SuperfineSDKThirdPartySharingSettings settings = new SuperfineSDKThirdPartySharingSettings();
        settings.AddValue("facebook", "data_processing_options_country", "1");
        settings.AddValue("facebook", "data_processing_options_state", "1000");
        settings.AddFlag("facebook", "install", true);
        settings.AddFlag("facebook", "events", false);
        settings.AddFlag("facebook", "sessions", false);
        SuperfineSDK.LogThirdPartySharingSettings(settings);

        SuperfineSDK.DisableThirdPartySharing();
        SuperfineSDK.EnableThirdPartySharing();

        SuperfineSDK.GdprForgetMe();
    }

    public void TestPushToken()
    {
        if (string.IsNullOrEmpty(testPushToken)) return;
        SuperfineSDK.SetPushToken(testPushToken);
    }

    public void TestAll()
    {
        SuperfineSDK.SetUserCity(userCity);
        SuperfineSDK.SetUserState(userState);
        SuperfineSDK.SetUserCountry(userCountry);
        SuperfineSDK.SetUserZipCode(userZipCode);
        SuperfineSDK.AddUserEmail(userEmail);
        SuperfineSDK.AddUserPhoneNumber(userPhoneCountryCode, userPhoneNumber);
        SuperfineSDK.SetUserName(userFirstName, userLastName);
        SuperfineSDK.SetUserDateOfBirth(userDateOfBirth.x, userDateOfBirth.y, userDateOfBirth.z);
        SuperfineSDK.SetUserGender(userGender);

        SuperfineSDK.LogFacebookLink("FACEBOOK_USER_ID");
        SuperfineSDK.LogInstagramLink("INSTAGRAM_USER_ID");
        SuperfineSDK.LogAppleLink("APPLE_USER_ID");
        SuperfineSDK.LogAppleGameCenterLink("APPLE_GAME_CENTER_USER_ID");
        SuperfineSDK.LogGoogleLink("GOOGLE_USER_ID");
        SuperfineSDK.LogGooglePlayGameServicesLink("GOOGLE_PLAY_GAME_SERVICES_USER_ID");
        SuperfineSDK.LogLinkedInLink("LINKEDIN_USER_ID");
        SuperfineSDK.LogMeetupLink("MEETUP_USER_ID");
        SuperfineSDK.LogGitHubLink("GITHUB_USER_ID");
        SuperfineSDK.LogDiscordLink("DISCORD_USER_ID");
        SuperfineSDK.LogTwitterLink("TWITTER_USER_ID");
        SuperfineSDK.LogSpotifyLink("SPOTIFY_USER_ID");
        SuperfineSDK.LogMicrosoftLink("MICROSOFT_USER_ID");
        SuperfineSDK.LogLINELink("LINE_USER_ID");
        SuperfineSDK.LogVKLink("VK_USER_ID");
        SuperfineSDK.LogQQLink("QQ_USER_ID");
        SuperfineSDK.LogWeChatLink("WECHAT_USER_ID");
        SuperfineSDK.LogTikTokLink("TIKTOK_USER_ID");
        SuperfineSDK.LogWeiboLink("WEIBO_USER_ID");

        SuperfineSDK.LogBootStart();
        SuperfineSDK.LogBootEnd();
        
        SuperfineSDK.LogLevelStart(levelId, levelName);
        SuperfineSDK.LogLevelEnd(levelId, levelName, true);
        SuperfineSDK.LogLevelEnd(levelId, levelName, false);

        SuperfineSDK.LogAdLoad(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdImpression(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdClick(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdClose(testAdUnit, testAdPlacementType, testAdPlacement);

        SuperfineSDK.LogIAPInitialization(true);
        SuperfineSDK.LogIAPRestorePurchase();

        SuperfineSDK.LogIAPResult(testIAPPackId, 1.0, 1, "USD", true);

        SuperfineSDK.LogIAPReceipt_Apple(testIAPReceipt);
        SuperfineSDK.LogIAPReceipt_Google(testIAPData, testIAPSignature);
        SuperfineSDK.LogIAPReceipt_Amazon(testIAPUserId, testIAPReceiptId);
        SuperfineSDK.LogIAPReceipt_Roku(testIAPTransactionId);
        SuperfineSDK.LogIAPReceipt_Windows(testIAPReceipt);
        SuperfineSDK.LogIAPReceipt_Facebook(testIAPReceipt);
        SuperfineSDK.LogIAPReceipt_Unity(testIAPReceipt);
        SuperfineSDK.LogIAPReceipt_AppStoreServer(testIAPTransactionId);
        SuperfineSDK.LogIAPReceipt_GooglePlayProduct(testIAPPackId, testIAPToken);
        SuperfineSDK.LogIAPReceipt_GooglePlaySubscription(testIAPPackId, testIAPToken);
        SuperfineSDK.LogIAPReceipt_GooglePlaySubscriptionv2(testIAPToken);

        SuperfineSDK.LogLocation(1.0, 2.0);

        SuperfineSDK.LogUpdateApp("2.0.0");
        SuperfineSDK.LogRateApp();

        SuperfineSDK.LogWalletLink(testWalletAddress, testWalletType);
        SuperfineSDK.LogWalletUnlink(testWalletAddress, testWalletType);

        SuperfineSDK.LogCryptoPayment(testCryptoPackId, 0.01, 1, "ETH", "ethereum");

        string param1 = "abc";
        int param2 = 100;
        bool param3 = true;
        long param4 = 9223372036854775807L;
        double param5 = 0.0000000000000001;
        Superfine.Unity.SimpleJSON.JSONObject adRevenueData = new Superfine.Unity.SimpleJSON.JSONObject
        {
            { "param1", param1 },
            { "param2", param2 },
            { "param3", param3 },
            { "param4", param4 },
            { "param5", param5 }
        };

        SuperfineSDK.LogAdRevenue(testAdNetworkId, 1.0, "USD", "DIRECT", adRevenueData);

        SuperfineSDK.LogLocation(testLocation.x, testLocation.y);

        SuperfineSDK.Log("test");
        SuperfineSDK.Log("test2", 123);
        SuperfineSDK.Log("test3", "ABC");
        SuperfineSDK.Log("test4", EventFlag.WAIT_OPEN_EVENT);

        Superfine.Unity.SimpleJSON.JSONObject testRevenueData = new Superfine.Unity.SimpleJSON.JSONObject
        {
            { "currency", "USD" },
            { "revenue", 0.01 }
        };
        SuperfineSDK.Log("test_revenue", testRevenueData);

        SuperfineSDK.Log("test_cache", EventFlag.CACHE);
    }

    public void FetchRemoteConfig()
    {
        SuperfineSDK.FetchRemoteConfig((obj) =>
        {
            if (obj == null)
            {
                Debug.LogError("REMOTE CONFIG = NULL");
            }
            else
            {
                Debug.LogError("REMOTE CONFIG = " + obj.ToString());
            }
        });
    }
}
