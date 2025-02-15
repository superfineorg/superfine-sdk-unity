using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfinerSDKIosNotificationAuthorizationRequest : ISerializationCallbackReceiver
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
                authorizationOptions = 
                    IosNotificationAuthorizationOptions.BADGE | 
                    IosNotificationAuthorizationOptions.SOUND | 
                    IosNotificationAuthorizationOptions.ALERT;
            }
        }

        public IosNotificationAuthorizationOptions authorizationOptions =
            IosNotificationAuthorizationOptions.BADGE |
            IosNotificationAuthorizationOptions.SOUND |
            IosNotificationAuthorizationOptions.ALERT;

        public bool registerRemote = false;

        public SimpleJSON.JSONObject CreateJSONObject()
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject
            {
                { "authorizationOptions", (int)authorizationOptions },
                { "registerRemote", registerRemote }
            };

            return ret;
        }
    }
}
