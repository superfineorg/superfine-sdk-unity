#import <Foundation/Foundation.h>
#import "SuperfineTrackingMiddleware.h"
#import "SuperfineTrackingManagerIntegration.h"

/**
 * NSNotification name, that is posted after integrations are loaded.
 */
extern NSString *_Nonnull SuperfineTrackingIntegrationDidStart;

@class SuperfineTrackingManager;


@interface SuperfineTrackingOperationQueue : NSObject

@property (nonatomic, strong) SuperfineTrackingManagerIntegration *integration;

- (instancetype _Nonnull)init:(SuperfineTrackingManager *_Nonnull)trackingManager;

- (NSString *_Nonnull)getUserId;
- (NSString *_Nonnull)createUserId;
- (void)saveUserId:(NSString *_Nonnull)userId;

@end


@interface SuperfineTrackingOperationQueue (SuperfineTrackingMiddleware) <SuperfineTrackingMiddleware>

@end
