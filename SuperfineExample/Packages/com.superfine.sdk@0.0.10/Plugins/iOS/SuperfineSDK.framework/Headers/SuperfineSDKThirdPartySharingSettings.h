#import <Foundation/Foundation.h>

@interface SuperfineSDKThirdPartySharingSettings : NSObject

@property (nonatomic, strong, readonly) NSMutableDictionary* values;
@property (nonatomic, strong, readonly) NSMutableDictionary* flags;

- (instancetype)initWithValues:(NSDictionary*)values;
- (instancetype)initWithFlags:(NSDictionary*)flags;

- (instancetype)initWithValues:(NSDictionary*)values andFlags:(NSDictionary*)flags;

- (void)addValue:(NSString*)partnerName key:(NSString*)key value:(NSString*)value;
- (void)addFlag:(NSString*)partnerName key:(NSString*)key value:(BOOL)value;

@end
