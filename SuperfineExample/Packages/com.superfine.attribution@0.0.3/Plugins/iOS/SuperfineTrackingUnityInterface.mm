#import <SuperfineTracking/SuperfineTracking.h>

#import <Foundation/NSJSONSerialization.h>

#include "SuperfineTrackingUnityInterface.h"
#include "SuperfineTrackingUnityUtility.h"

@interface SuperfineTrackingUnityInterface()

@property (nonatomic, assign) bool hasInited;

@end

@implementation SuperfineTrackingUnityInterface

#pragma mark Object Initialization

+ (SuperfineTrackingUnityInterface *)sharedInstance
{
  static dispatch_once_t pred;
  static SuperfineTrackingUnityInterface *shared = nil;

  dispatch_once(&pred, ^{
      shared = [[SuperfineTrackingUnityInterface alloc] init];
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
    
    NSData* jsonData = [[SuperfineTrackingUnityUtility stringFromCString: params] dataUsingEncoding:NSUTF8StringEncoding];
    
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
    
    SuperfineTrackingConfiguration *config = [SuperfineTrackingConfiguration configurationWithAppId:appId appSecret:appSecret];
    
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
            config.customUserId = [str boolValue];
        }
    }
        
    {
        NSString* str = [data objectForKey:@"userId"];
        if (str != nil)
        {
            config.userId = str;
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
            config.storeType = (SuperfineTrackingStoreType)[str intValue];
        }
    }
    
    {
        NSDictionary* tenjinObj = [data objectForKey:@"tenjin"];
        if (tenjinObj != nil)
        {
            NSString* sdkKey = [tenjinObj objectForKey:@"sdkKey"];
            if (sdkKey != nil)
            {
                SuperfineTrackingTenjinConfiguration *tenjinConfig = [SuperfineTrackingTenjinConfiguration configurationWithSdkKey:sdkKey];
                
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

    [SuperfineTrackingManager setupWithConfiguration:config];
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
    [[SuperfineTrackingManager sharedTrackingManager] start];
}

- (void)stop
{
    if (![self assertInited]) return;
    [SuperfineTrackingManager shutdown];
}

- (char*)getVersion
{
    if (![self assertInited]) return [SuperfineTrackingUnityUtility makeStringReturn:@""];
    return [SuperfineTrackingUnityUtility makeStringReturn:[SuperfineTrackingManager version]];
}

- (char*)getUserId
{
    if (![self assertInited]) return [SuperfineTrackingUnityUtility makeStringReturn:@""];
    return [SuperfineTrackingUnityUtility makeStringReturn:[[SuperfineTrackingManager sharedTrackingManager] getUserId]];
}

- (void)setConfigId:(const char*)configId
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] setConfigId:[SuperfineTrackingUnityUtility stringFromCString:configId]];
}

- (void)setUserId:(const char*)userId
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] setUserId:[SuperfineTrackingUnityUtility stringFromCString:userId]];
}

- (void)track:(const char*)eventName
{
    if (![self assertInited]) return;
    
    [[SuperfineTrackingManager sharedTrackingManager] track:[SuperfineTrackingUnityUtility stringFromCString:eventName]];
}

- (void)track:(const char*)eventName intValue:(int)value;
{
    if (![self assertInited]) return;

    [[SuperfineTrackingManager sharedTrackingManager] track:[SuperfineTrackingUnityUtility stringFromCString:eventName] intValue:value];
}

- (void)track:(const char*)eventName stringValue:(const char*)value;
{
    if (![self assertInited]) return;

    [[SuperfineTrackingManager sharedTrackingManager] track:[SuperfineTrackingUnityUtility stringFromCString:eventName] stringValue:[SuperfineTrackingUnityUtility stringFromCString:value]];
}


- (void)track:(const char*)eventName jsonValue:(const char*)value;
{
    if (![self assertInited]) return;

    NSData* jsonData = [[SuperfineTrackingUnityUtility stringFromCString: value] dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError* e = nil;
    NSDictionary* data = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&e];
    
    if (e != nil)
    {
        NSLog(@"Error while parsing track parameters: %@", [e localizedDescription]);
        return;
    }
    
    [[SuperfineTrackingManager sharedTrackingManager] track:[SuperfineTrackingUnityUtility stringFromCString:eventName] jsonValue:data];
}

- (void)trackBootStart
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackBootStart];
}

- (void)trackBootEnd
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackBootEnd];
}

