using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKIosInitializationActionSettings : ISerializationCallbackReceiver
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
                type = IosInitializationActionType.REQUEST_TRACKING_AUTHORIZATION;
            }
        }

        public IosInitializationActionType type;

        [Space(10)]

        [ShowIf("type", IosInitializationActionType.REQUEST_NOTIFICATION_AUTHORIZATION)]
        [ExtractChildren]
        public SuperfinerSDKIosNotificationAuthorizationRequest notificationAuthorizationRequest;

        public SimpleJSON.JSONObject CreateJSONObject()
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject
            {
                { "type", (int)type }
            };

            switch (type)
            {
                case IosInitializationActionType.REQUEST_NOTIFICATION_AUTHORIZATION:
                    {
                        if (notificationAuthorizationRequest != null)
                        {
                            SimpleJSON.JSONObject data = notificationAuthorizationRequest.CreateJSONObject();
                            if (data != null)
                            {
                                ret.Add("data", data);
                            }
                        }
                    }
                    break;

                case IosInitializationActionType.REQUEST_TRACKING_AUTHORIZATION:
                    break;

                default:
                    break;
            }

            return ret;
        }
    }
}
