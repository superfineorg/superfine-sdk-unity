#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "SuperfineTrackingManager.h"
#import "SuperfineTrackingStoreKitCaptureData.h"

NS_ASSUME_NONNULL_BEGIN

@interface SuperfineTrackingStoreKitCapturer : NSObject <SKPaymentTransactionObserver, SKProductsRequestDelegate>

+ (instancetype)captureTransactionsForTrackingManager:(SuperfineTrackingManager *)trackingManager;

@end

NS_ASSUME_NONNULL_END
