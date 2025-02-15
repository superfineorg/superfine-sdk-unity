## [0.0.3]

### Added

- Added support for MacOS universal.
- Added support for MacOS ARM 64 powered by Apple Silicon chips (M1 or later).
- Added support for MacOS x86 x64.
- Added support for Win32, the 32-bit version of the Windows operating system.
- Added support for Win64, the 64-bit version of the Windows operating system.
- Added support for Linux i686, running on 32-bit x86 processors, compatible with older hardware.
- Added support for Linux x86 x64, running on 64-bit x86 processors, supporting modern 64-bit hardware.
- Added a setting dashboard.
- Added support for UpdatePostbackConversionValue constants:
  - UpdatePostbackConversionValue(int conversionValue, string coarseValue, bool lockWindow);
  - UpdatePostbackConversionValue(int conversionValue);
  - UpdatePostbackConversionValue(int conversionValue, string coarseValue).
- Added support for RequestTrackingAuthorization to check ATT status callback.
- Updated SKAD list.
- Added Stop Tracking method.

### Changed

- Moved the SuperfineSDK to the Packages directory.
- Moved Tenjin to native lib.
- Separated the start SuperfineSDK function.

## [0.0.4]

### Added

- Added deep link support for iOS, Android, Windows, and Mac platforms.
- Introduced addon functions:
  - Applovin: Helper class for reporting revenue from Max Mediation to calculate LTV.
  - Ironsource: Helper class for reporting revenue from Ironsource Mediation to calculate LTV.
  - Admob: Helper class for reporting revenue from Admob Mediation to calculate LTV.
  - Facebook: Helper class for sending events to Facebook for marketing purposes.
- Added LogAdRevenue function.
- Added support for Universal Links on iOS and Mac.
- Added support for App Links on Android.

### Changed
- Change `TrackingManager` to `SuperfineSDK`
- Change function's name from `TrackingXXX` to `LogXXX`
- Change `TrackingInitOptions` to `SuperfineSDKInitOptions`

## [0.0.5]

- Customized the ATT messages prompt.
- Expanded Android install referral support to include Samsung, Xiaomi, and Huawei.

## [0.0.6]
- Enhanced configuration layout for improved usability.
- Updated Unity SDK Continuous Integration (CI) for better development workflows.
- Added Location method.
- Released iOS SDK 0.0.6.
- Released Android SDK 0.0.6.

## [0.0.7]
- Added additional constructors for the Log function.
- Renamed functions: LogRateGame to LogRateApp, and LogUpdateGame to LogUpdateApp for consistency.
- Introduced OAID (Open Anonymous Identifier) support.
- Released iOS SDK version 0.0.7.
- Released Android SDK version 0.0.7.

## [0.0.8]
- Adding SKAN conversion schema.
- Reformat revenue in ad_revenue event.
- Released iOS SDK version 0.0.8.
- Released Android SDK version 0.0.8.

## [0.0.9]
- Adding iAP Addon
- Detect Facebook initialization in Facebook Addon
- Steam support for PC
- Released iOS SDK version 0.0.9.
- Released Android SDK version 0.0.9.

## [0.0.10]
- Adding iAP receipt event
- Released iOS SDK version 0.0.10.
- Released Android SDK version 0.0.10.

## [0.0.13]
- Adding Appsflyer Module.
- Adding Adjust Module.
- Adding Singular Module.
- Adding Appodeal Addon for Unity Engine.
- Released iOS SDK version 0.0.13.
- Released Android SDK version 0.0.13.

## [0.0.15]
- Released iOS SDK version 0.0.15.
- Released Android SDK version 0.0.15.