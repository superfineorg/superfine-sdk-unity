# Superfine SDK Unity
# 1 Setup
## 1.1 Import Unity package
Download the SuperfineSDK zip file, unzip it, and copy the extracted files into your Packages folder.

\* The SDK requires the External Dependency Manager to function properly. You have the flexibility to remove or modify these dependencies to match your application's version requirements. You can download the External Dependency Manager from [here](https://developers.google.com/unity/archive#external_dependency_manager_for_unity).

## 1.2 Get App Information
Go to the project section on the Superfine.org dashboard, select your project, and copy the **Project ID** and **Project Secret**.

## 1.3 Update Superfine Setting
From the menu pick **Superfine/Edit Settings**
Update your **Project ID**, **Project Secret**

## 1.4 Initialize SDK
Add code to initialize the SDK (this could be placed in the `Awake` function of a new component).

```csharp
void Awake()
{
    SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources().Clone();

#if !UNITY_EDITOR
#if UNITY_ANDROID
    // Enable VERBOSE (or INFO, DEBUG) logging for Android.
    settings.logLevel = LogLevel.VERBOSE;
#elif UNITY_IOS
    // Enable debug mode for iOS.
    settings.debug = true; 
#endif
#endif
    // Create an instance of SuperfineSDK.
    SuperfineSDK.CreateInstance(settings);
}
```
**SuperfineSDKSettings**: Can’t be null. Contains:

- `flushInterval` (**Long**): Time interval (in milliseconds) for data flush to the server.
- `flushQueueSize` (**Integer**): Maximum number of stored events before a server flush.
- `customUserId` (**Boolean**): Flag for using a custom user ID. 
- `userId` (**String**): Custom user identifier to associate events with a user
- `waitConfigId` (**Boolean**): Flag to wait for the configuration ID before starting. 
- `autoStart` (**Boolean**). Flag to automatically start the SDK on initialization. 
- `storeType` (**StoreType**): Can be UNKNOWN, GOOGLE_PLAY, APP_STORE
- `logLevel` (**LogLevel**): for Android only. Can be VERBOSE, INFO, DEBUG)
- `debug` (**Boolean**): for iOS only. If set to true, will display the debug log
- `captureInAppPurchases` (**Boolean**): Enable capturing in-app purchases.

You can start it whenever you like if you set `autoStart` to false.

```csharp
settings.autoStart = false;
SuperfineSDK.CreateInstance(settings);

// Start SuperfineSDK when you want to.
SuperfineSDK.Start();
```
You can stop SuperfineSDK by calling this function:

```csharp
SuperfineSDK.Stop();
```

# 2 Send Events
## 2.1 Wallet Events
### LogWalletLink
> **`void LogWalletLink(const String wallet, const String type = "ethereum");`**

**Description**: Call this method when you want to link the user's wallet address.

| **Parameters**       |                   |
|-----------------|---------------     |
| `wallet`            | **String**: Cannot be null. The wallet address you want to log the linking event for|
|`type`               | **String**: Default is "ethereum". The chain of the wallet address|

*Example:*
```csharp
//Link wallet address "ronin:50460c4cd74094cd591455cad457e99c4ab8be0" in the "ronin" chain
SuperfineSDK.LogWalletLink("ronin:50460c4cd74094cd591455cad457e99c4ab8be0", "ronin");
```
### LogWalletUnlink
> **`void LogWalletUnlink(const String wallet, const String type = "ethereum");`**

**Description**: Call this method when you want to unlink the user’s wallet address. 

| Parameters       |                   |
|-----------------|---------------     |
| `wallet`        | **String**: Can’t be null. The wallet address you want to log the unlinking event for.|
| `type`          | **String**: Default is "ethereum". The chain of the wallet address.|

*Example:*

```csharp
//Unlink wallet address "ronin:50460c4cd74094cd591455cad457e99c4ab8be0" in "ronin" chain
SuperfineSDK.LogWalletUnlink("ronin:50460c4cd74094cd591455cad457e99c4ab8be0", "ronin");
```
## 2.2 Game Level Events
### LogLevelStart
> **`void LogLevelStart(int id, const String name);`**

