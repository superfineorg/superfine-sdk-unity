#import <Foundation/Foundation.h>
#import "SuperfineTrackingSerializableValue.h"

NS_ASSUME_NONNULL_BEGIN


@interface SuperfineTrackingOperation : NSObject

@end


@interface SuperfineTrackingApplicationLifecycleOperation : SuperfineTrackingOperation

@property (nonatomic, strong) NSString *notificationName;

// ApplicationDidFinishLaunching only
@property (nonatomic, strong, nullable) NSDictionary *launchOptions;

@end


@interface SuperfineTrackingContinueUserActivityOperation : SuperfineTrackingOperation

@property (nonatomic, strong) NSUserActivity *activity;

@end


@interface SuperfineTrackingOpenURLOperation : SuperfineTrackingOperation

@property (nonatomic, strong) NSURL *url;
@property (nonatomic, strong) NSDictionary *options;

@end

NS_ASSUME_NONNULL_END


@interface SuperfineTrackingRemoteNotificationOperation : SuperfineTrackingOperation

// PHGEventTypeHandleActionWithForRemoteNotification
@property (nonatomic, strong, nullable) NSString *actionIdentifier;

// PHGEventTypeHandleActionWithForRemoteNotification
// PHGEventTypeReceivedRemoteNotification
@property (nonatomic, strong, nullable) NSDictionary *userInfo;

// PHGEventTypeFailedToRegisterForRemoteNotifications
@property (nonatomic, strong, nullable) NSError *error;

// PHGEventTypeRegisteredForRemoteNotifications
@property (nonatomic, strong, nullable) NSData *deviceToken;

@end
