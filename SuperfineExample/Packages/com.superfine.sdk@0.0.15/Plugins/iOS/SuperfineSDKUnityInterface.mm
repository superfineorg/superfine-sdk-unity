#import <SuperfineSDK/SuperfineSDK.h>

#import <Foundation/NSJSONSerialization.h>

#include "SuperfineSDKUnityInterface.h"
#include "SuperfineSDKUnityUtility.h"

@interface SuperfineSDKLifecycleCallbackWrapper : NSObject<SuperfineSDKLifecycleDelegate>

@property (nonatomic, nullable) NSMutableArray* startCallbacks;
@property (nonatomic, nullable) NSMutableArray* stopCallbacks;
@property (nonatomic, nullable) NSMutableArray* pauseCallbacks;
@property (nonatomic, nullable) NSMutableArray* resumeCallbacks;

@end

@implementation SuperfineSDKLifecycleCallbackWrapper

- (instancetype)init
{
    if (self = [super init])
    {
        _startCallbacks = NULL;
        _stopCallbacks = NULL;
        _pauseCallbacks = NULL;
        _resumeCallbacks = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return
        self.startCallbacks != NULL ||
        self.stopCallbacks != NULL ||
        self.pauseCallbacks != NULL ||
        self.resumeCallbacks != NULL;
}

- (void)addStartCallback:(void(*)(void))callback
{
    if (self.startCallbacks == NULL)
    {
        self.startCallbacks = [[NSMutableArray alloc] init];
    }
    
    [self.startCallbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeStartCallback:(void(*)(void))callback
{
    if (self.startCallbacks == NULL)
    {
        return;
    }
    
    [self.startCallbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.startCallbacks count] == 0) self.startCallbacks = NULL;
}

- (void)addStopCallback:(void(*)(void))callback
{
    if (self.stopCallbacks == NULL)
    {
        self.stopCallbacks = [[NSMutableArray alloc] init];
    }
    
    [self.stopCallbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeStopCallback:(void(*)(void))callback
{
    if (self.stopCallbacks == NULL)
    {
        return;
    }
    
    [self.stopCallbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.stopCallbacks count] == 0) self.stopCallbacks = NULL;
}

- (void)addPauseCallback:(void(*)(void))callback
{
    if (self.pauseCallbacks == NULL)
    {
        self.pauseCallbacks = [[NSMutableArray alloc] init];
    }
    
    [self.pauseCallbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removePauseCallback:(void(*)(void))callback
{
    if (self.pauseCallbacks == NULL)
    {
        return;
    }
    
    [self.pauseCallbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.pauseCallbacks count] == 0) self.pauseCallbacks = NULL;
}

- (void)addResumeCallback:(void(*)(void))callback
{
    if (self.resumeCallbacks == NULL)
    {
        self.resumeCallbacks = [[NSMutableArray alloc] init];
    }
    
    [self.resumeCallbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeResumeCallback:(void(*)(void))callback
{
    if (self.resumeCallbacks == NULL)
    {
        return;
    }
    
    [self.resumeCallbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.resumeCallbacks count] == 0) self.resumeCallbacks = NULL;
}

- (void)onStart
{
    if (self.startCallbacks != NULL)
    {
        int count = (int)[self.startCallbacks count];
        for (int i = 0; i < count; ++i)
        {
            void(*startCallback)(void) = (void(*)(void))[self.startCallbacks[i] pointerValue];
            if (startCallback != nullptr)
            {
                startCallback();
            }
        }
    }
}

- (void)onStop
{
    if (self.stopCallbacks != NULL)
    {
        int count = (int)[self.stopCallbacks count];
        for (int i = 0; i < count; ++i)
        {
            void(*stopCallback)(void) = (void(*)(void))[self.stopCallbacks[i] pointerValue];
            if (stopCallback != nullptr)
            {
                stopCallback();
            }
        }
    }
}

- (void)onPause
{
    if (self.pauseCallbacks != NULL)
    {
        int count = (int)[self.pauseCallbacks count];
        for (int i = 0; i < count; ++i)
        {
            void(*pauseCallback)(void) = (void(*)(void))[self.pauseCallbacks[i] pointerValue];
            if (pauseCallback != nullptr)
            {
                pauseCallback();
            }
        }
    }
}


- (void)onResume
{
    if (self.resumeCallbacks != NULL)
    {
        int count = (int)[self.resumeCallbacks count];
        for (int i = 0; i < count; ++i)
        {
            void(*resumeCallback)(void) = (void(*)(void))[self.resumeCallbacks[i] pointerValue];
            if (resumeCallback != nullptr)
            {
                resumeCallback();
            }
        }
    }
}

@end

@interface SuperfineSDKDeepLinkCallbackWrapper : NSObject<SuperfineSDKDeepLinkDelegate>

@property (nonatomic, nullable) NSMutableArray* callbacks;

@end

@implementation SuperfineSDKDeepLinkCallbackWrapper

- (instancetype)init
{
    if (self = [super init])
    {
        _callbacks = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callbacks != NULL;
}

- (void)addCallback:(void(*)(const char*))callback
{
    if (self.callbacks == NULL)
    {
        self.callbacks = [[NSMutableArray alloc] init];
    }
    
    [self.callbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeCallback:(void(*)(const char*))callback
{
    if (self.callbacks == NULL)
    {
        return;
    }
    
    [self.callbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.callbacks count] == 0) self.callbacks = NULL;
}

- (void)onSetDeepLink:(nonnull NSURL*)url
{
    if (self.callbacks != NULL)
    {
        int count = (int)[self.callbacks count];
        if (count == 0) return;
        
        const char* urlRet = [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
        
        for (int i = 0; i < count; ++i)
        {
            void(*deepLinkCallback)(const char*) = (void(*)(const char*))[self.callbacks[i] pointerValue];
            if (deepLinkCallback != nullptr)
            {
                deepLinkCallback(urlRet);
            }
        }
        
        if (urlRet != NULL)
        {
            delete urlRet;
        }
    }
}

@end

@interface SuperfineSDKDeviceTokenCallbackWrapper : NSObject<SuperfineSDKDeviceTokenDelegate>

@property (nonatomic, nullable) NSMutableArray* callbacks;

@end

@implementation SuperfineSDKDeviceTokenCallbackWrapper

- (instancetype)init
{
    if (self = [super init])
    {
        _callbacks = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callbacks != NULL;
}

- (void)addCallback:(void(*)(const char*))callback
{
    if (self.callbacks == NULL)
    {
        self.callbacks = [[NSMutableArray alloc] init];
    }
    
    [self.callbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeCallback:(void(*)(const char*))callback
{
    if (self.callbacks == NULL)
    {
        return;
    }
    
    [self.callbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.callbacks count] == 0) self.callbacks = NULL;
}

- (void)onSetDeviceToken:(NSString *)token
{
    if (self.callbacks != NULL)
    {
        int count = (int)[self.callbacks count];
        if (count == 0) return;
        
        const char* tokenRet = [SuperfineSDKUnityUtility makeStringReturn:token];
        
        for (int i = 0; i < count; ++i)
        {
            void(*deviceTokenCallback)(const char*) = (void(*)(const char*))[self.callbacks[i] pointerValue];
            if (deviceTokenCallback != nullptr)
            {
                deviceTokenCallback(tokenRet);
            }
        }
        
        if (tokenRet != NULL)
        {
            delete tokenRet;
        }
    }
}

@end

@interface SuperfineSDKSendEventCallbackWrapper : NSObject<SuperfineSDKSendEventDelegate>

@property (nonatomic, nullable) NSMutableArray* callbacks;

@end

@implementation SuperfineSDKSendEventCallbackWrapper

- (instancetype)init
{
    if (self = [super init])
    {
        _callbacks = NULL;
    }
    
    return self;
}

- (BOOL)hasCallback
{
    return self.callbacks != NULL;
}

- (void)addCallback:(void(*)(const char*, const char*))callback
{
    if (self.callbacks == NULL)
    {
        self.callbacks = [[NSMutableArray alloc] init];
    }
    
    [self.callbacks addObject:[NSValue valueWithPointer:(void*)callback]];
}

- (void)removeCallback:(void(*)(const char*, const char*))callback
{
    if (self.callbacks == NULL)
    {
        return;
    }
    
    [self.callbacks removeObject:[NSValue valueWithPointer:(void*)callback]];
    if ([self.callbacks count] == 0) self.callbacks = NULL;
}

- (void)onSendEvent:(nonnull NSString *)eventName data:(NSString * _Nullable)eventData
{
    if (self.callbacks != NULL)
    {
        int count = (int)[self.callbacks count];
        if (count == 0) return;
        
        const char* eventNameRet = [SuperfineSDKUnityUtility makeStringReturn:eventName];
        const char* eventDataRet = [SuperfineSDKUnityUtility makeStringReturn:eventData];
        
        for (int i = 0; i < count; ++i)
        {
            void(*sendEventCallback)(const char*, const char*) = (void(*)(const char*, const char*))[self.callbacks[i] pointerValue];
            if (sendEventCallback != nullptr)
            {
                sendEventCallback(eventNameRet, eventDataRet);
            }
        }
        
        if (eventNameRet != nullptr)
        {
            delete eventNameRet;
        }
        
        if (eventDataRet != nullptr)
        {
            delete eventDataRet;
        }
    }
}

@end

@interface SuperfineSDKUnityInterface()

@end

@implementation SuperfineSDKUnityInterface

SuperfineSDKLifecycleCallbackWrapper* lifecycleWrapper = NULL;
SuperfineSDKDeepLinkCallbackWrapper* deepLinkWrapper = NULL;
SuperfineSDKDeviceTokenCallbackWrapper* deviceTokenWrapper = NULL;
SuperfineSDKSendEventCallbackWrapper* sendEventWrapper = NULL;

NSMutableArray* pendingEvents;

#pragma mark Object Initialization

#ifndef NON_UNITY
+ (void)load
{
    static dispatch_once_t onceToken;

    dispatch_once(&onceToken, ^{
        [SuperfineSDKUnityInterface sharedInstance];
    });
}

+ (NSData*)deviceTokenFromNotification:(NSNotification*)notification
{
    NSData* deviceToken = NULL;
    if ([notification.userInfo isKindOfClass:[NSData class]])
    {
        deviceToken = (NSData*)notification.userInfo;
    }
    
    return deviceToken;
}
#endif

+ (SuperfineSDKUnityInterface *)sharedInstance
{
  static dispatch_once_t onceToken;
  static SuperfineSDKUnityInterface *sharedInstance = nil;

  dispatch_once(&onceToken, ^{
      sharedInstance = [[SuperfineSDKUnityInterface alloc] init];
      
#ifndef NON_UNITY
      NSNotificationCenter* center = [NSNotificationCenter defaultCenter];
      
      [center addObserverForName: kUnityDidRegisterForRemoteNotificationsWithDeviceToken object: nil queue: [NSOperationQueue mainQueue] usingBlock:^(NSNotification* notification){
          NSLog(@"didRegisterForRemoteNotificationsWithDeviceToken");
          [SuperfineSDKManager onRegisteredForRemoteNotificationsWithDeviceToken:[self deviceTokenFromNotification:notification]];
      }];

      [center addObserverForName: kUnityDidFailToRegisterForRemoteNotificationsWithError object: nil queue: [NSOperationQueue mainQueue] usingBlock:^(NSNotification* notification){
          NSLog(@"didFailToRegisterForRemoteNotificationsWithError");
          [SuperfineSDKManager onFailedToRegisterForRemoteNotificationsWithError:[[NSError alloc] init]];

      }];
#endif
  });

  return sharedInstance;
}

- (instancetype)init
{
    if (self = [super init])
    {
        lifecycleWrapper = NULL;
        deepLinkWrapper = NULL;
        deviceTokenWrapper = NULL;
        sendEventWrapper = NULL;
        
        pendingEvents = [[NSMutableArray alloc] init];
    }
    
    return self;
}

- (void)initialize:(const char *)params callback:(void(*)())callback
{
    SuperfineSDKManager* manager = [SuperfineSDKManager sharedInstance];
    
    if (manager != NULL)
    {
        NSLog(@"Superfine SDK Manager already exists");
        return;
    }
    
    SuperfineSDKConfiguration* config = [SuperfineSDKManager createConfigurationFromJson:[SuperfineSDKUnityUtility stringFromCString:params]];
    
    if (config == NULL)
    {
        NSLog(@"Unable to create config data");
        return;
    }
    
    id handler = ^(void)
    {
        if (callback != NULL)
        {
            callback();
        }
    };
        
    [SuperfineSDKManager initialize:config completion:handler];
}

- (bool)isInitialized
{
    SuperfineSDKManager* manager = [SuperfineSDKManager sharedInstance];
    if (manager == NULL) return FALSE;
    
    return [manager isInitialized];
}

- (bool)assertSuperfineSDKManager
{
    SuperfineSDKManager* manager = [SuperfineSDKManager sharedInstance];
    
    if (manager == NULL)
    {
        NSLog(@"Superfine SDK Manager doesn't exist");
        return false;
    }
    
    if (![manager isInitialized])
    {
        NSLog(@"Superfine SDK Manager isn't initialized");
        return false;
    }
    
    return true;
}

- (void)start
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] start];
}

- (void)stop
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] stop];
}

- (void)shutdown
{
    if (![self assertSuperfineSDKManager]) return;
    
    [self stop];
    [SuperfineSDKManager shutdown];
    
    lifecycleWrapper = NULL;
    deepLinkWrapper = NULL;
    deviceTokenWrapper = NULL;
    sendEventWrapper = NULL;
}

- (void)setOffline:(BOOL)value
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setOffline:value];
}

- (void)addStartCallback:(void(*)(void))callback
{
    if (callback == NULL) return;
        
    if (lifecycleWrapper == NULL)
    {
        lifecycleWrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init];
        [lifecycleWrapper addStartCallback:callback];
                
        [SuperfineSDKManager addLifecycleDelegate:lifecycleWrapper];
    }
    else
    {
        [lifecycleWrapper addStartCallback:callback];
    }
}

- (void)removeStartCallback:(void(*)(void))callback
{
    if (callback == NULL || lifecycleWrapper == NULL) return;
    
    [lifecycleWrapper removeStartCallback:callback];
    
    if (![lifecycleWrapper hasCallback])
    {
        [SuperfineSDKManager removeLifecycleDelegate:lifecycleWrapper];
        lifecycleWrapper = NULL;
    }
}

- (void)addStopCallback:(void(*)(void))callback
{
    if (callback == NULL) return;
    
    if (lifecycleWrapper == NULL)
    {
        lifecycleWrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init];
        [lifecycleWrapper addStopCallback:callback];
                
        [SuperfineSDKManager addLifecycleDelegate:lifecycleWrapper];
    }
    else
    {
        [lifecycleWrapper addStopCallback:callback];
    }
}

