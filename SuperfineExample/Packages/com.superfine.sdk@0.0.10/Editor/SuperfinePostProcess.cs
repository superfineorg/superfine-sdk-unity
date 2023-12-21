using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS || UNITY_STANDALONE_OSX
using UnityEditor.iOS.Xcode;
#endif

using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Superfine.Unity
{
    public class SuperfinePostProcess
    {
        [PostProcessBuild(int.MaxValue)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
#if UNITY_IOS || UNITY_STANDALONE_OSX
            UpdateCapabilities(pathToBuiltProject);
            UpdatePBXProject(pathToBuiltProject);
            UpdateInfoPlist(pathToBuiltProject);
#endif
        }

#if UNITY_IOS || UNITY_STANDALONE_OSX
        private static void UpdateCapabilities(string pathToBuiltProject)
        {

#if UNITY_STANDALONE_OSX
            if (!UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                return;
            }
#endif


#if UNITY_IOS
            string targetName = "Unity-iPhone";
#else
            string targetName = Application.productName;
#endif

#if UNITY_IOS
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
#else
            string projectPath = $"{pathToBuiltProject}/{Path.GetFileName(pathToBuiltProject)}.xcodeproj/project.pbxproj";
#endif

            string entitlementsFileName = Application.productName + ".entitlements";

            var capManager = new ProjectCapabilityManager(projectPath, entitlementsFileName, targetName);

            SuperfineSDKSettings settings = SuperfineSDKSettings.NullableInstance;
            if (settings != null && settings.associatedDomains != null)
            {
                int numAssociatedDomains = settings.associatedDomains.Length;
                if (numAssociatedDomains > 0)
                {
                    string[] keys = new string[numAssociatedDomains];
                    for (int i = 0; i < numAssociatedDomains; ++i)
                    {
                        keys[i] = "applinks:" + settings.associatedDomains[i];
                    }

                    capManager.AddAssociatedDomains(keys);
                }
            }

            capManager.WriteToFile();

#if UNITY_STANDALONE_OSX
            var plist = new PlistDocument();

            string entitlementsFilePath = Path.Combine(Directory.GetParent(Path.GetDirectoryName(projectPath)).FullName, entitlementsFileName);
            plist.ReadFromFile(entitlementsFilePath);

            plist.root.SetBoolean("com.apple.security.app-sandbox", true);
            plist.root.SetBoolean("com.apple.security.network.client", true);
            /*
            plist.root.SetBoolean("com.apple.security.assets.movies.read-write", true);
            plist.root.SetBoolean("com.apple.security.assets.music.read-write", true);
            plist.root.SetBoolean("com.apple.security.assets.pictures.read-write", true);
            plist.root.SetBoolean("com.apple.security.files.downloads.read-write", true);
            plist.root.SetBoolean("com.apple.security.files.user-selected.read-write", true);
            */

            plist.WriteToFile(entitlementsFilePath);
#endif
        }

        private static void UpdatePBXProject(string pathToBuiltProject)
        {
#if UNITY_STANDALONE_OSX
            if (!UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                return;
            }
#endif

#if UNITY_IOS
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
#else
            string projectPath = $"{pathToBuiltProject}/{Path.GetFileName(pathToBuiltProject)}.xcodeproj/project.pbxproj";
#endif

            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projectPath);

#if UNITY_IOS
            string targetGUID = proj.GetUnityMainTargetGuid();
#else
            string targetGUID = proj.TargetGuidByName(PlayerSettings.productName);
#endif

#if UNITY_IOS
            string frameworkTargetGUID = proj.GetUnityFrameworkTargetGuid();

            proj.SetBuildProperty(targetGUID, "ENABLE_BITCODE", "NO");
            proj.SetBuildProperty(frameworkTargetGUID, "ENABLE_BITCODE", "NO");
#else
            string teamId = PlayerSettings.iOS.appleDeveloperTeamID;
            if (!string.IsNullOrEmpty(teamId))
            {
                proj.SetTeamId(targetGUID, teamId);
            }
#endif

            proj.WriteToFile(projectPath);
        }

        private static void UpdateInfoPlist(string pathToBuiltProject)
        {
            string plistPath;

#if UNITY_IOS
            plistPath = pathToBuiltProject + "/Info.plist";
#else
            if (UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                plistPath = pathToBuiltProject + "/" + PlayerSettings.productName + "/Info.plist";
            }
            else
            {
                plistPath = pathToBuiltProject + "/Contents/Info.plist";
            }
#endif
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            SuperfineSDKSettings settings = SuperfineSDKSettings.NullableInstance;

#if UNITY_IOS
            string userTrackingUsageDescription = string.Empty;
            string advertisingAttributionReportEndpoint = string.Empty;

            if (settings != null)
            {
                userTrackingUsageDescription = settings.userTrackingUsageDescription;
                advertisingAttributionReportEndpoint =  settings.advertisingAttributionReportEndpoint;
            }

            if (string.IsNullOrEmpty(userTrackingUsageDescription))
            {
                userTrackingUsageDescription = "We use your data to serve you more relevant ads and improve your experience.";
            }
                        
            if (string.IsNullOrEmpty(advertisingAttributionReportEndpoint))
            {
                advertisingAttributionReportEndpoint = "https://superfine.org";
            }

            rootDict.SetString("NSUserTrackingUsageDescription", userTrackingUsageDescription);
            rootDict.SetString("NSAdvertisingAttributionReportEndpoint", advertisingAttributionReportEndpoint);
#endif

            if (settings != null && settings.uriSchemes != null)
            {
                int numUriSchemes = settings.uriSchemes.Length;
                if (numUriSchemes > 0)
                {
                    var urlTypeArray = rootDict.CreateArray("CFBundleURLTypes");
                    PlistElementDict dict = urlTypeArray.AddDict();
                    dict.SetString("CFBundleTypeRole", "Editor");
                    dict.SetString("CFBundleURLName", Application.identifier);
                    var uriSchemeArray = dict.CreateArray("CFBundleURLSchemes");
                    for (int i = 0; i < numUriSchemes; ++i)
                    {
                        uriSchemeArray.AddString(settings.uriSchemes[i]);
                    }
                }
            }

#if UNITY_IOS
            List<string> skadPathList = new List<string>();

            PackageInfo packageInfo = SuperfineSDK.GetSelfPackageInfo();
            if (packageInfo != null)
            {
                var path = packageInfo.resolvedPath;
                skadPathList.AddRange(Directory.EnumerateFiles(path, "skadnetworks.txt", SearchOption.AllDirectories).ToList());
            }

            int numSkadPaths = skadPathList.Count;

            if (numSkadPaths > 0)
            {
                var skadArray = rootDict.CreateArray("SKAdNetworkItems");

                for (int i = 0; i < numSkadPaths; ++i)
                {
                    string path = skadPathList[i];

                    string content = File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(content))
                    {
                        string[] ids = content.Split('\n');

                        int numIds = ids.Length;
                        for (int j = 0; j < numIds; ++j)
                        {
                            if (string.IsNullOrEmpty(ids[j])) continue;
                            skadArray.AddDict().SetString("SKAdNetworkIdentifier", ids[i]);
                        }
                    }
                }
            }
#endif

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
#endif
    }
}
