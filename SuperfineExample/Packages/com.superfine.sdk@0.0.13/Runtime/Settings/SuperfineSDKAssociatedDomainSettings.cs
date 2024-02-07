using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAssociatedDomainSettings
    {
        [Space(10)]
        public string host;

        [Label("iOS")]
        [AllowNesting]
        public bool ios;

        [Label("macOS")]
        [AllowNesting]
        public bool macos;
    }
}
