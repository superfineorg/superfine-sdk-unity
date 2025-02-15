# Superfine SDK Unity
# 1. Setup

1. **Import Unity Package**  
   Download the SuperfineSDK zip file, unzip it, and copy the extracted files into your `Packages` folder.  
   > ⚠️ **Important:** The SDK requires the External Dependency Manager to function properly. You can remove or modify these dependencies to match your application's version requirements. Download the External Dependency Manager from [here](https://developers.google.com/unity/archive#external_dependency_manager_for_unity).

2. **Get App Information**  
   Go to the project section on the Superfine.org dashboard, select your project, and copy the **Project ID** and **Project Secret**.

3. **Update Superfine Settings**  
   From the menu, select **Superfine/Edit Settings** to update your **Project ID** and **Project Secret**.

4. **Initialize SDK**  
   Add the following code to initialize the SDK (this can be placed in the `Awake` function of a new component).

```csharp
void Awake()
{
    SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources().Clone();
    settings.autoStart = false;

    // Initialize SuperfineSDK.
    SuperfineSDK.Initialize(settings, OnInitialized);

    // Note: Call SuperfineSDK.Start(); after MMP and Mediation SDK initialization.
    SuperfineSDK.Start();
}

// Callback after Superfine initialization is complete
void OnInitialized()
{
}
```
If `autoStart` is set to `false`, you can manually start the SDK as follows:

```csharp
settings.autoStart = false;
SuperfineSDK.Initialize(settings, OnInitialized);

// Start SuperfineSDK when needed.
SuperfineSDK.Start();
```
To stop the SDK, call:

```csharp
SuperfineSDK.Stop();
```
> ⚠️ **Important:** You should call `Unity.SuperfineSDK.Initialize();` after MMP and Mediation SDK initialization.

# 2. Revenue
## Ad Revenue Auto-Tracking

These events are used to track ad revenue from your app. We support automatic ad revenue tracking.  
Our SDK provides ad revenue reporting support through add-on classes. You can receive detailed reports by implementing the appropriate class based on your chosen mediation platform and registering for events.  
Currently, we support the following mediation platforms:

- **MAX Mediation (AppLovin)**
- **Appodeal** (both UMP and Manual versions)
- **IronSource Mediation**
- **Google AdMob Mediation**

### 1. MAX Mediation (AppLovin)

- **Integration:** Add the AppLovin Helper add-on to your project by navigating to **Superfine** > **Add-ons** > **AppLovin**.
- **Event Registration:** Start logging revenue and impressions by calling `SuperfineSDKApplovin.RegisterPaidEvent()`.  
  To stop logging, call `SuperfineSDKApplovin.UnregisterPaidEvent()` or do so when your manager class is destroyed.

### 2. Appodeal Mediation

- **Integration:** Add the Appodeal Helper add-on to your project by navigating to:  
  - **Superfine** > **Add-ons** > **Appodeal (UMP)** (for the UMP version).  
  - **Superfine** > **Add-ons** > **Appodeal (Manual)** (for the Manual version).
- **Event Registration:** Start logging revenue and impressions by calling `SuperfineSDKAppodeal.RegisterPaidEvent()`.  
  To stop logging, call `SuperfineSDKAppodeal.UnregisterPaidEvent()` or do so when your manager class is destroyed.

### 3. IronSource Mediation

- **Integration:** Add the IronSource Helper add-on to your project by navigating to **Superfine** > **Add-ons** > **IronSource**.
- **Event Registration:** Start logging revenue and impressions by calling `SuperfineSDKIronSource.RegisterPaidEvent()`.  
  To stop logging, call `SuperfineSDKIronSource.UnregisterPaidEvent()` when done or when your manager class is removed.

### 4. Google AdMob Mediation

