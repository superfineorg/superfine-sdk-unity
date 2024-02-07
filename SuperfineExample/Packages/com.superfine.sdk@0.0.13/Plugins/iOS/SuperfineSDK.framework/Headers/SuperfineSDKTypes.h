#ifndef SuperfineSDKTypes_h
#define SuperfineSDKTypes_h

#define JSON_DICT NSMutableDictionary*

typedef void (^SendEventCallback)(const NSString* eventName, const NSString* eventData, int);

typedef NS_ENUM(NSUInteger, SuperfineSDKOperationFlag)
{
    kSuperfineSDKOperationFlag_None = 0,
    kSuperfineSDKOperationFlag_OpenEvent = 1 << 0,
    kSuperfineSDKOperationFlag_ForceDisabled = 1 << 1,
    kSuperfineSDKOperationFlag_WaitOpenEvent = 1 << 2,
    kSuperfineSDKOperationFlag_Cache = 1 << 3
};

typedef void (^TrackingAuthorizationCompletion)(NSUInteger status);

typedef void (^FetchRemoteConfigCompletion)(const NSString* data);

typedef NS_ENUM(NSInteger, SuperfineSDKAuthorizationTrackingStatus) {
    SuperfineSDKAuthorizationTrackingStatus_NotDetermined = 0,
    SuperfineSDKAuthorizationTrackingStatus_Restricted = 1,
    SuperfineSDKAuthorizationTrackingStatus_Denied = 2,
    SuperfineSDKAuthorizationTrackingStatus_Authorized = 3
};

typedef NS_ENUM(NSInteger, SuperfineSDKStoreType) {
    SuperfineSDKStoreType_Unknown = -1,
    SuperfineSDKStoreType_AppStore = 0,
    SuperfineSDKStoreType_GooglePlay = 1,
    SuperfineSDKStoreType_AmazonStore = 2,
    SuperfineSDKStoreType_MicrosoftStore = 3
};

typedef NS_ENUM(NSInteger, SuperfineSDKAdPlacementType) {
    SuperfineSDKAdPlacementType_Banner = 0,
    SuperfineSDKAdPlacementType_Interstitial = 1,
    SuperfineSDKAdPlacementType_RewardedVideo = 2
};

typedef NS_ENUM(NSInteger, SuperfineSDKAdPlacement) {
    SuperfineSDKAdPlacement_Unknown = -1,
    SuperfineSDKAdPlacement_Bottom = 0,
    SuperfineSDKAdPlacement_Top = 1,
    SuperfineSDKAdPlacement_Left = 2,
    SuperfineSDKAdPlacement_Right = 3,
    SuperfineSDKAdPlacement_FullScreen = 4
};

typedef NS_ENUM(NSInteger, SuperfineSDKUserGender) {
    SuperfineSDKUserGender_Male = 0,
    SuperfineSDKUserGender_Female = 1
};

#endif
