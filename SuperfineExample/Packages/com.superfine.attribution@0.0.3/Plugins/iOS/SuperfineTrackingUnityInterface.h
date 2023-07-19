#import <UIKit/UIKit.h>

#ifndef NON_UNITY
#import "AppDelegateListener.h"

//if we are on a version of unity that has the version number defined use it, otherwise we have added it ourselves in the post build step
#if HAS_UNITY_VERSION_DEF
#include "UnityTrampolineConfigure.h"
#endif
#endif

@interface SuperfineTrackingUnityInterface :
#ifndef NON_UNITY
    NSObject <AppDelegateListener>
#else
    NSObject
#endif
{
}

+ (SuperfineTrackingUnityInterface *)sharedInstance;
@end

#ifdef __cplusplus
extern "C"
{
#endif
void SuperfineTrackingInit(const char* params);
void SuperfineTrackingStart();
void SuperfineTrackingStop();
char* SuperfineTrackingGetVersion();
void SuperfineTrackingSetConfigId(const char* configId);
void SuperfineTrackingSetUserId(const char* userId);
char* SuperfineTrackingGetUserId();
void SuperfineTrackingTrack(const char* eventName);
void SuperfineTrackingTrackWithIntValue(const char* eventName, int value);
void SuperfineTrackingTrackWithStringValue(const char* eventName, const char* value);
void SuperfineTrackingTrackWithJsonValue(const char* eventName, const char* value);
void SuperfineTrackingTrackBootStart();
void SuperfineTrackingTrackBootEnd();
void SuperfineTrackingTrackLevelStart(int levelId, const char* name);
void SuperfineTrackingTrackLevelEnd(int levelId, const char* name, bool isSuccess);
void SuperfineTrackingTrackAdLoad(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineTrackingTrackAdClose(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineTrackingTrackAdClick(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineTrackingTrackAdImpression(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineTrackingTrackIAPInitialization(bool isSuccess);
void SuperfineTrackingTrackIAPRestorePurchase();
void SuperfineTrackingTrackIAPBuyStart(const char* pack, float price, int amount, const char* currency);
void SuperfineTrackingTrackIAPBuyStart2(const char* pack, float price, int amount, const char* currency, const char* transactionId);
void SuperfineTrackingTrackIAPBuyEnd(const char* pack, float price, int amount, const char* currency, bool isSuccess);
void SuperfineTrackingTrackIAPBuyEnd2(const char* pack, float price, int amount, const char* currency, const char* transactionId, bool isSuccess);
void SuperfineTrackingTrackIAPBuyEnd3(const char* pack, float price, int amount, const char* currency, const char* transactionId, const char* receipt, bool isSuccess);
void SuperfineTrackingTrackFacebookLogin(const char* facebookId);
void SuperfineTrackingTrackFacebookLogout(const char* facebookId);
void SuperfineTrackingTrackUpdateGame(const char* newVersion);
void SuperfineTrackingTrackRateGame();
void SuperfineTrackingTrackAuthorizationTrackingStatus(int status);
void SuperfineTrackingTrackAccountLogin(const char* accountId, const char* type);
void SuperfineTrackingTrackAccountLogout(const char* accountId, const char* type);
void SuperfineTrackingTrackAccountLink(const char* accountId, const char* type);
void SuperfineTrackingTrackAccountUnlink(const char* accountId, const char* type);
void SuperfineTrackingTrackWalletLink(const char* wallet, const char* type);
void SuperfineTrackingTrackWalletUnlink(const char* wallet, const char* type);
void SuperfineTrackingTrackCryptoPayment(const char* pack, float price, int amount, const char* currency, const char* chain);

void SuperfineTrackingRequestTrackingAuthorization(void (*callback)(int));
int SuperfineTrackingGetTrackingAuthorizationStatus();

void SuperfineTrackingUpdatePostbackConversionValue(int conversionValue);
void SuperfineTrackingUpdatePostbackConversionValue2(int conversionValue, const char* coarseValue);
void SuperfineTrackingUpdatePostbackConversionValue3(int conversionValue, const char* coarseValue, bool lockWindow);

#ifdef __cplusplus
}
#endif
