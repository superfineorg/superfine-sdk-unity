using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKModuleSettings : ScriptableObject
    {
        public virtual string GetModuleName()
        {
            return string.Empty;
        }

        public virtual SuperfineSDKModule CreateModule()
        {
            return null;
        }

        protected bool TryGetSdkConfigBool(SimpleJSON.JSONObject sdkConfig, string key, out bool value)
        {
            value = false;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsBoolean) return false;

            value = node.AsBool;
            return true;
        }

        protected bool TryGetSdkConfigString(SimpleJSON.JSONObject sdkConfig, string key, out string value)
        {
            value = null;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsString) return false;

            value = node.Value;

            if (string.IsNullOrEmpty(value)) return false;
            else return true;
        }

        protected bool TryGetSdkConfigInt(SimpleJSON.JSONObject sdkConfig, string key, out int value)
        {
            value = 0;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsNumber) return false;

            value = node.AsInt;
            return true;
        }

        protected bool TryGetSdkConfigLong(SimpleJSON.JSONObject sdkConfig, string key, out long value)
        {
            value = 0L;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsNumber) return false;

            value = node.AsLong;
            return true;
        }

        protected bool TryGetSdkConfigFloat(SimpleJSON.JSONObject sdkConfig, string key, out float value)
        {
            value = 0.0f;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsNumber) return false;

            value = node.AsFloat;
            return true;
        }

        protected bool TryGetSdkConfigDouble(SimpleJSON.JSONObject sdkConfig, string key, out double value)
        {
            value = 0.0;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsNumber) return false;

            value = node.AsDouble;
            return true;
        }

        protected bool TryGetSdkConfigArray(SimpleJSON.JSONObject sdkConfig, string key, out SimpleJSON.JSONArray value)
        {
            value = null;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsArray) return false;

            value = (SimpleJSON.JSONArray)node;
            return true;
        }

        protected bool TryGetSdkConfigObject(SimpleJSON.JSONObject sdkConfig, string key, out SimpleJSON.JSONObject value)
        {
            value = null;

            if (sdkConfig == null) return false;

            if (!sdkConfig.TryGetValue(key, out SimpleJSON.JSONNode node)) return false;
            if (!node.IsObject) return false;

            value = (SimpleJSON.JSONObject)node;
            return true;
        }

        protected bool TryGetSdkConfigStringMap(SimpleJSON.JSONObject sdkConfig, string key, out Dictionary<string, string> value)
        {
            value = null;

            if (!TryGetSdkConfigObject(sdkConfig, key, out SimpleJSON.JSONObject mapObject)) return false;

            value = new Dictionary<string, string>();
            foreach (var pair in mapObject)
            {
                string mapKey = pair.Key;
                SimpleJSON.JSONNode mapValue = pair.Value;
                    
                if (mapValue != null && mapValue.IsString)
                {
                    value.Add(mapKey, mapValue.Value);
                }
            }

            if (value.Count == 0)
            {
                value = null;
                return false;
            }

            return true;
        }

        protected bool TryGetSdkConfigStringList(SimpleJSON.JSONObject sdkConfig, string key, out List<string> value)
        {
            value = null;

            if (!TryGetSdkConfigArray(sdkConfig, key, out SimpleJSON.JSONArray listObject)) return false;

            value = new List<string>();

            int num = listObject.Count;
            for (int i = 0; i < num; ++i)
            {
                SimpleJSON.JSONNode listValue = listObject[i];

                if (listValue != null && listValue.IsString)
                {
                    value.Add(listValue.Value);
                }
            }

            if (value.Count == 0)
            {
                value = null;
                return false;
            }

            return true;
        }

        protected void MergeStringArray(ref string[] arr, List<string> updatedValues)
        {
            if (updatedValues == null || updatedValues.Count == 0)
            {
                return;
            }

            if (arr == null)
            {
                arr = updatedValues.ToArray();
                return;
            }

            int numBaseValues = arr.Length;
            int numUpdatedValues = updatedValues.Count;

            if (numBaseValues >= numUpdatedValues)
            {
                for (int i = 0; i < numUpdatedValues; ++i)
                {
                    arr[i] = updatedValues[i];
                }
            }
            else
            {
                arr = updatedValues.ToArray();
            }
        }

        public virtual void MergeSdkConfig(SimpleJSON.JSONObject sdkConfig)
        {
        }
    }
}
