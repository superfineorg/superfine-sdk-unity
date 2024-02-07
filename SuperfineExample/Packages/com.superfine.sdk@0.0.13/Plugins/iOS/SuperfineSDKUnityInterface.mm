#import <SuperfineSDK/SuperfineSDK.h>

#import <Foundation/NSJSONSerialization.h>

#include "SuperfineSDKUnityInterface.h"
#include "SuperfineSDKUnityUtility.h"

@interface SuperfineSDKLifecycleCallbackWrapper : NSObject<SuperfineSDKLifecycleDelegate>

@property (nonatomic, readonly) int requestCode;
@property (nonatomic, nullable) void(*startCallback)(int);
@property (nonatomic, nullable) void(*stopCallback)(int);

@property (nonatomic, nullable) void(*pauseCallback)(int);
@property (nonatomic, nullable) void(*resumeCallback)(int);

@end

@implementation SuperfineSDKLifecycleCallbackWrapper

- (instancetype)init:(int)requestCode
{
    if (self = [super init])
    {
        _requestCode = requestCode;
        
        _startCallback = NULL;
        _stopCallback = NULL;
        
        _pauseCallback = NULL;
        _resumeCallback = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return
        self.startCallback != NULL ||
        self.stopCallback != NULL ||
        self.pauseCallback != NULL ||
        self.resumeCallback != NULL;
}

- (void)onStart
{
    if (self.startCallback != NULL)
    {
        self.startCallback(self.requestCode);
    }
}

- (void)onStop
{
    if (self.stopCallback != NULL)
    {
        self.stopCallback(self.requestCode);
    }
}

- (void)onPause
{
    if (self.pauseCallback != NULL)
    {
        self.pauseCallback(self.requestCode);
    }
}


- (void)onResume
{
    if (self.resumeCallback != NULL)
    {
        self.resumeCallback(self.requestCode);
    }
}

@end

@interface SuperfineSDKDeepLinkCallbackWrapper : NSObject<SuperfineSDKDeepLinkDelegate>

@property (nonatomic, readonly) int requestCode;
@property (nonatomic, nullable) void(*callback)(const char*, int);

@end

@implementation SuperfineSDKDeepLinkCallbackWrapper

- (instancetype)init:(int)requestCode
{
    if (self = [super init])
    {
        _requestCode = requestCode;
        _callback = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callback != NULL;
}

- (void)onSetDeepLink:(nonnull NSURL*)url
{
    if (self.callback != NULL)
    {
        self.callback([SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]],
                      self.requestCode);
    }
}

@end

@interface SuperfineSDKDeviceTokenCallbackWrapper : NSObject<SuperfineSDKDeviceTokenDelegate>

@property (nonatomic, readonly) int requestCode;
@property (nonatomic, nullable) void(*callback)(const char*, int);

@end

@implementation SuperfineSDKDeviceTokenCallbackWrapper

- (instancetype)init:(int)requestCode
{
    if (self = [super init])
    {
        _requestCode = requestCode;
        _callback = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callback != NULL;
}

- (void)onSetDeviceToken:(NSString *)token
{
    if (self.callback != NULL)
    {
        self.callback([SuperfineSDKUnityUtility makeStringReturn:token],
                      self.requestCode);
    }
}

@end

@interface SuperfineSDKSendEventCallbackWrapper : NSObject<SuperfineSDKSendEventDelegate>

@property (nonatomic, readonly) int requestCode;
@property (nonatomic, nullable) void(*callback)(const char*, const char*, int);

@end

@implementation SuperfineSDKSendEventCallbackWrapper

- (instancetype)init:(int)requestCode
{
    if (self = [super init])
    {
        _requestCode = requestCode;
        _callback = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callback != NULL;
}

- (void)onSendEvent:(nonnull NSString *)eventName data:(NSString * _Nullable)eventData
{
    if (self.callback != NULL)
    {
        self.callback([SuperfineSDKUnityUtility makeStringReturn:eventName],
                      [SuperfineSDKUnityUtility makeStringReturn:eventData],
                      self.requestCode);
    }
}

@end

@interface SuperfineSDKUnityInterface()

@property (nonatomic, assign) bool hasInited;

@end

@implementation SuperfineSDKUnityInterface

NSMutableDictionary<NSNumber*, SuperfineSDKLifecycleCallbackWrapper*>* lifecycleWrappers = nil;
NSMutableDictionary<NSNumber*, SuperfineSDKDeepLinkCallbackWrapper*>* deepLinkWrappers = nil;
NSMutableDictionary<NSNumber*, SuperfineSDKDeviceTokenCallbackWrapper*>* deviceTokenWrappers = nil;
NSMutableDictionary<NSNumber*, SuperfineSDKSendEventCallbackWrapper*>* sendEventWrappers = nil;

#pragma mark Object Initialization

+ (SuperfineSDKUnityInterface *)sharedInstance
{
  static dispatch_once_t pred;
  static SuperfineSDKUnityInterface *shared = nil;

  dispatch_once(&pred, ^{
      shared = [[SuperfineSDKUnityInterface alloc] init];
  });

  return shared;
}

- (instancetype)init
{
    if (self = [super init])
    {
        lifecycleWrappers = [[NSMutableDictionary alloc] init];
        deepLinkWrappers = [[NSMutableDictionary alloc] init];
        deviceTokenWrappers = [[NSMutableDictionary alloc] init];
        sendEventWrappers = [[NSMutableDictionary alloc] init];
        
        self.hasInited = false;
    }
    
    return self;
}

- (void)init:(const char *)params
{
    if (self.hasInited)
    {
        NSLog(@"Superfine SDK is already initialized");
        return;
    }
    
    SuperfineSDKConfiguration* config = [SuperfineSDKManager createConfigurationFromJson:[SuperfineSDKUnityUtility stringFromCString:params]];
    
    if (config == NULL)
    {
        NSLog(@"Unable to create config data");
        return;
    }
    
    self.hasInited = true;

    [SuperfineSDKManager setupWithConfiguration:config];
}

- (void)notifyInit
{
    self.hasInited = true;
}

- (bool)assertInited
{
    if (!self.hasInited)
    {
        NSLog(@"Superfine SDK isn't initialized");
        return false;
    }
    
    return true;
}

- (void)start
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] start];
}

