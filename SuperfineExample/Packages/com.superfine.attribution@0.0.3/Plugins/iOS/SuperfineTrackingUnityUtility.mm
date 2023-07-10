#import "SuperfineTrackingUnityUtility.h"

#include <string>
#import <Foundation/Foundation.h>

@implementation SuperfineTrackingUnityUtility

+ (char*) makeStringReturn:(NSString*)string
{
    return [self makeStringCopy:[string UTF8String]];
}

+ (char*) makeStringCopy:(const char*)string
{
  if (string == NULL)
    return NULL;

  char* res = (char*)malloc(strlen(string) + 1);
  strcpy(res, string);
  return res;
}

+ (NSString *)stringFromCString:(const char *)string
{
  if (string && string[0] != 0) {
    return [NSString stringWithUTF8String:string];
  }

  return nil;
}

+ (NSDictionary *)dictionaryFromKeys:(const char **)keys
                              values:(const char **)vals
                              length:(int)length
{
  NSMutableDictionary *params = nil;
  if(length > 0 && keys && vals) {
    params = [NSMutableDictionary dictionaryWithCapacity:length];
    for(int i = 0; i < length; i++) {
      if (vals[i] && vals[i] != 0 && keys[i] && keys[i] != 0) {
        params[[NSString stringWithUTF8String:keys[i]]] = [NSString stringWithUTF8String:vals[i]];
      }
    }
  }

  return params;
}

@end

