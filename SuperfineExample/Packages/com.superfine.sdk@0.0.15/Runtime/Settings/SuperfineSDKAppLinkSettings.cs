using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAppLinkSettings : ISerializationCallbackReceiver
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
            }
        }

        [Space(10)]
        public string host;
        public bool disableVerify;
    }
}
