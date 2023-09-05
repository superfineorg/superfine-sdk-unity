using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    public class SuperfineSDKSettings : ScriptableObject
    {
        private const string AssetPath = "Assets/Resources";
        private const string AssetName = "SuperfineSettings.asset";

        public string appId = "YOUR APP ID";
        public string appSecret = "YOUR APP SECRET";

        public string[] uriSchemes = null;
        public string[] associatedDomains = null;

        public string host = string.Empty;

        //iOS
        public string advertisingAttributionReportEndpoint = string.Empty;
        public string userTrackingUsageDescription = string.Empty;

        //Tenjin
        public string tenjinAPIKeyIOS = string.Empty;
        public string tenjinAPIKeyAndroid = string.Empty;

        private static SuperfineSDKSettings instance;

#if UNITY_EDITOR

        [MenuItem("Superfine/Edit Settings")]
        private static void Edit()
        {
            var instance = NullableInstance;

            if (instance == null)
            {
                instance = CreateInstance<SuperfineSDKSettings>();
                string properPath = Path.Combine(Application.dataPath, AssetPath);
                if (!Directory.Exists(properPath))
                {
                    Directory.CreateDirectory(properPath);
                }

                string fullPath = Path.Combine(AssetPath, AssetName);
                AssetDatabase.CreateAsset(instance, fullPath);
            }

            Selection.activeObject = Instance;
        }
#endif

        public static SuperfineSDKSettings Instance
        {
            get
            {
                instance = NullableInstance;

                if (instance == null)
                {
                    // If not found, autocreate the asset object.
                    instance = CreateInstance<SuperfineSDKSettings>();
                }

                return instance;
            }
        }

        public static SuperfineSDKSettings NullableInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(Path.GetFileNameWithoutExtension(AssetName)) as SuperfineSDKSettings;
                }

                return instance;
            }
        }
    }
}