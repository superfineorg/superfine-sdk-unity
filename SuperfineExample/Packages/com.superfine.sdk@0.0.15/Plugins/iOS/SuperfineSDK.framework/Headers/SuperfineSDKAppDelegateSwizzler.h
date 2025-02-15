#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import <SuperfineSDK/SuperfineSDKTypes.h>

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineSDKAppDelegateSwizzler : NSProxy

+ (nullable NSString*)registerInterceptor:(id<UIApplicationDelegate>)interceptor;
+ (void)unregisterInterceptor:(NSString*)interceptorId;

+ (SuperfineSDKAppSwizzleFlag)getFlag;

+ (void)setupProxy:(SuperfineSDKAppSwizzleFlag)flag;

- (instancetype)init NS_UNAVAILABLE;

NS_ASSUME_NONNULL_END

@end
