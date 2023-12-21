using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

namespace Superfine.Unity
{
    public static class SuperfineSDKMenu
    {
#if UNITY_EDITOR
        private static void CopyAddonFile(string fileName)
        {
            PackageInfo packageInfo = SuperfineSDK.GetSelfPackageInfo();

            if (packageInfo != null)
            {
                string sourceFolder = Path.Combine(packageInfo.assetPath, "Addons");

                string targetFolder = Path.Combine(Application.dataPath, "SuperfineSDK/Addons");
                Directory.CreateDirectory(targetFolder);

                File.Copy(Path.Combine(sourceFolder, fileName), Path.Combine(targetFolder, fileName));
            }
        }

        [MenuItem("Superfine/Copy UnityIAP addon")]
        private static void CopyUnityIAP()
        {
            CopyAddonFile("SuperfineSDKUnityIAP.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Copy Facebook addon")]
        private static void CopyFacebook()
        {
            CopyAddonFile("SuperfineSDKFacebook.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Copy AppLovin addon")]
        private static void CopyAppLovin()
        {
            CopyAddonFile("SuperfineSDKAppLovin.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Copy IronSource addon")]
        private static void CopyIronSource()
        {
            CopyAddonFile("SuperfineSDKIronSource.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Copy AdMob addon")]
        private static void CopyAdMob()
        {
            CopyAddonFile("SuperfineSDKAdMob.cs");
            AssetDatabase.Refresh();
        }

        [MenuItem("Superfine/Copy Steamworks addon")]
        private static void CopySteamworks()
        {
            CopyAddonFile("SuperfineSDKSteamworks.cs");
            AssetDatabase.Refresh();
        }
#endif
    }
}
