using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using UnityEngine;
using System;

#if UNITY_ANDROID
using UnityEditor.Android;
#endif

namespace Superfine.Unity
{
#if UNITY_ANDROID
    public class PostGenerateGradleAndroidProject : IPostGenerateGradleAndroidProject
    {
        private string manifestFilePath = string.Empty;

        public int callbackOrder => 0;

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            string manifestPath = GetManifestPath(basePath);

            XmlDocument androidManifest = new XmlDocument();
            androidManifest.Load(manifestPath);

            XmlNode manifestNode = FindChildNode(androidManifest, "manifest");
            XmlNode applicationNode = FindChildNode(manifestNode, "application");

            XmlNode activityNode = FindChildNode(applicationNode, "activity");
            if (activityNode != null)
            {
                string androidNamespace = activityNode.GetNamespaceOfPrefix("android");

                XmlNodeList intentNodeList = ((XmlElement)activityNode).GetElementsByTagName("intent-filter");
                if (intentNodeList.Count > 0)
                {
                    List<XmlNode> toRemove = new List<XmlNode>();

                    foreach (XmlNode intentNode in intentNodeList)
                    {
                        if (intentNode is XmlElement)
                        {
                            XmlElement intentElement = (XmlElement)intentNode;
                            if (intentElement.GetAttribute("superfine") == "true")
                            {
                                toRemove.Add(intentNode);
                            }
                        }
                    }

                    foreach (XmlNode node in toRemove)
                    {
                        activityNode.RemoveChild(node);
                    }
                }

                SuperfineSDKSettings settings = SuperfineSDKSettings.LoadFromResources();

                if (settings != null)
                {
                    List<string> uriSchemes = settings.GetCustomSchemes();
                    int numUriSchemes = uriSchemes != null ? uriSchemes.Count : 0;
                    for (int i = 0; i < numUriSchemes; ++i)
                    {
                        XmlElement intentElement = androidManifest.CreateElement("intent-filter");
                        intentElement.SetAttribute("superfine", "true");

                        XmlElement actionElement = androidManifest.CreateElement("action");
                        actionElement.SetAttribute("name", androidNamespace, "android.intent.action.VIEW");
                        intentElement.AppendChild(actionElement);

                        XmlElement categoryElement = androidManifest.CreateElement("category");
                        categoryElement.SetAttribute("name", androidNamespace, "android.intent.category.DEFAULT");
                        intentElement.AppendChild(categoryElement);

                        categoryElement = androidManifest.CreateElement("category");
                        categoryElement.SetAttribute("name", androidNamespace, "android.intent.category.BROWSABLE");
                        intentElement.AppendChild(categoryElement);

                        XmlElement dataSchemeElement = androidManifest.CreateElement("data");
                        dataSchemeElement.SetAttribute("scheme", androidNamespace, uriSchemes[i]);
                        intentElement.AppendChild(dataSchemeElement);

                        activityNode.AppendChild(intentElement);
                    }

                    List<Tuple<string, bool>> appLinks = settings.GetAppLinks();
                    int numAppLinks = appLinks != null ? appLinks.Count : 0;
                    for (int i = 0; i < numAppLinks; ++i)
                    {
                        XmlElement intentElement = androidManifest.CreateElement("intent-filter");

                        if (appLinks[i].Item2)
                        {
                            intentElement.SetAttribute("autoVerify", androidNamespace, "true");
                        }

                        intentElement.SetAttribute("superfine", "true");

                        XmlElement actionElement = androidManifest.CreateElement("action");
                        actionElement.SetAttribute("name", androidNamespace, "android.intent.action.VIEW");
                        intentElement.AppendChild(actionElement);

                        XmlElement categoryElement = androidManifest.CreateElement("category");
                        categoryElement.SetAttribute("name", androidNamespace, "android.intent.category.DEFAULT");
                        intentElement.AppendChild(categoryElement);

                        categoryElement = androidManifest.CreateElement("category");
                        categoryElement.SetAttribute("name", androidNamespace, "android.intent.category.BROWSABLE");
                        intentElement.AppendChild(categoryElement);

                        XmlElement dataSchemeElement = androidManifest.CreateElement("data");
                        dataSchemeElement.SetAttribute("scheme", androidNamespace, "http");
                        intentElement.AppendChild(dataSchemeElement);

                        dataSchemeElement = androidManifest.CreateElement("data");
                        dataSchemeElement.SetAttribute("scheme", androidNamespace, "https");
                        intentElement.AppendChild(dataSchemeElement);

                        XmlElement dataHostElement = androidManifest.CreateElement("data");
                        dataHostElement.SetAttribute("host", androidNamespace, appLinks[i].Item1);
                        intentElement.AppendChild(dataHostElement);

                        activityNode.AppendChild(intentElement);
                    }
                }

                AddPermissions(androidManifest, settings);
            }

