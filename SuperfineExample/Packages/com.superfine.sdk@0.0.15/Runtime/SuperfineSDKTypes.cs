using System;
using UnityEngine;

namespace Superfine.Unity
{
    public enum LogLevel
    {
        [InspectorName("None")]
        NONE = 0,

        [InspectorName("Info")]
        INFO = 1,

        [InspectorName("Debug")]
        DEBUG = 2,

        [InspectorName("Verbose")]
        VERBOSE = 3
    }

    public enum AdPlacementType
    {
        BANNER = 0,
        INTERSTITIAL = 1,
        REWARDED_VIDEO = 2
    }

    public enum AdPlacement
    {
        UNKNOWN = -1,
        BOTTOM = 0,
        TOP = 1,
        LEFT = 2,
        RIGHT = 3,
        FULL_SCREEN = 4
    }

    public enum StoreType
    {
        [InspectorName("Unknown")]
        UNKNOWN = -1,

        [InspectorName("Apple App Store")]
        APP_STORE = 0,

        [InspectorName("Google Play Store")]
        GOOGLE_PLAY = 1,

        [InspectorName("Amazon Store")]
        AMAZON_STORE = 2,

        [InspectorName("Microsoft Store")]
        MICROSOFT_STORE = 3
    }

    [Flags]
    public enum EventFlag
    {
        NONE = 0,
        //OPEN_EVENT = 1 << 0,
        //FORCE_DISABLED = 1 << 1,
        WAIT_OPEN_EVENT = 1 << 2,
        CACHE = 1 << 3
    }

    public enum UserGender
    {
	    MALE = 0,
	    FEMALE = 1
    }

    public enum AndroidInitializationActionType
    {
        [InspectorName("Request Permissions")]
        REQUEST_PERMISSIONS = 0
    }

    public enum AndroidPermissionResult
    {
        GRANTED = 0,
        DENIED = -1,
        DENIED_APP_OP = -2
    }

    public enum IosInitializationActionType
    {
        [InspectorName("Request Tracking Authorization")]
        REQUEST_TRACKING_AUTHORIZATION = 0,

        [InspectorName("Request Notification Authorization")]
        REQUEST_NOTIFICATION_AUTHORIZATION = 1
    }

    [Flags]
    public enum IosNotificationAuthorizationOptions
    {
        [InspectorName("None")]
        NONE = 0,

        [InspectorName("Badge")]
        BADGE = 1 << 0,

        [InspectorName("Sound")]
        SOUND = 1 << 1,

        [InspectorName("Alert")]
        ALERT = 1 << 2,

        [InspectorName("CarPlay")]
        CAR_PLAY = 1 << 3,

        [InspectorName("Critical Alert")]
        CRITICAL_ALERT = 1 << 4,

        [InspectorName("Provides App Notification Settings")]
        PROVIDES_APP_NOTIFICATION_SETTINGS = 1 << 5,

        [InspectorName("Provisional")]
        PROVISIONAL = 1 << 6,
    }

    public enum IosTrackingAuthorizationStatus
    {
        NOT_DETERMINED = 0,
        RESTRICTED = 1,
        DENIED = 2,
        AUTHORIZED = 3
    }
}