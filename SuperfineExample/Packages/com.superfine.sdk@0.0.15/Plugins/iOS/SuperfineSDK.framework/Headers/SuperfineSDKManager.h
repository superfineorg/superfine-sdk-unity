#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

#import <SuperfineSDK/SuperfineSDKEvent.h>
#import <SuperfineSDK/SuperfineSDKConfiguration.h>
#import <SuperfineSDK/SuperfineSDKTypes.h>
#import <SuperfineSDK/SuperfineSDKThirdPartySharingSettings.h>
#import <SuperfineSDK/SuperfineSDKLifecycleDelegate.h>
#import <SuperfineSDK/SuperfineSDKDeepLinkDelegate.h>
#import <SuperfineSDK/SuperfineSDKDeviceTokenDelegate.h>
#import <SuperfineSDK/SuperfineSDKSendEventDelegate.h>

NS_ASSUME_NONNULL_BEGIN

@class SuperfineSDKConfiguration;

@interface SuperfineSDKManager : NSObject<UIApplicationDelegate>

+ (SuperfineSDKConfiguration * _Nullable)createConfigurationFromJson:(NSString *)jsonString;

+ (void)addModuleClassNameMap:(NSString* _Nonnull)name className:(NSString* _Nonnull)className;

+ (void)initialize:(SuperfineSDKConfiguration *_Nonnull)configuration;
+ (void)initialize:(SuperfineSDKConfiguration *_Nonnull)configuration completion:(InitializationCompletion _Nullable)completion;

+ (void)shutdown;

+ (void)setupSwizzle:(SuperfineSDKAppSwizzleFlag)flag;

+ (void)debug:(BOOL)showDebugLogs;

+ (instancetype _Nullable)sharedInstance;

+ (NSString*)serializeObject:(id)object;

+ (NSDictionary *_Nullable)deepCopyDictionary:(NSDictionary* _Nullable)dictionary isMutable:(BOOL)isMutable;
+ (NSArray *_Nullable)deepCopyArray:(NSArray* _Nullable)array isMutable:(BOOL)isMutable;

+ (void)logDebug:(NSString *)format, ...;

- (BOOL)isInitialized;

- (id)getModule:(NSString*)className;

- (JSON_DICT _Nullable)getSdkConfig;
- (NSString*)getSdkConfigString;

- (NSString*)getAppId;

- (NSString*)getFacebookAppId;
- (NSString*)getInstagramAppId;

- (NSString*)getAppleAppId;
- (NSString*)getAppleSignInClientId;
- (NSString*)getAppleDeveloperTeamId;

- (NSString*)getGooglePlayGameServicesProjectId;
- (NSString*)getGooglePlayDeveloperAccountId;

- (NSString*)getLinkedInAppId;

- (NSString*)getQQAppId;
- (NSString*)getWeChatAppId;
- (NSString*)getTikTokAppId;

- (NSString*)getSnapAppId;

- (BOOL)log:(NSString *)eventName;
- (BOOL)log:(NSString *)eventName flag:(SuperfineSDKOperationFlag)flag;

- (BOOL)log:(NSString *)eventName intValue:(NSInteger)value;
- (BOOL)log:(NSString *)eventName intValue:(NSInteger)value flag:(SuperfineSDKOperationFlag)flag;

- (BOOL)log:(NSString *)eventName stringValue:(NSString* _Nullable)value;
- (BOOL)log:(NSString *)eventName stringValue:(NSString* _Nullable)value flag:(SuperfineSDKOperationFlag)flag;

- (BOOL)log:(NSString *)eventName mapValue:(NSDictionary<NSString*, NSString*>* _Nullable)value;
- (BOOL)log:(NSString *)eventName mapValue:(NSDictionary<NSString*, NSString*>* _Nullable)value flag:(SuperfineSDKOperationFlag)flag;

- (BOOL)log:(NSString *)eventName jsonValue:(JSON_DICT _Nullable)value;
- (BOOL)log:(NSString *)eventName jsonValue:(JSON_DICT _Nullable)value flag:(SuperfineSDKOperationFlag)flag;

- (BOOL)logEvent:(SuperfineSDKEvent* _Nonnull)event;
+ (void)logEvent:(SuperfineSDKEvent* _Nonnull)event;

+ (void)setUserActivity:(NSUserActivity*)userActivity;
+ (NSUserActivity*)getUserActivity;

+ (void)openURL:(NSURL *)url;
+ (NSURL *)getDeepLinkUrl;

- (NSDictionary*)getLaunchOptions;

+ (void)setDeviceToken:(NSData *)deviceToken;
+ (void)setDeviceTokenString:(NSString *)deviceTokenString;

+ (NSData *)getDeviceToken;
+ (NSString *)getDeviceTokenString;

- (void)flush;
- (void)reset;

+ (NSString *)version;

- (NSString *)getUserId;

- (NSString *)getSessionId;

- (SuperfineSDKStoreType)getStoreType;

- (NSURL *)getHost;
- (NSURL *)getConfigUrl;

- (NSString *)getCredential;

