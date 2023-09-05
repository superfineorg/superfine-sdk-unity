#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import <SuperfineSDK/SuperfineSDKTypes.h>

typedef NSMutableURLRequest *_Nonnull (^SuperfineSDKRequestFactory)(NSURL *_Nonnull);

@protocol SuperfineSDKApplicationProtocol;

@protocol SuperfineSDKCrypto;
@protocol SuperfineSDKMiddleware;

@interface SuperfineSDKTenjinConfiguration : NSObject

+ (_Nonnull instancetype)configurationWithSdkKey:(NSString *_Nonnull)sdkKey;

@property (nonatomic, copy, readonly, nonnull) NSString *sdkKey;

@property (nonatomic, copy, nullable) NSString *deepLinkUrl;

@property (nonatomic, copy, nullable) NSArray *optInParams;
@property (nonatomic, copy, nullable) NSArray *optOutParams;

@property (nonatomic, assign) NSUInteger appSubversion;

@property (nonatomic, assign) BOOL cacheEventSetting;

@end

@interface SuperfineSDKConfiguration : NSObject

+ (_Nonnull instancetype)configurationWithAppId:(NSString *_Nonnull)appId appSecret:(NSString *_Nonnull)appSecret;

@property (nonatomic, copy, readonly, nonnull) NSString *appId;
@property (nonatomic, copy, readonly, nonnull) NSString *appSecret;

@property (nonatomic, copy, nonnull) NSString *libraryName;
@property (nonatomic, copy, nonnull) NSString *libraryVersion;

//@property (nonatomic, assign) BOOL shouldUseLocationServices;

@property (nonatomic, strong, nullable) SuperfineSDKRequestFactory requestFactory;
@property (nonatomic, strong, nullable) id<SuperfineSDKCrypto> crypto;
@property (nonatomic, strong, nullable) NSArray<id<SuperfineSDKMiddleware>> *middlewares;
@property (nonatomic, strong, nullable) id<SuperfineSDKApplicationProtocol> application;

@property (nonatomic, assign) NSUInteger flushAt;
@property (nonatomic, assign) NSTimeInterval flushInterval;

@property (nonatomic, assign) NSUInteger maxQueueSize;

@property (nonatomic, assign) BOOL captureInAppPurchases;

@property (nonatomic, strong, nonnull) NSDictionary<NSString*, NSString*>* payloadFilters;

@property (nonatomic, strong, nullable) id<NSURLSessionDelegate> httpSessionDelegate;

@property (nonatomic, assign) BOOL waitConfigId;

@property (nonatomic, assign) BOOL autoStart;

@property (nonatomic, assign) BOOL enableCoppa;

@property (nonatomic, copy, nullable) NSString *configId;
@property (nonatomic, copy, nullable) NSString *customUserId;

@property (nonatomic, copy, nullable) NSString *host;

@property (nonatomic, assign) BOOL debug;

@property (nonatomic, assign) SuperfineSDKStoreType storeType;

@property (nonatomic, retain, nullable) SuperfineSDKTenjinConfiguration *tenjinConfiguration;

@end