- (void)removeStopCallback:(void(*)(void))callback
{
    if (callback == NULL || lifecycleWrapper == NULL) return;
    
    [lifecycleWrapper removeStopCallback:callback];
    
    if (![lifecycleWrapper hasCallback])
    {
        [SuperfineSDKManager removeLifecycleDelegate:lifecycleWrapper];
        lifecycleWrapper = NULL;
    }
}

- (void)addPauseCallback:(void(*)(void))callback
{
    if (callback == NULL) return;
    
    if (lifecycleWrapper == NULL)
    {
        lifecycleWrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init];
        [lifecycleWrapper addPauseCallback:callback];
                
        [SuperfineSDKManager addLifecycleDelegate:lifecycleWrapper];
    }
    else
    {
        [lifecycleWrapper addPauseCallback:callback];
    }
}

- (void)removePauseCallback:(void(*)(void))callback
{
    if (callback == NULL || lifecycleWrapper == NULL) return;
    
    [lifecycleWrapper removePauseCallback:callback];
    
    if (![lifecycleWrapper hasCallback])
    {
        [SuperfineSDKManager removeLifecycleDelegate:lifecycleWrapper];
        lifecycleWrapper = NULL;
    }
}

- (void)addResumeCallback:(void(*)(void))callback
{
    if (callback == NULL) return;
    
    if (lifecycleWrapper == NULL)
    {
        lifecycleWrapper = [[SuperfineSDKLifecycleCallbackWrapper alloc] init];
        [lifecycleWrapper addResumeCallback:callback];
                
        [SuperfineSDKManager addLifecycleDelegate:lifecycleWrapper];
    }
    else
    {
        [lifecycleWrapper addResumeCallback:callback];
    }
}

