using System;
using UnityEngine;

namespace Superfine.Unity
{
	[Serializable]
	public class SuperfineSDKIosError : ISerializationCallbackReceiver
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

        public int code = 0;

		public string message = string.Empty;
		public string domain = string.Empty;

        public SimpleJSON.JSONObject CreateJSONObject()
        {
            SimpleJSON.JSONObject ret = new SimpleJSON.JSONObject
            {
                { "code", code }
            };

            if (!string.IsNullOrEmpty(message))
            {
                ret.Add("message", message);
            }

            if (!string.IsNullOrEmpty(domain))
            {
                ret.Add("domain", domain);
            }

            return ret;
        }

        public static SuperfineSDKIosError Create(SimpleJSON.JSONObject data)
        {
            if (data == null) return null;

            SuperfineSDKIosError ret = new SuperfineSDKIosError();

            SimpleJSON.JSONNode node;

            if (data.TryGetValue("code", out node) && node.IsNumber)
            {
                ret.code = node.AsInt;
            }

            if (data.TryGetValue("message", out node) && node.IsString)
            {
                ret.message = node.Value;
            }

            if (data.TryGetValue("domain", out node) && node.IsString)
            {
                ret.domain = node.Value;
            }

            return ret;
        }
    }
}
