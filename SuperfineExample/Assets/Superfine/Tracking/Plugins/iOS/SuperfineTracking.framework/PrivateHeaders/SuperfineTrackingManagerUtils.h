#import <Foundation/Foundation.h>
#import "SuperfineTrackingSerializableValue.h"

NS_ASSUME_NONNULL_BEGIN

NSString *createUUIDString(void);

long getTimestampMs(void);

NSString *getIPAddress(bool preferIPv4);

// Validation Utils
BOOL serializableDictionaryTypes(NSDictionary *dict);

// Date Utils
NSString *createISO8601FormattedString(NSDate *date);

void trimQueueItems(NSMutableArray *array, NSUInteger size);

// Async Utils
dispatch_queue_t superfine_tracking_dispatch_queue_create_specific(const char *label,
                                                    dispatch_queue_attr_t _Nullable attr);
BOOL superfine_tracking_dispatch_is_on_specific_queue(dispatch_queue_t queue);
void superfine_tracking_dispatch_specific(dispatch_queue_t queue, dispatch_block_t block,
                           BOOL waitForCompletion);
void superfine_tracking_dispatch_specific_async(dispatch_queue_t queue,
                                 dispatch_block_t block);
void superfine_tracking_dispatch_specific_sync(dispatch_queue_t queue, dispatch_block_t block);

// Logging

void SuperfineTrackingSetShowDebugLogs(BOOL showDebugLogs);
void SuperfineTrackingLog(NSString *format, ...);

// JSON Utils

JSON_DICT SuperfineTrackingCoerceDictionary(NSDictionary *_Nullable dict);

NSString *SuperfineTrackingEventNameForScreenTitle(NSString *title);

// Deep copy and check NSCoding conformance
@protocol SuperfineTrackingSerializableDeepCopy <NSObject>
-(id _Nullable) serializableDeepCopy;
@end

@interface NSDictionary(SerializableDeepCopy) <SuperfineTrackingSerializableDeepCopy>
@end

@interface NSArray(SerializableDeepCopy) <SuperfineTrackingSerializableDeepCopy>
@end


NS_ASSUME_NONNULL_END
