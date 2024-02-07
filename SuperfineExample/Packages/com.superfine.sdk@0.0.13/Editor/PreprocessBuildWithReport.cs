using UnityEngine;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

using System.IO;

namespace Superfine.Unity
{
    public class PreprocessBuildWithReport : IPreprocessBuildWithReport
    {
        public int callbackOrder => int.MaxValue;

        public void OnPreprocessBuild(BuildReport report)
        {
            SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();
            if (settings != null)
            {
                string appleDeveloperTeamId = settings.appleDeveloperTeamId;
                if (!string.IsNullOrEmpty(appleDeveloperTeamId))
                {
                    PlayerSettings.iOS.appleDeveloperTeamID = appleDeveloperTeamId;
                }

#if UNITY_ANDROID
                bool captureInAppPurchases = settings.androidCaptureInAppPurchases;

                PluginImporter googlePlayBillingPluginImporter = AssetImporter.GetAtPath("Packages/com.superfine.sdk/Plugins/Android/superfine-sdk-googleplay-billing-0.0.13.aar") as PluginImporter;
                if (googlePlayBillingPluginImporter != null)
                {
                    if (SetPluginEnabled(googlePlayBillingPluginImporter, BuildTarget.Android, captureInAppPurchases))
                    {
                        EditorUtility.SetDirty(googlePlayBillingPluginImporter);
                        googlePlayBillingPluginImporter.SaveAndReimport();
                    }
                }
#endif
            }

#if UNITY_STANDALONE_LINUX
            PackageInfo packageInfo = SuperfineSDK.GetSelfPackageInfo();

            if (packageInfo != null)
            {
                string sourceFolder = Path.Combine(packageInfo.resolvedPath, "LocalStreamingAssets/Linux");
                string targetFolder = Application.streamingAssetsPath;

                CopyDirectory(sourceFolder, targetFolder);
            }
#endif
        }

        private bool SetPluginEnabled(PluginImporter pluginImporter, BuildTarget platform, bool enabled)
        {
            bool dirty = false;

            if (pluginImporter.GetExcludeFromAnyPlatform(platform) == enabled)
            {
                pluginImporter.SetExcludeFromAnyPlatform(platform, !enabled);
                dirty = true;
            }

            if (pluginImporter.GetCompatibleWithPlatform(platform) != enabled)
            {
                pluginImporter.SetCompatibleWithPlatform(platform, enabled);
                dirty = true;
            }

            return dirty;
        }

        private void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyDirectory(diSource, diTarget);
        }

        private void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                if (fi.Extension == ".meta") continue;

                string targetPath = Path.Combine(target.FullName, fi.Name);

                try
                {
                    fi.CopyTo(targetPath, true);
                }
                catch (IOException)
                {
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}