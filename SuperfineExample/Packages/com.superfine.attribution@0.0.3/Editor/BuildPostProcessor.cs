//
//  Copyright (c) 2023 Superfine. All rights reserved.
//

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using System.Collections.Generic;
using UnityEngine.TextCore;
using UnityEditor.PackageManager;
using System.Linq;

public class SuperfinePostProcess
{
    //Check Skads
    [PostProcessBuild(700)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
#if UNITY_IOS
            UpdateInfoPlist(pathToBuiltProject);
#endif
        }

#if UNITY_IOS
        static void UpdateInfoPlist(string pathToBuiltProject)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            // Get root
            PlistElementDict rootDict = plist.root;

            rootDict.SetString("NSUserTrackingUsageDescription", "We use your data to serve you more relevant ads and improve your experience.");
            rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://superfine.gg");

            List<string> skadPathList = new List<string>();

            var request = Client.List();
            do { } while (!request.IsCompleted);
            if (request.Status == StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    var path = package.resolvedPath;
                    if (!path.Contains("com.superfine.attribution")) continue;

                    skadPathList.AddRange(Directory.EnumerateFiles(path, "skadnetworks.txt", SearchOption.AllDirectories).ToList());
                }    
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

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
#endif
    }
}