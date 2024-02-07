using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Superfine.Unity
{
    [CreateAssetMenu(fileName = "SuperfineAppsFlyerAdRevenueSettings", menuName = "Superfine/SuperfineAppsFlyerAdRevenueSettings", order = 1)]
    public class SuperfineSDKAppsFlyerAdRevenueSettings : SuperfineSDKModuleSettings
    {
#if UNITY_EDITOR
        private const string SettingsExportPath = "SuperfineSDK/Modules/AppsFlyerAdRevenue/SuperfineAppsFlyerAdRevenueSettings.asset";

        private static SuperfineSDKAppsFlyerAdRevenueSettings instance = null;

        public static SuperfineSDKAppsFlyerAdRevenueSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:SuperfineSDKAppsFlyerAdRevenueSettings");
                    if (guids.Length > 1)
                    {
                        Debug.LogWarning("Multiple Superfine SDK AppsFlyer AdRevenue settings files found.");
                    }

                    if (guids.Length != 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        instance = AssetDatabase.LoadAssetAtPath<SuperfineSDKAppsFlyerAdRevenueSettings>(path);
                        return instance;
                    }

                    string settingsFilePath = Path.Combine("Assets", SettingsExportPath);

                    var settingsDir = Path.GetDirectoryName(settingsFilePath);
                    if (!Directory.Exists(settingsDir))
                    {
                        Directory.CreateDirectory(settingsDir);
                    }

                    instance = CreateInstance<SuperfineSDKAppsFlyerAdRevenueSettings>();
                    AssetDatabase.CreateAsset(instance, settingsFilePath);

                    Debug.Log("Creating new Superfine SDK AppsFlyer AdRevenue settings file at path: " + settingsFilePath);
                }

                return instance;
            }
        }
#endif
        [Space(10)]

        public bool autoStart = true;

        [Space(10)]

        public bool debug = false;

        public override string GetModuleName()
        {
            return "AppsFlyerAdRevenueUnity";
        }

        public override SuperfineSDKModule CreateModule()
        {
            return new SuperfineSDKAppsFlyerAdRevenueModule(this);
        }
    }
}
