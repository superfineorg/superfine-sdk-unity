#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import <SuperfineSDK/SuperfineSDKTypes.h>

typedef NSMutableURLRequest *_Nonnull (^SuperfineSDKRequestFactory)(NSURL *_Nonnull);

@protocol SuperfineSDKApplicationProtocol;

@protocol SuperfineSDKCrypto;
@protocol SuperfineSDKMiddleware;

@interface SuperfineSDKConfigurationModuleData : NSObject

-(instancetype _Nonnull)init;

-(instancetype _Nonnull)initWithClassName:(NSString* _Nonnull)className andData:(NSDictionary* _Nullable)data;

-(instancetype _Nonnull)initWithModuleData:(SuperfineSDKConfigurationModuleData* _Nonnull)moduleData;

@property (nonatomic, copy, readwrite, nullable) NSString *className;
@property (nonatomic, strong, nullable) NSMutableDictionary *data;

@end

@interface SuperfineSDKInitializationAction : NSObject

-(instancetype _Nonnull)init:(SuperfineSDKInitializationActionType)type;

-(instancetype _Nonnull)init:(SuperfineSDKInitializationActionType)type andData:(NSDictionary* _Nullable)data;

@property (nonatomic) SuperfineSDKInitializationActionType type;
@property (nonatomic, strong, nullable) NSMutableDictionary *data;

@end

@interface SuperfineSDKConfiguration : NSObject

+ (_Nonnull instancetype)configurationWithAppId:(NSString *_Nonnull)appId appSecret:(NSString *_Nonnull)appSecret;

- (void)addModule:(NSString *_Nonnull)className;
- (void)addModule:(NSString *_Nonnull)className withString:(NSString *_Nonnull)dataString;
- (void)addModule:(NSString *_Nonnull)className withDictionary:(NSDictionary *_Nonnull)data;

- (NSMutableArray* _Nullable)getModuleDataList;

- (void)addInitializationAction:(SuperfineSDKInitializationActionType)type;
- (void)addInitializationAction:(SuperfineSDKInitializationActionType)type withString:(NSString *_Nonnull)dataString;
- (void)addInitializationAction:(SuperfineSDKInitializationActionType)type withDictionary:(NSDictionary *_Nonnull)data;

- (NSMutableArray* _Nullable)getInitializationActionList;

- (void)addExcludedModuleName:(NSString *_Nonnull)name;
- (void)addExcludedModuleNames:(NSArray *_Nonnull)names;

- (NSMutableArray* _Nullable) getExcludedModuleNames;

@property (nonatomic, copy, readonly, nonnull) NSString *appId;
@property (nonatomic, copy, readonly, nonnull) NSString *appSecret;

@property (nonatomic, copy, nonnull) NSString *libraryName;
@property (nonatomic, copy, nonnull) NSString *libraryVersion;

//@property (nonatomic, assign) BOOL shouldUseLocationServices;

@property (nonatomic, strong, nullable) SuperfineSDKRequestFactory requestFactory;
@property (nonatomic, strong, nullable) id<SuperfineSDKCrypto> crypto;
@property (nonatomic, strong, nullable) NSArray<id<SuperfineSDKMiddleware>> *middlewares;
@property (nonatomic, strong, nullable) id<SuperfineSDKApplicationProtocol> application;

@property (nonatomic, copy, nullable) NSString* facebookAppId;
@property (nonatomic, copy, nullable) NSString* instagramAppId;

@property (nonatomic, copy, nullable) NSString* appleAppId;
@property (nonatomic, copy, nullable) NSString* appleSignInClientId;
@property (nonatomic, copy, nullable) NSString* appleDeveloperTeamId;

@property (nonatomic, copy, nullable) NSString* googlePlayGameServicesProjectId;
@property (nonatomic, copy, nullable) NSString* googlePlayDeveloperAccountId;

@property (nonatomic, copy, nullable) NSString* linkedInAppId;

@property (nonatomic, copy, nullable) NSString* qqAppId;
@property (nonatomic, copy, nullable) NSString* weChatAppId;
@property (nonatomic, copy, nullable) NSString* tikTokAppId;

@property (nonatomic, copy, nullable) NSString* snapAppId;

@property (nonatomic, assign) NSUInteger flushAt;
@property (nonatomic, assign) NSTimeInterval flushInterval;

@property (nonatomic, assign) NSUInteger maxQueueSize;

@property (nonatomic, assign) BOOL captureInAppPurchases;

@property (nonatomic, assign) BOOL useSkanConversionSchema;
@property (nonatomic, assign) BOOL disableOnlineSdkConfig;

@property (nonatomic, strong, nonnull) NSDictionary<NSString*, NSString*>* payloadFilters;

@property (nonatomic, strong, nullable) id<NSURLSessionDelegate> httpSessionDelegate;

@property (nonatomic, assign) BOOL waitConfigId;

@property (nonatomic, assign) BOOL autoStart;

@property (nonatomic, assign) BOOL offline;
@property (nonatomic, assign) BOOL sendInBackground;

@property (nonatomic, assign) BOOL enableCoppa;

@property (nonatomic, copy, nullable) NSString *configId;
@property (nonatomic, copy, nullable) NSString *customUserId;

@property (nonatomic, copy, nullable) NSString *host;
@property (nonatomic, copy, nullable) NSString *configUrl;

@property (nonatomic, copy, nullable) NSString *wrapper;
@property (nonatomic, copy, nullable) NSString *wrapperVersion;

@property (nonatomic, assign) BOOL debug;

@property (nonatomic, assign) SuperfineSDKAppSwizzleFlag appSwizzleFlag;

@property (nonatomic, assign) SuperfineSDKStoreType storeType;

@property (nonatomic, strong, nullable) NSDictionary *launchOptions;

@end
