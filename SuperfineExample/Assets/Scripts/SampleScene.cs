using System.Collections;
using UnityEngine;
using Superfine.Tracking.Unity;
using TMPro;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    public int levelId;
    public string levelName;

    public string testWalletAddress;
    public string testWalletType = "ethereum";

    public string testAdUnit;

    public AdPlacementType testAdPlacementType = AdPlacementType.INTERSTITIAL;
    public AdPlacement testAdPlacement = AdPlacement.UNKNOWN;

    public string testIAPPackId;

    public string testFacebookId;

    public string testAccountId;
    public string testAccountType;

    public string testCryptoPackId;

    public TextMeshProUGUI userIdText;
    public Toggle autoStartToggle;

    public GameObject startButtonObject;

    void Awake()
    {
        TrackingManagerInitOptions options = new TrackingManagerInitOptions();

#if !UNITY_EDITOR
#if UNITY_ANDROID
        options.logLevel = LogLevel.VERBOSE;
#elif UNITY_IOS
        options.debug = false;
#elif UNITY_STANDALONE
        //options.proxy = "127.0.0.1:8888";
        //options.caPath = "FiddlerRoot.pem";
#endif
#endif

        TrackingManager.CreateInstance(options);

        userIdText.text = TrackingManager.GetInstance().GetUserId();
        autoStartToggle.isOn = options.autoStart;

        startButtonObject.SetActive(!options.autoStart);
    }

    public void TestStart()
    {
        TrackingManager.GetInstance().Start();
        userIdText.text = TrackingManager.GetInstance().GetUserId();

        startButtonObject.SetActive(false);
    }

    public void TestBoot()
    {
        StartCoroutine(TestBootCoroutine());
    }

    private IEnumerator TestBootCoroutine()
    {
        TrackingManager.GetInstance().TrackBootStart();
        yield return new WaitForSecondsRealtime(3.0f);
        TrackingManager.GetInstance().TrackBootEnd();
    }

    public void TestLinkWallet()
    {
        TrackingManager.GetInstance().TrackWalletLink(testWalletAddress, testWalletType);
    }

    public void TestUnlinkWallet()
    {
        TrackingManager.GetInstance().TrackWalletUnlink(testWalletAddress, testWalletType);
    }

    public void TestLevelStart()
    {
        TrackingManager.GetInstance().TrackLevelStart(levelId, levelName);
    }

    public void TestLevelCompleted()
    {
        TrackingManager.GetInstance().TrackLevelEnd(levelId, levelName, true);
    }

    public void TestLevelFailed()
    {
        TrackingManager.GetInstance().TrackLevelEnd(levelId, levelName, false);
    }

    public void TestAdImpression()
    {
        TrackingManager.GetInstance().TrackAdImpression(testAdUnit, testAdPlacementType, testAdPlacement);
    }

    public void TestAll()
    {
        TrackingManager.GetInstance().TrackBootStart();
        TrackingManager.GetInstance().TrackBootEnd();

        TrackingManager.GetInstance().TrackLevelStart(levelId, levelName);
        TrackingManager.GetInstance().TrackLevelEnd(levelId, levelName, true);
        TrackingManager.GetInstance().TrackLevelEnd(levelId, levelName, false);

        TrackingManager.GetInstance().TrackAdLoad(testAdUnit, testAdPlacementType, testAdPlacement);
        TrackingManager.GetInstance().TrackAdImpression(testAdUnit, testAdPlacementType, testAdPlacement);
        TrackingManager.GetInstance().TrackAdClick(testAdUnit, testAdPlacementType, testAdPlacement);
        TrackingManager.GetInstance().TrackAdClose(testAdUnit, testAdPlacementType, testAdPlacement);

        TrackingManager.GetInstance().TrackIAPInitialization(true);
        TrackingManager.GetInstance().TrackIAPRestorePurchase();
        TrackingManager.GetInstance().TrackIAPBuyStart(testIAPPackId, 1.0f, 1, "USD");
        TrackingManager.GetInstance().TrackIAPBuyEnd(testIAPPackId, 1.0f, 1, "USD", true);

        TrackingManager.GetInstance().TrackFacebookLogin(testFacebookId);
        TrackingManager.GetInstance().TrackFacebookLogout(testFacebookId);

        TrackingManager.GetInstance().TrackUpdateGame("2.0.0");
        TrackingManager.GetInstance().TrackRateGame();
        TrackingManager.GetInstance().TrackAuthorizationTrackingStatus(AuthorizationTrackingStatus.AUTHORIZED);

        TrackingManager.GetInstance().TrackAccountLogin(testAccountId, testAccountType);
        TrackingManager.GetInstance().TrackAccountLogout(testAccountId, testAccountType);
        TrackingManager.GetInstance().TrackAccountLink(testFacebookId, "facebook");
        TrackingManager.GetInstance().TrackAccountUnlink(testFacebookId, "facebook");

        TrackingManager.GetInstance().TrackWalletLink(testWalletAddress, testWalletType);
        TrackingManager.GetInstance().TrackWalletUnlink(testWalletAddress, testWalletType);

        TrackingManager.GetInstance().TrackCryptoPayment(testCryptoPackId, 0.01f, 1, "ETH", "ethereum");
    }
}
