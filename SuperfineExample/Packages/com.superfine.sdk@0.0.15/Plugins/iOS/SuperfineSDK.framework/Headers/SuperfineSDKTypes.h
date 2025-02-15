#ifndef SuperfineSDKTypes_h
#define SuperfineSDKTypes_h

#define JSON_DICT NSMutableDictionary*

typedef NS_OPTIONS(NSUInteger, SuperfineSDKOperationFlag)
{
    kSuperfineSDKOperationFlag_None = 0,
    kSuperfineSDKOperationFlag_OpenEvent = 1 << 0,
    kSuperfineSDKOperationFlag_ForceDisabled = 1 << 1,
    kSuperfineSDKOperationFlag_WaitOpenEvent = 1 << 2,
    kSuperfineSDKOperationFlag_Cache = 1 << 3
};

typedef NS_ENUM(NSInteger, SuperfineSDKInitializationActionType)
{
    SuperfineSDKInitializationActionType_RequestTrackingAuthorization = 0,
    SuperfineSDKInitializationActionType_RequestNotificationAuthorization = 1
};

typedef void (^InitializationCompletion)(void);

typedef void (^NotificationAuthorizationCompletion)(BOOL granted, NSError* _Nullable error);

typedef void (^RegisterRemoteNotificationCompletion)(NSError* _Nullable error);

typedef void (^TrackingAuthorizationCompletion)(NSUInteger status);

typedef void (^FetchRemoteConfigCompletion)(NSString* _Nullable data);

typedef NS_OPTIONS(NSUInteger, SuperfineSDKAppSwizzleFlag) {
    SuperfineSDKAppSwizzleFlag_None = 0,
    SuperfineSDKAppSwizzleFlag_ContinueUserActivity = (1 << 0),
    SuperfineSDKAppSwizzleFlag_OpenURL = (1 << 1),
    SuperfineSDKAppSwizzleFlag_HandleEventsForBackgroundURLSession = (1 << 2),
    SuperfineSDKAppSwizzleFlag_RegisterForRemoteNotifications = (1 << 3),
    SuperfineSDKAppSwizzleFlag_ReceiveRemoteNotification = (1 << 4),
    SuperfineSDKAppSwizzleFlag_Default =
        SuperfineSDKAppSwizzleFlag_ContinueUserActivity |
        SuperfineSDKAppSwizzleFlag_OpenURL,
    SuperfineSDKSwizzleFlag_All_Manager =
        SuperfineSDKAppSwizzleFlag_ContinueUserActivity |
        SuperfineSDKAppSwizzleFlag_OpenURL |
        SuperfineSDKAppSwizzleFlag_HandleEventsForBackgroundURLSession |
        SuperfineSDKAppSwizzleFlag_RegisterForRemoteNotifications,
    SuperfineSDKSwizzleFlag_All =
        SuperfineSDKAppSwizzleFlag_ContinueUserActivity |
        SuperfineSDKAppSwizzleFlag_OpenURL |
        SuperfineSDKAppSwizzleFlag_HandleEventsForBackgroundURLSession |
        SuperfineSDKAppSwizzleFlag_RegisterForRemoteNotifications |
        SuperfineSDKAppSwizzleFlag_ReceiveRemoteNotification
};

typedef NS_ENUM(NSInteger, SuperfineSDKTrackingAuthorizationStatus) {
    SuperfineSDKTrackingAuthorizationStatus_NotDetermined = 0,
    SuperfineSDKTrackingAuthorizationStatus_Restricted = 1,
    SuperfineSDKTrackingAuthorizationStatus_Denied = 2,
    SuperfineSDKTrackingAuthorizationStatus_Authorized = 3
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