- (void)removeResumeCallback:(void(*)(void))callback
{
    if (callback == NULL || lifecycleWrapper == NULL) return;
    
    [lifecycleWrapper removeResumeCallback:callback];
    
    if (![lifecycleWrapper hasCallback])
    {
        [SuperfineSDKManager removeLifecycleDelegate:lifecycleWrapper];
        lifecycleWrapper = NULL;
    }
}

- (void)addDeepLinkCallback:(void(*)(const char*))callback
{
    if (callback == NULL) return;
    
    if (deepLinkWrapper == NULL)
    {
        deepLinkWrapper = [[SuperfineSDKDeepLinkCallbackWrapper alloc] init];
        [deepLinkWrapper addCallback:callback];
                
        [SuperfineSDKManager addDeepLinkDelegate:deepLinkWrapper];
    }
    else
    {
        [deepLinkWrapper addCallback:callback];
    }
}

- (void)removeDeepLinkCallback:(void(*)(const char*))callback
{
    if (callback == NULL || deepLinkWrapper == NULL) return;
    
    [deepLinkWrapper removeCallback:callback];
    
    if (![deepLinkWrapper hasCallback])
    {
        [SuperfineSDKManager removeDeepLinkDelegate:deepLinkWrapper];
        deepLinkWrapper = NULL;
    }
}

- (void)addDeviceTokenCallback:(void(*)(const char*))callback
{
    if (callback == NULL) return;
    
    if (deviceTokenWrapper == NULL)
    {
        deviceTokenWrapper = [[SuperfineSDKDeviceTokenCallbackWrapper alloc] init];
        [deviceTokenWrapper addCallback:callback];
                
        [SuperfineSDKManager addDeviceTokenDelegate:deviceTokenWrapper];
    }
    else
    {
        [deviceTokenWrapper addCallback:callback];
    }
}

- (void)removeDeviceTokenCallback:(void(*)(const char*))callback
{
    if (callback == NULL || deviceTokenWrapper == NULL) return;
    
    [deviceTokenWrapper removeCallback:callback];
    
    if (![deviceTokenWrapper hasCallback])
    {
        [SuperfineSDKManager removeDeviceTokenDelegate:deviceTokenWrapper];
        deviceTokenWrapper = NULL;
    }
}

