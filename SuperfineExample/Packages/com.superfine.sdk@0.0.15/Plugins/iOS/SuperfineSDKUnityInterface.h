#import <UIKit/UIKit.h>

#ifndef NON_UNITY
#import "UnityAppController.h"

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
void SuperfineSDKInitialize(const char* params);
void SuperfineSDKInitialize2(const char* params, void (*callback)(void));
int SuperfineSDKIsInitialized(void);
void SuperfineSDKStart(void);
void SuperfineSDKStop(void);
void SuperfineSDKShutdown(void);
void SuperfineSDKSetOffline(bool value);
void SuperfineSDKAddStartCallback(void (*callback)(void));
void SuperfineSDKRemoveStartCallback(void (*callback)(void));
void SuperfineSDKAddStopCallback(void (*callback)(void));
void SuperfineSDKRemoveStopCallback(void (*callback)(void));
void SuperfineSDKAddPauseCallback(void (*callback)(void));
void SuperfineSDKRemovePauseCallback(void (*callback)(void));
void SuperfineSDKAddResumeCallback(void (*callback)(void));
void SuperfineSDKRemoveResumeCallback(void (*callback)(void));
void SuperfineSDKAddDeepLinkCallback(void (*callback)(const char*));
void SuperfineSDKRemoveDeepLinkCallback(void (*callback)(const char*));
void SuperfineSDKAddDeviceTokenCallback(void (*callback)(const char*));
void SuperfineSDKRemoveDeviceTokenCallback(void (*callback)(const char*));
void SuperfineSDKAddSendEventCallback(void (*callback)(const char*, const char*));
void SuperfineSDKRemoveSendEventCallback(void (*callback)(const char*, const char*));
void SuperfineSDKGdprForgetMe(void);
void SuperfineSDKDisableThirdPartySharing(void);
void SuperfineSDKEnableThirdPartySharing(void);
void SuperfineSDKLogThirdPartySharing(const char* params);
char* SuperfineSDKGetVersion(void);
void SuperfineSDKSetConfigId(const char* configId);
void SuperfineSDKSetCustomUserId(const char* userId);
char* SuperfineSDKGetAppId(void);
char* SuperfineSDKGetUserId(void);
char* SuperfineSDKGetSessionId(void);
char* SuperfineSDKGetHost(void);
char* SuperfineSDKGetConfigUrl(void);
char* SuperfineSDKGetSdkConfig(void);
int SuperfineSDKGetStoreType(void);
char* SuperfineGetFacebookAppId(void);
char* SuperfineGetInstagramAppId(void);
char* SuperfineSDKGetAppleAppId(void);
char* SuperfineSDKGetAppleSignInClientId(void);
char* SuperfineSDKGetAppleDeveloperTeamId(void);
char* SuperfineSDKGetGooglePlayGameServicesProjectId(void);
char* SuperfineSDKGetGooglePlayDeveloperAccountId(void);
char* SuperfineSDKGetLinkedInAppId(void);
char* SuperfineSDKGetQQAppId(void);
char* SuperfineSDKGetWeChatAppId(void);
char* SuperfineSDKGetTikTokAppId(void);
char* SuperfineSDKGetSnapAppId(void);
char* SuperfineSDKGetDeepLinkUrl(void);
char* SuperfineSDKGetDeviceToken(void);
void SuperfineSDKFetchRemoteConfig(void(*callback)(const char* data));
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

int SuperfineSDKBeginLogEvent(const char* eventName);
void SuperfineSDKSetLogEventIntValue(int eventRef, int value);
void SuperfineSDKSetLogEventStringValue(int eventRef, const char* value);
void SuperfineSDKSetLogEventMapValue(int eventRef, const char* value);
void SuperfineSDKSetLogEventJsonValue(int eventRef, const char* value);
void SuperfineSDKSetLogEventFlag(int eventRef, int flag);
void SuperfineSDKSetLogEventRevenue(int eventRef, double revenue, const char* currency);
void SuperfineSDKEndLogEvent(int eventRef, bool cache);

