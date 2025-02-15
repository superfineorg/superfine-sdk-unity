using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;

namespace Superfine.Unity
{
    [ExecuteInEditMode]
    [Serializable]
    public class SuperfineSDKJSONData
    {
        [SerializeField]
        private string rawText;

        public SimpleJSON.JSONObject GetJsonObject()
        {
            return GetJsonObject(rawText);
        }

        public static SimpleJSON.JSONObject GetJsonObject(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            if (SimpleJSON.JSON.TryParse(s, out SimpleJSON.JSONNode node))
            {
                if (node.IsObject)
                {
                    return (SimpleJSON.JSONObject)node;
                }
            }

            return null;
        }

        public SuperfineSDKJSONData(string s)
        {
            SimpleJSON.JSONObject jsonObject = GetJsonObject(s);
            if (jsonObject != null)
            {
                rawText = jsonObject.ToString();
            }
            else
            {
                rawText = string.Empty;
            }
        }
    }
}