#import <UIKit/UIKit.h>

#import "AppDelegateListener.h"

//if we are on a version of unity that has the version number defined use it, otherwise we have added it ourselves in the post build step
#if HAS_UNITY_VERSION_DEF
#include "UnityTrampolineConfigure.h"
#endif

@interface SuperfineTrackingUnityInterface : NSObject <AppDelegateListener>
{
}

+ (SuperfineTrackingUnityInterface *)sharedInstance;
@end