- (void)trackLevelStart_id:(int)levelId name:(const char*)name
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackLevelStart_id:levelId name:[SuperfineTrackingUnityUtility stringFromCString:name]];
}

- (void)trackLevelEnd_id:(int)levelId name:(const char*)name isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackLevelEnd_id:levelId name:[SuperfineTrackingUnityUtility stringFromCString:name] isSuccess:isSuccess];
}

- (void)trackAdLoad_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAdLoad_adUnit:[SuperfineTrackingUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement];
}

- (void)trackAdClose_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAdClose_adUnit:[SuperfineTrackingUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement];
}

- (void)trackAdClick_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAdClick_adUnit:[SuperfineTrackingUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement];
}

- (void)trackAdImpression_adUnit:(const char*)adUnit adPlacementType:(int)adPlacementType adPlacement:(int)adPlacement
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAdImpression_adUnit:[SuperfineTrackingUnityUtility stringFromCString:adUnit] adPlacementType:(SuperfineTrackingAdPlacementType)adPlacementType adPlacement:(SuperfineTrackingAdPlacement)adPlacement];
}

- (void)trackIAPInitialization:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPInitialization:isSuccess];
}

- (void)trackIAPRestorePurchase
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPRestorePurchase];
}

- (void)trackIAPBuyStart_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyStart_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency]];
}

- (void)trackIAPBuyStart_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyStart_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] transactionId:[SuperfineTrackingUnityUtility stringFromCString:transactionId] receipt:nil];
}

- (void)trackIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyEnd_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] isSuccess:isSuccess];
}

- (void)trackIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyEnd_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] transactionId:[SuperfineTrackingUnityUtility stringFromCString:transactionId] receipt:nil isSuccess:isSuccess];
}

- (void)trackIAPBuyEnd_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency transactionId:(const char*)transactionId receipt:(const char*)receipt isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyEnd_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] transactionId:[SuperfineTrackingUnityUtility stringFromCString:transactionId] receipt:[SuperfineTrackingUnityUtility stringFromCString:receipt] isSuccess:isSuccess];
}

- (void)trackFacebookLogin:(const char*)facebookId
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackFacebookLogin:[SuperfineTrackingUnityUtility stringFromCString:facebookId]];
}

- (void)trackFacebookLogout:(const char*)facebookId
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackFacebookLogout:[SuperfineTrackingUnityUtility stringFromCString:facebookId]];
}

- (void)trackUpdateGame:(const char*)newVersion
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackUpdateGame:[SuperfineTrackingUnityUtility stringFromCString:newVersion]];
}

- (void)trackRateGame
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackRateGame];
}

- (void)trackAuthorizationTrackingStatus:(int)status
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAuthorizationTrackingStatus:(SuperfineTrackingAuthorizationTrackingStatus)status];
}

- (void)trackAccountLogin_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAccountLogin_id:[SuperfineTrackingUnityUtility stringFromCString:accountId] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackAccountLogout_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAccountLogout_id:[SuperfineTrackingUnityUtility stringFromCString:accountId] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackAccountLink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAccountLink_id:[SuperfineTrackingUnityUtility stringFromCString:accountId] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackAccountUnlink_id:(const char*)accountId type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackAccountUnlink_id:[SuperfineTrackingUnityUtility stringFromCString:accountId] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackWalletLink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackWalletLink_wallet:[SuperfineTrackingUnityUtility stringFromCString:wallet] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackWalletUnlink_wallet:(const char*)wallet type:(const char*)type
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackWalletUnlink_wallet:[SuperfineTrackingUnityUtility stringFromCString:wallet] type:[SuperfineTrackingUnityUtility stringFromCString:type]];
}

- (void)trackCryptoPayment_pack:(const char*)pack price:(float)price amount:(int)amount currency:(const char*)currency chain:(const char*)chain
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackCryptoPayment_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:price amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] chain:[SuperfineTrackingUnityUtility stringFromCString:chain]];
}

@end

#pragma mark - Actual Unity C# interface (extern C)

