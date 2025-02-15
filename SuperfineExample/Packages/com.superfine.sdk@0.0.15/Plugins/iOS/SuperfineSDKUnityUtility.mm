#import "SuperfineSDKUnityUtility.h"
#import <Foundation/Foundation.h>

#include <string>

@implementation SuperfineSDKUnityUtility

+ (char*)makeStringReturn:(const NSString*)string
{
    if (string == NULL)
    {
        return [self makeStringCopy:""];
    }
        
    return [self makeStringCopy:[string UTF8String]];
}

+ (char*)makeStringCopy:(const char*)string
{
  if (string == NULL)
    return NULL;

  char* res = (char*)malloc(strlen(string) + 1);
  strcpy(res, string);
  return res;
}

+ (NSMutableDictionary*)makeErrorDict:(const NSError*)error
{
    if (error == NULL)
    {
        return NULL;
    }
    
    NSMutableDictionary* ret = [[NSMutableDictionary alloc] init];
    [ret setObject:[NSNumber numberWithInteger:error.code] forKey:@"code"];
    
    NSString* message = [error localizedDescription];
    if (message != NULL && message.length > 0)
    {
        [ret setObject:message forKey:@"message"];
    }
    
    NSString* domain = [error domain];
    if (domain != NULL && domain.length > 0)
    {
        [ret setObject:domain forKey:@"domain"];
    }
    
    return ret;
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

