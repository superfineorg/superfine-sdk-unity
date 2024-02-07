using System;

namespace Superfine.Unity
{
    [Flags]
    public enum SuperfineSDKPlatformFlag
    {
        None = 0,
        Android = 1 << 0,
        iOS = 1 << 1,
        Windows = 1 << 2,
        macOS = 1 << 3,
        Linux = 1 << 4,
        All = ~0
    }
}
