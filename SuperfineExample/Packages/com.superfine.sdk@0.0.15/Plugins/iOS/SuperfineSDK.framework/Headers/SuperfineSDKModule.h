#import <Foundation/Foundation.h>

#import <SuperfineSDK/SuperfineSDKManager.h>
#import <SuperfineSDK/SuperfineSDKModuleInitializationParameters.h>

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineSDKModule : NSObject

@property (nonatomic, strong, nonnull, readonly) SuperfineSDKManager *manager;

- (instancetype)init:(SuperfineSDKManager*)manager parameters:(SuperfineSDKModuleInitializationParameters*)initializationParameters;

@end

NS_ASSUME_NONNULL_END