- (void)setCustomUserId:(NSString *_Nonnull)value;
- (void)setConfigId:(NSString *_Nonnull)value;

- (id)getContextValue:(NSString *_Nonnull)key;

- (BOOL)checkReady;

+ (void)addLifecycleDelegate:(id<SuperfineSDKLifecycleDelegate>)delegate;
+ (void)removeLifecycleDelegate:(id<SuperfineSDKLifecycleDelegate>)delegate;

+ (void)addSendEventDelegate:(id<SuperfineSDKSendEventDelegate>)delegate;
+ (void)removeSendEventDelegate:(id<SuperfineSDKSendEventDelegate>)delegate;

+ (void)addDeepLinkDelegate:(id<SuperfineSDKDeepLinkDelegate>)delegate;
+ (void)removeDeepLinkDelegate:(id<SuperfineSDKDeepLinkDelegate>)delegate;

+ (void)addDeviceTokenDelegate:(id<SuperfineSDKDeviceTokenDelegate>)delegate;
+ (void)removeDeviceTokenDelegate:(id<SuperfineSDKDeviceTokenDelegate>)delegate;

- (void)start;
- (void)stop;

- (void)setOffline:(BOOL)value;

- (void)gdprForgetMe;

- (void)onFinishFlush;
- (void)onSendEvent:(NSMutableDictionary *_Nonnull)event;

- (void)disableThirdPartySharing;
- (void)enableThirdPartySharing;

- (void)logThirdPartySharing_settings:(SuperfineSDKThirdPartySharingSettings* _Nonnull)settings;

- (void)logBootStart;
- (void)logBootEnd;

- (void)logLevelStart_id:(int)levelId name:(NSString* _Nonnull)name;
- (void)logLevelEnd_id:(int)levelId name:(NSString* _Nonnull)name isSuccess:(bool)isSuccess;

- (void)logAdLoad_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType;
- (void)logAdLoad_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement;
- (void)logAdClose_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType;
- (void)logAdClose_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement;
- (void)logAdClick_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType;
- (void)logAdClick_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement;
- (void)logAdImpression_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType;
- (void)logAdImpression_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement;

- (void)logIAPInitialization:(bool)isSuccess;
- (void)logIAPRestorePurchase;
- (void)logIAPResult_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency isSuccess:(bool)isSuccess;

- (void)logIAPReceipt_Apple_receipt:(NSString* _Nonnull)receipt;
- (void)logIAPReceipt_Google_data:(NSString* _Nonnull)data signature:(NSString* _Nonnull)signature;
- (void)logIAPReceipt_Amazon_userId:(NSString* _Nonnull)userId receiptId:(NSString* _Nonnull)receiptId;
- (void)logIAPReceipt_Roku_transactionId:(NSString* _Nonnull)transactionId;
- (void)logIAPReceipt_Windows_receipt:(NSString* _Nonnull)receipt;
- (void)logIAPReceipt_Facebook_receipt:(NSString* _Nonnull)receipt;

- (void)logIAPReceipt_Unity_receipt:(NSString* _Nonnull)receipt;

- (void)logIAPReceipt_AppStoreServer_transactionId:(NSString* _Nonnull)transactionId;

- (void)logIAPReceipt_GooglePlayProduct_productId:(NSString* _Nonnull)productId token:(NSString* _Nonnull)token;
- (void)logIAPReceipt_GooglePlaySubscription_subscriptionId:(NSString* _Nonnull)subscriptionId token:(NSString* _Nonnull)token;
- (void)logIAPReceipt_GooglePlaySubscriptionv2_token:(NSString* _Nonnull)token;

- (void)logUpdateApp:(NSString* _Nonnull)newVersion;
- (void)logRateApp;

- (void)logLocation_latitude:(double)latitude longitude:(double)longitude;

- (void)logTrackingAuthorizationStatus:(SuperfineSDKTrackingAuthorizationStatus)status;

