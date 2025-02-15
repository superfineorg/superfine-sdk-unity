using System;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAndroidInitializationActionSettings : ISerializationCallbackReceiver
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
                type = AndroidInitializationActionType.REQUEST_PERMISSIONS;
            }
        }

        public AndroidInitializationActionType type;

        [Space(10)]

        [ShowIf("type", AndroidInitializationActionType.REQUEST_PERMISSIONS)]
        [ExtractChildren]
        public SuperfineSDKAndroidPermissionRequest permissionRequest;

        public SimpleJSON.JSONObject CreateJSONObject()
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject
            {
                { "type", type.ToString() }
            };

            switch (type)
            {
                case AndroidInitializationActionType.REQUEST_PERMISSIONS:
                    {
                        if (permissionRequest != null)
                        {
                            SimpleJSON.JSONObject data = permissionRequest.CreateJSONObject();
                            if (data != null)
                            {
                                ret.Add("data", data);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return ret;
        }
    }
}
