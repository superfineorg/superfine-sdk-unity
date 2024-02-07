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
void SuperfineSDKNotifyInit();
void SuperfineSDKStart();
void SuperfineSDKStop();
void SuperfineSDKShutdown();
void SuperfineSDKSetOffline(bool value);
void SuperfineSDKSetStartCallback(void (*callback)(int), int requestCode);
void SuperfineSDKRemoveStartCallback(int requestCode);
void SuperfineSDKSetStopCallback(void (*callback)(int), int requestCode);
void SuperfineSDKRemoveStopCallback(int requestCode);
void SuperfineSDKSetPauseCallback(void (*callback)(int), int requestCode);
void SuperfineSDKRemovePauseCallback(int requestCode);
void SuperfineSDKSetResumeCallback(void (*callback)(int), int requestCode);
void SuperfineSDKRemoveResumeCallback(int requestCode);
void SuperfineSDKSetDeepLinkCallback(void (*callback)(const char*, int), int requestCode);
void SuperfineSDKRemoveDeepLinkCallback(int requestCode);
void SuperfineSDKSetDeviceTokenCallback(void (*callback)(const char*, int), int requestCode);
void SuperfineSDKRemoveDeviceTokenCallback(int requestCode);
void SuperfineSDKSetSendEventCallback(void (*callback)(const char*, const char*, int), int requestCode);
void SuperfineSDKRemoveSendEventCallback(int requestCode);
void SuperfineSDKGdprForgetMe();
void SuperfineSDKDisableThirdPartySharing();
void SuperfineSDKEnableThirdPartySharing();
void SuperfineSDKLogThirdPartySharing(const char* params);
char* SuperfineSDKGetVersion();
void SuperfineSDKSetConfigId(const char* configId);
void SuperfineSDKSetCustomUserId(const char* userId);
char* SuperfineSDKGetAppId();
char* SuperfineSDKGetUserId();
char* SuperfineSDKGetSessionId();
char* SuperfineSDKGetHost();
int SuperfineSDKGetStoreType();
char* SuperfineGetFacebookAppId();
char* SuperfineGetInstagramAppId();
char* SuperfineSDKGetAppleAppId();
char* SuperfineSDKGetAppleSignInClientId();
char* SuperfineSDKGetAppleDeveloperTeamId();
char* SuperfineSDKGetGooglePlayGameServicesProjectId();
char* SuperfineSDKGetGooglePlayDeveloperAccountId();
char* SuperfineSDKGetLinkedInAppId();
char* SuperfineSDKGetQQAppId();
char* SuperfineSDKGetWeChatAppId();
char* SuperfineSDKGetTikTokAppId();
char* SuperfineSDKGetDeepLinkUrl();
char* SuperfineSDKGetDeviceToken();
void SuperfineSDKFetchRemoteConfig(void (*callback)(const char* data));
void SuperfineSDKLog(const char* eventName);
void SuperfineSDKLogWithFlag(const char* eventName, int flag);
void SuperfineSDKLogWithIntValue(const char* eventName, int value);
void SuperfineSDKLogWithIntValueAndFlag(const char* eventName, int value, int flag);
void SuperfineSDKLogWithStringValue(const char* eventName, const char* value);
void SuperfineSDKLogWithStringValueAndFlag(const char* eventName, const char* value, int flag);
void SuperfineSDKLogWithMapValue(const char* eventName, const char* value);
void SuperfineSDKLogWithMapValueAndFlag(const char* eventName, const char* value, int flag);
void SuperfineSDKLogWithJsonValue(const char* eventName, const char* value);
void SuperfineSDKLogWithJsonValueAndFlag(const char* eventName, const char* value, int flag);

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
void SuperfineSDKLogUpdateApp(const char* newVersion);
void SuperfineSDKLogRateApp();
void SuperfineSDKLogLocation(double latitude, double longitude);
void SuperfineSDKLogAuthorizationTrackingStatus(int status);
void SuperfineSDKLogFacebookLink(const char* userId);
void SuperfineSDKLogFacebookUnlink();
void SuperfineSDKLogInstagramLink(const char* userId);
void SuperfineSDKLogInstagramUnlink();
void SuperfineSDKLogAppleLink(const char* userId);
void SuperfineSDKLogAppleUnlink();
void SuperfineSDKLogAppleGameCenterLink(const char* gamePlayerId);
void SuperfineSDKLogAppleGameCenterTeamLink(const char* teamPlayerId);
void SuperfineSDKLogAppleGameCenterUnlink();
void SuperfineSDKLogGoogleLink(const char* userId);
void SuperfineSDKLogGoogleUnlink();
void SuperfineSDKLogGooglePlayGameServicesLink(const char* gamePlayerId);
void SuperfineSDKLogGooglePlayGameServicesDeveloperLink(const char* developerPlayerKey);
void SuperfineSDKLogGooglePlayGameServicesUnlink();
void SuperfineSDKLogLinkedInLink(const char* personId);
void SuperfineSDKLogLinkedInUnlink();
void SuperfineSDKLogMeetupLink(const char* userId);
void SuperfineSDKLogMeetupUnlink();
void SuperfineSDKLogGitHubLink(const char* userId);
void SuperfineSDKLogGitHubUnlink();
void SuperfineSDKLogDiscordLink(const char* userId);
void SuperfineSDKLogDiscordUnlink();
void SuperfineSDKLogTwitterLink(const char* userId);
void SuperfineSDKLogTwitterUnlink();
void SuperfineSDKLogSpotifyLink(const char* userId);
void SuperfineSDKLogSpotifyUnlink();
void SuperfineSDKLogMicrosoftLink(const char* userId);
void SuperfineSDKLogMicrosoftUnlink();
void SuperfineSDKLogLINELink(const char* userId);
void SuperfineSDKLogLINEUnlink();
void SuperfineSDKLogVKLink(const char* userId);
void SuperfineSDKLogVKUnlink();
void SuperfineSDKLogQQLink(const char* openId);
void SuperfineSDKLogQQUnionLink(const char* unionId);
void SuperfineSDKLogQQUnlink();
void SuperfineSDKLogWeChatLink(const char* openId);
void SuperfineSDKLogWeChatUnionLink(const char* unionId);
void SuperfineSDKLogWeChatUnlink();
void SuperfineSDKLogTikTokLink(const char* openId);
void SuperfineSDKLogTikTokUnionLink(const char* unionId);
void SuperfineSDKLogTikTokUnlink();
void SuperfineSDKLogWeiboLink(const char* userId);
void SuperfineSDKLogWeiboUnlink();
void SuperfineSDKLogAccountLink(const char* accountId, const char* type);
void SuperfineSDKLogAccountLink2(const char* accountId, const char* type, const char* scopeId);
void SuperfineSDKLogAccountLink3(const char* accountId, const char* type, const char* scopeId, const char* scopeType);
void SuperfineSDKLogAccountUnlink(const char* type);
void SuperfineSDKAddUserPhoneNumber(int countryCode, const char* number);
void SuperfineSDKRemoveUserPhoneNumber(int countryCode, const char* number);
void SuperfineSDKAddUserEmail(const char* email);
void SuperfineSDKRemoveUserEmail(const char* email);
void SuperfineSDKSetUserName(const char* firstName, const char* lastName);
void SuperfineSDKSetUserFirstName(const char* firstName);
void SuperfineSDKSetUserLastName(const char* lastName);
void SuperfineSDKSetUserCity(const char* city);
void SuperfineSDKSetUserState(const char* state);
void SuperfineSDKSetUserCountry(const char* country);
void SuperfineSDKSetUserZipCode(const char* zipCode);
void SuperfineSDKSetUserDateOfBirth(int day, int month, int year);
void SuperfineSDKSetUserDateOfBirth2(int day, int month);
void SuperfineSDKSetUserYearOfBirth(int year);
void SuperfineSDKSetUserGender(int gender);
void SuperfineSDKLogWalletLink(const char* wallet, const char* type);
void SuperfineSDKLogWalletUnlink(const char* wallet, const char* type);
void SuperfineSDKLogCryptoPayment(const char* pack, double price, int amount, const char* currency, const char* chain);
void SuperfineSDKLogAdRevenue(const char* source, double revenue, const char* currency);
void SuperfineSDKLogAdRevenue2(const char* source, double revenue, const char* currency, const char* network);
void SuperfineSDKLogAdRevenue3(const char* source, double revenue, const char* currency, const char* network, const char* networkData);

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
