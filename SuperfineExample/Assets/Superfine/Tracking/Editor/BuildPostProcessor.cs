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
        const string SKAdNetworkIDsPath = "Superfine/Tracking/Editor/skadnetworks.txt";
        static void UpdateInfoPlist(string pathToBuiltProject)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            // Get root
            PlistElementDict rootDict = plist.root;
            // SKAdNetworks
            string SKADFilePath = Path.Combine(Application.dataPath, SKAdNetworkIDsPath);
            if (File.Exists(SKADFilePath))
            {
                string content = File.ReadAllText(SKADFilePath);
                if (!string.IsNullOrEmpty(content))
                {
                    string[] ids = content.Split('\n');
                    var skAdArray = rootDict.CreateArray("SKAdNetworkItems");
                    for (int i = 0, length = ids.Length; i < length; ++i)
                    {
                        if (string.IsNullOrEmpty(ids[i])) continue;
                        skAdArray.AddDict().SetString("SKAdNetworkIdentifier", ids[i]);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"====Missing skAdNetworks ids list at: {SKADFilePath}");
            }
            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
#endif
    }
}