- (void)addSendEventCallback:(void(*)(const char*, const char*))callback
{
    if (callback == NULL) return;
    
    if (sendEventWrapper == NULL)
    {
        sendEventWrapper = [[SuperfineSDKSendEventCallbackWrapper alloc] init];
        [sendEventWrapper addCallback:callback];
                
        [SuperfineSDKManager addSendEventDelegate:sendEventWrapper];
    }
    else
    {
        [sendEventWrapper addCallback:callback];
    }
}

- (void)removeSendEventCallback:(void(*)(const char*, const char*))callback
{
    if (callback == NULL || sendEventWrapper == NULL) return;
    
    [sendEventWrapper removeCallback:callback];
    
    if (![sendEventWrapper hasCallback])
    {
        [SuperfineSDKManager removeSendEventDelegate:sendEventWrapper];
        sendEventWrapper = NULL;
    }
}

- (void)gdprForgetMe
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] gdprForgetMe];
}

- (void)disableThirdPartySharing
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] disableThirdPartySharing];
}

- (void)enableThirdPartySharing
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] enableThirdPartySharing];
}

- (void)logThirdPartySharing:(const char*)params
{
    if (![self assertSuperfineSDKManager]) return;
    
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
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[SuperfineSDKManager version]];
}

- (char*)getAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppId]];
}

- (char*)getUserId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getUserId]];
}

- (char*)getSessionId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getSessionId]];
}

- (char*)getHost
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    NSURL* url = [[SuperfineSDKManager sharedInstance] getHost];
    if (url == NULL) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    return [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
}

- (char*)getConfigUrl
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    NSURL* url = [[SuperfineSDKManager sharedInstance] getConfigUrl];
    if (url == NULL) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    return [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
}

- (char*)getSdkConfig
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    NSString* sdkConfig = [[SuperfineSDKManager sharedInstance] getSdkConfigString];    
    return [SuperfineSDKUnityUtility makeStringReturn:sdkConfig];
}

- (SuperfineSDKStoreType)getStoreType
{
    if (![self assertSuperfineSDKManager]) return SuperfineSDKStoreType_AppStore;
    return [[SuperfineSDKManager sharedInstance] getStoreType];
}

- (char*)getFacebookAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getFacebookAppId]];
}

- (char*)getInstagramAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getInstagramAppId]];
}

- (char*)getAppleAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleAppId]];
}

- (char*)getAppleSignInClientId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleSignInClientId]];
}

- (char*)getAppleDeveloperTeamId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getAppleDeveloperTeamId]];
}

- (char*)getGooglePlayGameServicesProjectId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getGooglePlayGameServicesProjectId]];
}

- (char*)getGooglePlayDeveloperAccountId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getGooglePlayDeveloperAccountId]];
}

- (char*)getLinkedInAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getLinkedInAppId]];
}

- (char*)getQQAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getQQAppId]];
}

- (char*)getWeChatAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getWeChatAppId]];
}

- (char*)getTikTokAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getTikTokAppId]];
}

- (char*)getSnapAppId
{
    if (![self assertSuperfineSDKManager]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getSnapAppId]];
}

- (char*)getDeepLinkUrl
{    
    NSURL* url = [SuperfineSDKManager getDeepLinkUrl];
    if (url == NULL) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    
    return [SuperfineSDKUnityUtility makeStringReturn:[url absoluteString]];
}

- (char*)getDeviceToken
{
    return [SuperfineSDKUnityUtility makeStringReturn:[SuperfineSDKManager getDeviceTokenString]];
}

- (void)setConfigId:(const char*)configId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setConfigId:[SuperfineSDKUnityUtility stringFromCString:configId]];
}

- (void)setCustomUserId:(const char*)customUserId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setCustomUserId:[SuperfineSDKUnityUtility stringFromCString:customUserId]];
}

- (void)fetchRemoteConfig:(void(*)(const char*))callback
{
    if (![self assertSuperfineSDKManager])
    {
        if (callback != NULL)
        {
            callback([SuperfineSDKUnityUtility makeStringReturn:@""]);
        }
        
        return;
    }
    
    id handler = ^(NSString* _Nullable result)
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
    if (![self assertSuperfineSDKManager]) return;
    
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] flag:(SuperfineSDKOperationFlag)flag];
}

- (void)log:(const char*)eventName intValue:(int)value flag:(int)flag
{
    if (![self assertSuperfineSDKManager]) return;

    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] intValue:value flag:(SuperfineSDKOperationFlag)flag];
}

- (void)log:(const char*)eventName stringValue:(const char*)value flag:(int)flag
{
    if (![self assertSuperfineSDKManager]) return;

    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] stringValue:[SuperfineSDKUnityUtility stringFromCString:value] flag:(SuperfineSDKOperationFlag)flag];
}

- (NSMutableDictionary*)createLogJsonData:(const char*)value
{
    if (value == NULL || value[0] == '\0') return NULL;

    NSData* jsonData = [[SuperfineSDKUnityUtility stringFromCString: value] dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError* e = nil;
    NSMutableDictionary* data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
    
    if (e != nil)
    {
        NSLog(@"Error while parsing track parameters: %@", [e localizedDescription]);
        return NULL;
    }
    
    return data;
}

- (void)log:(const char*)eventName jsonValue:(const char*)value flag:(int)flag
{
    if (![self assertSuperfineSDKManager]) return;
 
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] jsonValue:[self createLogJsonData:value] flag:(SuperfineSDKOperationFlag)flag];
}

- (NSMutableDictionary<NSString *, NSString *>*)createLogMapData:(const char*)value
{
    if (value == NULL || value[0] == '\0') return NULL;

    NSString *str = [SuperfineSDKUnityUtility stringFromCString:value];
    
    NSArray *entries = [str componentsSeparatedByString:@","];
    if (entries == NULL) return NULL;
                
    NSMutableDictionary<NSString *, NSString *> *data = NULL;

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
                if (data == NULL) data = [[NSMutableDictionary alloc] init];
                [data setValue:value forKey:key];
            }
        }
    }
    
    return data;
}

- (void)log:(const char*)eventName mapValue:(const char*)value flag:(int)flag
{
    if (![self assertSuperfineSDKManager]) return;
        
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] mapValue:[self createLogMapData:value] flag:(SuperfineSDKOperationFlag)flag];
}

- (SuperfineSDKEvent*)getPendingEvent:(int)eventRef
{
    if (eventRef <= 0 || eventRef > pendingEvents.count) return NULL;
    
    id eventPtr = pendingEvents[eventRef - 1];
    if (eventPtr == NULL || eventPtr == [NSNull null]) return NULL;
    
    return eventPtr;
}