- **Integration:** Add the AdMob Helper add-on to your project by navigating to **Superfine** > **Add-ons** > **AdMob**.
- **Event Registration:** You must register events for all ad placements:

  - **Banner Ads:**  
    ```csharp
    SuperfineSDKAdMob.RegisterBannerViewPaidEvent(bannerView, adUnitId);
    SuperfineSDKAdMob.UnregisterBannerViewPaidEvent(bannerView, adUnitId);
    ```

  - **Interstitial Ads:**  
    ```csharp
    SuperfineSDKAdMob.RegisterInterstitialAdPaidEvent(interstitialAd, adUnitId);
    SuperfineSDKAdMob.UnregisterInterstitialAdPaidEvent(interstitialAd, adUnitId);
    ```

  - **Rewarded Video Ads:**  
    ```csharp
    SuperfineSDKAdMob.RegisterRewardedAdPaidEvent(rewardedAd, adUnitId);
    SuperfineSDKAdMob.UnregisterRewardedAdPaidEvent(rewardedAd, adUnitId);
    ```

  - **Rewarded Interstitial Ads:**  
    ```csharp
    SuperfineSDKAdMob.RegisterRewardedInterstitialAdPaidEvent(rewardedInterstitialAd, adUnitId);
    SuperfineSDKAdMob.UnregisterRewardedInterstitialAdPaidEvent(rewardedInterstitialAd, adUnitId);
    ```

  - **App Open Ads:**  
    ```csharp
    SuperfineSDKAdMob.RegisterAppOpenAdPaidEvent(appOpenAd, adUnitId);
    SuperfineSDKAdMob.UnregisterAppOpenAdPaidEvent(appOpenAd, adUnitId);
    ```

  - **Ad Revenue Tracking:**  
    ```csharp
    SuperfineSDK.LogAdRevenue();
    ```

## IAP Revenue Recognition

The `SuperfineSDKUnityIAP` class simplifies the process of sending In-App Purchase (IAP) receipt events.  
Ensure you are using the **Unity IAP** package for this functionality.

- **Integration:** Add the Unity IAP Helper add-ons to your project by navigating to **Superfine** > **Add-ons** > **UnityIAP**.
- **Sending Events:** Call `Superfine.Unity.SuperfineSDKUnityIAP.LogIAPReceipt(Product p)` when a purchase is successfully completed using the Unity IAP plugin.

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


## Wallet Events - For On-Chain Revenue Recognition
1. LogWalletLink
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
2. LogWalletUnlink
> **`void LogWalletUnlink(const String wallet, const String type = "ethereum");`**

**Description**: Call this method when you want to unlink the user's wallet address. 

| Parameters       |                   |
|-----------------|---------------     |
| `wallet`        | **String**: Can't be null. The wallet address you want to log the unlinking event for.|
| `type`          | **String**: Default is "ethereum". The chain of the wallet address.|

*Example:*

```csharp
//Unlink wallet address "ronin:50460c4cd74094cd591455cad457e99c4ab8be0" in "ronin" chain
SuperfineSDK.LogWalletUnlink("ronin:50460c4cd74094cd591455cad457e99c4ab8be0", "ronin");
```

## Additional Revenue Sources
You can log addional revenue by using 
> **`void LogIAPResult(string pack, float price, int amount, string currency, bool isSuccess);`**

**Description**: Call this method when the user attempts to buy an IAP item.

| Parameters       |                   |
|-----------------|---------------     |
| `pack`         | **String**: Can't be null. The unique identifier of the purchased item or pack.|
| `price`        | **Float**: Can't be null. The price of the item or pack. |
| `amount`       | **Integer**: Can't be null. The quantity of the purchased item or pack.  |
| `currency`     | **String**:  The currency code (e.g., "USD", "EUR") corresponding to the revenue. |
| `isSuccess`    | **Boolean**: Can't be null. True if the purchase was successful. False if the purchase failed. |

*Example:*

```csharp
//Log when the user completed to purchase a package with ID "test_pack" for 0.99 USD, getting 150 units.
SuperfineSDK.LogIAPResult("test_pack", 0.99, 150, "USD", true);
```