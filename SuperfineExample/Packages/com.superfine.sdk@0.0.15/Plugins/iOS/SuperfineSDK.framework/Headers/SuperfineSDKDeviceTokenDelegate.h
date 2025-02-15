#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol SuperfineSDKDeviceTokenDelegate<NSObject>

- (void)onSetDeviceToken:(NSString*)token;

@end

NS_ASSUME_NONNULL_END
