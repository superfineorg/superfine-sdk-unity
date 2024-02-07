#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol SuperfineSDKSendEventDelegate<NSObject>

- (void)onSendEvent:(NSString*)eventName data:(NSString* _Nullable)eventData;

@end

NS_ASSUME_NONNULL_END
