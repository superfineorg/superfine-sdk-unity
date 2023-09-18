using Superfine.Unity;

using System.IO;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

public class PreprocessBuildWithReport : IPreprocessBuildWithReport
{
    public int callbackOrder => int.MaxValue;

    public void OnPreprocessBuild(BuildReport report)
    {
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