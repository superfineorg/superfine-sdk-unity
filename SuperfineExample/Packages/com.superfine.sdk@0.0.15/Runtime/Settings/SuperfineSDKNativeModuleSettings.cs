using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKNativeModuleSettings : ISerializationCallbackReceiver
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
                platform = SuperfineSDKSettings.PlatformFlag.None;
            }
        }

        [Space(10)]
        public string classPath;
        public SuperfineSDKSettings.PlatformFlag platform = SuperfineSDKSettings.PlatformFlag.None;

        [Space(10)]
        public SuperfineSDKJSONData data;
    }
}
