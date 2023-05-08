#import <AVFoundation/AVFoundation.h>

@interface SuperfineTrackingUnityUtility : NSObject
+ (NSDictionary *)dictionaryFromKeys:(const char **)keys values:(const char **)vals length:(int)length;

+ (char*) makeStringReturn:(NSString*)string;
+ (char*) makeStringCopy:(const char*)string;
+ (NSString *)stringFromCString:(const char *)string;
@end