- (void)stop
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] stop];
}

- (void)shutdown
{
    if (![self assertInited]) return;
    
    if ([SuperfineSDKManager sharedInstance] != NULL)
    {
        [self stop];
        [SuperfineSDKManager shutdown];
    }
    
    lifecycleWrappers = nil;
    deepLinkWrappers = nil;
    deviceTokenWrappers = nil;
    sendEventWrappers = nil;
    
    [self setHasInited:false];
}

- (void)setOffline:(BOOL)value;
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setOffline:value];
}

- (void)setStartCallback:(void(*)(int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init:requestCode];
        [wrapper setStartCallback:callback];
        
        [lifecycleWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addLifecycleDelegate:wrapper];
    }
    else
    {
        [wrapper setStartCallback:callback];
    }
}

- (void)removeStartCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setStartCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeLifecycleDelegate:wrapper];
            [lifecycleWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)setStopCallback:(void(*)(int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init:requestCode];
        [wrapper setStopCallback:callback];
        
        [lifecycleWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addLifecycleDelegate:wrapper];
    }
    else
    {
        [wrapper setStopCallback:callback];
    }
}

- (void)removeStopCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setStopCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeLifecycleDelegate:wrapper];
            [lifecycleWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)setPauseCallback:(void(*)(int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init:requestCode];
        [wrapper setPauseCallback:callback];
        
        [lifecycleWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addLifecycleDelegate:wrapper];
    }
    else
    {
        [wrapper setPauseCallback:callback];
    }
}

- (void)removePauseCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setPauseCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeLifecycleDelegate:wrapper];
            [lifecycleWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)setResumeCallback:(void(*)(int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init:requestCode];
        [wrapper setResumeCallback:callback];
        
        [lifecycleWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addLifecycleDelegate:wrapper];
    }
    else
    {
        [wrapper setResumeCallback:callback];
    }
}

- (void)removeResumeCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKLifecycleCallbackWrapper* wrapper = [lifecycleWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setResumeCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeLifecycleDelegate:wrapper];
            [lifecycleWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)setDeepLinkCallback:(void(*)(const char*, int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKDeepLinkCallbackWrapper* wrapper = [deepLinkWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKDeepLinkCallbackWrapper alloc] init:requestCode];
        [wrapper setCallback:callback];
        
        [deepLinkWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addDeepLinkDelegate:wrapper];
    }
    else
    {
        [wrapper setCallback:callback];
    }
}

- (void)removeDeepLinkCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKDeepLinkCallbackWrapper* wrapper = [deepLinkWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeDeepLinkDelegate:wrapper];
            [deepLinkWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)setDeviceTokenCallback:(void(*)(const char*, int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKDeviceTokenCallbackWrapper* wrapper = [deviceTokenWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKDeviceTokenCallbackWrapper alloc] init:requestCode];
        [wrapper setCallback:callback];
        
        [deviceTokenWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addDeviceTokenDelegate:wrapper];
    }
    else
    {
        [wrapper setCallback:callback];
    }
}

- (void)removeDeviceTokenCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKDeviceTokenCallbackWrapper* wrapper = [deviceTokenWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeDeviceTokenDelegate:wrapper];
            [deviceTokenWrappers removeObjectForKey:@(requestCode)];
        }
    }
}


- (void)setSendEventCallback:(void(*)(const char*, const char*, int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;
    
    if (callback == NULL) return;
    
    SuperfineSDKSendEventCallbackWrapper* wrapper = [sendEventWrappers objectForKey:@(requestCode)];
    
    if (wrapper == NULL)
    {
        wrapper = [[SuperfineSDKSendEventCallbackWrapper alloc] init:requestCode];
        [wrapper setCallback:callback];
        
        [sendEventWrappers setObject:wrapper forKey:@(requestCode)];
        
        [[SuperfineSDKManager sharedInstance] addSendEventDelegate:wrapper];
    }
    else
    {
        [wrapper setCallback:callback];
    }
}

- (void)removeSendEventCallback:(int)requestCode
{
    if (![self assertInited]) return;
    
    SuperfineSDKSendEventCallbackWrapper* wrapper = [sendEventWrappers objectForKey:@(requestCode)];
    if (wrapper != NULL)
    {
        [wrapper setCallback:NULL];
        
        if (![wrapper hasCallback])
        {
            [[SuperfineSDKManager sharedInstance] removeSendEventDelegate:wrapper];
            [sendEventWrappers removeObjectForKey:@(requestCode)];
        }
    }
}

- (void)gdprForgetMe
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] gdprForgetMe];
}

- (void)disableThirdPartySharing
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] disableThirdPartySharing];
}

- (void)enableThirdPartySharing
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] enableThirdPartySharing];
}

