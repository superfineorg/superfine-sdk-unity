#ifndef SuperfineTrackingEnumTypes_h
#define SuperfineTrackingEnumTypes_h

#define JSON_DICT NSDictionary<NSString *, id> *

typedef void (^TrackingAuthorizationCompletion)(NSUInteger status);

typedef NS_ENUM(NSInteger, SuperfineTrackingAuthorizationTrackingStatus) {
    SuperfineTrackingAuthorizationTrackingStatus_NotDetermined = 0,
    SuperfineTrackingAuthorizationTrackingStatus_Restricted = 1,
    SuperfineTrackingAuthorizationTrackingStatus_Denied = 2,
    SuperfineTrackingAuthorizationTrackingStatus_Authorized = 3
};

typedef NS_ENUM(NSInteger, SuperfineTrackingStoreType) {
    SuperfineTrackingStoreType_Unknown = -1,
    SuperfineTrackingStoreType_AppStore = 0,
    SuperfineTrackingStoreType_GooglePlay = 1,
    SuperfineTrackingStoreType_AmazonStore = 2,
    SuperfineTrackingStoreType_MicrosoftStore = 3
};

typedef NS_ENUM(NSInteger, SuperfineTrackingAdPlacementType) {
    SuperfineTrackingAdPlacementType_Banner = 0,
    SuperfineTrackingAdPlacementType_Interstitial = 1,
    SuperfineTrackingAdPlacementType_RewardedVideo = 2
};

typedef NS_ENUM(NSInteger, SuperfineTrackingAdPlacement) {
    SuperfineTrackingAdPlacement_Unknown = -1,
    SuperfineTrackingAdPlacement_Bottom = 0,
    SuperfineTrackingAdPlacement_Top = 1,
    SuperfineTrackingAdPlacement_Left = 2,
    SuperfineTrackingAdPlacement_Right = 3,
    SuperfineTrackingAdPlacement_FullScreen = 4
};

#endif /* SuperfineTrackingEnumTypes_h */
