using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

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

                SuperfineSDKSettings settings = SuperfineSDKSettings.NullableInstance;

                if (settings != null && settings.uriSchemes != null)
                {
                    int numUriSchemes = settings.uriSchemes.Length;
                    if (numUriSchemes > 0)
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

                        for (int i = 0; i < numUriSchemes; ++i)
                        {
                            XmlElement dataSchemeElement = androidManifest.CreateElement("data");
                            dataSchemeElement.SetAttribute("scheme", androidNamespace, settings.uriSchemes[i]);
                            intentElement.AppendChild(dataSchemeElement);
                        }

                        activityNode.AppendChild(intentElement);
                    }
                }

                if (settings != null && settings.associatedDomains != null)
                {
                    int numAssociatedDomains = settings.associatedDomains.Length;
                    if (numAssociatedDomains > 0)
                    {
                        XmlElement intentElement = androidManifest.CreateElement("intent-filter");
                        intentElement.SetAttribute("autoVerify", androidNamespace, "true");
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

                        for (int i = 0; i < numAssociatedDomains; ++i)
                        {
                            XmlElement dataHostElement = androidManifest.CreateElement("data");
                            dataHostElement.SetAttribute("host", androidNamespace, settings.associatedDomains[i]);
                            intentElement.AppendChild(dataHostElement);
                        }

                        activityNode.AppendChild(intentElement);
                    }
                }
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
    }
#endif
}
