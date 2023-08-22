namespace Superfine.Unity
{
    public enum LogLevel
    {
        NONE = 0,
        INFO = 1,
        DEBUG = 2,
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
        UNKNOWN = -1,
        APP_STORE = 0,
        GOOGLE_PLAY = 1,
        AMAZON_STORE = 2,
        MICROSOFT_STORE = 3
    }

    public enum AuthorizationTrackingStatus
    {
        NOT_DETERMINED = 0,
        RESTRICTED = 1,
        DENIED = 2,
        AUTHORIZED = 3
    }
}