- (void)logThirdPartySharing:(const char*)params
{
    if (![self assertInited]) return;
    
    NSData* jsonData = [[SuperfineSDKUnityUtility stringFromCString: params] dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError* e = nil;
    NSDictionary* data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
    
    if (e != nil)
    {
        NSLog(@"Error while parsing third parting sharing settings: %@", [e localizedDescription]);
        return;
    }
    
    NSDictionary* valueParams = [data objectForKey:@"values"];
    NSDictionary* flagParams = [data objectForKey:@"flags"];
    
    SuperfineSDKThirdPartySharingSettings* settings = [[SuperfineSDKThirdPartySharingSettings alloc] initWithValues:valueParams andFlags:flagParams];
    
    [[SuperfineSDKManager sharedInstance] logThirdPartySharing_settings:settings];
}

- (char*)getVersion
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[SuperfineSDKManager version]];
}

- (char*)getAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppId]];
}

- (char*)getUserId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getUserId]];
}

- (char*)getSessionId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getSessionId]];
}

- (char*)getHost
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    NSURL* url = [[SuperfineSDKManager sharedInstance] getHost];
    if (url == NULL) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    return [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
}

- (SuperfineSDKStoreType)getStoreType
{
    if (![self assertInited]) return SuperfineSDKStoreType_AppStore;
    return [[SuperfineSDKManager sharedInstance] getStoreType];
}

- (char*)getFacebookAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getFacebookAppId]];
}

- (char*)getInstagramAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getInstagramAppId]];
}

- (char*)getAppleAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleAppId]];
}

- (char*)getAppleSignInClientId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleSignInClientId]];
}

- (char*)getAppleDeveloperTeamId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleDeveloperTeamId]];
}

- (char*)getGooglePlayGameServicesProjectId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getGooglePlayGameServicesProjectId]];
}

- (char*)getGooglePlayDeveloperAccountId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getGooglePlayDeveloperAccountId]];
}

- (char*)getLinkedInAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getLinkedInAppId]];
}

- (char*)getQQAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getQQAppId]];
}

- (char*)getWeChatAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getWeChatAppId]];
}

- (char*)getTikTokAppId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getTikTokAppId]];
}

- (char*)getDeepLinkUrl
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    NSURL* url = [[SuperfineSDKManager sharedInstance] getDeepLinkUrl];
    if (url == NULL) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    return [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
}

- (char*)getDeviceToken
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getDeviceTokenString]];
}

- (void)setConfigId:(const char*)configId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setConfigId:[SuperfineSDKUnityUtility stringFromCString:configId]];
}

- (void)setCustomUserId:(const char*)customUserId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setCustomUserId:[SuperfineSDKUnityUtility stringFromCString:customUserId]];
}

- (void)fetchRemoteConfig:(void(*)(const char*))callback
{
    if (![self assertInited])
    {
        if (callback != NULL)
        {
            callback([SuperfineSDKUnityUtility makeStringReturn:@""]);
        }
        
        return;
    }
    
    id handler = ^(const NSString* result)
    {
        if (callback != NULL)
        {
            if (result != NULL)
            {
                callback([SuperfineSDKUnityUtility makeStringReturn:result]);
            }
            else
            {
                callback([SuperfineSDKUnityUtility makeStringReturn:@""]);
            }
        }
    };
    
    [[SuperfineSDKManager sharedInstance] fetchRemoteConfig:handler];
}

- (void)log:(const char*)eventName flag:(int)flag
{
    if (![self assertInited]) return;
    
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] flag:(SuperfineSDKOperationFlag)flag];
}

- (void)log:(const char*)eventName intValue:(int)value flag:(int)flag
{
    if (![self assertInited]) return;

    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] intValue:value flag:(SuperfineSDKOperationFlag)flag];
}

- (void)log:(const char*)eventName stringValue:(const char*)value flag:(int)flag
{
    if (![self assertInited]) return;

    if (value != NULL)
    {
        [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] stringValue:[SuperfineSDKUnityUtility stringFromCString:value] flag:(SuperfineSDKOperationFlag)flag];
    }
    else
    {
        [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] flag:(SuperfineSDKOperationFlag)flag];
    }
}

- (void)log:(const char*)eventName jsonValue:(const char*)value flag:(int)flag
{
    if (![self assertInited]) return;
    
    NSMutableDictionary* data = NULL;
    
    if (value != NULL)
    {
        NSData* jsonData = [[SuperfineSDKUnityUtility stringFromCString: value] dataUsingEncoding:NSUTF8StringEncoding];
        
        NSError* e = nil;
        data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
        
        if (e != nil)
        {
            NSLog(@"Error while parsing track parameters: %@", [e localizedDescription]);
            data = NULL;
        }
    }
        
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] jsonValue:data flag:(SuperfineSDKOperationFlag)flag];
}

- (void)log:(const char*)eventName mapValue:(const char*)value flag:(int)flag
{
    if (![self assertInited]) return;
    
    NSMutableDictionary<NSString *, NSString *> *data = NULL;
    
    if (value != NULL)
    {
        NSString *str = [SuperfineSDKUnityUtility stringFromCString:value];
        
        NSArray *entries = [str componentsSeparatedByString:@","];
        if (entries != NULL)
        {
            data = [[NSMutableDictionary alloc] init];
            
            int numEntries = (int)[entries count];
            for (int i = 0; i < numEntries; ++i)
            {
                NSString* entry = entries[i];
                NSRange range = [entry rangeOfString:@":"];
                
                NSInteger location = range.location;
                if (location != NSNotFound)
                {
                    NSString* key = [[entry substringToIndex:location] stringByRemovingPercentEncoding];
                    NSString* value = [[entry substringFromIndex:location + 1] stringByRemovingPercentEncoding];
                    
                    if (key != NULL && key.length > 0 && value != NULL && value.length > 0)
                    {
                        [data setValue:value forKey:key];
                    }
                }
            }
        }
    }
    
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] mapValue:data flag:(SuperfineSDKOperationFlag)flag];
}

- (void)logBootStart
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logBootStart];
}

