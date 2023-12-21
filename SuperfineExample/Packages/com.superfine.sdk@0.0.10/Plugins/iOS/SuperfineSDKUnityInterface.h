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
void SuperfineSDKLogIAPResult(const char* pack, double price, int amount, const char* currency, bool isSuccess);
void SuperfineSDKLogIAPReceipt_Apple(const char* receipt);
void SuperfineSDKLogIAPReceipt_Google(const char* data, const char* signature);
void SuperfineSDKLogIAPReceipt_Amazon(const char* userId, const char* receiptId);
void SuperfineSDKLogIAPReceipt_Roku(const char* transactionId);
void SuperfineSDKLogIAPReceipt_Windows(const char* receipt);
void SuperfineSDKLogIAPReceipt_Facebook(const char* receipt);
void SuperfineSDKLogIAPReceipt_Unity(const char* receipt);
void SuperfineSDKLogIAPReceipt_AppStoreServer(const char* transactionId);
void SuperfineSDKLogIAPReceipt_GooglePlayProduct(const char* productId, const char* token);
void SuperfineSDKLogIAPReceipt_GooglePlaySubscription(const char* subscriptionId, const char* token);
void SuperfineSDKLogIAPReceipt_GooglePlaySubscriptionv2(const char* token);
void SuperfineSDKLogFacebookLogin(const char* facebookId);
void SuperfineSDKLogFacebookLogout(const char* facebookId);
void SuperfineSDKLogUpdateApp(const char* newVersion);
void SuperfineSDKLogRateApp();
void SuperfineSDKLogLocation(double latitude, double longitude);
void SuperfineSDKLogAuthorizationTrackingStatus(int status);
void SuperfineSDKLogAccountLogin(const char* accountId, const char* type);
void SuperfineSDKLogAccountLogout(const char* accountId, const char* type);
void SuperfineSDKLogAccountLink(const char* accountId, const char* type);
void SuperfineSDKLogAccountUnlink(const char* accountId, const char* type);
void SuperfineSDKLogWalletLink(const char* wallet, const char* type);
void SuperfineSDKLogWalletUnlink(const char* wallet, const char* type);
void SuperfineSDKLogCryptoPayment(const char* pack, double price, int amount, const char* currency, const char* chain);
void SuperfineSDKLogAdRevenue(const char* network, double revenue, const char* currency);
void SuperfineSDKLogAdRevenue2(const char* network, double revenue, const char* currency, const char* mediation);
void SuperfineSDKLogAdRevenue3(const char* network, double revenue, const char* currency, const char* mediation, const char* networkData);

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
