#import <Foundation/Foundation.h>

#import "SuperfineTrackingTrackOperation.h"

#import "SuperfineTrackingContext.h"

NS_ASSUME_NONNULL_BEGIN

@protocol SuperfineTrackingIntegration <NSObject>

@optional
// Capture will be called when the user calls either of the following:
// 1. [[PHGPostHog sharedInstance] track:someEvent];
// 2. [[PHGPostHog sharedInstance] track:someEvent properties:someProperties];
// 3. [[PHGPostHog sharedInstance] track:someEvent properties:someProperties options:someOptions];
- (void)track:(SuperfineTrackingTrackOperation *)operation;

// Reset is invoked when the user logs out, and any data saved about the user should be cleared.
- (void)reset;

// Flush is invoked when any queued events should be uploaded.
- (void)flush;

// App Delegate Callbacks

// Callbacks for notifications changes.
// ------------------------------------
- (void)receivedRemoteNotification:(NSDictionary *)userInfo;
- (void)failedToRegisterForRemoteNotificationsWithError:(NSError *)error;
- (void)registeredForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken;
- (void)handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo;

// Callbacks for app state changes
// -------------------------------

- (void)applicationDidFinishLaunching:(NSNotification *)notification;
- (void)applicationDidEnterBackground;
- (void)applicationWillEnterForeground;
- (void)applicationWillTerminate;
- (void)applicationWillResignActive;
- (void)applicationDidBecomeActive;

- (void)continueUserActivity:(NSUserActivity *)activity;
- (void)openURL:(NSURL *)url options:(NSDictionary *)options;

@end

NS_ASSUME_NONNULL_END
