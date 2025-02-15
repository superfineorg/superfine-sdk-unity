using System;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    [Serializable]
    public class SuperfineSDKAndroidPermissionRequest : ISerializationCallbackReceiver
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

        public string[] permissions;

        [Space(10)]

        public int requestCode = 0;

        [Space(10)]

        [Label("Rationale Message")]
        public string rationaleText;

        [Label("Rationale Title")]
        public string rationaleTitleText;

        [Label("Rationale Accept Button")]
        public string rationaleAcceptText;

        [Label("Rationale Deny Button")]
        public string rationaleDenyText;

        [Label("Rationale Theme Resource Id")]
        public int theme = 0;

        public SuperfineSDKAndroidPermissionRequest(string permission)
        {
            permissions = new string[] { permission };
            Init();
        }

        public SuperfineSDKAndroidPermissionRequest(string[] permissions)
        {
            this.permissions = (string[])permissions.Clone();
            Init();
        }

        public SuperfineSDKAndroidPermissionRequest(List<string> permissions)
        {
            this.permissions = permissions.ToArray();
            Init();
        }

        private void Init()
        {
            rationaleText = string.Empty;
            rationaleTitleText = string.Empty;
            rationaleAcceptText = string.Empty;
            rationaleDenyText = string.Empty;

            theme = 0;

            requestCode = 0;
        }

        public SimpleJSON.JSONObject CreateJSONObject()
        {
            if (permissions == null) return null;

            int numPermissions = permissions.Length;
            if (numPermissions == 0) return null;

            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject();

            if (numPermissions == 1)
            {
                ret.Add("permission", permissions[0]);
            }
            else
            {
                SimpleJSON.JSONArray permissionArray = new SimpleJSON.JSONArray();
                for (int i = 0; i < numPermissions; ++i)
                {
                    permissionArray.Add(permissions[i]);
                }

                ret.Add("permissions", permissionArray);
            }

            if (requestCode > 0)
            {
                ret.Add("requestCode", requestCode);
            }

            if (!string.IsNullOrEmpty(rationaleText))
            {
                ret.Add("rationaleText", rationaleText);
            }

            if (!string.IsNullOrEmpty(rationaleAcceptText))
            {
                ret.Add("rationaleAcceptText", rationaleAcceptText);
            }

            if (!string.IsNullOrEmpty(rationaleDenyText))
            {
                ret.Add("rationaleDenyText", rationaleDenyText);
            }

            if (!string.IsNullOrEmpty(rationaleTitleText))
            {
                ret.Add("rationaleTitleText", rationaleTitleText);
            }

            if (theme > 0)
            {
                ret.Add("theme", theme);
            }

            return ret;
        }
    }
}