void SuperfineSDKLogBootStart(void);
void SuperfineSDKLogBootEnd(void);
void SuperfineSDKLogLevelStart(int levelId, const char* name);
void SuperfineSDKLogLevelEnd(int levelId, const char* name, bool isSuccess);
void SuperfineSDKLogAdLoad(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdClose(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdClick(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogAdImpression(const char* adUnit, int adPlacementType, int adPlacement);
void SuperfineSDKLogIAPInitialization(bool isSuccess);
void SuperfineSDKLogIAPRestorePurchase(void);
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
void SuperfineSDKLogRateApp(void);
void SuperfineSDKLogLocation(double latitude, double longitude);
void SuperfineSDKLogTrackingAuthorizationStatus(int status);
void SuperfineSDKLogFacebookLink(const char* userId);
void SuperfineSDKLogFacebookUnlink(void);
void SuperfineSDKLogInstagramLink(const char* userId);
void SuperfineSDKLogInstagramUnlink(void);
void SuperfineSDKLogAppleLink(const char* userId);
void SuperfineSDKLogAppleUnlink(void);
void SuperfineSDKLogAppleGameCenterLink(const char* gamePlayerId);
void SuperfineSDKLogAppleGameCenterTeamLink(const char* teamPlayerId);
void SuperfineSDKLogAppleGameCenterUnlink(void);
void SuperfineSDKLogGoogleLink(const char* userId);
void SuperfineSDKLogGoogleUnlink(void);
void SuperfineSDKLogGooglePlayGameServicesLink(const char* gamePlayerId);
void SuperfineSDKLogGooglePlayGameServicesDeveloperLink(const char* developerPlayerKey);
void SuperfineSDKLogGooglePlayGameServicesUnlink(void);
void SuperfineSDKLogLinkedInLink(const char* personId);
void SuperfineSDKLogLinkedInUnlink(void);
void SuperfineSDKLogMeetupLink(const char* userId);
void SuperfineSDKLogMeetupUnlink(void);
void SuperfineSDKLogGitHubLink(const char* userId);
void SuperfineSDKLogGitHubUnlink(void);
void SuperfineSDKLogDiscordLink(const char* userId);
void SuperfineSDKLogDiscordUnlink(void);
void SuperfineSDKLogTwitterLink(const char* userId);
void SuperfineSDKLogTwitterUnlink(void);
void SuperfineSDKLogSpotifyLink(const char* userId);
void SuperfineSDKLogSpotifyUnlink(void);
void SuperfineSDKLogMicrosoftLink(const char* userId);
void SuperfineSDKLogMicrosoftUnlink(void);
void SuperfineSDKLogLINELink(const char* userId);
void SuperfineSDKLogLINEUnlink(void);
void SuperfineSDKLogVKLink(const char* userId);
void SuperfineSDKLogVKUnlink(void);
void SuperfineSDKLogQQLink(const char* openId);
void SuperfineSDKLogQQUnionLink(const char* unionId);
void SuperfineSDKLogQQUnlink(void);
void SuperfineSDKLogWeChatLink(const char* openId);
void SuperfineSDKLogWeChatUnionLink(const char* unionId);
void SuperfineSDKLogWeChatUnlink(void);
void SuperfineSDKLogTikTokLink(const char* openId);
void SuperfineSDKLogTikTokUnionLink(const char* unionId);
void SuperfineSDKLogTikTokUnlink(void);
void SuperfineSDKLogWeiboLink(const char* userId);
void SuperfineSDKLogWeiboUnlink(void);
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
int SuperfineSDKGetTrackingAuthorizationStatus(void);
void SuperfineSDKRequestNotificationAuthorization(void (*callback)(bool, const char*), int options);
void SuperfineSDKRequestNotificationAuthorization2(void (*callback)(bool, const char*), int options, bool registerRemote);
void SuperfineSDKRegisterForRemoteNotifications(void (*callback)(const char*));
void SuperfineSDKUnregisterForRemoteNotifications(void);
bool SuperfineSDKIsRegisteredForRemoteNotifications(void);
void SuperfineSDKRegisterAppForAdNetworkAttribution(void);
void SuperfineSDKUpdatePostbackConversionValue(int conversionValue);
void SuperfineSDKUpdatePostbackConversionValue2(int conversionValue, const char* coarseValue);
void SuperfineSDKUpdatePostbackConversionValue3(int conversionValue, const char* coarseValue, bool lockWindow);

#ifdef __cplusplus
}
#endif