**Description**: Call this method at the start of a level.

| Parameters       |                   |
|-----------------|---------------     |
| `id`              | **Integer**: Can’t be null. The level id that you want to log.|
| `name`            | **String**: Can’t be null. The name of the level that you want to log.|

*Example:*

```csharp
// Log starting level ID 10 with the name "level_10".
SuperfineSDK.LogLevelStart(10, "level_10");
```
### LogLevelEnd
> **`void LogLevelEnd(int id, const String name, bool isSuccess);`**

**Description**: Call this method upon completing a level.

| Parameters       |                   |
|-----------------|---------------     |
| `id`            | **Integer**: Can’t be null. The level id you want to log.|
| `name`          | **String**: Can’t be null. The name of the level you want to log.|
| `isSuccess`     | **Boolean**: Can’t be null. True if the level was passed, false otherwise.|

*Example:*

```csharp
// Log that you completed level ID 10 with the name "level_10" and won.
SuperfineSDK.LogLevelEnd(10, "level_10", true);
```
## 2.3 Ads Events
These events are used to track ads from your app. You can use the Superfine dashboard later to check ad performance based on these events.
### LogAdLoad
> **`void LogAdLoad(const String adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement::UNKNOWN);`**

**Description**: Call this method when an ad placement is loaded.

| Parameters       |                   |
|-----------------|---------------     |
| `adUnit`         | **String**: Can’t be null. The ad unit you want to log.|
| `adPlacementType` | **enum AdPlacementType**: Cannot be null. Can be BANNER, INTERSTITIAL, REWARDED_VIDEO.|
|`adPlacement`      |**enum AdPlacement**: Can be UNKNOWN, BOTTOM, TOP, LEFT, RIGHT, FULL_SCREEN. |

*Example:*

```csharp
// Log ad unit "ad_unit_test" with ad placement type INTERSTITIAL at placement FULL_SCREEN loaded.
SuperfineSDK.LogAdLoad("ad_unit_test", AdPlacementType.INTERSTITIAL, AdPlacement.FULL_SCREEN);
```
### LogAdClosed
> **`void LogAdClosed(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);`**

**Description**: Call this method when an ad is closed.

| Parameters       |                   |
|-----------------|---------------     |
| `adUnit`         | **String**: Can’t be null. The ad unit you want to log.|
| `adPlacementType` | **enum AdPlacementType**: Can’t be null. Can be BANNER, INTERSTITIAL, REWARDED_VIDEO.|
|`adPlacement`      |**enum AdPlacement**: Can’t be null. Can be UNKNOWN, BOTTOM, TOP, LEFT, RIGHT, FULL_SCREEN. |

*Example:*

```csharp
// Log ad unit "ad_unit_test" with ad placement type INTERSTITIAL at placement FULL_SCREEN closed.
SuperfineSDK.LogAdClosed("ad_unit_test", AdPlacementType.INTERSTITIAL, AdPlacement.FULL_SCREEN);
```
### LogAdImpression
> **`void LogAdImpression(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);`**

**Description**: Call this method when the ad impression is displayed.

| Parameters       |                   |
|-----------------|---------------     |
| `adUnit`         | **String**: Can’t be null. The ad unit you want to log.|
| `adPlacementType` | **enum AdPlacementType**: Cannot be null. Can be BANNER, INTERSTITIAL, REWARDED_VIDEO.|
|`adPlacement`      |**enum AdPlacement**: Can’t be null. Can be UNKNOWN, BOTTOM, TOP, LEFT, RIGHT, FULL_SCREEN. |

*Example:*

```csharp
// Log ad unit "ad_unit_test" with ad placement type INTERSTITIAL at placement FULL_SCREEN impression.
SuperfineSDK.LogAdImpression("ad_unit_test", AdPlacementType.INTERSTITIAL, AdPlacement.FULL_SCREEN); 
```
### LogAdClick
> **`void LogAdClick(string adUnit, AdPlacementType adPlacementType, AdPlacement adPlacement = AdPlacement.UNKNOWN);`**