- (int)beginLogEvent:(const char*)eventName
{
    SuperfineSDKEvent* event = [[SuperfineSDKEvent alloc] init:[SuperfineSDKUnityUtility stringFromCString:eventName]];
    [pendingEvents addObject:event];
    
    return (int)pendingEvents.count;
}

- (void)setLogEventValue:(int)eventRef intValue:(int)value
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    [event setIntValue:value];
}

- (void)setLogEventValue:(int)eventRef stringValue:(const char*)value
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    [event setStringValue:[SuperfineSDKUnityUtility stringFromCString:value]];
}

- (void)setLogEventValue:(int)eventRef mapValue:(const char*)value
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    [event setMapValue:[self createLogMapData:value]];
}

- (void)setLogEventValue:(int)eventRef jsonValue:(const char*)value
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    [event setJsonValue:[self createLogJsonData:value]];
}

- (void)setLogEventFlag:(int)eventRef flag:(int)flag
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    event.flag = (SuperfineSDKOperationFlag)flag;
}

- (void)setLogEventRevenue:(int)eventRef revenue:(double)revenue currency:(const char*)currency
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
        
    [event setRevenue:revenue currency:[SuperfineSDKUnityUtility stringFromCString:currency]];
}

- (void)endLogEvent:(int)eventRef cache:(BOOL)cache;
{
    SuperfineSDKEvent* event = [self getPendingEvent:eventRef];
    if (event == NULL) return;
    
    int numEvents = (int)pendingEvents.count;
    if (eventRef == numEvents)
    {
        [pendingEvents removeLastObject];
        numEvents--;
        
        while (numEvents > 0 && [pendingEvents lastObject] == [NSNull null])
        {
            [pendingEvents removeLastObject];
            numEvents--;
        }
    }
    else
    {
        [pendingEvents setObject:[NSNull null] atIndexedSubscript:eventRef - 1];
    }
    
    if (cache)
    {
        [SuperfineSDKManager logEvent:event];
    }
    else
    {
        if (![self assertSuperfineSDKManager]) return;
        [[SuperfineSDKManager sharedInstance] logEvent:event];
    }
}

- (void)logBootStart
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logBootStart];
}

- (void)logBootEnd
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logBootEnd];
}

- (void)logLevelStart_id:(int)levelId name:(const char*)name
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLevelStart_id:levelId name:[SuperfineSDKUnityUtility stringFromCString:name]];
}

- (void)logLevelEnd_id:(int)levelId name:(const char*)name isSuccess:(bool)isSuccess
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLevelEnd_id:levelId name:[SuperfineSDKUnityUtility stringFromCString:name] isSuccess:isSuccess];
}

- (void)logAdLoad_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAdLoad_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdClose_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAdClose_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdClick_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAdClick_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logAdImpression_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAdImpression_adUnit:[SuperfineSDKUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineSDKAdPlacementType)adPlacementType adPlacement:(SuperfineSDKAdPlacement)adPlacement];
}

- (void)logIAPInitialization:(bool)isSuccess
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPInitialization:isSuccess];
}

- (void)logIAPRestorePurchase
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPRestorePurchase];
}

- (void)logIAPResult_pack:(const char*)pack price:(double)price amount:(int)amount currency:(const char*)currency isSuccess:(bool)isSuccess
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPResult_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] isSuccess:isSuccess];
}

- (void)logIAPReceipt_Apple_receipt:(const char*)receipt
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Apple_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Google_data:(const char*)data signature:(const char*)signature
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Google_data:[SuperfineSDKUnityUtility stringFromCString:data] signature:[SuperfineSDKUnityUtility stringFromCString:signature]];
}

- (void)logIAPReceipt_Amazon_userId:(const char*)userId receiptId:(const char*)receiptId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Amazon_userId:[SuperfineSDKUnityUtility stringFromCString:userId] receiptId:[SuperfineSDKUnityUtility stringFromCString:receiptId]];
}

- (void)logIAPReceipt_Roku_transactionId:(const char*)transactionId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Roku_transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId]];
}

- (void)logIAPReceipt_Windows_receipt:(const char*)receipt
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Windows_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Facebook_receipt:(const char*)receipt
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Facebook_receipt:[SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_Unity_receipt:(const char*)receipt
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_Unity_receipt: [SuperfineSDKUnityUtility stringFromCString:receipt]];
}

- (void)logIAPReceipt_AppStoreServer_transactionId:(const char*)transactionId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_AppStoreServer_transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId]];
}

- (void)logIAPReceipt_GooglePlayProduct_productId:(const char*)productId token:(const char*)token
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlayProduct_productId: [SuperfineSDKUnityUtility stringFromCString:productId] token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logIAPReceipt_GooglePlaySubscription_subscriptionId:(const char*)subscriptionId token:(const char*)token
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlaySubscription_subscriptionId: [SuperfineSDKUnityUtility stringFromCString:subscriptionId] token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logIAPReceipt_GooglePlaySubscriptionv2_token:(const char*)token
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logIAPReceipt_GooglePlaySubscriptionv2_token:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)logUpdateApp:(const char*)newVersion
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logUpdateApp:[SuperfineSDKUnityUtility stringFromCString:newVersion]];
}

- (void)logRateApp
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logRateApp];
}

- (void)logLocation_latitude:(double)latitude longitude:(double)longitude
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLocation_latitude:latitude longitude:longitude];
}

- (void)logTrackingAuthorizationStatus:(int)status
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTrackingAuthorizationStatus:(SuperfineSDKTrackingAuthorizationStatus)status];
}

- (void)logFacebookLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logFacebookUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookUnlink];
}

- (void)logInstagramLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logInstagramLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logInstagramUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logInstagramUnlink];
}

- (void)logAppleLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAppleLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logAppleUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAppleUnlink];
}

- (void)logAppleGameCenterLink:(const char*)gamePlayerId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterLink:[SuperfineSDKUnityUtility stringFromCString:gamePlayerId]];
}

- (void)logAppleGameCenterTeamLink:(const char*)teamPlayerId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterTeamLink:[SuperfineSDKUnityUtility stringFromCString:teamPlayerId]];
}

- (void)logAppleGameCenterUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAppleGameCenterUnlink];
}

