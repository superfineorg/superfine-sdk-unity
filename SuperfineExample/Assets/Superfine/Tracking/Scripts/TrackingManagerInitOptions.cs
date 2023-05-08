namespace Superfine.Tracking
{
    public class TrackingManagerInitOptions
    {
        public long flushInterval = 30 * 1000; //ms
        public int flushQueueSize = 5;

        public bool customUserId = false;
        public string userId = string.Empty;

        public bool waitConfigId = false;

#if UNITY_ANDROID && !UNITY_EDITOR
        public LogLevel logLevel = LogLevel.NONE;
#elif UNITY_IOS && !UNITY_EDITOR
        public bool debug = false;
        public bool captureInAppPurchases = false;
#endif
    }
}