- (void)logBootEnd
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logBootEnd];
}

- (void)logLevelStart_id:(int)levelId name:(const char*)name
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLevelStart_id:levelId name:[SuperfineSDKUnityUtility stringFromCString:name]];
}

- (void)logLevelEnd_id:(int)levelId name:(const char*)name isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLevelEnd_id:levelId name:[SuperfineSDKUnityUtility stringFromCString:name] isSuccess:isSuccess];
}

- (void)logAdLoad_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAdLoad_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdClose_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAdClose_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdClick_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAdClick_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdImpression_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAdImpression_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logIAPInitialization:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPInitialization:isSuccess];
}

- (void)logIAPRestorePurchase
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPRestorePurchase];
}

- (void)logIAPResult_pack:(const char*)pack price:(double)price amount:(int)amount currency:(const char*)currency isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPResult_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] isSuccess:isSuccess];
}

- (void)logIAPReceipt_Apple_receipt:(const char*)receipt
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Apple_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Google_data:(const char*)data signature:(const char*)signature
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Google_data:[SuperfineSDKUnityUtility stringFromCString:data] signature:[SuperfineSDKUnityUtility stringFromCString:signature]];
}

- (void)logIAPReceipt_Amazon_userId:(const char*)userId receiptId:(const char*)receiptId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Amazon_userId:[SuperfineSDKUnityUtility stringFromCString:userId] receiptId:[SuperfineSDKUnityUtility stringFromCString:receiptId]];
}

- (void)logIAPReceipt_Roku_transactionId:(const char*)transactionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Roku_transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId]];
}

- (void)logIAPReceipt_Windows_receipt:(const char*)receipt
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Windows_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Facebook_receipt:(const char*)receipt
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Facebook_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Unity_receipt:(const char*)receipt
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Unity_receipt: [SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_AppStoreServer_transactionId:(const char*)transactionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_AppStoreServer_transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId]];
}

- (void)logIAPReceipt_GooglePlayProduct_productId:(const char*)productId token:(const char*)token
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlayProduct_productId: [SuperfineSDKUnityUtility stringFromCString:productId] token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logIAPReceipt_GooglePlaySubscription_subscriptionId:(const char*)subscriptionId token:(const char*)token
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlaySubscription_subscriptionId: [SuperfineSDKUnityUtility stringFromCString:subscriptionId] token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logIAPReceipt_GooglePlaySubscriptionv2_token:(const char*)token
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlaySubscriptionv2_token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logUpdateApp:(const char*)newVersion
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logUpdateApp:[SuperfineSDKUnityUtility stringFromCString:newVersion]];
}

- (void)logRateApp
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logRateApp];
}

- (void)logLocation_latitude:(double)latitude longitude:(double)longitude
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLocation_latitude:latitude longtitude:longitude];
}

- (void)logAuthorizationTrackingStatus:(int)status
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAuthorizationTrackingStatus:(SuperfineSDKAuthorizationTrackingStatus)status];
}

- (void)logFacebookLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logFacebookUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookUnlink];
}

- (void)logInstagramLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logInstagramLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logInstagramUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logInstagramUnlink];
}

- (void)logAppleLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAppleLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logAppleUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAppleUnlink];
}

- (void)logAppleGameCenterLink:(const char*)gamePlayerId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterLink:[SuperfineSDKUnityUtility stringFromCString:gamePlayerId]];
}

- (void)logAppleGameCenterTeamLink:(const char*)teamPlayerId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterTeamLink:[SuperfineSDKUnityUtility stringFromCString:teamPlayerId]];
}

- (void)logAppleGameCenterUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterUnlink];
}

- (void)logGoogleLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGoogleLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logGoogleUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGoogleUnlink];
}

- (void)logGooglePlayGameServicesLink:(const char*)gamePlayerId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesLink:[SuperfineSDKUnityUtility stringFromCString:gamePlayerId]];
}

- (void)logGooglePlayGameServicesDeveloperLink:(const char*)developerPlayerKey
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesDeveloperLink:[SuperfineSDKUnityUtility stringFromCString:developerPlayerKey]];
}

- (void)logGooglePlayGameServicesUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesUnlink];
}

- (void)logLinkedInLink:(const char*)personId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLinkedInLink:[SuperfineSDKUnityUtility stringFromCString:personId]];
}

- (void)logLinkedInUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLinkedInUnlink];
}

- (void)logMeetupLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logMeetupLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logMeetupUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logMeetupUnlink];
}

- (void)logGitHubLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGitHubLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logGitHubUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logGitHubUnlink];
}

- (void)logDiscordLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logDiscordLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logDiscordUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logDiscordUnlink];
}

- (void)logTwitterLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logTwitterLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logTwitterUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logTwitterUnlink];
}

- (void)logSpotifyLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logSpotifyLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logSpotifyUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logSpotifyUnlink];
}

- (void)logMicrosoftLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logMicrosoftLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logMicrosoftUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logMicrosoftUnlink];
}

- (void)logLINELink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLINELink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logLINEUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logLINEUnlink];
}

- (void)logVKLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logVKLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logVKUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logVKUnlink];
}

- (void)logQQLink:(const char*)openId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logQQLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logQQUnionLink:(const char*)unionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logQQUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logQQUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logQQUnlink];
}

- (void)logWeChatLink:(const char*)openId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logWeChatUnionLink:(const char*)unionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logWeChatUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatUnlink];
}

- (void)logTikTokLink:(const char*)openId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logTikTokUnionLink:(const char*)unionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logTikTokUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokUnlink];
}

