#import <SuperfineSDK/SuperfineSDK.h>

#import <Foundation/NSJSONSerialization.h>

#include "SuperfineSDKUnityInterface.h"
#include "SuperfineSDKUnityUtility.h"

@interface SuperfineSDKUnityInterface()

@property (nonatomic, assign) bool hasInited;

@end

@implementation SuperfineSDKUnityInterface

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
        self.hasInited = false;
    }
    
    return self;
}

- (void)configurationWithParams:(const char *)params
{
    if (self.hasInited)
    {
        NSLog(@"Superfine Tracking is already initialized");
        return;
    }
    
    NSData* jsonData = [[SuperfineSDKUnityUtility stringFromCString: params] dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError* e = nil;
    NSDictionary* data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
    
    if (e != nil)
    {
        NSLog(@"Error while parsing init parameters: %@", [e localizedDescription]);
        return;
    }
    
    NSString* appId = [data objectForKey:@"appId"];
    if (appId == nil)
    {
        NSLog(@"Missing appId when initializing");
        return;
    }
    
    NSString* appSecret = [data objectForKey:@"appSecret"];
    if (appSecret == nil)
    {
        NSLog(@"Missing appSecret when initializing");
        return;
    }
    
    self.hasInited = true;
    
    SuperfineSDKConfiguration *config = [SuperfineSDKConfiguration configurationWithAppId:appId appSecret:appSecret];
    
    {
        NSString* str = [data objectForKey:@"flushInterval"];
        if (str != nil)
        {
            config.flushInterval = [str longLongValue] / 1000.0;
        }
    }
    
    {
        NSString* str = [data objectForKey:@"flushQueueSize"];
        if (str != nil)
        {
            config.flushAt = [str intValue];
        }
    }
    
    {
        NSString* str = [data objectForKey:@"customUserId"];
        if (str != nil)
        {
            config.customUserId = str;
        }
    }
    
    {
        NSString* str = [data objectForKey:@"waitConfigId"];
        if (str != nil)
        {
            config.waitConfigId = [str boolValue];
        }
    }
    
    {
        NSString* str = [data objectForKey:@"host"];
        if (str != nil)
        {
            config.host = str;
        }
    }
    
    {
        NSString* str = [data objectForKey:@"debug"];
        if (str != nil)
        {
            config.debug = [str boolValue];
        }
    }
    
    {
        NSString* str = [data objectForKey:@"captureInAppPurchases"];
        if (str != nil)
        {
            config.captureInAppPurchases = [str boolValue];
        }
    }
    
    {
        NSString* str = [data objectForKey:@"autoStart"];
        if (str != nil)
        {
            config.autoStart = [str boolValue];
        }
    }
    
    {
        NSString* str = [data objectForKey:@"storeType"];
        if (str != nil)
        {
            config.storeType = (SuperfineSDKStoreType)[str intValue];
        }
    }
    
    {
        NSDictionary* tenjinObj = [data objectForKey:@"tenjin"];
        if (tenjinObj != nil)
        {
            NSString* sdkKey = [tenjinObj objectForKey:@"sdkKey"];
            if (sdkKey != nil)
            {
                SuperfineSDKTenjinConfiguration *tenjinConfig = [SuperfineSDKTenjinConfiguration configurationWithSdkKey:sdkKey];
                
                config.tenjinConfiguration = tenjinConfig;
                
                {
                    NSString* str = [tenjinObj objectForKey:@"deepLinkUrl"];
                    if (str != nil)
                    {
                        tenjinConfig.deepLinkUrl = str;
                    }
                }
                
                {
                    NSArray* arr = [tenjinObj objectForKey:@"optInParams"];
                    if (arr != nil)
                    {
                        tenjinConfig.optInParams = arr;
                    }
                }
                
                {
                    NSArray* arr = [tenjinObj objectForKey:@"optOutParams"];
                    if (arr != nil)
                    {
                        tenjinConfig.optOutParams = arr;
                    }
                }
                
                {
                    NSString* str = [tenjinObj objectForKey:@"appSubversion"];
                    if (str != nil)
                    {
                        tenjinConfig.appSubversion = [str intValue];
                    }
                }
                
                {
                    NSString* str = [tenjinObj objectForKey:@"cacheEventSetting"];
                    if (str != nil)
                    {
                        tenjinConfig.cacheEventSetting = [str boolValue];
                    }
                }
            }
        }
    }

    [SuperfineSDKManager setupWithConfiguration:config];
}

- (bool)assertInited
{
    if (!self.hasInited)
    {
        NSLog(@"Superfine Tracking isn't initialized");
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
    [SuperfineSDKManager shutdown];
}

- (void)setSendEventCallback:(void(*)(const char*, const char*, int))callback requestCode:(int)requestCode
{
    if (![self assertInited]) return;

    if (callback != NULL)
    {
      id handler = ^(const NSString* eventName, const NSString* eventData, int code)
      {
          callback([SuperfineSDKUnityUtility makeStringReturn:eventName],
                   [SuperfineSDKUnityUtility makeStringReturn:eventData],
                   code);
      };
      
      [SuperfineSDKManager sharedInstance].sendEventCallback = handler;
      [SuperfineSDKManager sharedInstance].sendEventRequestCode = requestCode;
    }
    else
    {
      [SuperfineSDKManager sharedInstance].sendEventCallback = NULL;
      [SuperfineSDKManager sharedInstance].sendEventRequestCode = requestCode;
    }
}

- (char*)getVersion
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[SuperfineSDKManager version]];
}

