#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineSDKModuleInitializationParameters : NSObject

- (instancetype)init;
- (instancetype)initWithString:(nonnull NSString*)dataString;
- (instancetype)initWithDictionary:(nonnull NSDictionary*)data;

- (NSMutableDictionary*)getData;

- (BOOL)hasKey:(nonnull NSString *)key;

- (NSString*)getString:(NSString*)key;
- (NSString*)getString:(NSString*)key fallback:(NSString*)fallback;

- (int)getInt:(NSString*)key fallback:(int)fallback;
- (long)getLong:(nonnull NSString *)key fallback:(long)fallback;
- (long long)getLongLong:(nonnull NSString *)key fallback:(long long)fallback;

- (int)getDouble:(NSString*)key fallback:(double)fallback;

- (BOOL)getBool:(NSString*)key fallback:(BOOL)fallback;

- (NSArray*)getArray:(nonnull NSString *)key;
- (NSDictionary*)getDictionary:(nonnull NSString *)key;

@end

NS_ASSUME_NONNULL_END