**Description**: Call this method when the user clicks on an ad.

| Parameters       |                   |
|-----------------|---------------     |
| `adUnit`         | **String**: Can’t be null. The ad unit you want to log.|
| `adPlacementType` | **enum AdPlacementType**: Can’t be null. Can be BANNER, INTERSTITIAL, REWARDED_VIDEO.|
|`adPlacement`      |**enum AdPlacement**: Can’t be null.  Can be UNKNOWN, BOTTOM, TOP, LEFT, RIGHT, FULL SCREEN. |

*Example:*

```csharp
// Log ad unit "ad_unit_test" with ad placement type INTERSTITIAL at placement FULL_SCREEN clicked.
SuperfineSDK.LogAdClick("ad_unit_test", AdPlacementType.INTERSTITIAL, AdPlacement.FULL_SCREEN);
```
### LogAdRevenue
> **`void LogAdRevenue(string network, double revenue, string currency, string mediation = "", SimpleJSON.JSONObject networkData = null)`**

**Description**: Call this method to record revenue obtained from an advertisement.

| Parameters       |                   |
|-----------------|---------------     |
| `network`         | **String**: Can’t be null. The ad network that generated the revenue.|
| `revenue` | **double**:  Can’t be null. The amount of revenue obtained from the ad.|
| `currency` | **String**:  The currency code (e.g., "USD", "EUR") corresponding to the revenue. |
| `mediation` | **String**:  Can be empty. The mediation platform used (e.g., Direct, Max, LevelPlay, etc.). |
| `networkData` | **SimpleJSON.JSONObject**:  Can be null. Additional information about the ad-network. |

*Example:*

```csharp
SuperfineSDK.LogAdRevenue(testAdNetworkId, 1.0, "USD", "DIRECT", new Superfine.Unity.SimpleJSON.JSONObject
        {
            { "param1", "abc" },
            { "param2", 100 },
            { "param3", true }
        }
);   
```
## 2.4 IAP Events 
These events are used to track in-app purchases from your app.
### LogIAPResult
> **`void LogIAPResult(string pack, float price, int amount, string currency, bool isSuccess);`**

**Description**: Call this method when the user attempts to buy an IAP item.

| Parameters       |                   |
|-----------------|---------------     |
| `pack`         | **String**: Can’t be null. The unique identifier of the purchased item or pack.|
| `price`        | **Float**: Can’t be null. The price of the item or pack. |
| `amount`       | **Integer**: Can’t be null. The quantity of the purchased item or pack.  |
| `currency`     | **String**:  The currency code (e.g., "USD", "EUR") corresponding to the revenue. |
| `isSuccess`    | **Boolean**: Can’t be null. True if the purchase was successful. False if the purchase failed. |

*Example:*

```csharp
//Log when the user completed to purchase a package with ID "test_pack" for 0.99 USD, getting 150 units.
SuperfineSDK.LogIAPResult("test_pack", 0.99, 150, "USD", true);
```
### LogAPRestorePurchase
> **`void LogAPRestorePurchase();`**

**Description**: Call this method when a user attempts to restore a purchase.

*Example:*

```csharp
SuperfineSDK.LogIAPRestorePurchase(); 
```

## 2.5 Custom Events
### Log
> **`void Log(string eventName, int data);`**

**Description**: Call this method to log a custom event with integer data.

| Parameters       |                   |
|-----------------|---------------     |
| `eventName`     | **String**: Can’t be null. The name of your custom event.|
| `data`         | **Int**: Can’t be null. The integer data associated with the event.|

> **`void Log(string eventName, string data);`**

**Description**: Call this method to log a custom event with string data.

| Parameters       |                   |
|-----------------|---------------     |
| `eventName`     | **String**: Can’t be null. The name of your custom event.|
| `data`         | **String**: Can’t be null. The string data associated with the event.|

> **`void Log(string eventName, Dictionary<string, string> data = null);`**

