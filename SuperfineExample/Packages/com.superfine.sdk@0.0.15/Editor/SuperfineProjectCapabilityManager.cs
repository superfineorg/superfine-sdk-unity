#if UNITY_IOS || UNITY_STANDALONE_OSX
using UnityEditor.iOS.Xcode;

using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Superfine.Unity
{ 
    public class SuperfineProjectCapabilityManager
    {
        private const BindingFlags NonPublicInstanceBinding = BindingFlags.NonPublic | BindingFlags.Instance;
        private const BindingFlags PublicInstanceBinding = BindingFlags.Public | BindingFlags.Instance;

        public class ICloudEntitlements
        {
            public static readonly string ContainerIdKey = "com.apple.developer.icloud-container-identifiers";
            public static readonly string ContainerIdValue = "iCloud.$(CFBundleIdentifier)";
            public static readonly string UbiquityContainerIdKey = "com.apple.developer.ubiquity-container-identifiers";
            public static readonly string UbiquityContainerIdValue = "iCloud.$(CFBundleIdentifier)";
            public static readonly string ServicesKey = "com.apple.developer.icloud-services";
            public static readonly string ServicesDocValue = "CloudDocuments";
            public static readonly string ServicesKitValue = "CloudKit";
            public static readonly string KeyValueStoreKey = "com.apple.developer.ubiquity-kvstore-identifier";
            public static readonly string KeyValueStoreValue = "$(TeamIdentifierPrefix)$(CFBundleIdentifier)";
        }

        public class MapsInfo
        {
            public static readonly string BundleKey = "CFBundleDocumentTypes";
            public static readonly string BundleNameKey = "CFBundleTypeName";
            public static readonly string BundleNameValue = "MKDirectionsRequest";
            public static readonly string BundleTypeKey = "LSItemContentTypes";
            public static readonly string BundleTypeValue = "com.apple.maps.directionsrequest";
            public static readonly string ModeKey = "MKDirectionsApplicationSupportedModes";
            public static readonly string ModePlaneValue = "MKDirectionsModePlane";
            public static readonly string ModeBikeValue = "MKDirectionsModeBike";
            public static readonly string ModeBusValue = "MKDirectionsModeBus";
            public static readonly string ModeCarValue = "MKDirectionsModeCar";
            public static readonly string ModeFerryValue = "MKDirectionsModeFerry";
            public static readonly string ModeOtherValue = "MKDirectionsModeOther";
            public static readonly string ModePedestrianValue = "MKDirectionsModePedestrian";
            public static readonly string ModeRideShareValue = "MKDirectionsModeRideShare";
            public static readonly string ModeStreetCarValue = "MKDirectionsModeStreetCar";
            public static readonly string ModeSubwayValue = "MKDirectionsModeSubway";
            public static readonly string ModeTaxiValue = "MKDirectionsModeTaxi";
            public static readonly string ModeTrainValue = "MKDirectionsModeTrain";
        }

        public class PushNotificationEntitlements
        {
            public static readonly string Key = "aps-environment";
            public static readonly string DevelopmentValue = "development";
            public static readonly string ProductionValue = "production";
        }

        public class SilentNotificationEntitlements
        {
            public static readonly string Key = "com.apple.developer.usernotifications.filtering";
        }

        public class TimeSensitiveNotificationEntitlements
        {
            public static readonly string Key = "com.apple.developer.usernotifications.time-sensitive";
        }

        public class GameCenterEntitlements
        {
            public static readonly string Key = "com.apple.developer.game-center";
        }

        public class GameCenterInfo
        {
            public static readonly string Key = "UIRequiredDeviceCapabilities";
            public static readonly string Value = "gamekit";
        }

        public class WalletEntitlements
        {
            public static readonly string Key = "com.apple.developer.pass-type-identifiers";
            public static readonly string BaseValue = "$(TeamIdentifierPrefix)";
            public static readonly string DefaultValue = "*";
        }

        public class SiriEntitlements
        {
            public static readonly string Key = "com.apple.developer.siri";
        }

        public class ApplePayEntitlements
        {
            public static readonly string Key = "com.apple.developer.in-app-payments";
        }

        public class VPNEntitlements
        {
            public static readonly string Key = "com.apple.developer.networking.vpn.api";
            public static readonly string Value = "allow-vpn";
        }

        public class BackgroundInfo
        {
            public static readonly string Key = "UIBackgroundModes";
            public static readonly string ModeAudioValue = "audio";
            public static readonly string ModeLocationValue = "location";
            public static readonly string ModeVOIPValue = "voip";
            public static readonly string ModeExtAccessoryValue = "external-accessory";
            public static readonly string ModeBluetoothValue = "bluetooth-central";
            public static readonly string ModeActsBluetoothValue = "bluetooth-peripheral";
            public static readonly string ModeFetchValue = "fetch";
            public static readonly string ModePushValue = "remote-notification";
            public static readonly string ModeProcessingValue = "processing";
            public static readonly string ModeNewsstandValue = "newsstand-content";
        }

        public class KeyChainEntitlements
        {
            public static readonly string Key = "keychain-access-groups";
            public static readonly string DefaultValue = "$(AppIdentifierPrefix)$(CFBundleIdentifier)";
        }

        public class AudioEntitlements
        {
            public static readonly string Key = "inter-app-audio";
        }

        public class AssociatedDomainsEntitlements
        {
            public static readonly string Key = "com.apple.developer.associated-domains";
        }

        public class AppGroupsEntitlements
        {
            public static readonly string Key = "com.apple.security.application-groups";
        }

        public class HomeKitEntitlements
        {
            public static readonly string Key = "com.apple.developer.homekit";
        }

        public class DataProtectionEntitlements
        {
            public static readonly string Key = "com.apple.developer.default-data-protection";
            public static readonly string Value = "NSFileProtectionComplete";
        }

        public class HealthKitEntitlements
        {
            public static readonly string Key = "com.apple.developer.healthkit";
        }

        public class HealthInfo
        {
            public static readonly string Key = "UIRequiredDeviceCapabilities";
            public static readonly string Value = "healthkit";
        }

        public class WirelessAccessoryConfigurationEntitlements
        {
            public static readonly string Key = "com.apple.external-accessory.wireless-configuration";
        }

        public class AccessWiFiInformationEntitlements
        {
            public static readonly string Key = "com.apple.developer.networking.wifi-info";
        }

        public class SignInWithAppleEntitlements
        {
            public static readonly string Key = "com.apple.developer.applesignin";
            public static readonly string Value = "Default";
        }

        public class AppSandboxEntitlements
        {
            public static readonly string Key = "com.apple.security.app-sandbox";
            public static readonly string NetworkServerKey = "com.apple.security.network.server";
            public static readonly string NetworkClientKey = "com.apple.security.network.client";
            public static readonly string HardwareCameraKey = "com.apple.security.device.camera";
            public static readonly string HardwareAudioInputKey = "com.apple.security.device.microphone";
            public static readonly string HardwareUsbKey = "com.apple.security.device.usb";
            public static readonly string HardwarePrintingKey = "com.apple.security.print";
            public static readonly string HardwareBluetoothKey = "com.apple.security.device.bluetooth";
            public static readonly string AppDataContactsKey = "com.apple.security.personal-information.addressbook";
            public static readonly string AppDataLocationKey = "com.apple.security.personal-information.location";
            public static readonly string AppDataCalendarKey = "com.apple.security.personal-information.calendars";
            public static readonly string FileAccessUserSelectedFileROKey = "com.apple.security.files.user-selected.read-only";
            public static readonly string FileAccessUserSelectedFileRWKey = "com.apple.security.files.user-selected.read-write";
            public static readonly string FileAccessDownloadFoldersROKey = "com.apple.security.files.downloads.read-only";
            public static readonly string FileAccessDownloadFoldersRWKey = "com.apple.security.files.downloads.read-write";
            public static readonly string FileAccessPicturesFolderROKey = "com.apple.security.assets.pictures.read-only";
            public static readonly string FileAccessPicturesFolderRWKey = "com.apple.security.assets.pictures.read-write";
            public static readonly string FileAccessMusicFolderROKey = "com.apple.security.assets.music.read-only";
            public static readonly string FileAccessMusicFolderRWKey = "com.apple.security.assets.music.read-write";
            public static readonly string FileAccessMoviesFolderROKey = "com.apple.security.assets.movies.read-only";
            public static readonly string FileAccessMoviesFolderRWKey = "com.apple.security.assets.movies.read-write";
        }

        class PBXPath
        {
            public static string FixSlashes(string path)
            {
                return path?.Replace('\\', '/');
            }

            public static void Combine(string path1, PBXSourceTree tree1, string path2, PBXSourceTree tree2, out string resPath, out PBXSourceTree resTree)
            {
                if (tree2 == PBXSourceTree.Group)
                {
                    resPath = Combine(path1, path2);
                    resTree = tree1;
                }
                else
                {
                    resPath = path2;
                    resTree = tree2;
                }
            }

            public static string Combine(string path1, string path2)
            {
                if (path2.StartsWith("/"))
                {
                    return path2;
                }

                if (path1.EndsWith("/"))
                {
                    return path1 + path2;
                }

                if (path1 == "")
                {
                    return path2;
                }

                if (path2 == "")
                {
                    return path1;
                }

                return path1 + "/" + path2;
            }

            public static string GetDirectory(string path)
            {
                path = path.TrimEnd('/');
                int num = path.LastIndexOf('/');
                if (num == -1)
                {
                    return "";
                }

                return path.Substring(0, num);
            }

            public static string GetCurrentDirectory()
            {
                if (Environment.OSVersion.Platform != PlatformID.MacOSX && Environment.OSVersion.Platform != PlatformID.Unix)
                {
                    throw new Exception("PBX project compatible current directory can only obtained on OSX");
                }

                string currentDirectory = Directory.GetCurrentDirectory();
                currentDirectory = FixSlashes(currentDirectory);
                if (!IsPathRooted(currentDirectory))
                {
                    return "/" + currentDirectory;
                }

                return currentDirectory;
            }

            public static string GetFilename(string path)
            {
                int num = path.LastIndexOf('/');
                if (num == -1)
                {
                    return path;
                }

                return path.Substring(num + 1);
            }

            public static bool IsPathRooted(string path)
            {
                if (path == null || path.Length == 0)
                {
                    return false;
                }

                return path[0] == '/';
            }

            public static string GetFullPath(string path)
            {
                if (IsPathRooted(path))
                {
                    return path;
                }

                return Combine(GetCurrentDirectory(), path);
            }

            public static string[] Split(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return new string[0];
                }

                return path.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private string m_BuildPath;

        private string m_TargetGuid;

        private string m_PBXProjectPath;

        private string m_EntitlementFilePath;

        private PlistDocument m_Entitlements = null;

        private PlistDocument m_InfoPlist = null;

        private PBXProject project;

        public PlistDocument Entitlements => m_Entitlements;
        public PlistDocument InfoPlist => m_InfoPlist;
        public PBXProject Project => project;

        private void Create(PBXProject _project, string entitlementFilePath, string targetName = null, string targetGuid = null)
        {
            m_BuildPath = ((m_PBXProjectPath != null) ? Directory.GetParent(Path.GetDirectoryName(m_PBXProjectPath)).FullName : null);
            m_EntitlementFilePath = entitlementFilePath;
            project = _project;
            if (targetName == "Unity-iPhone")
            {
                targetName = null;
                targetGuid = project.GetUnityMainTargetGuid();
            }

            if ((targetName == null && targetGuid == null) || (targetName != null && targetGuid != null))
            {
                throw new Exception($"Invalid targets please specify only one of them targetName: {targetName} and targetGuid: {targetGuid} ");
            }

            if (targetName != null)
            {
                m_TargetGuid = project.TargetGuidByName(targetName);
                if (m_TargetGuid == null)
                {
                    throw new Exception($"Could not find target: {targetName} in xcode project.");
                }
            }
            else
            {
                m_TargetGuid = targetGuid;
            }
        }

        internal SuperfineProjectCapabilityManager(PBXProject _project, string entitlementFilePath, string targetName = null, string targetGuid = null)
        {
            Create(_project, entitlementFilePath, targetName, targetGuid);
        }

        public SuperfineProjectCapabilityManager(string pbxProjectPath, string entitlementFilePath, string targetName = null, string targetGuid = null)
        {
            m_PBXProjectPath = pbxProjectPath;
            project = new PBXProject();
            project.ReadFromString(File.ReadAllText(pbxProjectPath));
            Create(project, entitlementFilePath, targetName, targetGuid);
        }

        public void WriteToFile()
        {
            File.WriteAllText(m_PBXProjectPath, project.WriteToString());
            if (m_Entitlements != null)
            {
                m_Entitlements.WriteToFile(PBXPath.Combine(m_BuildPath, m_EntitlementFilePath));
            }

            if (m_InfoPlist != null)
            {
                m_InfoPlist.WriteToFile(PBXPath.Combine(m_BuildPath, "Info.plist"));
            }
        }

        public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, string[] customContainers)
        {
            AddiCloud(enableKeyValueStorage, enableiCloudDocument, enablecloudKit: true, addDefaultContainers: true, customContainers);
        }

        public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, bool enablecloudKit, bool addDefaultContainers, string[] customContainers)
        {
            PlistDocument orCreateEntitlementDoc = GetOrCreateEntitlementDoc();
            PlistElement plistElement2 = (orCreateEntitlementDoc.root[ICloudEntitlements.ContainerIdKey] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            PlistElementArray plistElementArray2 = null;
            if (enableiCloudDocument || enablecloudKit)
            {
                plistElement2 = (orCreateEntitlementDoc.root[ICloudEntitlements.ServicesKey] = new PlistElementArray());
                plistElementArray2 = plistElement2 as PlistElementArray;
            }

            if (enableiCloudDocument)
            {
                plistElementArray.values.Add(new PlistElementString(ICloudEntitlements.ContainerIdValue));
                plistElementArray2.values.Add(new PlistElementString(ICloudEntitlements.ServicesDocValue));
                plistElement2 = (orCreateEntitlementDoc.root[ICloudEntitlements.UbiquityContainerIdKey] = new PlistElementArray());
                PlistElementArray plistElementArray3 = plistElement2 as PlistElementArray;
                if (addDefaultContainers)
                {
                    plistElementArray3.values.Add(new PlistElementString(ICloudEntitlements.UbiquityContainerIdValue));
                }

                if (customContainers != null && customContainers.Length != 0)
                {
                    for (int i = 0; i < customContainers.Length; i++)
                    {
                        plistElementArray3.values.Add(new PlistElementString(customContainers[i]));
                    }
                }
            }

            if (enablecloudKit)
            {
                if (addDefaultContainers && !enableiCloudDocument)
                {
                    plistElementArray.values.Add(new PlistElementString(ICloudEntitlements.ContainerIdValue));
                }

                if (customContainers != null && customContainers.Length != 0)
                {
                    for (int j = 0; j < customContainers.Length; j++)
                    {
                        plistElementArray.values.Add(new PlistElementString(customContainers[j]));
                    }
                }

                plistElementArray2.values.Add(new PlistElementString(ICloudEntitlements.ServicesKitValue));
            }

            if (enableKeyValueStorage)
            {
                orCreateEntitlementDoc.root[ICloudEntitlements.KeyValueStoreKey] = new PlistElementString(ICloudEntitlements.KeyValueStoreValue);
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.iCloud, m_EntitlementFilePath, enablecloudKit);
        }

        public void AddPushNotifications(bool development)
        {
            GetOrCreateEntitlementDoc().root[PushNotificationEntitlements.Key] = new PlistElementString(development ? PushNotificationEntitlements.DevelopmentValue : PushNotificationEntitlements.ProductionValue);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.PushNotifications, m_EntitlementFilePath);
        }

        public void AddSilentNotifications()
        {
            GetOrCreateEntitlementDoc().root[SilentNotificationEntitlements.Key] = new PlistElementBoolean(v: true);

#if UNITY_6000_0
            GetOrCreateEntitlementDoc().root["aps-environment"] = new PlistElementString("development");
            PBXCapabilityType capabilityType = PBXCapabilityType.BackgroundModes;
#elif UNITY_2022_3_OR_NEWER
            PBXCapabilityType capabilityType = new PBXCapabilityType(SilentNotificationEntitlements.Key, _requiresEntitlements: true);
#else
            // in old unity versions PBXCapabilityType had internal ctor; that was changed to public afterwards - try both
            var constructorInfo = GetPBXCapabilityTypeConstructor(PublicInstanceBinding) ??
                                  GetPBXCapabilityTypeConstructor(NonPublicInstanceBinding);
            PBXCapabilityType capabilityType = constructorInfo.Invoke(new object[] { SilentNotificationEntitlements.Key, true, string.Empty, true }) as PBXCapabilityType;
#endif
            project.AddCapability(m_TargetGuid, capabilityType, m_EntitlementFilePath);
        }

        public void AddTimeSensitiveNotifications()
        {
            GetOrCreateEntitlementDoc().root[TimeSensitiveNotificationEntitlements.Key] = new PlistElementBoolean(v: true);

#if UNITY_6000_0
            PBXCapabilityType capabilityType = PBXCapabilityType.TimeSensitiveNotifications;
#elif UNITY_2022_3_OR_NEWER
            PBXCapabilityType capabilityType = new PBXCapabilityType(TimeSensitiveNotificationEntitlements.Key, _requiresEntitlements: true);
#else
            // in old unity versions PBXCapabilityType had internal ctor; that was changed to public afterwards - try both
            var constructorInfo = GetPBXCapabilityTypeConstructor(PublicInstanceBinding) ??
                                  GetPBXCapabilityTypeConstructor(NonPublicInstanceBinding);
            PBXCapabilityType capabilityType = constructorInfo.Invoke(new object[] { TimeSensitiveNotificationEntitlements.Key, true, string.Empty, true }) as PBXCapabilityType;
#endif
            project.AddCapability(m_TargetGuid, capabilityType, m_EntitlementFilePath);
        }

        public void AddGameCenter()
        {
            GetOrCreateEntitlementDoc().root[GameCenterEntitlements.Key] = new PlistElementBoolean(v: true);
            PlistElementArray plistElementArray = (GetOrCreateInfoDoc().root[GameCenterInfo.Key] ?? (GetOrCreateInfoDoc().root[GameCenterInfo.Key] = new PlistElementArray())) as PlistElementArray;
            plistElementArray.values.Add(new PlistElementString(GameCenterInfo.Value));
            project.AddCapability(m_TargetGuid, PBXCapabilityType.GameCenter, m_EntitlementFilePath);
        }

        private void SetAppSandboxFileAccess(PlistDocument document, SuperfineSDKAppSandboxSettings.FileAccess fileAccess, string readOnlyKey, string readWriteKey)
        {
            IDictionary<string, PlistElement> values = document.root.values;

            switch (fileAccess)
            {
                case SuperfineSDKAppSandboxSettings.FileAccess.READ_ONLY:
                    values[readOnlyKey] = new PlistElementBoolean(v: true);
                    values.Remove(readWriteKey);
                    break;

                case SuperfineSDKAppSandboxSettings.FileAccess.READ_WRITE:
                    values.Remove(AppSandboxEntitlements.FileAccessUserSelectedFileROKey);
                    values[AppSandboxEntitlements.FileAccessUserSelectedFileRWKey] = new PlistElementBoolean(v: true);
                    break;

                default:
                    values.Remove(AppSandboxEntitlements.FileAccessUserSelectedFileROKey);
                    values.Remove(AppSandboxEntitlements.FileAccessUserSelectedFileRWKey);
                    break;
            }
        }

        public void AddAppSandbox(SuperfineSDKAppSandboxSettings settings)
        {
            PlistDocument orCreateEntitlementDoc = GetOrCreateEntitlementDoc();
            orCreateEntitlementDoc.root[AppSandboxEntitlements.Key] = new PlistElementBoolean(v: true);

            if (settings != null)
            {
                if (settings.incomingConnections)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.NetworkServerKey] = new PlistElementBoolean(v: true);
                }

                if (settings.outgoingConnections)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.NetworkClientKey] = new PlistElementBoolean(v: true);
                }

                if (settings.camera)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.HardwareCameraKey] = new PlistElementBoolean(v: true);
                }

                if (settings.audioInput)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.HardwareAudioInputKey] = new PlistElementBoolean(v: true);
                }

                if (settings.usb)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.HardwareUsbKey] = new PlistElementBoolean(v: true);
                }

                if (settings.printing)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.HardwarePrintingKey] = new PlistElementBoolean(v: true);
                }

                if (settings.bluetooth)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.HardwareBluetoothKey] = new PlistElementBoolean(v: true);
                }

                if (settings.contacts)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.AppDataContactsKey] = new PlistElementBoolean(v: true);
                }

                if (settings.location)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.AppDataLocationKey] = new PlistElementBoolean(v: true);
                }

                if (settings.calendar)
                {
                    orCreateEntitlementDoc.root[AppSandboxEntitlements.AppDataCalendarKey] = new PlistElementBoolean(v: true);
                }

                SetAppSandboxFileAccess(orCreateEntitlementDoc, settings.userSelectedFile, AppSandboxEntitlements.FileAccessUserSelectedFileROKey, AppSandboxEntitlements.FileAccessUserSelectedFileRWKey);
                SetAppSandboxFileAccess(orCreateEntitlementDoc, settings.downloadsFolder, AppSandboxEntitlements.FileAccessDownloadFoldersROKey, AppSandboxEntitlements.FileAccessDownloadFoldersRWKey);
                SetAppSandboxFileAccess(orCreateEntitlementDoc, settings.picturesFolder, AppSandboxEntitlements.FileAccessPicturesFolderROKey, AppSandboxEntitlements.FileAccessPicturesFolderRWKey);
                SetAppSandboxFileAccess(orCreateEntitlementDoc, settings.musicFolder, AppSandboxEntitlements.FileAccessMusicFolderROKey, AppSandboxEntitlements.FileAccessMusicFolderRWKey);
                SetAppSandboxFileAccess(orCreateEntitlementDoc, settings.moviesFolder, AppSandboxEntitlements.FileAccessMoviesFolderROKey, AppSandboxEntitlements.FileAccessMoviesFolderRWKey);
            }
        }

        public void AddWallet(string[] passSubset)
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[WalletEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            if ((passSubset == null || passSubset.Length == 0) && plistElementArray != null)
            {
                plistElementArray.values.Add(new PlistElementString(WalletEntitlements.BaseValue + WalletEntitlements.BaseValue));
            }
            else
            {
                for (int i = 0; i < passSubset.Length; i++)
                {
                    plistElementArray?.values.Add(new PlistElementString(WalletEntitlements.BaseValue + passSubset[i]));
                }
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.Wallet, m_EntitlementFilePath);
        }

        public void AddSiri()
        {
            GetOrCreateEntitlementDoc().root[SiriEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.Siri, m_EntitlementFilePath);
        }

        public void AddApplePay(string[] merchants)
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[ApplePayEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            for (int i = 0; i < merchants.Length; i++)
            {
                plistElementArray.values.Add(new PlistElementString(merchants[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.ApplePay, m_EntitlementFilePath);
        }

        public void AddInAppPurchase()
        {
#if UNITY_6000_0
            for (int i = 0; i < PBXCapabilityType.InAppPurchase.frameworks.Length; i++)
            {
                project.AddFrameworkToProject(m_TargetGuid, PBXCapabilityType.InAppPurchase.frameworks[i], weak: false);
            }
#else
            project.AddFrameworkToProject(m_TargetGuid, PBXCapabilityType.InAppPurchase.framework, weak: false);
#endif
            project.AddCapability(m_TargetGuid, PBXCapabilityType.InAppPurchase);
        }

        public void AddMaps(MapsOptions options)
        {
            PlistElementArray plistElementArray = (GetOrCreateInfoDoc().root[MapsInfo.BundleKey] ?? (GetOrCreateInfoDoc().root[MapsInfo.BundleKey] = new PlistElementArray())) as PlistElementArray;
            plistElementArray.values.Add(new PlistElementDict());
            PlistElementDict orCreateUniqueDictElementInArray = GetOrCreateUniqueDictElementInArray(plistElementArray);
            orCreateUniqueDictElementInArray[MapsInfo.BundleNameKey] = new PlistElementString(MapsInfo.BundleNameValue);
            PlistElementArray root = (orCreateUniqueDictElementInArray[MapsInfo.BundleTypeKey] ?? (orCreateUniqueDictElementInArray[MapsInfo.BundleTypeKey] = new PlistElementArray())) as PlistElementArray;
            GetOrCreateStringElementInArray(root, MapsInfo.BundleTypeValue);
            PlistElementArray root2 = (GetOrCreateInfoDoc().root[MapsInfo.ModeKey] ?? (GetOrCreateInfoDoc().root[MapsInfo.ModeKey] = new PlistElementArray())) as PlistElementArray;
            if ((options & MapsOptions.Airplane) == MapsOptions.Airplane)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModePlaneValue);
            }

            if ((options & MapsOptions.Bike) == MapsOptions.Bike)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeBikeValue);
            }

            if ((options & MapsOptions.Bus) == MapsOptions.Bus)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeBusValue);
            }

            if ((options & MapsOptions.Car) == MapsOptions.Car)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeCarValue);
            }

            if ((options & MapsOptions.Ferry) == MapsOptions.Ferry)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeFerryValue);
            }

            if ((options & MapsOptions.Other) == MapsOptions.Other)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeOtherValue);
            }

            if ((options & MapsOptions.Pedestrian) == MapsOptions.Pedestrian)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModePedestrianValue);
            }

            if ((options & MapsOptions.RideSharing) == MapsOptions.RideSharing)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeRideShareValue);
            }

            if ((options & MapsOptions.StreetCar) == MapsOptions.StreetCar)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeStreetCarValue);
            }

            if ((options & MapsOptions.Subway) == MapsOptions.Subway)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeSubwayValue);
            }

            if ((options & MapsOptions.Taxi) == MapsOptions.Taxi)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeTaxiValue);
            }

            if ((options & MapsOptions.Train) == MapsOptions.Train)
            {
                GetOrCreateStringElementInArray(root2, MapsInfo.ModeTrainValue);
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.Maps);
        }

        public void AddPersonalVPN()
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[VPNEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            plistElementArray.values.Add(new PlistElementString(VPNEntitlements.Value));
            project.AddCapability(m_TargetGuid, PBXCapabilityType.PersonalVPN, m_EntitlementFilePath);
        }

        public void AddBackgroundModes(BackgroundModesOptions options)
        {
            PlistElementArray root = (GetOrCreateInfoDoc().root[BackgroundInfo.Key] ?? (GetOrCreateInfoDoc().root[BackgroundInfo.Key] = new PlistElementArray())) as PlistElementArray;
            if ((options & BackgroundModesOptions.ActsAsABluetoothLEAccessory) == BackgroundModesOptions.ActsAsABluetoothLEAccessory)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeActsBluetoothValue);
            }

            if ((options & BackgroundModesOptions.AudioAirplayPiP) == BackgroundModesOptions.AudioAirplayPiP)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeAudioValue);
            }

            if ((options & BackgroundModesOptions.BackgroundFetch) == BackgroundModesOptions.BackgroundFetch)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeFetchValue);
            }

            if ((options & BackgroundModesOptions.ExternalAccessoryCommunication) == BackgroundModesOptions.ExternalAccessoryCommunication)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeExtAccessoryValue);
            }

            if ((options & BackgroundModesOptions.LocationUpdates) == BackgroundModesOptions.LocationUpdates)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeLocationValue);
            }

            if ((options & BackgroundModesOptions.NewsstandDownloads) == BackgroundModesOptions.NewsstandDownloads)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeNewsstandValue);
            }