- (void)logWeiboLink:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWeiboLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logWeiboUnlink
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWeiboUnlink];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type scopeId:(const char*)scopeId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type] scopeId:[SuperfineSDKUnityUtility stringFromCString:scopeId]];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type scopeId:(const char*)scopeId scopeType:(const char*)scopeType
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type] scopeId:[SuperfineSDKUnityUtility stringFromCString:scopeId] scopeType:[SuperfineSDKUnityUtility stringFromCString:scopeType]];
}

- (void)logAccountUnlink_type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountUnlink_type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)addUserPhoneNumber_countryCode:(int)countryCode number:(const char*)number
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] addUserPhoneNumber_countryCode:countryCode number:[SuperfineSDKUnityUtility stringFromCString:number]];
}

- (void)removeUserPhoneNumber_countryCode:(int)countryCode number:(const char*)number
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] removeUserPhoneNumber_countryCode:countryCode number:[SuperfineSDKUnityUtility stringFromCString:number]];
}

- (void)addUserEmail:(const char*)email
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] addUserEmail:[SuperfineSDKUnityUtility stringFromCString:email]];
}

- (void)removeUserEmail:(const char*)email
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] removeUserEmail:[SuperfineSDKUnityUtility stringFromCString:email]];
}

- (void)setUserName_firstName:(const char*)firstName lastName:(const char*)lastName
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserName_firstName:[SuperfineSDKUnityUtility stringFromCString:firstName] lastName:[SuperfineSDKUnityUtility stringFromCString:lastName]];
}

- (void)setUserCity:(const char*)city
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserCity:[SuperfineSDKUnityUtility stringFromCString:city]];
}

- (void)setUserState:(const char*)state
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserState:[SuperfineSDKUnityUtility stringFromCString:state]];
}

- (void)setUserCountry:(const char*)country
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserCountry:[SuperfineSDKUnityUtility stringFromCString:country]];
}

- (void)setUserZipCode:(const char*)zipCode
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserZipCode:[SuperfineSDKUnityUtility stringFromCString:zipCode]];
}

- (void)setUserDateOfBirth_day:(int)day month:(int)month year:(int)year
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserDateOfBirth_day:day month:month year:year];
}

- (void)setUserDateOfBirth_day:(int)day month:(int)month
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserDateOfBirth_day:day month:month];
}

- (void)setUserYearOfBirth:(int)year
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserYearOfBirth:year];
}

- (void)setUserGender:(int)gender
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setUserGender:(SuperfineSDKUserGender)gender];
}
 
- (void)logWalletLink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWalletLink_wallet:[SuperfineSDKUnityUtility stringFromCString:wallet] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logWalletUnlink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logWalletUnlink_wallet:[SuperfineSDKUnityUtility stringFromCString:wallet] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logCryptoPayment_pack:(const char*)pack price:(double)price amount:(int)amount currency:(const char*)currency chain:(const char*)chain
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logCryptoPayment_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] chain:[SuperfineSDKUnityUtility stringFromCString:chain]];
}

- (void)logAdRevenue_source:(const char*)source revenue:(double)revenue currency:(const char*)currency network:(const char*)network networkData:(const char*)networkData
{
    if (![self assertInited]) return;
    
    NSString* networkStr = NULL;
    if (network != NULL)
    {
        networkStr = [SuperfineSDKUnityUtility stringFromCString:network];
    }
    
    JSON_DICT data = NULL;
    
    if (networkData != NULL)
    {
        NSData* jsonData = [[SuperfineSDKUnityUtility stringFromCString: networkData] dataUsingEncoding:NSUTF8StringEncoding];
        
        NSError* e = nil;
        data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
        
        if (e != nil)
        {
            NSLog(@"Error while parsing track parameters: %@", [e localizedDescription]);
            data = NULL;
        }
    }
    
    [[SuperfineSDKManager sharedInstance] logAdRevenue_source:[SuperfineSDKUnityUtility stringFromCString:source] revenue:revenue currency:[SuperfineSDKUnityUtility stringFromCString:currency] network:networkStr networkData:data];
}

- (void)openURL:(const char*)url
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] openURL:[NSURL URLWithString:[SuperfineSDKUnityUtility stringFromCString:url]]];
}

- (void)setDeviceToken:(const char*)token
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] setDeviceTokenString:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)requestTrackingAuthorization:(void(*)(int))callback
{
    id handler = ^(NSUInteger result)
    {
        callback((int)result);
    };
    
    [[SuperfineSDKManager sharedInstance] requestTrackingAuthorization:handler];
}

- (int)getTrackingAuthorizationStatus
{
    return (int)[[SuperfineSDKManager sharedInstance] getTrackingAuthorizationStatus];
}

- (void)updatePostbackConversionValue:(int)conversionValue
{
    [[SuperfineSDKManager sharedInstance] updatePostbackConversionValue:conversionValue];
}

- (void)updatePostbackConversionValue:(int)conversionValue coarseValue:(const char*) coarseValue
{
    [[SuperfineSDKManager sharedInstance] updatePostbackConversionValue:conversionValue coarseValue:[SuperfineSDKUnityUtility stringFromCString:coarseValue]];
}

- (void)updatePostbackConversionValue:(int)conversionValue coarseValue:(const char*) coarseValue lockWindow:(bool)lockWindow
{
    [[SuperfineSDKManager sharedInstance] updatePostbackConversionValue:conversionValue coarseValue:[SuperfineSDKUnityUtility stringFromCString:coarseValue] lockWindow:lockWindow];
}

@end

#pragma mark - Actual Unity C# interface (extern C)

