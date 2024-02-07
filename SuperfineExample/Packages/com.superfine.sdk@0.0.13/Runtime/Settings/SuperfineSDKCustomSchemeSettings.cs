using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKCustomSchemeSettings
    {
        [Space(10)]
        public string scheme;
        public SuperfineSDKPlatformFlag platform;
    }
}
