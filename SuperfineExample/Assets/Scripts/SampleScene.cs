using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Superfine.Unity;
using Superfine.Unity.SimpleJSON;

public class SampleScene : MonoBehaviour
{
    public int levelId;
    public string levelName;

    public string configId;
    public string customUserId;

    public string testWalletAddress;
    public string testWalletType = "ethereum";

    public string testAdUnit;

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

    public TextMeshProUGUI userIdText;
    public Toggle autoStartToggle;

    public GameObject startButtonObject;
    public GameObject stopButtonObject;

    public GameObject testButtonPanel;
    public GameObject testAllButtonObject;

    void Awake()
    {
        SuperfineSDKInitOptions options = new SuperfineSDKInitOptions();
        options.autoStart = false;

        options.configId = configId;
        options.customUserId = customUserId;

#if !UNITY_EDITOR
#if UNITY_ANDROID
        options.logLevel = LogLevel.VERBOSE;
#elif UNITY_IOS
        options.debug = true;
#elif UNITY_STANDALONE
        options.logLevel = LogLevel.VERBOSE;
        options.registerURIScheme = true;

        //options.proxy = "127.0.0.1:8888";
        //options.sslVerify = false;

        //options.steamBuild = true;
        //options.steamAppId = 480;
#endif
#endif

        SuperfineSDK.CreateInstance(options);

        userIdText.text = SuperfineSDK.GetUserId();
        autoStartToggle.isOn = options.autoStart;

        bool autoStart = options.autoStart;
        startButtonObject.SetActive(!autoStart);
        stopButtonObject.SetActive(autoStart);
    }

    public void TestStart()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            SuperfineSDK.RequestTrackingAuthorization((AuthorizationTrackingStatus status) =>
            {
                Debug.Log("Authorization Tracking Status = " + status.ToString());

                SuperfineSDK.UpdatePostbackConversionValue(10, "medium", true);

                StartTracking();
            });
        }
        else
        {
            StartTracking();
        }
    }

    public void TestStop()
    {
        SuperfineSDK.Stop();

        stopButtonObject.SetActive(false);

        testButtonPanel.SetActive(false);
        testAllButtonObject.SetActive(false);
    }

    private void PrintEvent(string eventName, string eventData)
    {
        Debug.LogError("SEND EVENT: " + eventName + " " + eventData);
    }

    private void StartTracking()
    {
        SuperfineSDK.Start();
        userIdText.text = SuperfineSDK.GetUserId();

        SuperfineSDK.AddSendEventCallback(PrintEvent);

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

    public void TestAll()
    {
        SuperfineSDK.LogBootStart();
        SuperfineSDK.LogBootEnd();

        SuperfineSDK.LogLevelStart(levelId, levelName);
        SuperfineSDK.LogLevelEnd(levelId, levelName, true);
        SuperfineSDK.LogLevelEnd(levelId, levelName, false);

        SuperfineSDK.LogAdLoad(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdImpression(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdClick(testAdUnit, testAdPlacementType, testAdPlacement);
        SuperfineSDK.LogAdClose(testAdUnit, testAdPlacementType, testAdPlacement);

        SuperfineSDK.SetConfigId(configId);
        SuperfineSDK.SetCustomUserId(customUserId);

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

        SuperfineSDK.LogFacebookLogin(testFacebookId);
        SuperfineSDK.LogFacebookLogout(testFacebookId);

        SuperfineSDK.LogUpdateApp("2.0.0");
        SuperfineSDK.LogRateApp();

        SuperfineSDK.LogAccountLogin(testAccountId, testAccountType);
        SuperfineSDK.LogAccountLogout(testAccountId, testAccountType);
        SuperfineSDK.LogAccountLink(testFacebookId, "facebook");
        SuperfineSDK.LogAccountUnlink(testFacebookId, "facebook");

        SuperfineSDK.LogWalletLink(testWalletAddress, testWalletType);
        SuperfineSDK.LogWalletUnlink(testWalletAddress, testWalletType);

        SuperfineSDK.LogCryptoPayment(testCryptoPackId, 0.01, 1, "ETH", "ethereum");

        JSONObject adRevenueData = new Superfine.Unity.SimpleJSON.JSONObject();
        string param1 = "abc";
        int param2 = 100;
        bool param3 = true;
        long param4 = 9223372036854775807L;
        double param5 = 0.0000000000000001;

        adRevenueData.Add("param1", param1);
        adRevenueData.Add("param2", param2);
        adRevenueData.Add("param3", param3);
        adRevenueData.Add("param4", param4);
        adRevenueData.Add("param5", param5);

        SuperfineSDK.LogAdRevenue(testAdNetworkId, 1.0, "USD", "DIRECT", adRevenueData);
    }
}
