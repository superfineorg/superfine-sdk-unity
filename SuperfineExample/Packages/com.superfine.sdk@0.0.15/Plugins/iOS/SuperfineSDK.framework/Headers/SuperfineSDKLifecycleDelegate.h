#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol SuperfineSDKLifecycleDelegate<NSObject>

- (void)onStart;
- (void)onStop;

- (void)onResume;
- (void)onPause;

@end

NS_ASSUME_NONNULL_END
