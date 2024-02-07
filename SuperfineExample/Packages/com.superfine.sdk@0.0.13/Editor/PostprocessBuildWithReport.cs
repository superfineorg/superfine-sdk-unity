using UnityEngine;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

#if UNITY_IOS || UNITY_STANDALONE_OSX
using UnityEditor.iOS.Xcode;
#endif

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Superfine.Unity
{
    public class PostprocessBuildWithReport : IPostprocessBuildWithReport
    {
        public int callbackOrder => int.MaxValue;
        public void OnPostprocessBuild(BuildReport report)
        {
            string pathToBuiltProject = report.summary.outputPath;

            SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();

#if UNITY_IOS || UNITY_STANDALONE_OSX
            UpdateCapabilities(pathToBuiltProject, settings);
            UpdatePBXProject(pathToBuiltProject, settings);
            UpdateInfoPlist(pathToBuiltProject, settings);
#endif
        }

#if UNITY_IOS || UNITY_STANDALONE_OSX
        private static void UpdateCapabilities(string pathToBuiltProject, SuperfineSDKSettings settings)
        {

#if UNITY_STANDALONE_OSX
            if (!UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(pathToBuiltProject + "/Contents/PlugIns/superfine-sdk-cpp.bundle");
                UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(pathToBuiltProject);

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

            if (settings != null)
            {
                List<string> associatedDomains = settings.GetAssociatedDomains();
                int numAssociatedDomains = associatedDomains != null ? associatedDomains.Count : 0;
                if (numAssociatedDomains > 0)
                {
                    string[] keys = new string[numAssociatedDomains];
                    for (int i = 0; i < numAssociatedDomains; ++i)
                    {
                        keys[i] = "applinks:" + associatedDomains[i];
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

        private static void UpdatePBXProject(string pathToBuiltProject, SuperfineSDKSettings settings)
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

            if (settings != null)
            {
                if (settings.iosFrameworkAdSupport)
                {
                    proj.AddFrameworkToProject(targetGUID, "AdSupport.framework", true);
                }

                if (settings.iosFrameworkAdServices)
                {
                    proj.AddFrameworkToProject(targetGUID, "AdServices.framework", true);
                }
                                
                if (settings.iosFrameworkStoreKit)
                {
                    proj.AddFrameworkToProject(targetGUID, "StoreKit.framework", true);
                }

                if (settings.iosFrameworkAppTrackingTransparency)
                {
                    proj.AddFrameworkToProject(targetGUID, "AppTrackingTransparency.framework", true);
                }

                if (settings.iosFrameworkAuthenticationServices)
                {
                    proj.AddFrameworkToProject(targetGUID, "AuthenticationServices.framework", true);
                }

                if (settings.iosFrameworkUserNotifications)
                {
                    proj.AddFrameworkToProject(targetGUID, "UserNotifications.framework", true);
                }

                if (settings.iosFrameworkCoreTelephony)
                {
                    proj.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", true);
                }

                if (settings.iosFrameworkSecurity)
                {
                    proj.AddFrameworkToProject(targetGUID, "Security.framework", true);
                }
            }
#else
            string teamId = PlayerSettings.iOS.appleDeveloperTeamID;
            if (!string.IsNullOrEmpty(teamId))
            {
                proj.SetTeamId(targetGUID, teamId);
            }
#endif

            proj.WriteToFile(projectPath);
        }

        private static void UpdateInfoPlist(string pathToBuiltProject, SuperfineSDKSettings settings)
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

            if (settings != null)
            {
                List<string> urlSchemes = settings.GetCustomSchemes();

                int numUriSchemes = urlSchemes != null ? urlSchemes.Count : 0;
                if (numUriSchemes > 0)
                {
                    var urlTypeArray = rootDict.CreateArray("CFBundleURLTypes");
                    PlistElementDict dict = urlTypeArray.AddDict();
                    dict.SetString("CFBundleTypeRole", "Editor");
                    dict.SetString("CFBundleURLName", Application.identifier);
                    var uriSchemeArray = dict.CreateArray("CFBundleURLSchemes");
                    for (int i = 0; i < numUriSchemes; ++i)
                    {
                        uriSchemeArray.AddString(urlSchemes[i]);
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