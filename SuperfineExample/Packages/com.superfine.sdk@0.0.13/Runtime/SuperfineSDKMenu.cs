using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

namespace Superfine.Unity
{
#if UNITY_EDITOR
    public class SuperfineAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (didDomainReload)
            {
                string settingClasses = EditorPrefs.GetString("PendingSettingsClass", string.Empty);

                if (!string.IsNullOrEmpty(settingClasses))
                {
                    SetupSettingsClass(settingClasses);
                }

                EditorPrefs.DeleteKey("PendingSettingsClass");
            }
        }

        private static void SetupSettingsClass(string className)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
            if (assembly != null)
            {
                Type settingType = assembly.GetType(className);

                if (settingType != null)
                {
                    PropertyInfo propertyInfo = settingType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
                    if (propertyInfo != null)
                    {
                        propertyInfo.GetValue(null, null);
                    }
                }
            }
        }
    }
#endif

    public static class SuperfineSDKMenu
    {
#if UNITY_EDITOR
        private static void CopyFolder(string sourcePath, string targetPath)
        {
            DirectoryInfo source = new DirectoryInfo(sourcePath);
            DirectoryInfo target = new DirectoryInfo(targetPath);

            CopyFolder(source, target);
        }

        private static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo file in source.GetFiles())
            {
                if (file.Extension == ".meta") continue;
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }

            foreach (DirectoryInfo subDir in source.GetDirectories())
            {
                string subDirName = subDir.Name;
                if (subDirName.StartsWith('_')) subDirName = subDirName.Remove(0, 1);

                DirectoryInfo targetSubDir = target.CreateSubdirectory(subDirName);
                CopyFolder(subDir, targetSubDir);
            }
        }

        private static void CopyAddonFile(string fileName, string overwriteName = null)
        {
            PackageInfo packageInfo = SuperfineSDK.GetSelfPackageInfo();

            if (packageInfo != null)
            {
                string sourceFolder = Path.Combine(packageInfo.assetPath, "Addons");

                string targetFolder = Path.Combine(Application.dataPath, "SuperfineSDK/Addons");
                Directory.CreateDirectory(targetFolder);

                string sourcePath = Path.Combine(sourceFolder, fileName);
                string targetPath = Path.Combine(targetFolder, string.IsNullOrEmpty(overwriteName) ? fileName : overwriteName);

                File.Copy(sourcePath, targetPath, true);
            }
        }

        private static void CopyModuleFolder(string moduleName)
        {
            PackageInfo packageInfo = SuperfineSDK.GetSelfPackageInfo();

            if (packageInfo != null)
            {
                string sourceFolder = Path.Combine(packageInfo.assetPath, "Modules");
                sourceFolder = Path.Combine(sourceFolder, moduleName);

                string targetFolder = Path.Combine(Application.dataPath, "SuperfineSDK/Modules");
                targetFolder = Path.Combine(targetFolder, moduleName);

                CopyFolder(sourceFolder, targetFolder);
            }
        }

        [MenuItem("Superfine/Addons/UnityIAP")]
        private static void CopyUnityIAP()
        {
            CopyAddonFile("SuperfineSDKUnityIAP.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/Facebook")]
        private static void CopyFacebook()
        {
            CopyAddonFile("SuperfineSDKFacebook.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/AppLovin")]
        private static void CopyAppLovin()
        {
            CopyAddonFile("SuperfineSDKAppLovin.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/IronSource")]
        private static void CopyIronSource()
        {
            CopyAddonFile("SuperfineSDKIronSource.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/AdMob")]
        private static void CopyAdMob()
        {
            CopyAddonFile("SuperfineSDKAdMob.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/Appodeal (UPM Distribution)")]
        private static void CopyAppodealUPM()
        {
            CopyAddonFile("SuperfineSDKAppodeal_UPM.cs", "SuperfineSDKAppodeal.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/Appodeal (Manual Distribution)")]
        private static void CopyAppodealManual()
        {
            CopyAddonFile("SuperfineSDKAppodeal_Manual.cs", "SuperfineSDKAppodeal.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Addons/Steamworks")]
        private static void CopySteamworks()
        {
            CopyAddonFile("SuperfineSDKSteamworks.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Modules/AppsFlyer")]
        private static void SetupAppsFlyerModule()
        {
            CopyModuleFolder("AppsFlyer");

            EditorPrefs.SetString("PendingSettingsClass", "Superfine.Unity.SuperfineSDKAppsFlyerSettings");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Modules/AppsFlyer AdRevenue")]
        private static void SetupAppsFlyerAdRevenueModule()
        {
            CopyModuleFolder("AppsFlyerAdRevenue");

            EditorPrefs.SetString("PendingSettingsClass", "Superfine.Unity.SuperfineSDKAppsFlyerAdRevenueSettings");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Modules/Tenjin")]
        private static void SetupTenjinModule()
        {
            CopyModuleFolder("Tenjin");

            EditorPrefs.SetString("PendingSettingsClass", "Superfine.Unity.SuperfineSDKTenjinSettings");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Modules/Adjust")]
        private static void SetupAdjustModule()
        {
            CopyModuleFolder("Adjust");

            EditorPrefs.SetString("PendingSettingsClass", "Superfine.Unity.SuperfineSDKAdjustSettings");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Modules/Singular")]
        private static void SetupSingularModule()
        {
            CopyModuleFolder("Singular");

            EditorPrefs.SetString("PendingSettingsClass", "Superfine.Unity.SuperfineSDKSingularSettings");
            AssetDatabase.Refresh();
        }
#endif
    }
}
