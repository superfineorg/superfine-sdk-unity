using UnityEngine;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if UNITY_IOS || UNITY_STANDALONE_OSX
using UnityEditor.iOS.Xcode;
#endif

using PackageInfo = UnityEditor.PackageManager.PackageInfo;

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Superfine.Unity
{
    public class PostprocessBuildWithReport : IPostprocessBuildWithReport
    {
        private const string DEFAULT_ADVERTISING_ATTRIBUTION_REPORT_ENDPOINT = "https://superfine.org";
        private const string DEFAULT_USER_TRACKING_USAGE_DESCRIPTION = "We use your data to serve you more relevant ads and improve your experience.";

        public int callbackOrder => int.MaxValue;

        private string pathToBuiltProject = string.Empty;

#if UNITY_IOS || UNITY_STANDALONE_OSX
        private string projectPath = string.Empty;

        private string mainTarget = string.Empty;
        private string unityFrameworkTarget = string.Empty;

        private string entitlementsFileName = string.Empty;

        private string GetProjectPath(string pathToBuiltProject)
        {
#if UNITY_STANDALONE_OSX
            if (!UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                return null;
            }
#endif

#if UNITY_IOS
            return PBXProject.GetPBXProjectPath(pathToBuiltProject);
#else
            return $"{pathToBuiltProject}/{Path.GetFileName(pathToBuiltProject)}.xcodeproj/project.pbxproj";
#endif
        }

        private PBXProject GetProject()
        {
            if (string.IsNullOrEmpty(projectPath)) return null;

            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);

            return project;
        }
#endif

        public void OnPostprocessBuild(BuildReport report)
        {
            pathToBuiltProject = report.summary.outputPath;

            SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();

#if UNITY_IOS || UNITY_STANDALONE_OSX
            projectPath = GetProjectPath(pathToBuiltProject);

            PBXProject project = GetProject();

            mainTarget = string.Empty;
            unityFrameworkTarget = string.Empty;

            if (project != null)
            {
#if UNITY_IOS
                var unityMainTargetGuidMethod = project.GetType().GetMethod("GetUnityMainTargetGuid");
                var unityFrameworkTargetGuidMethod = project.GetType().GetMethod("GetUnityFrameworkTargetGuid");

                if (unityMainTargetGuidMethod != null && unityFrameworkTargetGuidMethod != null)
                {
                    mainTarget = (string)unityMainTargetGuidMethod.Invoke(project, null);
                    unityFrameworkTarget = (string)unityFrameworkTargetGuidMethod.Invoke(project, null);
                }
                else
                {
                    mainTarget = project.TargetGuidByName("Unity-iPhone");
                }
#else
                mainTarget = project.TargetGuidByName(Application.productName);
#endif

                entitlementsFileName = project.GetBuildPropertyForAnyConfig(mainTarget, "CODE_SIGN_ENTITLEMENTS");
                if (string.IsNullOrEmpty(entitlementsFileName))
                {
                    entitlementsFileName = Application.productName + ".entitlements";
                }
            }

#if UNITY_STANDALONE_OSX
            if (!UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject)
            {
                UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(pathToBuiltProject + "/Contents/PlugIns/superfine-sdk-cpp.bundle");
                UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(pathToBuiltProject);

                string plistPath = pathToBuiltProject + "/Contents/Info.plist";
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                UpdateInfoPlist(plist, settings);

                File.WriteAllText(plistPath, plist.WriteToString());
            }
#endif

            UpdateCapabilities(settings);
            UpdatePreprocessor(settings);
#endif
        }

#if UNITY_IOS
        SuperfineSDKSettings.ApsEnvironment? GetIosApsEnvironment(SuperfineSDKSettings settings)
        {
            if (settings == null) return null;

            SuperfineSDKSettings.ApsEnvironment? ret = settings.iosCapabilityPushNotifications;

            if (ret != null) return ret;

            bool registerRemote = false;
            
            List<SuperfineSDKIosInitializationActionSettings> actions = settings.iosInitializationActions;
            if (actions != null)
            {
                int numActions = actions.Count;
                for (int i = 0; i < numActions; ++i)
                {
                    SuperfineSDKIosInitializationActionSettings action = actions[i];
                    if (action.type == IosInitializationActionType.REQUEST_NOTIFICATION_AUTHORIZATION)
                    {
                        SuperfinerSDKIosNotificationAuthorizationRequest request = action.notificationAuthorizationRequest;
                        if (request != null && request.registerRemote)
                        {
                            registerRemote = true;
                            break;
                        }
                    }
                }
            }

            if (registerRemote)
            {
                return SuperfineSDKSettings.ApsEnvironment.DEVELOPMENT;
            }
            else
            {
                return null;
            }
        }
#endif


#if UNITY_IOS || UNITY_STANDALONE_OSX
        private void UpdateCapabilities(SuperfineSDKSettings settings)
        {
            if (string.IsNullOrEmpty(projectPath) || string.IsNullOrEmpty(entitlementsFileName)) return;

#if UNITY_IOS
            string targetName = "Unity-iPhone";
#else
            string targetName = Application.productName;
#endif

            var capManager = new SuperfineProjectCapabilityManager(projectPath, entitlementsFileName, targetName);

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

#if UNITY_IOS
                SuperfineSDKSettings.ApsEnvironment? apsEnvironment = GetIosApsEnvironment(settings);

                if (apsEnvironment != null)
                {
                    switch (apsEnvironment.Value)
                    {
                        case SuperfineSDKSettings.ApsEnvironment.DEVELOPMENT:
                            capManager.AddPushNotifications(true);
                            break;

                        case SuperfineSDKSettings.ApsEnvironment.PRODUCTION:
                            capManager.AddPushNotifications(false);
                            break;

                        default:
                            break;
                    }

                    capManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
                }

                if (settings.iosCapabilitySilentNotifications)
                {
                    capManager.AddSilentNotifications();
                }

                if (settings.iosCapabilityTimeSensitiveNotifications)
                {
                    capManager.AddTimeSensitiveNotifications();
                }

                if (settings.iosCapabilityInAppPurchase)
                {
                    capManager.AddInAppPurchase();
                }

                if (settings.iosCapabilitySignInWithApple)
                {
                    capManager.AddSignInWithApple();
                }

                if (settings.iosCapabilityGameCenter)
                {
                    capManager.AddGameCenter();
                }

                if (settings.iosCapabilityHomeKit)
                {
                    capManager.AddHomeKit();
                }

                if (settings.iosCapabilityDataProtection)
                {
                    capManager.AddDataProtection();
                }

                if (settings.iosCapabilityHealthKit)
                {
                    capManager.AddHealthKit();
                }

                if (settings.iosCapabilitySiri)
                {
                    capManager.AddSiri();
                }
#endif

#if UNITY_STANDALONE_OSX
                if (settings.macosCapabilityAppSandbox)
                {
                    capManager.AddAppSandbox(settings.macosAppSandboxSettings);
                }

                SuperfineSDKSettings.ApsEnvironment? apsEnvironment = settings.macosCapabilityPushNotifications;
                if (apsEnvironment != null)
                {
                    switch (apsEnvironment.Value)
                    {
                        case SuperfineSDKSettings.ApsEnvironment.DEVELOPMENT:
                            capManager.AddPushNotifications(true);
                            break;

                        case SuperfineSDKSettings.ApsEnvironment.PRODUCTION:
                            capManager.AddPushNotifications(false);
                            break;

                        default:
                            break;
                    }
                }

                if (settings.macosCapabilitySilentNotifications)
                {
                    capManager.AddSilentNotifications();
                }

                if (settings.macosCapabilityTimeSensitiveNotifications)
                {
                    capManager.AddTimeSensitiveNotifications();
                }

                if (settings.macosCapabilityInAppPurchase)
                {
                    capManager.AddInAppPurchase();
                }

                if (settings.macosCapabilitySignInWithApple)
                {
                    capManager.AddSignInWithApple();
                }

                if (settings.macosCapabilityGameCenter)
                {
                    capManager.AddGameCenter();
                }
#endif
            }

            UpdateInfoPlist(capManager.GetOrCreateInfoDoc(), settings);

            PBXProject project = capManager.Project;
            if (project != null)
            {
                UpdatePBXProject(project, settings);
            }

            capManager.WriteToFile();
        }

        private void UpdatePBXProject(PBXProject project, SuperfineSDKSettings settings)
        {
            if (project == null) return;

#if UNITY_IOS
            project.SetBuildProperty(mainTarget, "ENABLE_BITCODE", "NO");

            if (!string.IsNullOrEmpty(unityFrameworkTarget))
            {
                project.SetBuildProperty(unityFrameworkTarget, "ENABLE_BITCODE", "NO");
            }

            if (settings != null)
            {
                if (settings.iosFrameworkAdSupport)
                {
                    project.AddFrameworkToProject(mainTarget, "AdSupport.framework", true);
                }

                if (settings.iosFrameworkAdServices)
                {
                    project.AddFrameworkToProject(mainTarget, "AdServices.framework", true);
                }
                                
                if (settings.iosFrameworkStoreKit)
                {
                    project.AddFrameworkToProject(mainTarget, "StoreKit.framework", true);
                }

                if (settings.iosFrameworkAppTrackingTransparency)
                {
                    project.AddFrameworkToProject(mainTarget, "AppTrackingTransparency.framework", true);
                }

                if (settings.iosFrameworkAuthenticationServices)
                {
                    project.AddFrameworkToProject(mainTarget, "AuthenticationServices.framework", true);
                }

                if (settings.iosFrameworkUserNotifications)
                {
                    project.AddFrameworkToProject(mainTarget, "UserNotifications.framework", true);
                }

                if (settings.iosFrameworkCoreLocation)
                {
                    project.AddFrameworkToProject(mainTarget, "CoreLocation.framework", true);
                }

                if (settings.iosFrameworkCoreTelephony)
                {
                    project.AddFrameworkToProject(mainTarget, "CoreTelephony.framework", true);
                }

                if (settings.iosFrameworkSecurity)
                {
                    project.AddFrameworkToProject(mainTarget, "Security.framework", true);
                }

                if (settings.iosLibZ)
                {
                    if (!string.IsNullOrEmpty(unityFrameworkTarget))
                    {
                        project.AddFrameworkToProject(unityFrameworkTarget, "libz.tbd", true);
                    }
                    else
                    {
                        project.AddFrameworkToProject(mainTarget, "libz.tbd", true);
                    }
                }

                if (settings.iosLibSqlite3)
                {
                    if (!string.IsNullOrEmpty(unityFrameworkTarget))
                    {
                        project.AddFrameworkToProject(unityFrameworkTarget, "libsqlite3.tbd", true);
                    }
                    else
                    {
                        project.AddFrameworkToProject(mainTarget, "libsqlite3.tbd", true);
                    }
                }
            }
#else
            string teamId = UnityEditor.PlayerSettings.iOS.appleDeveloperTeamID;
            if (!string.IsNullOrEmpty(teamId))
            {
                project.SetTeamId(mainTarget, teamId);
            }
#endif
        }

        private string StandardizeSKAdNetworkId(string networkId)
        {
            if (string.IsNullOrEmpty(networkId)) return null;

            networkId = networkId.ToLower().Trim();

            int len = networkId.Length;
            if (len == 0) return null;

            if (networkId.EndsWith(".skadnetwork"))
            {
                if (len <= 12) return null;
                networkId = networkId.Substring(0, len - 12);
            }

            return networkId;
        }

        private void UpdateInfoPlist(PlistDocument infoPlist, SuperfineSDKSettings settings)
        {
            if (infoPlist == null) return;

            PlistElementDict rootDict = infoPlist.root;

            if (settings != null)
            {
#if UNITY_IOS
                if (settings.setAdvertisingAttributionReportEndpoint)
                {
                    string advertisingAttributionReportEndpoint = settings.advertisingAttributionReportEndpoint;
                    if (string.IsNullOrEmpty(advertisingAttributionReportEndpoint))
                    {
                        advertisingAttributionReportEndpoint = DEFAULT_ADVERTISING_ATTRIBUTION_REPORT_ENDPOINT;
                    }

                    rootDict.SetString("NSAdvertisingAttributionReportEndpoint", advertisingAttributionReportEndpoint);
                }

                if (settings.setUserTrackingUsageDescription)
                {
                    string userTrackingUsageDescription = settings.userTrackingUsageDescription;
                    if (string.IsNullOrEmpty(userTrackingUsageDescription))
                    {
                        userTrackingUsageDescription = DEFAULT_USER_TRACKING_USAGE_DESCRIPTION;
                    }

                    rootDict.SetString("NSUserTrackingUsageDescription", userTrackingUsageDescription);
                }
#endif

#if UNITY_STANDALONE_OSX
                string appDataUsageDescription = settings.appDataUsageDescription;
                if (!string.IsNullOrEmpty(appDataUsageDescription))
                {
                    rootDict.SetString("NSAppDataUsageDescription", appDataUsageDescription);
                }
#endif
            }

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
            HashSet<string> skadIds = new HashSet<string>();

            bool disableDefaultSKAdNetworkIdFile = false;
            TextAsset[] extraSKAdNetworkIdFiles = null;

            if (settings != null)
            {
                disableDefaultSKAdNetworkIdFile = settings.disableDefaultSKAdNetworkIdFile;
                extraSKAdNetworkIdFiles = settings.extraSKAdNetworkIdFiles;
            }

            if (!disableDefaultSKAdNetworkIdFile)
            {
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
                                string id = StandardizeSKAdNetworkId(ids[j]);
                                if (string.IsNullOrEmpty(id)) continue;

                                skadIds.Add(id);
                            }
                        }
                    }
                }
            }

            if (extraSKAdNetworkIdFiles != null)
            {
                int numFiles = extraSKAdNetworkIdFiles.Length;
                for (int i = 0; i < numFiles; ++i)
                {
                    TextAsset file = extraSKAdNetworkIdFiles[i];
                    if (file == null) continue;

                    string content = file.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        string[] ids = content.Split('\n');

                        int numIds = ids.Length;
                        for (int j = 0; j < numIds; ++j)
                        {
                            string id = StandardizeSKAdNetworkId(ids[j]);
                            if (string.IsNullOrEmpty(id)) continue;

                            skadIds.Add(id);
                        }
                    }
                }
            }

            if (skadIds.Count > 0)
            {
                var skadArray = rootDict.CreateArray("SKAdNetworkItems");
                foreach (string id in skadIds)
                {
                    skadArray.AddDict().SetString("SKAdNetworkIdentifier", id + ".skadnetwork");
                }
            }
#endif
        }

        private void UpdatePreprocessor(SuperfineSDKSettings settings)
        {
            string preprocessorPath = pathToBuiltProject + "/Classes/Preprocessor.h";
            string preprocessor = File.ReadAllText(preprocessorPath);

            bool dirty = false;

            if (settings != null)
            {
#if UNITY_IOS
                SuperfineSDKSettings.ApsEnvironment? apsEnvironment = GetIosApsEnvironment(settings);

                if (apsEnvironment.HasValue)
                {
                    if (preprocessor.Contains("UNITY_USES_REMOTE_NOTIFICATIONS"))
                    {
                        preprocessor = preprocessor.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
                        dirty = true;
                    }
                }
#endif
            }

            if (dirty)
            {
                File.WriteAllText(preprocessorPath, preprocessor);
            }
        }
#endif
            }
        }