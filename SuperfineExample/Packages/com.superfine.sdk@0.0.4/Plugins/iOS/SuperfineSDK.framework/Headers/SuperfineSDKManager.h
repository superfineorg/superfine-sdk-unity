#import <Foundation/Foundation.h>
#import <SuperfineSDK/SuperfineSDKConfiguration.h>
#import <SuperfineSDK/SuperfineSDKTypes.h>

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
- (void)logIAPBuyStart_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency;
- (void)logIAPBuyStart_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency transactionId:(NSString* _Nonnull)transactionId receipt:(NSString* _Nullable)receipt;
- (void)logIAPBuyEnd_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency isSuccess:(bool)isSuccess;
- (void)logIAPBuyEnd_pack:(NSString* _Nonnull)pack price:(double)price amount:(NSUInteger)amount currency:(NSString* _Nonnull)currency transactionId:(NSString* _Nonnull)transactionId receipt:(NSString* _Nullable)receipt isSuccess:(bool)isSuccess;

- (void)logFacebookLogin:(NSString* _Nonnull)facebookId;
- (void)logFacebookLogout:(NSString* _Nonnull)facebookId;

- (void)logUpdateGame:(NSString* _Nonnull)newVersion;
- (void)logRateGame;

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