            XmlWriterSettings writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(manifestPath, writerSettings))
            {
                androidManifest.Save(xmlWriter);
            }
        }

        private string GetManifestPath(string basePath)
        {
            if (string.IsNullOrEmpty(manifestFilePath))
            {
                var pathBuilder = new StringBuilder(basePath);
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");

                manifestFilePath = pathBuilder.ToString();
            }
            return manifestFilePath;
        }

        private XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }

                curr = curr.NextSibling;
            }

            return null;
        }

        private void SetOrReplaceXmlElement(XmlNode parent, XmlElement newElement)
        {
            string attrNameValue = newElement.GetAttribute("name");
            string elementType = newElement.Name;

            XmlElement existingElment;
            if (TryFindElementWithAndroidName(parent, attrNameValue, out existingElment, elementType))
            {
                parent.ReplaceChild(newElement, existingElment);
            }
            else
            {
                parent.AppendChild(newElement);
            }
        }

        private bool TryFindElementWithAndroidName(XmlNode parent, string attrNameValue, out XmlElement element, string elementType = "activity")
        {
            string ns = parent.GetNamespaceOfPrefix("android");
            var curr = parent.FirstChild;
            while (curr != null)
            {
                var currXmlElement = curr as XmlElement;
                if (currXmlElement != null &&
                    currXmlElement.Name == elementType &&
                    currXmlElement.GetAttribute("name", ns) == attrNameValue)
                {
                    element = currXmlElement;
                    return true;
                }

                curr = curr.NextSibling;
            }

            element = null;
            return false;
        }

        private void AddAndroidNamespaceAttribute(XmlDocument manifest, string key, string value, XmlElement node)
        {
            var androidSchemeAttribute = manifest.CreateAttribute("android", key, "http://schemas.android.com/apk/res/android");
            androidSchemeAttribute.InnerText = value;
            node.SetAttributeNode(androidSchemeAttribute);
        }

        private XmlNamespaceManager GetNamespaceManager(XmlDocument manifest)
        {
            var namespaceManager = new XmlNamespaceManager(manifest.NameTable);
            namespaceManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");
            return namespaceManager;
        }

        private bool DoesPermissionExist(XmlDocument manifest, string permissionValue)
        {
            var xpath = string.Format("/manifest/uses-permission[@android:name='{0}']", permissionValue);
            return manifest.DocumentElement.SelectSingleNode(xpath, GetNamespaceManager(manifest)) != null;
        }

        private bool AddPermission(XmlDocument manifest, string permissionValue)
        {
            if (DoesPermissionExist(manifest, permissionValue))
            {
                //Debug.Log(string.Format("Your app's AndroidManifest.xml file already contains {0} permission.", permissionValue));
                return false;
            }

            var element = manifest.CreateElement("uses-permission");
            AddAndroidNamespaceAttribute(manifest, "name", permissionValue, element);
            manifest.DocumentElement.AppendChild(element);
            Debug.Log(string.Format("{0} permission successfully added to your app's AndroidManifest.xml file.", permissionValue));

            return true;
        }

        private bool AddPermissions(XmlDocument manifest, SuperfineSDKSettings settings)
        {
            if (manifest == null || settings == null) return false;

            var manifestHasChanged = false;

            if (settings.androidPermissionInternet)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.INTERNET");
            }

            if (settings.androidPermissionAccessNetworkState)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_NETWORK_STATE");
            }

            if (settings.androidPermissionWriteExternalStorage)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.WRITE_EXTERNAL_STORAGE");
            }

            if (settings.androidPermissionAccessWifiState)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_WIFI_STATE");
            }

            if (settings.androidPermissionVibrate)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.VIBRATE");
            }

            if (settings.androidPermissionInstallReferrerService)
            {
                manifestHasChanged |= AddPermission(manifest, "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE");
            }

            if (settings.androidPermissionAdId)
            {
                manifestHasChanged |= AddPermission(manifest, "com.google.android.gms.permission.AD_ID");
            }

            if (settings.androidPermissionReadPhoneState)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.READ_PHONE_STATE");
            }

            if (settings.androidPermissionReadPrivilegedPhoneState)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.READ_PRIVILEGED_PHONE_STATE");
            }

            if (settings.androidPermissionWakeLock)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.WAKE_LOCK");
            }

            if (settings.androidPermissionAccessCoarseLocation)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_COARSE_LOCATION");
            }

            if (settings.androidPermissionAccessFineLocation)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_FINE_LOCATION");
            }

            if (settings.androidPermissionCheckLicense)
            {
                manifestHasChanged |= AddPermission(manifest, "com.android.vending.CHECK_LICENSE");
            }

            if (settings.androidPermissionAccessAdservicesAttribution)
            {
                manifestHasChanged |= AddPermission(manifest, "android.permission.ACCESS_ADSERVICES_ATTRIBUTION");
            }

            return manifestHasChanged;
        }
    }
#endif
}
