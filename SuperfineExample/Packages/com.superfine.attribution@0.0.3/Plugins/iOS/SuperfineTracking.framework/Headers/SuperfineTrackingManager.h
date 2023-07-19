#import <Foundation/Foundation.h>
#import <SuperfineTracking/SuperfineTrackingConfiguration.h>
#import <SuperfineTracking/SuperfineTrackingEnumTypes.h>

NS_ASSUME_NONNULL_BEGIN

/**
 * This object provides an API for recording posthog.
 */
@class SuperfineTrackingConfiguration;


@interface SuperfineTrackingManager : NSObject

/**
 * Whether or not the posthog client is currently enabled.
 */
@property (nonatomic, assign, readonly) BOOL enabled;

/**
 * Used by the posthog client to configure various options.
 */
@property (nonatomic, strong, readonly) SuperfineTrackingConfiguration *configuration;

/**
 * Setup this posthog client instance.
 *
 * @param configuration The configuration used to setup the client.
 */
- (instancetype)initWithConfiguration:(SuperfineTrackingConfiguration *)configuration;

/**
 * Setup the posthog client.
 *
 * @param configuration The configuration used to setup the client.
 */
+ (void)setupWithConfiguration:(SuperfineTrackingConfiguration *)configuration;

/**
 * Shutdown the client.
 */
+ (void)shutdown;

/**
 * Enabled/disables debug logging to trace your data going through the SDK.
 *
 * @param showDebugLogs `YES` to enable logging, `NO` otherwise. `NO` by default.
 */
+ (void)debug:(BOOL)showDebugLogs;

/**
 * Returns the shared posthog client.
 *
 * @see -setupWithConfiguration:
 */
+ (instancetype _Nullable)sharedTrackingManager;

- (void)track:(NSString *)eventName;
- (void)track:(NSString *)eventName intValue:(NSInteger)value;
- (void)track:(NSString *)eventName stringValue:(NSString* _Nonnull)value;
- (void)track:(NSString *)eventName jsonValue:(JSON_DICT _Nullable)value;

// todo: docs
- (void)receivedRemoteNotification:(NSDictionary *)userInfo;
- (void)failedToRegisterForRemoteNotificationsWithError:(NSError *)error;
- (void)registeredForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken;
- (void)handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo;
- (void)continueUserActivity:(NSUserActivity *)activity;
- (void)openURL:(NSURL *)url options:(NSDictionary *)options;

/*!
 @method

 @abstract
 Trigger an upload of all queued events.

 @discussion
 This is useful when you want to force all messages queued on the device to be uploaded. Please note that not all integrations
 respond to this method.
 */
- (void)flush;

/*!
 @method

 @abstract
 Reset any user state that is cached on the device.

 @discussion
 This is useful when a user logs out and you want to clear the identity. It will clear any
 properties or distinctId's cached on the device.
 */
- (void)reset;

/*!
 @method

 @abstract
 Enable the sending of posthog data. Enabled by default.

 @discussion
 Occasionally used in conjunction with disable user opt-out handling.
 */
- (void)enable;


/*!
 @method

 @abstract
 Completely disable the sending of any posthog data.

 @discussion
 If have a way for users to actively or passively (sometimes based on location) opt-out of
 posthog data collection, you can use this method to turn off all data collection.
 */
- (void)disable;

/**
 * Version of the library.
 */
+ (NSString *)version;

/** Returns the anonymous ID of the current user. */
- (NSString *)getUserId;

- (NSURL *)getHost;
- (NSString *)getCredential;

- (void)setUserId:(NSString *_Nonnull)value;
- (void)setConfigId:(NSString *_Nonnull)value;

- (id)getContextValue:(NSString *_Nonnull)key;

/** Returns the configuration used to create the posthog client. */
- (SuperfineTrackingConfiguration *)configuration;

- (void)start;

- (void)trackBootStart;
- (void)trackBootEnd;

- (void)trackLevelStart_id:(int)levelId name:(NSString* _Nonnull)name;
- (void)trackLevelEnd_id:(int)levelId name:(NSString* _Nonnull)name isSuccess:(bool)isSuccess;

- (void)trackAdLoad_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType;
- (void)trackAdLoad_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement;
- (void)trackAdClose_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType;
- (void)trackAdClose_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement;
- (void)trackAdClick_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType;
- (void)trackAdClick_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement;
- (void)trackAdImpression_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType;
- (void)trackAdImpression_adUnit:(NSString* _Nonnull)adUnit adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement;

- (void)trackIAPInitialization:(bool)isSuccess;
- (void)trackIAPRestorePurchase;
- (void)trackIAPBuyStart_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency;
- (void)trackIAPBuyStart_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency transactionId:(NSString* _Nonnull)transactionId receipt:(NSString* _Nullable)receipt;
- (void)trackIAPBuyEnd_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency isSuccess:(bool)isSuccess;
- (void)trackIAPBuyEnd_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency transactionId:(NSString* _Nonnull)transactionId receipt:(NSString* _Nullable)receipt isSuccess:(bool)isSuccess;

- (void)trackFacebookLogin:(NSString* _Nonnull)facebookId;
- (void)trackFacebookLogout:(NSString* _Nonnull)facebookId;

- (void)trackUpdateGame:(NSString* _Nonnull)newVersion;
- (void)trackRateGame;

- (void)trackAuthorizationTrackingStatus:(SuperfineTrackingAuthorizationTrackingStatus)status;

- (void)trackAccountLogin_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)trackAccountLogout_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)trackAccountLink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)trackAccountUnlink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;

- (void)trackWalletLink_wallet:(NSString* _Nonnull)wallet;
- (void)trackWalletLink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;
- (void)trackWalletUnlink_wallet:(NSString* _Nonnull)wallet;
- (void)trackWalletUnlink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;

- (void)trackCryptoPayment_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount;
- (void)trackCryptoPayment_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency;
- (void)trackCryptoPayment_pack:(NSString* _Nonnull)pack price:(float)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency chain:(NSString* _Nonnull)chain;

- (void)requestTrackingAuthorization:(TrackingAuthorizationCompletion)completion;
- (NSUInteger)getTrackingAuthorizationStatus;

- (void)updatePostbackConversionValue:(NSInteger)conversionValue;
- (void)updatePostbackConversionValue:(NSInteger)conversionValue
                          coarseValue:(NSString*)coarseValue;
- (void)updatePostbackConversionValue:(NSInteger)conversionValue
                          coarseValue:(NSString*)coarseValue
                           lockWindow:(BOOL)lockWindow;

@end

NS_ASSUME_NONNULL_END
