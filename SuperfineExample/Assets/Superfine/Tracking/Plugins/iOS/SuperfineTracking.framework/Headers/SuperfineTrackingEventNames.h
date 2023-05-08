//
//  SuperfineTrackingEventNames.h
//  SuperfineTracking
//
//  Copyright © 2022 Superfine. All rights reserved.
//

#ifndef SuperfineTrackingEventNames_h
#define SuperfineTrackingEventNames_h

#import <Foundation/Foundation.h>

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_AD_IMPRESSION;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_AD_CLICK;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_AD_LOAD;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_AD_CLOSE;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_OPEN;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_FIRST_OPEN;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_BOOT_START;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_BOOT_END;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_LEVEL_ATTEMPT;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_LEVEL_COMPLETE;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_LEVEL_FAIL;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_IAP_INITIALIZATION;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_IAP_RESTORE_PURCHASE;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_IAP_BUY;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_IAP_SUCCESS_PAYMENT;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_IAP_FAIL_PAYMENT;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_FACEBOOK_LOGIN;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_FACEBOOK_LOGOUT;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_UPDATE_GAME;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_RATE_GAME;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_SOUND_MODE;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_AUTHORIZATION_TRACKING_STATUS;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_ACCOUNT_LOGIN;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_ACCOUNT_LOGOUT;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_ACCOUNT_LINK;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_ACCOUNT_UNLINK;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_WALLET_LINK;
FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_WALLET_UNLINK;

FOUNDATION_EXPORT NSString *const kSuperfineTrackingEvent_CRYPTO_PAYMENT;

#endif /* SuperfineTrackingEventNames_h */
