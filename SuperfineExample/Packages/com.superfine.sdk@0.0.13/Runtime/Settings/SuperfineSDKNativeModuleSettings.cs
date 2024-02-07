using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKNativeModuleSettings
    {
        [Space(10)]
        public string classPath;
        public SuperfineSDKPlatformFlag platform;

        [Space(10)]
        public SuperfineSDKJSONData data;
    }
}