**Description**: Call this method to log a custom event with dictionary data.

| Parameters       |                   |
|-----------------|---------------     |
| `eventName`     | **String**: Can’t be null. The name of your custom event.|
| `data`         | **Dictionary<string, string>**: Default is null. The dictionary data associated with the event.|

> **`void Log(string eventName, SimpleJSON.JSONObject data = null);`**

**Description**: Call this method to log a custom event with JSON object data.

| Parameters       |                   |
|-----------------|---------------     |
| `eventName`     | **String**: Can’t be null. The name of your custom event.|
| `data`          | **SimpleJSON.JSONObject**: Default is null. The JSON object data associated with the event.|

*Example:*

```csharp
// Log a custom event with JSON object data
SimpleJSON.JSONObject eventData = new SimpleJSON.JSONObject();
eventData["score"] = 1000;
eventData["level"] = "beginner";
//Log your event
SuperfineSDK.Log("My_Custom_Event_Name", eventData);
```

## 2.6 Addons

### Unity IAP Helper Class
The SuperfineSDKUnityIAP class simplifies the process of sending In-App Purchase (IAP) receipt events. Ensure you are using the **Unity IAP** package for this functionality.
- **Integration** Add the Uinity IAP Helper Addons to your project by going to the **Superfine** > **Add ons** > **UnityIAP**.
- **Sending Event**: Call `Superfine.Unity.SuperfineSDKUnityIAP.LogIAPReceipt(Product p)` when a purchase is successfully completed using the Unity IAP plugin.

*Example:*

```csharp
// Callback function for successful purchase
private void ProcessProductFinal(Product p, string receipt = null)
    {
        // Implement your logic here

        // Log the first purchase event
        Superfine.Unity.SuperfineSDK.LogIAPResult(p.definition.id, (double)(p.metadata.localizedPrice), 1, p.metadata.isoCurrencyCode, true);

        // Use Unity IAP helper to send the receipt event for LTV calculation of renewing purchases
        Superfine.Unity.SuperfineSDKUnityIAP.LogIAPReceipt(p);
    }
```


### Ads Reporting Helper Class
We offer ad revenue reporting support through our addon classes. Automatically receive detailed reports by implementing the appropriate class based on your chosen mediation platform and registering for events. Currently, we provide support for Max Mediation (AppLovin), Appodeal (both UMP and Manual version), IronSource Mediation, and Google AdMob Mediation.

#### Applovin Addons Helper Class
- **Integration**: Add the AppLovin Helper Addon to your project by going to **Superfine** > **Add ons** > **AppLovin**.
- **Event Registration**: Begin logging revenue and impressions by calling `SuperfineSDKApplovin.RegisterPaidEvent()`. When you're done, turn it off with `SuperfineSDKApplovin.UnregisterPaidEvent()` or when your manager class is destroyed.

#### Appodeal Addons Helper Class
- **Integration**: Add the Appodeal Helper Addons to your project by going to: 
    - **Superfine** > **Add ons** > **Appodeal (UMP)** (for the UMP version).
    - **Superfine** > **Add ons** > **Appodeal (Manual)** (for the Manual version).
- **Event Registration**: Begin logging revenue and impressions by calling `SuperfineSDKAppodeal.RegisterPaidEvent()`. When you're done, turn it off with `SuperfineSDKAppodeal.UnregisterPaidEvent()` or when your manager class is destroyed.

#### IronSource Addons Helper Class
- **Integration**: Add the Ironsource Helper Addon to your project by going to **Superfine** > **Add ons** > **IronSource**.
- **Event Registration**: Begin logging revenue and impressions with `SuperfineSDKIronSource.RegisterPaidEvent()`. Turn it off with `SuperfineSDKIronSource.UnregisterPaidEvent()` when done or when your manager class is removed.