- (void)logGoogleLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGoogleLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logGoogleUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGoogleUnlink];
}

- (void)logGooglePlayGameServicesLink:(const char*)gamePlayerId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesLink:[SuperfineSDKUnityUtility stringFromCString:gamePlayerId]];
}

- (void)logGooglePlayGameServicesDeveloperLink:(const char*)developerPlayerKey
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesDeveloperLink:[SuperfineSDKUnityUtility stringFromCString:developerPlayerKey]];
}

- (void)logGooglePlayGameServicesUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGooglePlayGameServicesUnlink];
}

- (void)logLinkedInLink:(const char*)personId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLinkedInLink:[SuperfineSDKUnityUtility stringFromCString:personId]];
}

- (void)logLinkedInUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLinkedInUnlink];
}

- (void)logMeetupLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logMeetupLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logMeetupUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logMeetupUnlink];
}

- (void)logGitHubLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGitHubLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logGitHubUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logGitHubUnlink];
}

- (void)logDiscordLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logDiscordLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logDiscordUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logDiscordUnlink];
}

- (void)logTwitterLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTwitterLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logTwitterUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTwitterUnlink];
}

- (void)logSpotifyLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logSpotifyLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logSpotifyUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logSpotifyUnlink];
}

- (void)logMicrosoftLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logMicrosoftLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logMicrosoftUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logMicrosoftUnlink];
}

- (void)logLINELink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLINELink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logLINEUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logLINEUnlink];
}

- (void)logVKLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logVKLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logVKUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logVKUnlink];
}

- (void)logQQLink:(const char*)openId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logQQLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logQQUnionLink:(const char*)unionId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logQQUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logQQUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logQQUnlink];
}

- (void)logWeChatLink:(const char*)openId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logWeChatUnionLink:(const char*)unionId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logWeChatUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWeChatUnlink];
}

- (void)logTikTokLink:(const char*)openId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokLink:[SuperfineSDKUnityUtility stringFromCString:openId]];
}

- (void)logTikTokUnionLink:(const char*)unionId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokUnionLink:[SuperfineSDKUnityUtility stringFromCString:unionId]];
}

- (void)logTikTokUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logTikTokUnlink];
}

- (void)logWeiboLink:(const char*)userId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWeiboLink:[SuperfineSDKUnityUtility stringFromCString:userId]];
}

- (void)logWeiboUnlink
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWeiboUnlink];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type scopeId:(const char*)scopeId
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type] scopeId:[SuperfineSDKUnityUtility stringFromCString:scopeId]];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type scopeId:(const char*)scopeId scopeType:(const char*)scopeType
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type] scopeId:[SuperfineSDKUnityUtility stringFromCString:scopeId] scopeType:[SuperfineSDKUnityUtility stringFromCString:scopeType]];
}

- (void)logAccountUnlink_type:(const char*)type
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logAccountUnlink_type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)addUserPhoneNumber_countryCode:(int)countryCode number:(const char*)number
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] addUserPhoneNumber_countryCode:countryCode number:[SuperfineSDKUnityUtility stringFromCString:number]];
}

- (void)removeUserPhoneNumber_countryCode:(int)countryCode number:(const char*)number
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] removeUserPhoneNumber_countryCode:countryCode number:[SuperfineSDKUnityUtility stringFromCString:number]];
}

- (void)addUserEmail:(const char*)email
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] addUserEmail:[SuperfineSDKUnityUtility stringFromCString:email]];
}

- (void)removeUserEmail:(const char*)email
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] removeUserEmail:[SuperfineSDKUnityUtility stringFromCString:email]];
}

- (void)setUserName_firstName:(const char*)firstName lastName:(const char*)lastName
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserName_firstName:[SuperfineSDKUnityUtility stringFromCString:firstName] lastName:[SuperfineSDKUnityUtility stringFromCString:lastName]];
}

- (void)setUserCity:(const char*)city
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserCity:[SuperfineSDKUnityUtility stringFromCString:city]];
}

- (void)setUserState:(const char*)state
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserState:[SuperfineSDKUnityUtility stringFromCString:state]];
}

- (void)setUserCountry:(const char*)country
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserCountry:[SuperfineSDKUnityUtility stringFromCString:country]];
}

- (void)setUserZipCode:(const char*)zipCode
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserZipCode:[SuperfineSDKUnityUtility stringFromCString:zipCode]];
}

- (void)setUserDateOfBirth_day:(int)day month:(int)month year:(int)year
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserDateOfBirth_day:day month:month year:year];
}

- (void)setUserDateOfBirth_day:(int)day month:(int)month
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserDateOfBirth_day:day month:month];
}

- (void)setUserYearOfBirth:(int)year
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserYearOfBirth:year];
}

- (void)setUserGender:(int)gender
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] setUserGender:(SuperfineSDKUserGender)gender];
}
 
- (void)logWalletLink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWalletLink_wallet:[SuperfineSDKUnityUtility stringFromCString:wallet] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logWalletUnlink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logWalletUnlink_wallet:[SuperfineSDKUnityUtility stringFromCString:wallet] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logCryptoPayment_pack:(const char*)pack price:(double)price amount:(int)amount currency:(const char*)currency chain:(const char*)chain
{
    if (![self assertSuperfineSDKManager]) return;
    [[SuperfineSDKManager sharedInstance] logCryptoPayment_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] chain:[SuperfineSDKUnityUtility stringFromCString:chain]];
}

- (void)logAdRevenue_source:(const char*)source revenue:(double)revenue currency:(const char*)currency network:(const char*)network networkData:(const char*)networkData
{
    if (![self assertSuperfineSDKManager]) return;
    
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
    [SuperfineSDKManager openURL:[NSURL URLWithString:[SuperfineSDKUnityUtility stringFromCString:url]]];
}

- (void)setDeviceToken:(const char*)token
{
    [SuperfineSDKManager setDeviceTokenString:[SuperfineSDKUnityUtility stringFromCString:token]];
}

- (void)requestTrackingAuthorization:(void(*)(int))callback
{
    id handler = ^(NSUInteger result)
    {
        callback((int)result);
    };
    
    [SuperfineSDKManager requestTrackingAuthorization:handler];
}

- (int)getTrackingAuthorizationStatus
{
    return (int)[SuperfineSDKManager getTrackingAuthorizationStatus];
}