extern "C"
{
    void SuperfineSDKInit(const char* params)
    {
        [[SuperfineSDKUnityInterface sharedInstance] init:params];
    }

    void SuperfineSDKNotifyInit()
    {
        [[SuperfineSDKUnityInterface sharedInstance] notifyInit];
    }

    void SuperfineSDKStart()
    {
        [[SuperfineSDKUnityInterface sharedInstance] start];
    }

    void SuperfineSDKStop()
    {
        [[SuperfineSDKUnityInterface sharedInstance] stop];
    }

    void SuperfineSDKShutdown()
    {
        [[SuperfineSDKUnityInterface sharedInstance] shutdown];
    }

    void SuperfineSDKSetOffline(bool value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setOffline:value];
    }

    void SuperfineSDKSetStartCallback(void (*callback)(int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setStartCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveStartCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeStartCallback:requestCode];
    }

    void SuperfineSDKSetStopCallback(void (*callback)(int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setStopCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveStopCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeStopCallback:requestCode];
    }

    void SuperfineSDKSetPauseCallback(void (*callback)(int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setPauseCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemovePauseCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removePauseCallback:requestCode];
    }

    void SuperfineSDKSetResumeCallback(void (*callback)(int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setResumeCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveResumeCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeResumeCallback:requestCode];
    }

    void SuperfineSDKSetDeepLinkCallback(void (*callback)(const char*, int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setDeepLinkCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveDeepLinkCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeDeepLinkCallback:requestCode];
    }

    void SuperfineSDKSetDeviceTokenCallback(void (*callback)(const char*, int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setDeviceTokenCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveDeviceTokenCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeDeviceTokenCallback:requestCode];
    }

    void SuperfineSDKSetSendEventCallback(void (*callback)(const char*, const char*, int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setSendEventCallback:callback requestCode:requestCode];
    }

    void SuperfineSDKRemoveSendEventCallback(int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeSendEventCallback:requestCode];
    }

    void SuperfineSDKGdprForgetMe()
    {
        [[SuperfineSDKUnityInterface sharedInstance] gdprForgetMe];
    }

    void SuperfineSDKDisableThirdPartySharing()
    {
        [[SuperfineSDKUnityInterface sharedInstance] disableThirdPartySharing];
    }

    void SuperfineSDKEnableThirdPartySharing()
    {
        [[SuperfineSDKUnityInterface sharedInstance] enableThirdPartySharing];
    }

    void SuperfineSDKLogThirdPartySharing(const char* params)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logThirdPartySharing:params];
    }
    
    char* SuperfineSDKGetVersion()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getVersion];
    }

    void SuperfineSDKSetConfigId(const char* configId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setConfigId:configId];
    }

    void SuperfineSDKSetCustomUserId(const char* customUserId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setCustomUserId:customUserId];
    }

    char* SuperfineSDKGetAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppId];
    }
    
    char* SuperfineSDKGetUserId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getUserId];
    }

    char* SuperfineSDKGetSessionId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getSessionId];
    }

    char* SuperfineSDKGetHost()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getHost];
    }

    int SuperfineSDKGetStoreType()
    {
        return (int)[[SuperfineSDKUnityInterface sharedInstance] getStoreType];
    }

    char* SuperfineGetFacebookAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getFacebookAppId];
    }

    char* SuperfineGetInstagramAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getInstagramAppId];
    }

    char* SuperfineSDKGetAppleAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleAppId];
    }

    char* SuperfineSDKGetAppleSignInClientId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleSignInClientId];
    }

    char* SuperfineSDKGetAppleDeveloperTeamId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleDeveloperTeamId];
    }

    char* SuperfineSDKGetGooglePlayGameServicesProjectId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getGooglePlayGameServicesProjectId];
    }

    char* SuperfineSDKGetGooglePlayDeveloperAccountId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getGooglePlayDeveloperAccountId];
    }

    char* SuperfineSDKGetLinkedInAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getLinkedInAppId];
    }

    char* SuperfineSDKGetQQAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getQQAppId];
    }

    char* SuperfineSDKGetWeChatAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getWeChatAppId];
    }

    char* SuperfineSDKGetTikTokAppId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getTikTokAppId];
    }

    char* SuperfineSDKGetDeepLinkUrl()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getDeepLinkUrl];
    }

    char* SuperfineSDKGetDeviceToken()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getDeviceToken];
    }

    void SuperfineSDKFetchRemoteConfig(void (*callback)(const char* data))
    {
        [[SuperfineSDKUnityInterface sharedInstance] fetchRemoteConfig:callback];
    }

    void SuperfineSDKLog(const char* eventName)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName flag:0];
    }

    void SuperfineSDKLogWithFlag(const char* eventName, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName flag:flag];
    }

    void SuperfineSDKLogWithIntValue(const char* eventName, int value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName intValue:value flag:0];
    }

    void SuperfineSDKLogWithIntValueAndFlag(const char* eventName, int value, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName intValue:value flag:flag];
    }

    void SuperfineSDKLogWithStringValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName stringValue:value flag:0];
    }

    void SuperfineSDKLogWithStringValueAndFlag(const char* eventName, const char* value, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName stringValue:value flag:flag];
    }

    void SuperfineSDKLogWithMapValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName mapValue:value flag:0];
    }

    void SuperfineSDKLogWithMapValueAndFlag(const char* eventName, const char* value, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName mapValue:value flag:flag];
    }

    void SuperfineSDKLogWithJsonValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName jsonValue:value flag:0];
    }

    void SuperfineSDKLogWithJsonValueAndFlag(const char* eventName, const char* value, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName jsonValue:value flag:flag];
    }

    void SuperfineSDKLogBootStart()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logBootStart];
    }

    void SuperfineSDKLogBootEnd()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logBootEnd];
    }

    void SuperfineSDKLogLevelStart(int levelId, const char* name)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLevelStart_id:levelId name:name];
    }

    void SuperfineSDKLogLevelEnd(int levelId, const char* name, bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLevelEnd_id:levelId name:name isSuccess:isSuccess];
    }

    void SuperfineSDKLogAdLoad(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdLoad_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineSDKLogAdClose(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdClose_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineSDKLogAdClick(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdClick_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineSDKLogAdImpression(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdImpression_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineSDKLogIAPInitialization(bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPInitialization:isSuccess];
    }

    void SuperfineSDKLogIAPRestorePurchase()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPRestorePurchase];
    }

    void SuperfineSDKLogIAPResult(const char* pack, double price, int amount, const char* currency, bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPResult_pack:pack price:price amount:amount currency:currency isSuccess:isSuccess];
    }

    void SuperfineSDKLogIAPReceipt_Apple(const char* receipt)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Apple_receipt:receipt];
    }

    void SuperfineSDKLogIAPReceipt_Google(const char* data, const char* signature)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Google_data:data signature:signature];
    }

    void SuperfineSDKLogIAPReceipt_Amazon(const char* userId, const char* receiptId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Amazon_userId:userId receiptId:receiptId];
    }

    void SuperfineSDKLogIAPReceipt_Roku(const char* transactionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Roku_transactionId:transactionId];
    }

    void SuperfineSDKLogIAPReceipt_Windows(const char* receipt)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Windows_receipt:receipt];
    }

    void SuperfineSDKLogIAPReceipt_Facebook(const char* receipt)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Facebook_receipt:receipt];
    }

    void SuperfineSDKLogIAPReceipt_Unity(const char* receipt)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_Unity_receipt:receipt];
    }

    void SuperfineSDKLogIAPReceipt_AppStoreServer(const char* transactionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_AppStoreServer_transactionId:transactionId];
    }

    void SuperfineSDKLogIAPReceipt_GooglePlayProduct(const char* productId, const char* token)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_GooglePlayProduct_productId:productId token:token];
    }

    void SuperfineSDKLogIAPReceipt_GooglePlaySubscription(const char* subscriptionId, const char* token)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_GooglePlaySubscription_subscriptionId:subscriptionId token:token];
    }

    void SuperfineSDKLogIAPReceipt_GooglePlaySubscriptionv2(const char* token)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPReceipt_GooglePlaySubscriptionv2_token:token];
    }

    void SuperfineSDKLogUpdateApp(const char* newVersion)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logUpdateApp:newVersion];
    }

    void SuperfineSDKLogRateApp()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logRateApp];
    }

    void SuperfineSDKLogLocation(double latitude, double longitude)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLocation_latitude:latitude longitude:longitude];
    }

    void SuperfineSDKLogAuthorizationTrackingStatus(int status)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAuthorizationTrackingStatus:status];
    }

    void SuperfineSDKLogFacebookLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookLink:userId];
    }

    void SuperfineSDKLogFacebookUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookUnlink];
    }

    void SuperfineSDKLogInstagramLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logInstagramLink:userId];
    }

    void SuperfineSDKLogInstagramUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logInstagramUnlink];
    }

    void SuperfineSDKLogAppleLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleLink:userId];
    }

    void SuperfineSDKLogAppleUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleUnlink];
    }

    void SuperfineSDKLogAppleGameCenterLink(const char* gamePlayerId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleGameCenterLink:gamePlayerId];
    }

    void SuperfineSDKLogAppleGameCenterTeamLink(const char* teamPlayerId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleGameCenterTeamLink:teamPlayerId];
    }

    void SuperfineSDKLogAppleGameCenterUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleGameCenterUnlink];
    }

    void SuperfineSDKLogGoogleLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGoogleLink:userId];
    }

    void SuperfineSDKLogGoogleUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGoogleUnlink];
    }

    void SuperfineSDKLogGooglePlayGameServicesLink(const char* gamePlayerId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGooglePlayGameServicesLink:gamePlayerId];
    }

    void SuperfineSDKLogGooglePlayGameServicesDeveloperLink(const char* developerPlayerKey)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGooglePlayGameServicesDeveloperLink:developerPlayerKey];
    }

    void SuperfineSDKLogGooglePlayGameServicesUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGooglePlayGameServicesUnlink];
    }

    void SuperfineSDKLogLinkedInLink(const char* personId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLinkedInLink:personId];
    }

    void SuperfineSDKLogLinkedInUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLinkedInUnlink];
    }

    void SuperfineSDKLogMeetupLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMeetupLink:userId];
    }

    void SuperfineSDKLogMeetupUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMeetupUnlink];
    }

    void SuperfineSDKLogGitHubLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGitHubLink:userId];
    }

    void SuperfineSDKLogGitHubUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGitHubUnlink];
    }

    void SuperfineSDKLogDiscordLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logDiscordLink:userId];
    }

    void SuperfineSDKLogDiscordUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logDiscordUnlink];
    }

    void SuperfineSDKLogTwitterLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTwitterLink:userId];
    }

    void SuperfineSDKLogTwitterUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTwitterUnlink];
    }

    void SuperfineSDKLogSpotifyLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logSpotifyLink:userId];
    }

    void SuperfineSDKLogSpotifyUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logSpotifyUnlink];
    }

    void SuperfineSDKLogMicrosoftLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMicrosoftLink:userId];
    }

    void SuperfineSDKLogMicrosoftUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMicrosoftUnlink];
    }

    void SuperfineSDKLogLINELink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLINELink:userId];
    }

    void SuperfineSDKLogLINEUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLINEUnlink];
    }

    void SuperfineSDKLogVKLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logVKLink:userId];
    }

    void SuperfineSDKLogVKUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logVKUnlink];
    }

    void SuperfineSDKLogQQLink(const char* openId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logQQLink:openId];
    }

    void SuperfineSDKLogQQUnionLink(const char* unionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logQQUnionLink:unionId];
    }

    void SuperfineSDKLogQQUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logQQUnlink];
    }

    void SuperfineSDKLogWeChatLink(const char* openId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeChatLink:openId];
    }

    void SuperfineSDKLogWeChatUnionLink(const char* unionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeChatUnionLink:unionId];
    }

    void SuperfineSDKLogWeChatUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeChatUnlink];
    }

    void SuperfineSDKLogTikTokLink(const char* openId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTikTokLink:openId];
    }

    void SuperfineSDKLogTikTokUnionLink(const char* unionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTikTokUnionLink:unionId];
    }

    void SuperfineSDKLogTikTokUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTikTokUnlink];
    }

    void SuperfineSDKLogWeiboLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeiboLink:userId];
    }

    void SuperfineSDKLogWeiboUnlink()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeiboUnlink];
    }

    void SuperfineSDKLogAccountLink(const char* accountId, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLink_id:accountId type:type];
    }

    void SuperfineSDKLogAccountLink2(const char* accountId, const char* type, const char* scopeId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLink_id:accountId type:type scopeId:scopeId];
    }

    void SuperfineSDKLogAccountLink3(const char* accountId, const char* type, const char* scopeId, const char* scopeType)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLink_id:accountId type:type scopeId:scopeId scopeType:scopeType];
    }

    void SuperfineSDKLogAccountUnlink(const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountUnlink_type:type];
    }

    void SuperfineSDKAddUserPhoneNumber(int countryCode, const char* number)
    {
        [[SuperfineSDKUnityInterface sharedInstance] addUserPhoneNumber_countryCode:countryCode number:number];
    }

    void SuperfineSDKRemoveUserPhoneNumber(int countryCode, const char* number)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeUserPhoneNumber_countryCode:countryCode number:number];
    }

    void SuperfineSDKAddUserEmail(const char* email)
    {
        [[SuperfineSDKUnityInterface sharedInstance] addUserEmail:email];
    }

    void SuperfineSDKRemoveUserEmail(const char* email)
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeUserEmail:email];
    }

    void SuperfineSDKSetUserName(const char* firstName, const char* lastName)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserName_firstName:firstName lastName:lastName];
    }

    void SuperfineSDKSetUserFirstName(const char* firstName)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserName_firstName:firstName lastName:NULL];
    }

    void SuperfineSDKSetUserLastName(const char* lastName)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserName_firstName:NULL lastName:lastName];
    }

    void SuperfineSDKSetUserCity(const char* city)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserCity:city];
    }

    void SuperfineSDKSetUserState(const char* state)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserState:state];
    }

    void SuperfineSDKSetUserCountry(const char* country)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserCountry:country];
    }

    void SuperfineSDKSetUserZipCode(const char* zipCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserZipCode:zipCode];
    }

    void SuperfineSDKSetUserDateOfBirth(int day, int month, int year)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserDateOfBirth_day:day month:month year:year];
    }

    void SuperfineSDKSetUserDateOfBirth2(int day, int month)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserDateOfBirth_day:day month:month];
    }

    void SuperfineSDKSetUserYearOfBirth(int year)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserYearOfBirth:year];
    }

    void SuperfineSDKSetUserGender(int gender)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setUserGender:gender];
    }

    void SuperfineSDKLogWalletLink(const char* wallet, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWalletLink_wallet:wallet type:type];
    }

    void SuperfineSDKLogWalletUnlink(const char* wallet, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWalletUnlink_wallet:wallet type:type];
    }

    void SuperfineSDKLogCryptoPayment(const char* pack, double price, int amount, const char* currency, const char* chain)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logCryptoPayment_pack:pack price:price amount:amount currency:currency chain:chain];
    }

    void SuperfineSDKLogAdRevenue(const char* source, double revenue, const char* currency)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_source:source revenue:revenue currency:currency network:NULL networkData:NULL];
    }

    void SuperfineSDKLogAdRevenue2(const char* source, double revenue, const char* currency, const char* network)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_source:source revenue:revenue currency:currency network:network networkData:NULL];
    }

    void SuperfineSDKLogAdRevenue3(const char* source, double revenue, const char* currency, const char* network, const char* networkData)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_source:source revenue:revenue currency:currency network:network networkData:networkData];
    }

    void SuperfineSDKOpenURL(const char* url)
    {
        [[SuperfineSDKUnityInterface sharedInstance] openURL:url];
    }

    void SuperfineSDKSetDeviceToken(const char* token)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setDeviceToken:token];
    }

    void SuperfineSDKRequestTrackingAuthorization(void (*callback)(int))
    {
        [[SuperfineSDKUnityInterface sharedInstance] requestTrackingAuthorization:callback];
    }

    int SuperfineSDKGetTrackingAuthorizationStatus()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getTrackingAuthorizationStatus];
    }

    void SuperfineSDKUpdatePostbackConversionValue(int conversionValue)
    {
        [[SuperfineSDKUnityInterface sharedInstance] updatePostbackConversionValue:conversionValue];
    }

    void SuperfineSDKUpdatePostbackConversionValue2(int conversionValue, const char* coarseValue)
    {
        [[SuperfineSDKUnityInterface sharedInstance] updatePostbackConversionValue:conversionValue coarseValue:coarseValue];
    }

    void SuperfineSDKUpdatePostbackConversionValue3(int conversionValue, const char* coarseValue, bool lockWindow)
    {
        [[SuperfineSDKUnityInterface sharedInstance] updatePostbackConversionValue:conversionValue coarseValue:coarseValue lockWindow:lockWindow];
    }
}