#### Google AdMob Addon Helper Class
- **Integration**: Add the Admob Helper Addon to your project by going to **Superfine** > **Add ons** > **Admob**.
-**Event Registration**: For Google AdMob, you have to register events for all placements that you have.
    - For Banner: `SuperfineSDKAdMob.RegisterBannerViewPaidEvent(bannerView, adUnitId)` and `SuperfineSDKAdMob.UnregisterBannerViewPaidEvent(bannerView, adUnitId)`.
    - For Interstitial: `SuperfineSDKAdMob.RegisterInterstitialAdPaidEvent(interstitialAd, adUnitId)` and `SuperfineSDKAdMob.UnregisterInterstitialAdPaidEvent(interstitialAd, adUnitId)`.
    - For Rewarded video ad:  `SuperfineSDKAdMob.RegisterRewardedAdPaidEvent(rewardedAd, adUnitId)` and `SuperfineSDKAdMob.UnregisterRewardedAdPaidEvent(rewardedAd, adUnitId)`.
    - For Rewarded Interstitial Ads `SuperfineSDKAdMob.RegisterRewardedInterstitialAdPaidEvent(rewardedInterstitialAd, adUnitId)` and `SuperfineSDKAdMob.UnregisterRewardedInterstitialAdPaidEvent(rewardedInterstitialAd, adUnitId)`.
    - For App Open `SuperfineSDKAdMob.RegisterAppOpenAdPaidEvent(appOpenAd, adUnitId)` and `SuperfineSDKAdMob.UnregisterAppOpenAdPaidEvent(appOpenAd, adUnitId)`.
    - For the Ad revenue `SuperfineSDK.LogAdRevenue()`.

### Facebook Events Addon Helper Class
The SuperfineSDKFacebook class simplifies sending events to Facebook for marketing purposes. To smoothly integrate this feature, follow these steps:
- **Integration**: 
    - Add the Facebook Addon to your project by going to **Superfine** > **Add ons** > **Facebook**
    - After initializing the Facebook SDK, call `SuperfineSDKFacebook.OnFacebookInitialized();`
- **Event Registration**: Begin logging events with `SuperfineSDKFacebook.RegisterSendEvent()`. When done, turn it off using `SuperfineSDKFacebook.UnregisterSendEvent()` or when your manager class is removed.

By following these instructions, you can effectively utilize the SuperfineSDKFacebook class to transmit custom events to Facebook, enhancing marketing insights and decision-making for your app.

## 2.7 Modules
### Appsflyer
The SuperfineSDKAppsFlyerModule helps connect Appsflyer MMP to Superfine. To integrate this feature smoothly, follow these steps:

- Make sure you have installed Appsflyer in your project.
- Add the Appsflyer Module by navigating to **Superfine > Modules > AppsFlyer**.

![image_1]

- Open SuperfineSDK settings by going to the menu **Superfine > Edit Settings** or by navigating to the folder: **SuperfineSDK > Resources > SuperfineSettings**.
- Drag SuperfineAppsFlyerSettings into **SuperfineSettings > Modules**.

![image_2]

- Update SuperfineAppsFlyerSettings by going to the file: **SuperfineSDK > Modules > AppsFlyer > SuperfineAppsFlyerSettings**.
    - Update your AppsFlyer's Dev key.
    - Enable auto-start if you want Superfine to initialize the AppsFlyer for you. Turn it off if you prefer to start it manually.

![image_3]

## 2.8 Postback Conversion Value for iOS
The method allows you to update both the conversion value and coarse conversion values, and it provides the option to send the postback before the conversion window ends. Additionally, it allows you to specify a completion handler to handle situations where the update fails.

```csharp
using Superfine.Tracking.Unity;

// Example 1: Basic usage without coarseValue or lockWindow
SuperfineSDK.UpdatePostbackConversionValue(10);

// Example 2: Usage with coarseValue
SuperfineSDK.UpdatePostbackConversionValue(8, "low");

// Example 3: Usage with lockWindow
SuperfineSDK.UpdatePostbackConversionValue(15, "medium", true);
```
[image_1]: ./assets/unity/1.png
[image_2]: ./assets/unity/2.png
[image_3]: ./assets/unity/3.png