using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineTenjinSettings", menuName = "Superfine/SuperfineTenjinSettings", order = 1)]
    public class SuperfineSDKTenjinSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/Tenjin/SuperfineTenjinSettings.asset";

        private static SuperfineSDKTenjinSettings instance = null;

        public static SuperfineSDKTenjinSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKTenjinSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK Tenjin settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKTenjinSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKTenjinSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK Tenjin settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif

        [Space(10)]

        public string sdkKey = "TENJIN SDK KEY";

        [Space(10)]

        public bool autoStart = true;
        public bool setCustomerId = true;

        [Space(10)]
        public bool sendLog = false;
        public string[] logFilters;
        public string[] logNotFilters;

        [Space(10)]
        public bool sendIAP = false;
        [HideInInspector]
        public bool sendAdRevenue = false;

        [Space(10)]
        public bool sendAttribution = true;
        public bool sendDeepLink = true;

        [Space(10)]
        public int subversion = 0;
        public bool cacheEvent = false;

        [Space(10)]
        [Label("Opt In")]
        public SuperfineNullable<bool> optIn = new SuperfineNullable<bool>(false, true);
        public string[] optInParams;
        public string[] optOutParams;

        [Space(10)]
        [InfoBox("iOS only", EInfoBoxType.Normal)]
        public bool debug = false;

        public override string GetModuleName()
        {
            return "TenjinUnity";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKTenjinModule(this);
        }
    }
}
