using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAssociatedDomainSettings : ISerializationCallbackReceiver
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
                ios = true;
            }
        }

        [Space(10)]
        public string host;

        [Label("iOS")]
        [AllowNesting]
        public bool ios = true;

        [Label("macOS")]
        [AllowNesting]
        public bool macos = false;
    }
}