- (char*)getUserId
{
    if (![self assertInited]) return [SuperfineSDKUnityUtility makeStringReturn:@""];
    return [SuperfineSDKUnityUtility makeStringReturn:[[SuperfineSDKManager sharedInstance] getUserId]];
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

- (void)log:(const char*)eventName
{
    if (![self assertInited]) return;
    
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName]];
}

- (void)log:(const char*)eventName intValue:(int)value;
{
    if (![self assertInited]) return;

    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] intValue:value];
}

- (void)log:(const char*)eventName stringValue:(const char*)value;
{
    if (![self assertInited]) return;

    if (value != NULL)
    {
        [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] stringValue:[SuperfineSDKUnityUtility stringFromCString:value]];
    }
    else
    {
        [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName]];
    }
}

- (void)log:(const char*)eventName jsonValue:(const char*)value;
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
        
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] jsonValue:data];
}

- (void)log:(const char*)eventName mapValue:(const char*)value;
{
    if (![self assertInited]) return;
    
    NSMutableDictionary<NSString *, NSString *> *data = NULL;
    
    if (value != NULL)
    {
        NSString *str = [SuperfineSDKUnityUtility stringFromCString: value];
        
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
    
    [[SuperfineSDKManager sharedInstance] log:[SuperfineSDKUnityUtility stringFromCString:eventName] mapValue:data];
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

- (void)logIAPBuyStart_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPBuyStart_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency]];
}

- (void)logIAPBuyStart_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPBuyStart_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId] receipt:nil];
}

- (void)logIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPBuyEnd_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] isSuccess:isSuccess];
}

- (void)logIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPBuyEnd_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId] receipt:nil isSuccess:isSuccess];
}

- (void)logIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId receipt:(const char*)receipt isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logIAPBuyEnd_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] transactionId:[SuperfineSDKUnityUtility stringFromCString:transactionId] receipt:[SuperfineSDKUnityUtility stringFromCString:receipt] isSuccess:isSuccess];
}

- (void)logFacebookLogin:(const char*)facebookId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookLogin:[SuperfineSDKUnityUtility stringFromCString:facebookId]];
}

- (void)logFacebookLogout:(const char*)facebookId
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logFacebookLogout:[SuperfineSDKUnityUtility stringFromCString:facebookId]];
}

- (void)logUpdateGame:(const char*)newVersion
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logUpdateGame:[SuperfineSDKUnityUtility stringFromCString:newVersion]];
}

- (void)logRateGame
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logRateGame];
}

- (void)logAuthorizationTrackingStatus:(int)status
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAuthorizationTrackingStatus:(SuperfineSDKAuthorizationTrackingStatus)status];
}

- (void)logAccountLogin_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLogin_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logAccountLogout_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLogout_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logAccountLink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountLink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
}

- (void)logAccountUnlink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logAccountUnlink_id:[SuperfineSDKUnityUtility stringFromCString:accountId] type:[SuperfineSDKUnityUtility stringFromCString:type]];
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

- (void)logCryptoPayment_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency chain:(const char*)chain
{
    if (![self assertInited]) return;
    [[SuperfineSDKManager sharedInstance] logCryptoPayment_pack:[SuperfineSDKUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineSDKUnityUtility stringFromCString:currency] chain:[SuperfineSDKUnityUtility stringFromCString:chain]];
}

