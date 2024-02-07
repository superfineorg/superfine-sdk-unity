using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAppLinkSettings
    {
        [Space(10)]
        public string host;
        public bool disableVerify;
    }
}
