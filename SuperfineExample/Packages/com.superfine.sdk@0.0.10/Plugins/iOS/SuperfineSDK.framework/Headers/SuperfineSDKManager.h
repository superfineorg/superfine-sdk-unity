#import <Foundation/Foundation.h>
#import <SuperfineSDK/SuperfineSDKConfiguration.h>
#import <SuperfineSDK/SuperfineSDKTypes.h>
#import <SuperfineSDK/SuperfineSDKThirdPartySharingSettings.h>

NS_ASSUME_NONNULL_BEGIN

@class SuperfineSDKConfiguration;

@interface SuperfineSDKManager : NSObject

@property (nonatomic, strong, readonly) SuperfineSDKConfiguration *configuration;

@property (nonatomic, readwrite, copy, nullable) SendEventCallback sendEventCallback;
@property (nonatomic, readwrite) int sendEventRequestCode;

- (instancetype)initWithConfiguration:(SuperfineSDKConfiguration *)configuration;
+ (void)setupWithConfiguration:(SuperfineSDKConfiguration *)configuration;

+ (void)shutdown;

+ (void)debug:(BOOL)showDebugLogs;

+ (instancetype _Nullable)sharedInstance;

- (void)log:(NSString *)eventName;
- (void)log:(NSString *)eventName intValue:(NSInteger)value;
- (void)log:(NSString *)eventName stringValue:(NSString* _Nonnull)value;
- (void)log:(NSString *)eventName jsonValue:(JSON_DICT _Nullable)value;
- (void)log:(NSString *)eventName mapValue:(NSDictionary<NSString*, NSString*>* _Nullable)value;

- (void)openURL:(NSURL *)url;

- (void)setDeviceToken:(NSData *)deviceToken;
- (void)setDeviceTokenString:(NSString *)deviceTokenString;

- (void)flush;
- (void)reset;

+ (NSString *)version;

- (NSString *)getUserId;

- (NSURL *)getHost;
- (NSString *)getCredential;

- (void)setCustomUserId:(NSString *_Nonnull)value;
- (void)setConfigId:(NSString *_Nonnull)value;

- (id)getContextValue:(NSString *_Nonnull)key;

- (BOOL)checkReady;

- (SuperfineSDKConfiguration *)configuration;

- (void)start;
- (void)stop;

- (void)gdprForgetMe;

- (void)onFinishFlush;

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

- (void)logFacebookLogin:(NSString* _Nonnull)facebookId;
- (void)logFacebookLogout:(NSString* _Nonnull)facebookId;

- (void)logUpdateApp:(NSString* _Nonnull)newVersion;
- (void)logRateApp;

- (void)logLocation_latitude:(double)latitude longtitude:(double)longitude;

- (void)logAuthorizationTrackingStatus:(SuperfineSDKAuthorizationTrackingStatus)status;

- (void)logAccountLogin_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)logAccountLogout_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)logAccountLink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;
- (void)logAccountUnlink_id:(NSString* _Nonnull)accountId type:(NSString* _Nonnull)type;

- (void)logWalletLink_wallet:(NSString* _Nonnull)wallet;
- (void)logWalletLink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;
- (void)logWalletUnlink_wallet:(NSString* _Nonnull)wallet;
- (void)logWalletUnlink_wallet:(NSString* _Nonnull)wallet type:(NSString* _Nonnull)type;

- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount;
- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency;
- (void)logCryptoPayment_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency chain:(NSString* _Nonnull)chain;

- (void)logAdRevenue_network:(NSString* _Nonnull)network revenue:(double)revenue currency:(NSString* _Nonnull)currency;
- (void)logAdRevenue_network:(NSString* _Nonnull)network revenue:(double)revenue currency:(NSString* _Nonnull)currency mediation:(NSString* _Nullable)mediation;
- (void)logAdRevenue_network:(NSString* _Nonnull)network revenue:(double)revenue currency:(NSString* _Nonnull)currency mediation:(NSString* _Nullable)mediation networkData:(JSON_DICT _Nullable)networkData;

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