- (void)requestNotificationAuthorization:(void(*)(bool, const char*))callback options:(int)options
{
    id handler = ^(BOOL granted, NSError * _Nullable error)
    {
        NSMutableDictionary* errorDict = [SuperfineSDKUnityUtility makeErrorDict:error];
        NSString* errorStr = (errorDict != NULL ? [SuperfineSDKManager serializeObject:errorDict] : NULL);
        
        callback(granted, [SuperfineSDKUnityUtility makeStringReturn:errorStr]);
    };
    
    [SuperfineSDKManager requestNotificationAuthorization:handler options:options];
}

- (void)requestNotificationAuthorization:(void(*)(bool, const char*))callback options:(int)options registerRemote:(bool)registerRemote
{
    id handler = ^(BOOL granted, NSError * _Nullable error)
    {
        NSMutableDictionary* errorDict = [SuperfineSDKUnityUtility makeErrorDict:error];
        NSString* errorStr = (errorDict != NULL ? [SuperfineSDKManager serializeObject:errorDict] : NULL);
        
        callback(granted, [SuperfineSDKUnityUtility makeStringReturn:errorStr]);
    };
    
    [SuperfineSDKManager requestNotificationAuthorization:handler options:options registerRemote:registerRemote];
}

- (void)registerForRemoteNotifications:(void(*)(const char*))callback
{
    id handler = ^(NSError * _Nullable error)
    {
        NSMutableDictionary* errorDict = [SuperfineSDKUnityUtility makeErrorDict:error];
        NSString* errorStr = (errorDict != NULL ? [SuperfineSDKManager serializeObject:errorDict] : NULL);
        
        callback([SuperfineSDKUnityUtility makeStringReturn:errorStr]);
    };
    
    [SuperfineSDKManager registerForRemoteNotifications:handler];
}

- (void)unregisterForRemoteNotifications
{
    [SuperfineSDKManager unregisterForRemoteNotifications];
}

- (BOOL)isRegisteredForRemoteNotifications
{
    return [SuperfineSDKManager isRegisteredForRemoteNotifications];
}

- (void)registerAppForAdNetworkAttribution
{
    [SuperfineSDKManager registerAppForAdNetworkAttribution];
}

- (void)updatePostbackConversionValue:(int)conversionValue
{
    [SuperfineSDKManager updatePostbackConversionValue:conversionValue];
}

- (void)updatePostbackConversionValue:(int)conversionValue coarseValue:(const char*) coarseValue
{
    [SuperfineSDKManager updatePostbackConversionValue:conversionValue coarseValue:[SuperfineSDKUnityUtility stringFromCString:coarseValue]];
}

- (void)updatePostbackConversionValue:(int)conversionValue coarseValue:(const char*) coarseValue lockWindow:(bool)lockWindow
{
    [SuperfineSDKManager updatePostbackConversionValue:conversionValue coarseValue:[SuperfineSDKUnityUtility stringFromCString:coarseValue] lockWindow:lockWindow];
}

@end

#pragma mark - Actual Unity C# interface (extern C)

