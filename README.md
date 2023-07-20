# Superfine SDK Unity
Version 0.0.3
# 1 Setup
## 1.1 Import Unity package
Download the SuperfineSDK zip file, unzip it, and copy the extracted files to your Packages folder.

\* The SDK requires JSON.NET and external-dependency-manager to function properly. We have included them within the SDK, but you have the flexibility to remove or modify them to match the version requirements of your application from the file: **Packages/com.superfine.attribution/package.json**
```groovy
"com.unity.nuget.newtonsoft-json": "3.2.1",
"com.google.external-dependency-manager": "1.2.176"
```
## 1.2 Get App Information
Go to the project section on the Superfine.org dashboard, select the project, and copy the **Project ID** and **Superfine App Secret**

## 1.3 Update Superfine Setting
From the menu pick **Superfine/Edit Settings**
Update your **Project ID**, **Superfine App Secret**, TenjinApiKey for **iOS** and **Android**

## 1.4 Initialize SDK
Add code to initialize the SDK (could be placed in the Awake function of a new component).

```groovy
void Awake()
{
    TrackingManagerInitOptions options = new TrackingManagerInitOptions();

#if !UNITY_EDITOR
#if UNITY_ANDROID
    options.logLevel = LogLevel.VERBOSE;
#elif UNITY_IOS
    options.debug = true;
    options.captureInAppPurchases = true;
#endif
#endif
    TrackingManager.CreateInstance(options);
}
```
You can disable the automatic startup of the SDK from the configuration.
```groovy
// Set autoStart to false
options.autoStart = false;
```
And then you can start it whenever you like.
```groovy
TrackingManager.GetInstance().Start();
```
You can stop tracking by calling this function:
```groovy
TrackingManager.GetInstance().Stop();
```
# 2 Send Events
## 2.1 Wallet Events
Call this function when you want to link the user wallet address:

```groovy
TrackingManager.GetInstance().TrackWalletLink(wallet_address, "ronin");
```

Call this function when you want to unlink the user wallet address:

```groovy
TrackingManager.GetInstance().TrackWalletUnlink(wallet_address, "ronin");
```
## 2.2 Game Level Events
Call this function when starting a level:

```groovy
TrackingManager.GetInstance().TrackLevelStart(level_id, level_name);
```

Call this function when completing a level:

```groovy
TrackingManager.GetInstance().TrackLevelEnd(level_id, level_name, true);
```

Call this function when failing a level:

```groovy
TrackingManager.GetInstance().TrackLevelEnd(level_id, level_name, false);
```
## 2.3 Ads Events
These events are used to track ads from your app. You can use the Superfine dashboard later to check ad performance based on these events.

Call this function when an ad placement is loaded:
```groovy
TrackingManager.GetInstance().TrackAdLoad(ad_unit, ad_placement_type, ad_placement); 
```

Call this function when an ad is closed:

```groovy
TrackingManager.GetInstance().TrackAdClose(ad_unit, ad_placement_type, ad_placement); 
```

Call this function when the user clicks on an ad:

```groovy
TrackingManager.GetInstance().TrackAdClick(ad_unit, ad_placement_type, ad_placement); 
```

Call this function when an ad is displayed:

```groovy
TrackingManager.GetInstance().TrackAdImpression(ad_unit, ad_placement_type, ad_placement); 
```

## 2.4 IAP Events 
These events are used to track in-app purchases from your app.

Call this function when the user attempts to buy an IAP item:

```groovy
TrackingManager.GetInstance().TrackIAPBuyStart(pack_id, price, amount, currency);
```

Call this function when the IAP purchase process is completed:

```groovy
TrackingManager.GetInstance().TrackIAPBuyEnd(pack_id, price, amount, currency);
```

Call this function when restoring a purchase:

```groovy
TrackingManager.GetInstance().TrackIAPRestorePurchase(); 
```

## 2.5 Custom Events
You can define any custom event to fit your needs.

Call this to send the custom event:

```groovy
TrackingManager.GetInstance().Track(string event_name)
```


You can also create a class extending from TrackBaseData to store data for the event:

```groovy
[Serializable]
public class YourCustomEventData : TrackBaseData
{
    public string ip_address;
    public string country;
    public string device;
    public string os_version;
    public string app_version;
    public string nft_id;
    public int nft_ammount;
    public string chain_id;
}
```

Call this to send the custom event with the custom data

```groovy
TrackingManager.GetInstance().Track(string event_name, TrackBaseData data = null)
```
## 2.6 Postback Conversion Value for iOS
The method allows you to update both the conversion value and coarse conversion values, and it provides the option to send the postback before the conversion window ends. Additionally, it allows you to specify a completion handler to handle situations where the update fails.
```groovy
using Superfine.Tracking.Unity;

// Assuming you have the TrackingManager instance obtained from your SDK
TrackingManager trackingManager = TrackingManager.GetInstance();

// Example 1: Basic usage without coarseValue or lockWindow
trackingManager.UpdatePostbackConversionValue(10);

// Example 2: Usage with coarseValue
trackingManager.UpdatePostbackConversionValue(8, "low");

// Example 3: Usage with lockWindow
trackingManager.UpdatePostbackConversionValue(15, "medium", true);
```