- (void)logAdRevenue_network:(const char*)network revenue:(float)revenue currency:(const char*)currency mediation:(const char*)mediation networkData:(const char*)networkData
{
    if (![self assertInited]) return;
    
    NSString* mediationStr = NULL;
    if (mediation != NULL)
    {
        mediationStr = [SuperfineSDKUnityUtility stringFromCString:mediation];
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
    
    [[SuperfineSDKManager sharedInstance] logAdRevenue_network:[SuperfineSDKUnityUtility stringFromCString:network] revenue:revenue currency:[SuperfineSDKUnityUtility stringFromCString:currency] mediation:mediationStr networkData:data];
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
        [[SuperfineSDKUnityInterface sharedInstance] configurationWithParams:params];
    }

    void SuperfineSDKStart()
    {
        [[SuperfineSDKUnityInterface sharedInstance] start];
    }

    void SuperfineSDKStop()
    {
        [[SuperfineSDKUnityInterface sharedInstance] stop];
    }

    void SuperfineSDKSetSendEventCallback(void (*callback)(const char*, const char*, int))
    {
        [[SuperfineSDKUnityInterface sharedInstance] setSendEventCallback:callback requestCode:0];
    }

    void SuperfineSDKSetSendEventCallback2(void (*callback)(const char*, const char*, int), int requestCode)
    {
        [[SuperfineSDKUnityInterface sharedInstance] setSendEventCallback:callback requestCode:requestCode];
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
    
    char* SuperfineSDKGetUserId()
    {
        return [[SuperfineSDKUnityInterface sharedInstance] getUserId];
    }

    void SuperfineSDKLog(const char* eventName)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName];
    }

    void SuperfineSDKLogWithIntValue(const char* eventName, int value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName intValue:value];
    }

    void SuperfineSDKLogWithStringValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName stringValue:value];
    }

    void SuperfineSDKLogWithJsonValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName jsonValue:value];
    }

    void SuperfineSDKLogWithMapValue(const char* eventName, const char* value)
    {
        [[SuperfineSDKUnityInterface sharedInstance] log:eventName mapValue:value];
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

    void SuperfineSDKLogIAPBuyStart(const char* pack, float price, int amount, const char* currency)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPBuyStart_pack:pack price:price amount:amount currency:currency];
    }

    void SuperfineSDKLogIAPBuyStart2(const char* pack, float price, int amount, const char* currency, const char* transactionId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPBuyStart_pack:pack price:price amount:amount currency:currency transactionId:transactionId];
    }

    void SuperfineSDKLogIAPBuyEnd(const char* pack, float price, int amount, const char* currency, bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPBuyEnd_pack:pack price:price amount:amount currency:currency isSuccess:isSuccess];
    }

    void SuperfineSDKLogIAPBuyEnd2(const char* pack, float price, int amount, const char* currency, const char* transactionId, bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPBuyEnd_pack:pack price:price amount:amount currency:currency transactionId:transactionId isSuccess:isSuccess];
    }

    void SuperfineSDKLogIAPBuyEnd3(const char* pack, float price, int amount, const char* currency, const char* transactionId, const char* receipt, bool isSuccess)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logIAPBuyEnd_pack:pack price:price amount:amount currency:currency transactionId:transactionId receipt:receipt isSuccess:isSuccess];
    }

    void SuperfineSDKLogFacebookLogin(const char* facebookId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookLogin:facebookId];
    }

    void SuperfineSDKLogFacebookLogout(const char* facebookId)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logFacebookLogout:facebookId];
    }

    void SuperfineSDKLogUpdateGame(const char* newVersion)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logUpdateGame:newVersion];
    }

    void SuperfineSDKLogRateGame()
    {
        [[SuperfineSDKUnityInterface sharedInstance] logRateGame];
    }

    void SuperfineSDKLogAuthorizationTrackingStatus(int status)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAuthorizationTrackingStatus:status];
    }

    void SuperfineSDKLogAccountLogin(const char* accountId, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLogin_id:accountId type:type];
    }

    void SuperfineSDKLogAccountLogout(const char* accountId, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLogout_id:accountId type:type];
    }

    void SuperfineSDKLogAccountLink(const char* accountId, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountLink_id:accountId type:type];
    }

    void SuperfineSDKLogAccountUnlink(const char* accountId, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAccountUnlink_id:accountId type:type];
    }

    void SuperfineSDKLogWalletLink(const char* wallet, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWalletLink_wallet:wallet type:type];
    }

    void SuperfineSDKLogWalletUnlink(const char* wallet, const char* type)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logWalletUnlink_wallet:wallet type:type];
    }

    void SuperfineSDKLogCryptoPayment(const char* pack, float price, int amount, const char* currency, const char* chain)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logCryptoPayment_pack:pack price:price amount:amount currency:currency chain:chain];
    }

    void SuperfineSDKLogAdRevenue(const char* network, float revenue, const char* currency)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_network:network revenue:revenue currency:currency mediation:NULL networkData:NULL];
    }

    void SuperfineSDKLogAdRevenue2(const char* network, float revenue, const char* currency, const char* mediation)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_network:network revenue:revenue currency:currency mediation:mediation networkData:NULL];
    }

    void SuperfineSDKLogAdRevenue3(const char* network, float revenue, const char* currency, const char* mediation, const char* networkData)
    {
        [[SuperfineSDKUnityInterface sharedInstance] logAdRevenue_network:network revenue:revenue currency:currency mediation:mediation networkData:networkData];
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

