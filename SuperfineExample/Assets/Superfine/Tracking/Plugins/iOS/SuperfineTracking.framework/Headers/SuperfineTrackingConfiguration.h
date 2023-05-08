#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol SuperfineTrackingApplicationProtocol <NSObject>
@property (nullable, nonatomic, assign) id<UIApplicationDelegate> delegate;
- (NSUInteger)superfine_tracking_beginBackgroundTaskWithName:(nullable NSString *)taskName expirationHandler:(void (^__nullable)(void))handler;
- (void)superfine_tracking_endBackgroundTask:(NSUInteger)identifier;
@end


@interface UIApplication (SuperfineTrackingApplicationProtocol) <SuperfineTrackingApplicationProtocol>
@end

typedef NSMutableURLRequest *_Nonnull (^SuperfineTrackingRequestFactory)(NSURL *_Nonnull);

@protocol SuperfineTrackingCrypto;
@protocol SuperfineTrackingMiddleware;

/**
 * This object provides a set of properties to control various policies of the posthog client. Other than `apiKey`, these properties can be changed at any time.
 */
@interface SuperfineTrackingConfiguration : NSObject


+ (_Nonnull instancetype)configurationWithAppId:(NSString *_Nonnull)appId appSecret:(NSString *_Nonnull)appSecret;

@property (nonatomic, copy, readonly, nonnull) NSString *appId;
@property (nonatomic, copy, readonly, nonnull) NSString *appSecret;

/**
 * Override the $lib property, used by the React Native client
 */
@property (nonatomic, copy, nonnull) NSString *libraryName;

/**
 * Override the $lib_version property, used by the React Native client
 */
@property (nonatomic, copy, nonnull) NSString *libraryVersion;

/**
 * Whether the posthog client should use location services.
 * If `YES` and the host app hasn't asked for permission to use location services then the user will be presented with an alert view asking to do so. `NO` by default.
 * If `YES`, please make sure to add a description for `NSLocationAlwaysUsageDescription` in your `Info.plist` explaining why your app is accessing Location APIs.
 */
//@property (nonatomic, assign) BOOL shouldUseLocationServices;

/**
 * The number of queued events that the posthog client should flush at. Setting this to `1` will not queue any events and will use more battery. `20` by default.
 */
@property (nonatomic, assign) NSUInteger flushAt;

/**
 * The amount of time to wait before each tick of the flush timer.
 * Smaller values will make events delivered in a more real-time manner and also use more battery.
 * A value smaller than 10 seconds will seriously degrade overall performance.
 * 30 seconds by default.
 */
@property (nonatomic, assign) NSTimeInterval flushInterval;

/**
 * The maximum number of items to queue before starting to drop old ones. This should be a value greater than zero, the behaviour is undefined otherwise. `1000` by default.
 */
@property (nonatomic, assign) NSUInteger maxQueueSize;

/**
 * Whether the posthog client should automatically capture in-app purchases from the App Store.
 */
@property (nonatomic, assign) BOOL captureInAppPurchases;

/**
 * Dictionary indicating the options the app was launched with.
 */
@property (nonatomic, strong, nullable) NSDictionary *launchOptions;

/**
 * Set a custom request factory.
 */
@property (nonatomic, strong, nullable) SuperfineTrackingRequestFactory requestFactory;

/**
 * Set a custom crypto
 */
@property (nonatomic, strong, nullable) id<SuperfineTrackingCrypto> crypto;

/**
 * Set custom middlewares. Will be run before all integrations
 */
@property (nonatomic, strong, nullable) NSArray<id<SuperfineTrackingMiddleware>> *middlewares;

/**
 * Leave this nil for iOS extensions, otherwise set to UIApplication.sharedApplication.
 */
@property (nonatomic, strong, nullable) id<SuperfineTrackingApplicationProtocol> application;

/**
 * A dictionary of filters to redact payloads before they are sent.
 * This is an experimental feature that currently only applies to Deep Links.
 * It is subject to change to allow for more flexible customizations in the future.
 *
 * The key of this dictionary should be a regular expression string pattern,
 * and the value should be a regular expression substitution template.
 *
 * By default, this contains a Facebook auth token filter, configured as such:
 * @code
 * @"(fb\\d+://authorize#access_token=)([^ ]+)": @"$1((redacted/fb-auth-token))"
 * @endcode
 *
 * This will replace any matching occurences to a redacted version:
 * @code
 * "fb123456789://authorize#access_token=secretsecretsecretsecret&some=data"
 * @endcode
 *
 * Becomes:
 * @code
 * "fb123456789://authorize#access_token=((redacted/fb-auth-token))"
 * @endcode
 *
 */
@property (nonatomic, strong, nonnull) NSDictionary<NSString*, NSString*>* payloadFilters;

/**
 * An optional delegate that handles NSURLSessionDelegate callbacks
 */
@property (nonatomic, strong, nullable) id<NSURLSessionDelegate> httpSessionDelegate;

@property (nonatomic, assign) BOOL waitConfigId;
@property (nonatomic, assign) BOOL customUserId;

@property (nonatomic, copy) NSString * _Nullable userId;

@property (nonatomic, assign) BOOL debug;

@end
