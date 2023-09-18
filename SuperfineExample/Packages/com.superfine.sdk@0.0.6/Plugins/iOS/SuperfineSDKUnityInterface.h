#import <UIKit/UIKit.h>

#ifndef NON_UNITY
#import "AppDelegateListener.h"

//if we are on a version of unity that has the version number defined use it, otherwise we have added it ourselves in the post build step
#if HAS_UNITY_VERSION_DEF
#include "UnityTrampolineConfigure.h"
#endif
#endif

@interface SuperfineSDKUnityInterface :
#ifndef NON_UNITY
    NSObject <AppDelegateListener>
#else
    NSObject
#endif
{
}

+ (SuperfineSDKUnityInterface *)sharedInstance;
@end

#ifdef __cplusplus
extern "C"
{
#endif
void SuperfineSDKInit(const char* params);
void SuperfineSDKStart();
void SuperfineSDKStop();
void SuperfineSDKShutdown();
void SuperfineSDKSetSendEventCallback(void (*callback)(const char*, const char*, int));
void SuperfineSDKSetSendEventCallback2(void (*callback)(const char*, const char*, int), int requestCode);
void SuperfineSDKGdprForgetMe();
void SuperfineSDKDisableThirdPartySharing();
void SuperfineSDKEnableThirdPartySharing();
void SuperfineSDKLogThirdPartySharing(const char* params);
char* SuperfineSDKGetVersion();
void SuperfineSDKSetConfigId(const char* configId);
void SuperfineSDKSetCustomUserId(const char* userId);
char* SuperfineSDKGetUserId();
void SuperfineSDKLog(const char* eventName);
void SuperfineSDKLogWithIntValue(const char* eventName, int value);
void SuperfineSDKLogWithStringValue(const char* eventName, const char* value);
void SuperfineSDKLogWithJsonValue(const char* eventName, const char* value);
void SuperfineSDKLogWithMapValue(const char* eventName, const char* value);
void SuperfineSDKLogBootStart();
void SuperfineSDKLogBootEnd();
void SuperfineSDKLogLevelStart(int levelId, const char* name);
void SuperfineSDKLogLevelEnd(int levelId, const char* name, bool isSuccess);
void SuperfineSDKLogAdLoad(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdClose(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdClick(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdImpression(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogIAPInitialization(bool isSuccess);
void SuperfineSDKLogIAPRestorePurchase();
void SuperfineSDKLogIAPBuy(const char* pack, float price, int amount, const char* currency, const char* transactionId, const char* receipt);
void SuperfineSDKLogIAPResult(const char* pack, float price, int amount, const char* currency, bool isSuccess);
void SuperfineSDKLogFacebookLogin(const char* facebookId);
void SuperfineSDKLogFacebookLogout(const char* facebookId);
void SuperfineSDKLogUpdateGame(const char* newVersion);
void SuperfineSDKLogRateGame();
void SuperfineSDKLogLocation(double latitude, double longitude);
void SuperfineSDKLogAuthorizationTrackingStatus(int status);
void SuperfineSDKLogAccountLogin(const char* accountId, const char* type);
void SuperfineSDKLogAccountLogout(const char* accountId, const char* type);
void SuperfineSDKLogAccountLink(const char* accountId, const char* type);
void SuperfineSDKLogAccountUnlink(const char* accountId, const char* type);
void SuperfineSDKLogWalletLink(const char* wallet, const char* type);
void SuperfineSDKLogWalletUnlink(const char* wallet, const char* type);
void SuperfineSDKLogCryptoPayment(const char* pack, float price, int amount, const char* currency, const char* chain);
void SuperfineSDKLogAdRevenue(const char* network, float revenue, const char* currency);
void SuperfineSDKLogAdRevenue2(const char* network, float revenue, const char* currency, const char* mediation);
void SuperfineSDKLogAdRevenue3(const char* network, float revenue, const char* currency, const char* mediation, const char* networkData);

void SuperfineSDKOpenURL(const char* url);

void SuperfineSDKSetDeviceToken(const char* token);

void SuperfineSDKRequestTrackingAuthorization(void (*callback)(int));
int SuperfineSDKGetTrackingAuthorizationStatus();

void SuperfineSDKUpdatePostbackConversionValue(int conversionValue);
void SuperfineSDKUpdatePostbackConversionValue2(int conversionValue, const char* coarseValue);
void SuperfineSDKUpdatePostbackConversionValue3(int conversionValue, const char* coarseValue, bool lockWindow);

#ifdef __cplusplus
}
#endif
