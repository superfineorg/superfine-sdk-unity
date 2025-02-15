using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKCustomSchemeSettings : ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private bool _serialized = false;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (_serialized == false)
            {
                _serialized = true;
                platform = 
                    SuperfineSDKSettings.PlatformFlag.Android |
                    SuperfineSDKSettings.PlatformFlag.iOS;
            }
        }

        [Space(10)]
        public string scheme;

        public SuperfineSDKSettings.PlatformFlag platform = 
            SuperfineSDKSettings.PlatformFlag.Android | 
            SuperfineSDKSettings.PlatformFlag.iOS;
    }
}