extern "C"
{
    void SuperfineSDKInitialize(const char* params)
    {
        [[SuperfineSDKUnityInterface sharedInstance] initialize:params callback:NULL];
    }

    void SuperfineSDKInitialize2(const char* params, void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] initialize:params callback:callback];
    }

    int SuperfineSDKIsInitialized(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] isInitialized] ? 1 : 0;
    }

    void SuperfineSDKStart(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] start];
    }

    void SuperfineSDKStop(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] stop];
    }

    void SuperfineSDKShutdown(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] shutdown];
    }

    void SuperfineSDKSetOffline(bool value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setOffline:value];
    }

    void SuperfineSDKAddStartCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addStartCallback:callback];
    }

    void SuperfineSDKRemoveStartCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeStartCallback:callback];
    }

    void SuperfineSDKAddStopCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addStopCallback:callback];
    }

    void SuperfineSDKRemoveStopCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeStopCallback:callback];
    }

    void SuperfineSDKAddPauseCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addPauseCallback:callback];
    }

    void SuperfineSDKRemovePauseCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removePauseCallback:callback];
    }

    void SuperfineSDKAddResumeCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addResumeCallback:callback];
    }

    void SuperfineSDKRemoveResumeCallback(void (*callback)(void))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeResumeCallback:callback];
    }

    void SuperfineSDKAddDeepLinkCallback(void (*callback)(const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addDeepLinkCallback:callback];
    }

    void SuperfineSDKRemoveDeepLinkCallback(void (*callback)(const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeDeepLinkCallback:callback];
    }

    void SuperfineSDKAddDeviceTokenCallback(void (*callback)(const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addDeviceTokenCallback:callback];
    }

    void SuperfineSDKRemoveDeviceTokenCallback(void (*callback)(const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeDeviceTokenCallback:callback];
    }

    void SuperfineSDKAddSendEventCallback(void (*callback)(const char*, const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] addSendEventCallback:callback];
    }

    void SuperfineSDKRemoveSendEventCallback(void (*callback)(const char*, const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] removeSendEventCallback:callback];
    }

    void SuperfineSDKGdprForgetMe(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] gdprForgetMe];
    }

    void SuperfineSDKDisableThirdPartySharing(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] disableThirdPartySharing];
    }

    void SuperfineSDKEnableThirdPartySharing(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] enableThirdPartySharing];
    }

    void SuperfineSDKLogThirdPartySharing(const char* params)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logThirdPartySharing:params];
    }
    
    char* SuperfineSDKGetVersion(void)
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

    char* SuperfineSDKGetAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppId];
    }
    
    char* SuperfineSDKGetUserId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getUserId];
    }

    char* SuperfineSDKGetSessionId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getSessionId];
    }

    char* SuperfineSDKGetHost(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getHost];
    }

    char* SuperfineSDKGetConfigUrl(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getConfigUrl];
    }

    char* SuperfineSDKGetSdkConfig(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getSdkConfig];
    }

    int SuperfineSDKGetStoreType(void)
    {
        return (int)[[SuperfineSDKUnityInterface sharedInstance] getStoreType];
    }

    char* SuperfineGetFacebookAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getFacebookAppId];
    }

    char* SuperfineGetInstagramAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getInstagramAppId];
    }

    char* SuperfineSDKGetAppleAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleAppId];
    }

    char* SuperfineSDKGetAppleSignInClientId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleSignInClientId];
    }

    char* SuperfineSDKGetAppleDeveloperTeamId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getAppleDeveloperTeamId];
    }

    char* SuperfineSDKGetGooglePlayGameServicesProjectId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getGooglePlayGameServicesProjectId];
    }

    char* SuperfineSDKGetGooglePlayDeveloperAccountId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getGooglePlayDeveloperAccountId];
    }

    char* SuperfineSDKGetLinkedInAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getLinkedInAppId];
    }

    char* SuperfineSDKGetQQAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getQQAppId];
    }

    char* SuperfineSDKGetWeChatAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getWeChatAppId];
    }

    char* SuperfineSDKGetTikTokAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getTikTokAppId];
    }

    char* SuperfineSDKGetSnapAppId(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getSnapAppId];
    }

    char* SuperfineSDKGetDeepLinkUrl(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getDeepLinkUrl];
    }

    char* SuperfineSDKGetDeviceToken(void)
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

    int SuperfineSDKBeginLogEvent(const char* eventName)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] beginLogEvent:eventName];
    }

    void SuperfineSDKSetLogEventIntValue(int eventRef, int value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventValue:eventRef intValue:value];
    }

    void SuperfineSDKSetLogEventStringValue(int eventRef, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventValue:eventRef stringValue:value];
    }

    void SuperfineSDKSetLogEventMapValue(int eventRef, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventValue:eventRef mapValue:value];
    }

    void SuperfineSDKSetLogEventJsonValue(int eventRef, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventValue:eventRef jsonValue:value];
    }

    void SuperfineSDKSetLogEventFlag(int eventRef, int flag)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventFlag:eventRef flag:flag];
    }

    void SuperfineSDKSetLogEventRevenue(int eventRef, double revenue, const char* currency)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setLogEventRevenue:eventRef revenue:revenue currency:currency];
    }

    void SuperfineSDKEndLogEvent(int eventRef, bool cache)
    {
        [[SuperfineSDKUnityInterface sharedInstance] endLogEvent:eventRef cache:cache];
    }

    void SuperfineSDKLogBootStart(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logBootStart];
    }

    void SuperfineSDKLogBootEnd(void)
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

    void SuperfineSDKLogIAPRestorePurchase(void)
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

    void SuperfineSDKLogRateApp(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logRateApp];
    }

    void SuperfineSDKLogLocation(double latitude, double longitude)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLocation_latitude:latitude longitude:longitude];
    }

    void SuperfineSDKLogTrackingAuthorizationStatus(int status)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTrackingAuthorizationStatus:status];
    }

    void SuperfineSDKLogFacebookLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookLink:userId];
    }

    void SuperfineSDKLogFacebookUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookUnlink];
    }

    void SuperfineSDKLogInstagramLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logInstagramLink:userId];
    }

    void SuperfineSDKLogInstagramUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logInstagramUnlink];
    }

    void SuperfineSDKLogAppleLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleLink:userId];
    }

    void SuperfineSDKLogAppleUnlink(void)
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

    void SuperfineSDKLogAppleGameCenterUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAppleGameCenterUnlink];
    }

    void SuperfineSDKLogGoogleLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGoogleLink:userId];
    }

    void SuperfineSDKLogGoogleUnlink(void)
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

    void SuperfineSDKLogGooglePlayGameServicesUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGooglePlayGameServicesUnlink];
    }

    void SuperfineSDKLogLinkedInLink(const char* personId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLinkedInLink:personId];
    }

    void SuperfineSDKLogLinkedInUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLinkedInUnlink];
    }

    void SuperfineSDKLogMeetupLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMeetupLink:userId];
    }

    void SuperfineSDKLogMeetupUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMeetupUnlink];
    }

    void SuperfineSDKLogGitHubLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGitHubLink:userId];
    }

    void SuperfineSDKLogGitHubUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logGitHubUnlink];
    }

    void SuperfineSDKLogDiscordLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logDiscordLink:userId];
    }

    void SuperfineSDKLogDiscordUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logDiscordUnlink];
    }

    void SuperfineSDKLogTwitterLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTwitterLink:userId];
    }

    void SuperfineSDKLogTwitterUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTwitterUnlink];
    }

    void SuperfineSDKLogSpotifyLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logSpotifyLink:userId];
    }

    void SuperfineSDKLogSpotifyUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logSpotifyUnlink];
    }

    void SuperfineSDKLogMicrosoftLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMicrosoftLink:userId];
    }

    void SuperfineSDKLogMicrosoftUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logMicrosoftUnlink];
    }

    void SuperfineSDKLogLINELink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLINELink:userId];
    }

    void SuperfineSDKLogLINEUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logLINEUnlink];
    }

    void SuperfineSDKLogVKLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logVKLink:userId];
    }

    void SuperfineSDKLogVKUnlink(void)
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

    void SuperfineSDKLogQQUnlink(void)
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

    void SuperfineSDKLogWeChatUnlink(void)
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

    void SuperfineSDKLogTikTokUnlink(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logTikTokUnlink];
    }

    void SuperfineSDKLogWeiboLink(const char* userId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWeiboLink:userId];
    }

    void SuperfineSDKLogWeiboUnlink(void)
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

    int SuperfineSDKGetTrackingAuthorizationStatus(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getTrackingAuthorizationStatus];
    }

    void SuperfineSDKRequestNotificationAuthorization(void (*callback)(bool, const char*), int options)
    {
        [[SuperfineSDKUnityInterface sharedInstance] requestNotificationAuthorization:callback options:options];
    }

    void SuperfineSDKRequestNotificationAuthorization2(void (*callback)(bool, const char*), int options, bool registerRemote)
    {
        [[SuperfineSDKUnityInterface sharedInstance] requestNotificationAuthorization:callback options:options registerRemote:registerRemote];
    }

    void SuperfineSDKRegisterForRemoteNotifications(void (*callback)(const char*))
    {
        [[SuperfineSDKUnityInterface sharedInstance] registerForRemoteNotifications:callback];
    }

    void SuperfineSDKUnregisterForRemoteNotifications(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] unregisterForRemoteNotifications];
    }

    bool SuperfineSDKIsRegisteredForRemoteNotifications(void)
    {
        return [[SuperfineSDKUnityInterface sharedInstance] isRegisteredForRemoteNotifications];
    }

    void SuperfineSDKRegisterAppForAdNetworkAttribution(void)
    {
        [[SuperfineSDKUnityInterface sharedInstance] registerAppForAdNetworkAttribution];
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