#if UNITY_6000_0_OR_NEWER
            if ((options & BackgroundModesOptions.Processing) == BackgroundModesOptions.Processing)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeProcessingValue);
            }
#endif
            if ((options & BackgroundModesOptions.RemoteNotifications) == BackgroundModesOptions.RemoteNotifications)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModePushValue);
            }

            if ((options & BackgroundModesOptions.VoiceOverIP) == BackgroundModesOptions.VoiceOverIP)
            {
                GetOrCreateStringElementInArray(root, BackgroundInfo.ModeVOIPValue);
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.BackgroundModes);
        }

        public void AddKeychainSharing(string[] accessGroups)
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[KeyChainEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            if (accessGroups != null)
            {
                for (int i = 0; i < accessGroups.Length; i++)
                {
                    plistElementArray.values.Add(new PlistElementString(accessGroups[i]));
                }
            }
            else
            {
                plistElementArray.values.Add(new PlistElementString(KeyChainEntitlements.DefaultValue));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.KeychainSharing, m_EntitlementFilePath);
        }

        public void AddInterAppAudio()
        {
            GetOrCreateEntitlementDoc().root[AudioEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.InterAppAudio, m_EntitlementFilePath);
        }

        public void AddAssociatedDomains(string[] domains)
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[AssociatedDomainsEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            for (int i = 0; i < domains.Length; i++)
            {
                plistElementArray.values.Add(new PlistElementString(domains[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.AssociatedDomains, m_EntitlementFilePath);
        }

        public void AddAppGroups(string[] groups)
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[AppGroupsEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            for (int i = 0; i < groups.Length; i++)
            {
                plistElementArray.values.Add(new PlistElementString(groups[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.AppGroups, m_EntitlementFilePath);
        }

        public void AddHomeKit()
        {
            GetOrCreateEntitlementDoc().root[HomeKitEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.HomeKit, m_EntitlementFilePath);
        }

        public void AddDataProtection()
        {
            GetOrCreateEntitlementDoc().root[DataProtectionEntitlements.Key] = new PlistElementString(DataProtectionEntitlements.Value);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.DataProtection, m_EntitlementFilePath);
        }

        public void AddHealthKit()
        {
            PlistElementArray root = (GetOrCreateInfoDoc().root[HealthInfo.Key] ?? (GetOrCreateInfoDoc().root[HealthInfo.Key] = new PlistElementArray())) as PlistElementArray;
            GetOrCreateStringElementInArray(root, HealthInfo.Value);
            GetOrCreateEntitlementDoc().root[HealthKitEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.HealthKit, m_EntitlementFilePath);
        }

        public void AddWirelessAccessoryConfiguration()
        {
            GetOrCreateEntitlementDoc().root[WirelessAccessoryConfigurationEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.WirelessAccessoryConfiguration, m_EntitlementFilePath);
        }

        public void AddAccessWiFiInformation()
        {
            GetOrCreateEntitlementDoc().root[AccessWiFiInformationEntitlements.Key] = new PlistElementBoolean(v: true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.AccessWiFiInformation, m_EntitlementFilePath);
        }

        public void AddSignInWithApple()
        {
            PlistElement plistElement2 = (GetOrCreateEntitlementDoc().root[SignInWithAppleEntitlements.Key] = new PlistElementArray());
            PlistElementArray plistElementArray = plistElement2 as PlistElementArray;
            plistElementArray.values.Add(new PlistElementString(SignInWithAppleEntitlements.Value));
            project.AddCapability(m_TargetGuid, PBXCapabilityType.SignInWithApple, m_EntitlementFilePath);
        }

        public PlistDocument GetOrCreateEntitlementDoc()
        {
            if (m_Entitlements == null)
            {
                m_Entitlements = new PlistDocument();
                string path = ((m_BuildPath != null) ? PBXPath.Combine(m_BuildPath, m_EntitlementFilePath) : m_EntitlementFilePath);
                if (File.Exists(path))
                {
                    m_Entitlements.ReadFromFile(path);
                }
                else
                {
                    m_Entitlements.Create();
                }
            }

            return m_Entitlements;
        }

        public PlistDocument GetOrCreateInfoDoc()
        {
            if (m_InfoPlist == null)
            {
                m_InfoPlist = new PlistDocument();
                string[] files = Directory.GetFiles(m_BuildPath + "/", "Info.plist");
                if (files.Length != 0)
                {
                    m_InfoPlist.ReadFromFile(files[0]);
                }
                else
                {
                    m_InfoPlist.Create();
                }
            }

            return m_InfoPlist;
        }

        private PlistElementString GetOrCreateStringElementInArray(PlistElementArray root, string value)
        {
            PlistElementString plistElementString = null;
            int count = root.values.Count;
            bool flag = false;
            for (int i = 0; i < count; i++)
            {
                if (root.values[i] is PlistElementString && (root.values[i] as PlistElementString).value == value)
                {
                    plistElementString = root.values[i] as PlistElementString;
                    flag = true;
                }
            }

            if (!flag)
            {
                plistElementString = new PlistElementString(value);
                root.values.Add(plistElementString);
            }

            return plistElementString;
        }

        private PlistElementDict GetOrCreateUniqueDictElementInArray(PlistElementArray root)
        {
            PlistElementDict plistElementDict;
            if (root.values.Count == 0)
            {
                plistElementDict = root.values[0] as PlistElementDict;
            }
            else
            {
                plistElementDict = new PlistElementDict();
                root.values.Add(plistElementDict);
            }

            return plistElementDict;
        }

        private static ConstructorInfo GetPBXCapabilityTypeConstructor(BindingFlags flags)
        {
            return typeof(PBXCapabilityType).GetConstructor(
                flags,
                null,
                new[] { typeof(string), typeof(bool), typeof(string), typeof(bool) },
                null);
        }
    }
}

#endif