#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, SuperfineTrackingOperationType) {
    // Should not happen, but default state
    OperationUndefined,
    
    // Core Capturing Methods
    OperationTrack,

    // General utility
    OperationReset,
    OperationFlush,
    
    // Remote Notification
    OperationReceivedRemoteNotification,
    OperationFailedToRegisterForRemoteNotifications,
    OperationRegisteredForRemoteNotifications,
    OperationHandleActionWithForRemoteNotification,

    // Application Lifecycle
    OperationApplicationLifecycle,
    //    OperaionApplicationDidFinishLaunching,
    //    OperationApplicationDidEnterBackground,
    //    OperationApplicationWillEnterForeground,
    //    OperationApplicationWillTerminate,
    //    OperationApplicationWillResignActive,
    //    OperationApplicationDidBecomeActive,

    // Misc.
    OperationContinueUserActivity,
    OperationOpenURL,
};

@class SuperfineTrackingManager;
@protocol SuperfineTrackingMutableContext;
@class SuperfineTrackingOperation;


@interface SuperfineTrackingContext : NSObject <NSCopying>

// Loopback reference to the top level PHGPostHog object.
// Not sure if it's a good idea to keep this around in the context.
// since we don't really want people to use it due to the circular
// reference and logic (Thus prefixing with underscore). But
// Right now it is required for integrations to work so I guess we'll leave it in.
@property (nonatomic, readonly, nonnull) SuperfineTrackingManager *trackingManager;
@property (nonatomic, readonly) SuperfineTrackingOperationType operationType;

@property (nonatomic, readonly, nullable) NSError *error;
@property (nonatomic, readonly, nullable) SuperfineTrackingOperation *operation;
@property (nonatomic, readonly) BOOL debug;

- (instancetype _Nonnull)init:(SuperfineTrackingManager *_Nonnull)trackingManager;

- (SuperfineTrackingContext *_Nonnull)modify:(void (^_Nonnull)(id<SuperfineTrackingMutableContext> _Nonnull ctx))modify;

@end

@protocol SuperfineTrackingMutableContext <NSObject>

@property (nonatomic) SuperfineTrackingOperationType operationType;
@property (nonatomic, nullable) SuperfineTrackingOperation *operation;
@property (nonatomic, nullable) NSError *error;
@property (nonatomic) BOOL debug;

@end
