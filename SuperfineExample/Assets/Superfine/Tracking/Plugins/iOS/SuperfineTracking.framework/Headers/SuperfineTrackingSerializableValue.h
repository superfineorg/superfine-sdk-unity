#import <Foundation/Foundation.h>

/*
 Acceptable dictionary values are
 NSString (String);
 NSNumber (Int, Float, Bool);
 NSNull
 NSDate => ISO8601 String
 NSURL => absoluteURL String
 NSArray of the above
 NSDictionary of the above
 */
#define JSON_DICT NSDictionary<NSString *, id> *