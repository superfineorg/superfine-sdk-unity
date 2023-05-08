#import <Foundation/Foundation.h>
#import "SuperfineTrackingStorage.h"


@interface SuperfineTrackingFileStorage : NSObject <SuperfineTrackingStorage>

@property (nonatomic, strong, nullable) id<SuperfineTrackingCrypto> crypto;

- (instancetype _Nonnull)initWithFolder:(NSURL *_Nonnull)folderURL crypto:(id<SuperfineTrackingCrypto> _Nullable)crypto;

- (NSURL *_Nonnull)urlForKey:(NSString *_Nonnull)key;
- (void)resetAll;

+ (NSURL *_Nullable)applicationSupportDirectoryURL;
+ (NSURL *_Nullable)cachesDirectoryURL;

@end
