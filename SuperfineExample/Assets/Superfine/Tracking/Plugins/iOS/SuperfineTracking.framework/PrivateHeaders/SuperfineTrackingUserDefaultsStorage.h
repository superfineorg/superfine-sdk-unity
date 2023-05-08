#import <Foundation/Foundation.h>
#import "SuperfineTrackingStorage.h"


@interface SuperfineTrackingUserDefaultsStorage : NSObject <SuperfineTrackingStorage>

@property (nonatomic, strong, nullable) id<SuperfineTrackingCrypto> crypto;
@property (nonnull, nonatomic, readonly) NSUserDefaults *defaults;
@property (nullable, nonatomic, readonly) NSString *namespacePrefix;

- (instancetype _Nonnull)initWithDefaults:(NSUserDefaults *_Nonnull)defaults namespacePrefix:(NSString *_Nullable)namespacePrefix crypto:(id<SuperfineTrackingCrypto> _Nullable)crypto;

@end
