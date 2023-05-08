#import <Foundation/Foundation.h>
#import "SuperfineTrackingSerializableValue.h"

typedef NS_ENUM(NSInteger, SuperfineTrackingAuthorizationTrackingStatus) {
    SuperfineTrackingAuthorizationTrackingStatus_NotDetermined = 0,
    SuperfineTrackingAuthorizationTrackingStatus_Restricted = 1,
    SuperfineTrackingAuthorizationTrackingStatus_Denied = 2,
    SuperfineTrackingAuthorizationTrackingStatus_Authorized = 3
};

typedef NS_ENUM(NSInteger, SuperfineTrackingStoreType) {
    SuperfineTrackingStoreType_Unknown = -1,
    SuperfineTrackingStoreType_AppStore = 0,
    SuperfineTrackingStoreType_GooglePlay = 1
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

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineTrackingDataBuilder : NSObject

+ (JSON_DICT)createTrackBaseData;

+ (JSON_DICT)createTrackOpenData_ip_address:(NSString*)ip_address
                                        country:(NSString*)country
                                         device:(NSString*)device
                                     os_version:(NSString*)os_version
                                    app_version:(NSString*)app_version
                                   build_number:(NSString*)build_number
                                 gj_sdk_version:(NSString*)gj_sdk_version
                                         cfg_id:(NSString*)cfg_id
                                    //app_set_id:(NSString*)app_set_id
                                 advertising_id:(NSString*)advertisting_id
                            developer_device_id:(NSString*)developer_device_id;

+ (JSON_DICT)createTrackAdData_ad_unit:(NSString*)ad_unit
                            placement_type:(SuperfineTrackingAdPlacementType)placement_type
                                 placement:(SuperfineTrackingAdPlacement)placement;

+ (JSON_DICT)createTrackLevelData_level:(int)level
                                      label:(NSString*)label;

+ (JSON_DICT)createTrackBootEndData_boot_time:(long)boot_time;
   
+ (JSON_DICT)createTrackIAPInitializationData_status:(NSString*)status;

+ (JSON_DICT)createTrackIAPPackageData_pack:(NSString*)pack
                                      price:(NSString*)price
                                     amount:(float)amount
                                   currency:(NSString*)currency;

+ (JSON_DICT)createTrackFacebookData_facebook_id:(NSString*)facebook_id;

+ (JSON_DICT)createTrackUpdateGameData_currentVersion:(NSString*)current_version
                                          new_version:(NSString*)new_version;

+ (JSON_DICT)createTrackRateGameData_store_type:(SuperfineTrackingStoreType)store_type;

+ (JSON_DICT)createTrackSoundModeData_sound_mode:(NSString*)sound_mode;

+ (JSON_DICT)createTrackAuthorizationTrackingStatusData_status:(SuperfineTrackingAuthorizationTrackingStatus)status;

+ (JSON_DICT)createTrackAccountData_id:(NSString*)id
                                  type:(NSString*)type;

+ (JSON_DICT)createTrackWalletData_wallet:(NSString*)wallet
                                     type:(NSString*)type;

+ (JSON_DICT)createTrackCryptoPaymentData_pack:(NSString*)pack
                                         price:(NSString*)price
                                        amount:(float)amount
                                      currency:(NSString*)currency
                                          type:(NSString*)type
                                         count:(int)count;

@end

NS_ASSUME_NONNULL_END
