using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SuperfineSettings : ScriptableObject
{
    public string appId = "YOUR APP ID";
    public string appSecret = "YOUR APP SECRET";

    //Tenjin key
    public string tenjinAPIKeyIOS = "TENJIN API KEY FOR IOS";
    public string tenjinAPIKeyAndroid = "TENJIN API KEY FOR ANDROID";

    private const string SuperfineSettingsAssetName = "SuperfineSettings";
    private const string SuperfineSettingsPath = "Resources";
    private const string SuperfineSettingsAssetExtension = ".asset";

    private static SuperfineSettings instance;

#if UNITY_EDITOR
    [MenuItem("Superfine/Edit Settings")]
    public static void Edit()
    {
        var instance = NullableInstance;

        if (instance == null)
        {
            instance = CreateInstance<SuperfineSettings>();
            string properPath = Path.Combine(Application.dataPath, SuperfineSettingsPath);
            if (!Directory.Exists(properPath))
            {
                Directory.CreateDirectory(properPath);
            }

            string fullPath = Path.Combine(
                                  Path.Combine("Assets", SuperfineSettingsPath),
                                  SuperfineSettingsAssetName + SuperfineSettingsAssetExtension);
            AssetDatabase.CreateAsset(instance, fullPath);
        }

        Selection.activeObject = Instance;
    }
#endif

    public static SuperfineSettings Instance
    {
        get
        {
            instance = NullableInstance;

            if (instance == null)
            {
                // If not found, autocreate the asset object.
                instance = CreateInstance<SuperfineSettings>();
            }

            return instance;
        }
    }

    public static SuperfineSettings NullableInstance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load(SuperfineSettingsAssetName) as SuperfineSettings;
            }

            return instance;
        }
    }
}