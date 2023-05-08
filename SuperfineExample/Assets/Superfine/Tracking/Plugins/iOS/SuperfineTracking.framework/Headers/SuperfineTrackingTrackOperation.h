#import <Foundation/Foundation.h>
#import "SuperfineTrackingOperation.h"

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineTrackingTrackOperation : SuperfineTrackingOperation

@property (nonatomic, readonly) NSString *eId;
@property (nonatomic, readonly) NSString *eventName;
@property (nonatomic, readonly, nullable) JSON_DICT value;
@property (nonatomic, readonly) NSString *id;
@property (nonatomic, readonly) NSNumber* timeInMs;
@property (nonatomic, readonly) NSString *cfgId;
@property (nonatomic, readonly) NSString *appVersion;

- (instancetype)init:(NSString *)eId
                eventName:(NSString *)eventName
                value:(JSON_DICT _Nullable)value
                id:(NSString *)id
                timeInMs:(long)timeInMs
                cfgId:(NSString*)cfgId
                appVersion:(NSString*)appVersion;

@end

NS_ASSUME_NONNULL_END
