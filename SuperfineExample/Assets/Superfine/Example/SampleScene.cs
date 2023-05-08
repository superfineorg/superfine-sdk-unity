using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Superfine.Tracking;
using System.Net;

public class SampleScene : MonoBehaviour
{
    public string appId;
    public string appSecret;

    public int levelId;
    public string levelName;
    public string testWalletAddress;

    void Awake()
    {
        TrackingManagerInitOptions options = new TrackingManagerInitOptions();

#if !UNITY_EDITOR
#if UNITY_ANDROID
        options.logLevel = LogLevel.VERBOSE;
#elif UNITY_IOS
        options.debug = false;
#endif
#endif

        TrackingManager.CreateInstance(appId, appSecret, options);
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

    public void LinkRoninWallet()
    {
        TrackingManager.GetInstance().TrackWalletLink(testWalletAddress, "ronin");
    }

    public void UnLinkRoninWallet()
    {
        TrackingManager.GetInstance().TrackAccountUnlink(testWalletAddress, "ronin");
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

}
