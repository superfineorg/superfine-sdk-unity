#import <Foundation/Foundation.h>
#import "SuperfineTrackingHTTPClient.h"
#import "SuperfineTrackingIntegration.h"
#import "SuperfineTrackingStorage.h"

@class SuperfineTrackingTrackOperation;

NS_ASSUME_NONNULL_BEGIN

FOUNDATION_EXPORT NSString *const SuperfineTrackingDidSendRequestNotification;
FOUNDATION_EXPORT NSString *const SuperfineTrackingRequestDidSucceedNotification;
FOUNDATION_EXPORT NSString *const SuperfineTrackingRequestDidFailNotification;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_VERSION_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_BUILD_VERSION_KEY;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_BUILD_NUMBER_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_APP_VERSION_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_BUNDLE_ID_KEY;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_PLATFORM_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_COUNTRY_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_DEVICE_MODEL_KEY;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_OS_VERSION_KEY;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_ADVERTISTING_ID_KEY;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingContext_DEVELOPER_DEVICE_ID_KEY;

@interface SuperfineTrackingManagerIntegration : NSObject <SuperfineTrackingIntegration>

- (id)init:(SuperfineTrackingManager *)trackingManager httpClient:(SuperfineTrackingHTTPClient *)httpClient fileStorage:(id<SuperfineTrackingStorage>)fileStorage userDefaultsStorage:(id<SuperfineTrackingStorage>)userDefaultsStorage;

//- (NSDictionary *)staticContext;
//- (NSDictionary *)liveContext;

- (id)getContextValue:(NSString *_Nonnull)key;

@end

NS_ASSUME_NONNULL_END
