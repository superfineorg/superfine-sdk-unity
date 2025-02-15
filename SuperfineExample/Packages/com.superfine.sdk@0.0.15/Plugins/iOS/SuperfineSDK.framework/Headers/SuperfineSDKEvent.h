#import <Foundation/Foundation.h>

#import <SuperfineSDK/SuperfineSDKTypes.h>

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineSDKEvent : NSObject

@property (nonatomic, copy, nonnull, readonly) NSString* eventName;
@property (nonatomic, copy, nullable, readonly) JSON_DICT value;

@property (nonatomic, assign) SuperfineSDKOperationFlag flag;

@property (nonatomic, assign, readonly) double revenue;
@property (nonatomic, copy, nullable, readonly) NSString* currency;

-(id)init:(nonnull NSString*)eventName;
-(id)init:(nonnull NSString*)eventName value:(JSON_DICT _Nullable)value;
-(id)init:(nonnull NSString*)eventName value:(JSON_DICT _Nullable)value flag:(SuperfineSDKOperationFlag)flag;

-(void)setIntValue:(int)value;
-(void)setStringValue:(NSString* _Nullable)value;
-(void)setMapValue:(NSDictionary<NSString*, NSString*>* _Nullable)value;
-(void)setJsonValue:(JSON_DICT _Nullable)value;

-(BOOL)hasRevenue;
-(void)setRevenue:(double)revenue currency:(nonnull NSString*)currency;

@end

NS_ASSUME_NONNULL_END
