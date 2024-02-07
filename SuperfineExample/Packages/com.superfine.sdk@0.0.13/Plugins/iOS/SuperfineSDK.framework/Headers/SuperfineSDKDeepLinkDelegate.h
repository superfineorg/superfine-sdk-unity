#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@protocol SuperfineSDKDeepLinkDelegate<NSObject>

- (void)onSetDeepLink:(NSURL*)url;

@end

NS_ASSUME_NONNULL_END
