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

    config.debug = true;
    
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

- (char*)getVersion
{
    if (![self assertInited]) return [SuperfineTrackingUnityUtility makeStringReturn:@""];
    return [SuperfineTrackingUnityUtility makeStringReturn:[SuperfineTrackingManager version]];
}

- (char *)getUserId
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

- (void)track:(const char*)eventName value:(const char*)value;
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
    
    [[SuperfineTrackingManager sharedTrackingManager] track:[SuperfineTrackingUnityUtility stringFromCString:eventName] value:data];
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

- (void)trackIAPBuyStart_pack:(const char*)pack price:(const char*)price amount:(float)amount currency:(const char*)currency
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyStart_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:[SuperfineTrackingUnityUtility stringFromCString:price] amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency]];
}

- (void)trackIAPBuyEnd_pack:(const char*)pack price:(const char*)price amount:(float)amount currency:(const char*)currency isSuccess:(bool)isSuccess
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackIAPBuyEnd_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:[SuperfineTrackingUnityUtility stringFromCString:price] amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] isSuccess:isSuccess];
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

- (void)trackRateGame:(int)storeType
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackRateGame:(SuperfineTrackingStoreType)storeType];
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

- (void)trackCryptoPayment_pack:(const char*)pack price:(const char*)price amount:(float)amount currency:(const char*)currency type:(const char*)type count:(int)count
{
    if (![self assertInited]) return;
    [[SuperfineTrackingManager sharedTrackingManager] trackCryptoPayment_pack:[SuperfineTrackingUnityUtility stringFromCString:pack] price:[SuperfineTrackingUnityUtility stringFromCString:price] amount:amount currency:[SuperfineTrackingUnityUtility stringFromCString:currency] type:[SuperfineTrackingUnityUtility stringFromCString:type] count:count];
}

@end

#pragma mark - Actual Unity C# interface (extern C)

extern "C"
{
    void SuperfineTrackingInit(const char* params)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] configurationWithParams:params];
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

    void SuperfineTrackingTrackWithValue(const char* eventName, const char* jsonValue)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] track:eventName value:jsonValue];
    }

    void SuperfineTrackingTrackBootStart()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackBootStart];
    }

    void SuperfineTrackingTrackBootEnd()
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackBootStart];
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

    void SuperfineTrackingTrackIAPBuyStart(const char* pack, const char* price, float amount, const char* currency)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyStart_pack:pack price:price amount:amount currency:currency];
    }

    void SuperfineTrackingTrackIAPBuyEnd(const char* pack, const char* price, float amount, const char* currency, bool isSuccess)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackIAPBuyEnd_pack:pack price:price amount:amount currency:currency isSuccess:isSuccess];
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

    void SuperfineTrackingTrackRateGame(int storeType)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackRateGame:storeType];
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

    void SuperfineTrackingTrackCryptoPayment(const char* pack, const char* price, float amount, const char* currency, const char* type, int count)
    {
        [[SuperfineTrackingUnityInterface sharedInstance] trackCryptoPayment_pack:pack price:price amount:amount currency:currency type:type count:count];
    }
}