extern "C"
{
    void SuperfineTrackingInit(const char* params)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] configurationWithParams:params];
    }

    void SuperfineTrackingStart()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] start];
    }

    void SuperfineTrackingStop()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] stop];
    }
    
    char* SuperfineTrackingGetVersion()
    {
        return [[SuperfineTrackingUnityInterface sharedInstance] getVersion];
    }

    void SuperfineTrackingSetConfigId(const char* configId)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] setConfigId:configId];
    }

    void SuperfineTrackingSetUserId(const char* userId)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] setUserId:userId];
    }
    
    char* SuperfineTrackingGetUserId()
    {
        return [[SuperfineTrackingUnityInterface sharedInstance] getUserId];
    }

    void SuperfineTrackingTrack(const char* eventName)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] track:eventName];
    }

    void SuperfineTrackingTrackWithIntValue(const char* eventName, int value)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] track:eventName intValue:value];
    }

    void SuperfineTrackingTrackWithStringValue(const char* eventName, const char* value)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] track:eventName stringValue:value];
    }

    void SuperfineTrackingTrackWithJsonValue(const char* eventName, const char* value)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] track:eventName jsonValue:value];
    }

    void SuperfineTrackingTrackBootStart()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackBootStart];
    }

    void SuperfineTrackingTrackBootEnd()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackBootEnd];
    }

    void SuperfineTrackingTrackLevelStart(int levelId, const char* name)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackLevelStart_id:levelId name:name];
    }

    void SuperfineTrackingTrackLevelEnd(int levelId, const char* name, bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackLevelEnd_id:levelId name:name isSuccess:isSuccess];
    }

    void SuperfineTrackingTrackAdLoad(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAdLoad_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineTrackingTrackAdClose(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAdClose_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineTrackingTrackAdClick(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAdClick_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineTrackingTrackAdImpression(const char* adUnit, int adPlacementType, int adPlacement)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAdImpression_adUnit:adUnit adPlacementType:adPlacementType adPlacement:adPlacement];
    }

    void SuperfineTrackingTrackIAPInitialization(bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPInitialization:isSuccess];
    }

    void SuperfineTrackingTrackIAPRestorePurchase()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPRestorePurchase];
    }

    void SuperfineTrackingTrackIAPBuyStart(const char* pack, float price, int amount, const char* currency)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyStart_pack:pack price:price amount:amount currency:currency];
    }

    void SuperfineTrackingTrackIAPBuyStart2(const char* pack, float price, int amount, const char* currency, const char* transactionId)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyStart_pack:pack price:price amount:amount currency:currency transactionId:transactionId];
    }

    void SuperfineTrackingTrackIAPBuyEnd(const char* pack, float price, int amount, const char* currency, bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyEnd_pack:pack price:price amount:amount currency:currency isSuccess:isSuccess];
    }

    void SuperfineTrackingTrackIAPBuyEnd2(const char* pack, float price, int amount, const char* currency, const char* transactionId, bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyEnd_pack:pack price:price amount:amount currency:currency transactionId:transactionId isSuccess:isSuccess];
    }

    void SuperfineTrackingTrackIAPBuyEnd3(const char* pack, float price, int amount, const char* currency, const char* transactionId, const char* receipt, bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyEnd_pack:pack price:price amount:amount currency:currency transactionId:transactionId receipt:receipt isSuccess:isSuccess];
    }

    void SuperfineTrackingTrackFacebookLogin(const char* facebookId)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackFacebookLogin:facebookId];
    }

    void SuperfineTrackingTrackFacebookLogout(const char* facebookId)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackFacebookLogout:facebookId];
    }

    void SuperfineTrackingTrackUpdateGame(const char* newVersion)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackUpdateGame:newVersion];
    }

    void SuperfineTrackingTrackRateGame()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackRateGame];
    }

    void SuperfineTrackingTrackAuthorizationTrackingStatus(int status)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAuthorizationTrackingStatus:status];
    }

    void SuperfineTrackingTrackAccountLogin(const char* accountId, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAccountLogin_id:accountId type:type];
    }

    void SuperfineTrackingTrackAccountLogout(const char* accountId, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAccountLogout_id:accountId type:type];
    }

    void SuperfineTrackingTrackAccountLink(const char* accountId, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAccountLink_id:accountId type:type];
    }

    void SuperfineTrackingTrackAccountUnlink(const char* accountId, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackAccountUnlink_id:accountId type:type];
    }

    void SuperfineTrackingTrackWalletLink(const char* wallet, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackWalletLink_wallet:wallet type:type];
    }

    void SuperfineTrackingTrackWalletUnlink(const char* wallet, const char* type)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackWalletUnlink_wallet:wallet type:type];
    }

    void SuperfineTrackingTrackCryptoPayment(const char* pack, float price, int amount, const char* currency, const char* chain)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackCryptoPayment_pack:pack price:price amount:amount currency:currency chain:chain];
    }
}

