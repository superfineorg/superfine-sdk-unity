#import <AVFoundation/AVFoundation.h>

@interface SuperfineSDKUnityUtility : NSObject
+ (NSDictionary *)dictionaryFromKeys:(const char **)keys values:(const char **)vals length:(int)length;

+ (char*)makeStringReturn:(const NSString*)string;
+ (char*)makeStringCopy:(const char*)string;

+ (NSMutableDictionary*)makeErrorDict:(const NSError*)error;

+ (NSString *)stringFromCString:(const char *)string;
@end