- (void)logFacebookLink:(NSString* _Nonnull)userId;
- (void)logFacebookUnlink;
- (void)logInstagramLink:(NSString* _Nonnull)userId;
- (void)logInstagramUnlink;
- (void)logAppleLink:(NSString* _Nonnull)userId;
- (void)logAppleUnlink;
- (void)logAppleGameCenterLink:(NSString* _Nonnull)gamePlayerId;
- (void)logAppleGameCenterTeamLink:(NSString* _Nonnull)teamPlayerId;
- (void)logAppleGameCenterUnlink;
- (void)logGoogleLink:(NSString* _Nonnull)userId;
- (void)logGoogleUnlink;
- (void)logGooglePlayGameServicesLink:(NSString* _Nonnull)gamePlayerId;
- (void)logGooglePlayGameServicesDeveloperLink:(NSString* _Nonnull)developerPlayerKey;
- (void)logGooglePlayGameServicesUnlink;
- (void)logLinkedInLink:(NSString* _Nonnull)personId;
- (void)logLinkedInUnlink;
- (void)logMeetupLink:(NSString* _Nonnull)userId;
- (void)logMeetupUnlink;
- (void)logGitHubLink:(NSString* _Nonnull)userId;
- (void)logGitHubUnlink;
- (void)logDiscordLink:(NSString* _Nonnull)userId;
- (void)logDiscordUnlink;
- (void)logTwitterLink:(NSString* _Nonnull)userId;
- (void)logTwitterUnlink;
- (void)logSpotifyLink:(NSString* _Nonnull)userId;
- (void)logSpotifyUnlink;
- (void)logMicrosoftLink:(NSString* _Nonnull)userId;
- (void)logMicrosoftUnlink;
- (void)logLINELink:(NSString* _Nonnull)userId;
- (void)logLINEUnlink;
- (void)logVKLink:(NSString* _Nonnull)userId;
- (void)logVKUnlink;
- (void)logQQLink:(NSString* _Nonnull)openId;
- (void)logQQUnionLink:(NSString* _Nonnull)unionId;
- (void)logQQUnlink;
- (void)logWeChatLink:(NSString* _Nonnull)openId;
- (void)logWeChatUnionLink:(NSString* _Nonnull)unionId;
- (void)logWeChatUnlink;
- (void)logTikTokLink:(NSString* _Nonnull)openId;
- (void)logTikTokUnionLink:(NSString* _Nonnull)unionId;
- (void)logTikTokUnlink;
- (void)logWeiboLink:(NSString* _Nonnull)userId;
- (void)logWeiboUnlink;

- (void)addUserPhoneNumber_countryCode:(int)countryCode number:(NSString* _Nonnull)number;
- (void)removeUserPhoneNumber_countryCode:(int)countryCode number:(NSString* _Nonnull)number;
- (void)addUserEmail:(NSString* _Nonnull)email;
- (void)removeUserEmail:(NSString* _Nonnull)email;
- (void)setUserName_firstName:(NSString* _Nullable)firstName lastName:(NSString* _Nullable)lastName;
- (void)setUserCity:(NSString* _Nonnull)city;
- (void)setUserState:(NSString* _Nonnull)state;
- (void)setUserCountry:(NSString* _Nonnull)country;
- (void)setUserZipCode:(NSString* _Nonnull)zipCode;
- (void)setUserDateOfBirth_day:(int)day month:(int)month year:(int)year;
- (void)setUserDateOfBirth_day:(int)day month:(int)month;
- (void)setUserYearOfBirth:(int)year;
- (void)setUserGender:(SuperfineSDKUserGender)gender;

- (void)logAccountLink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)logAccountLink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type scopeId:(NSString* _Nonnull)scopeId;
- (void)logAccountLink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type scopeId:(NSString* _Nonnull)scopeId scopeType:(NSString* _Nonnull)scopeType;
- (void)logAccountUnlink_type:(NSString* _Nonnull)type;

- (void)logWalletLink_wallet:(NSString* _Nonnull)wallet;
- (void)logWalletLink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;
- (void)logWalletUnlink_wallet:(NSString* _Nonnull)wallet;
- (void)logWalletUnlink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;

- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount;
- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency;
- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency chain:(NSString* _Nonnull)chain;

- (void)logAdRevenue_source:(NSString* _Nonnull)source revenue:(double)revenue currency:(NSString* _Nonnull)currency;
- (void)logAdRevenue_source:(NSString* _Nonnull)source revenue:(double)revenue currency:(NSString* _Nonnull)currency network:(NSString* _Nullable)network;
- (void)logAdRevenue_source:(NSString* _Nonnull)source revenue:(double)revenue currency:(NSString* _Nonnull)currency network:(NSString* _Nullable)network networkData:(JSON_DICT _Nullable)networkData;

- (void)fetchRemoteConfig:(FetchRemoteConfigCompletion _Nonnull)completion;

+ (void)requestNotificationAuthorization:(NotificationAuthorizationCompletion _Nonnull)completion options:(UNAuthorizationOptions)options;
+ (void)requestNotificationAuthorization:(NotificationAuthorizationCompletion _Nonnull)completion options:(UNAuthorizationOptions)options registerRemote:(BOOL)registerRemote;

+ (void)registerForRemoteNotifications:(RegisterRemoteNotificationCompletion _Nonnull)completion;
+ (void)unregisterForRemoteNotifications;
+ (BOOL)isRegisteredForRemoteNotifications;

+ (void)onFailedToRegisterForRemoteNotificationsWithError:(NSError *)error;
+ (void)onRegisteredForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken;

- (BOOL)canUpdatePostbackConversionValue;

+ (void)requestTrackingAuthorization:(TrackingAuthorizationCompletion _Nonnull)completion;
+ (NSUInteger)getTrackingAuthorizationStatus;

+ (void)registerAppForAdNetworkAttribution;

+ (void)updatePostbackConversionValue:(NSInteger)conversionValue;
+ (void)updatePostbackConversionValue:(NSInteger)conversionValue
                          coarseValue:(NSString*)coarseValue;
+ (void)updatePostbackConversionValue:(NSInteger)conversionValue
                          coarseValue:(NSString*)coarseValue
                           lockWindow:(BOOL)lockWindow;

@end

NS_ASSUME_NONNULL_END
