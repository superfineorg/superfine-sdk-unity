using System;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKManager :
#if UNITY_EDITOR
        SuperfineSDKManagerStub
#elif UNITY_ANDROID
        SuperfineSDKManagerAndroid
#elif UNITY_IOS
        SuperfineSDKManagerIos
#elif UNITY_STANDALONE
        SuperfineSDKManagerStandalone
#else
        SuperfineSDKManagerStub
#endif
    {
        private List<string> moduleNameList;
        private Dictionary<string, SuperfineSDKModuleSettings> moduleSettingsList;

        private List<SuperfineSDKModule> modules;

        private bool autoStart;
        private bool disableOnlineSdkConfig;
        private bool hasRegisteredDeepLink;

        public static SuperfineSDKManager Create()
        {
            return new SuperfineSDKManager();
        }

        protected SuperfineSDKManager() : base()
        {
            moduleNameList = null;
            moduleSettingsList = null;

            modules = new List<SuperfineSDKModule>();

            autoStart = false;
            disableOnlineSdkConfig = false;
            hasRegisteredDeepLink = false;
        }

        ~SuperfineSDKManager()
        {
            Destroy();
        }

        public virtual void Initialize(SuperfineSDKSettings settings)
        {
            moduleNameList = null;

            if (settings.modules != null)
            {
                int numModules = settings.modules.Count;

                if (numModules > 0)
                {
                    for (int i = 0; i < numModules; ++i)
                    {
                        SuperfineSDKModuleSettings moduleSettings = GameObject.Instantiate(settings.modules[i]);

                        string moduleName = moduleSettings.GetModuleName();
                        if (string.IsNullOrEmpty(moduleName)) continue;

                        if (moduleNameList == null) moduleNameList = new List<string>();
                        else if (moduleNameList.Contains(moduleName)) continue;

                        moduleNameList.Add(moduleName);

                        if (moduleSettingsList == null) moduleSettingsList = new Dictionary<string, SuperfineSDKModuleSettings>();
                        moduleSettingsList[moduleName] = moduleSettings;
                    }
                }
            }

            autoStart = settings.autoStart;

            //Disable auto start setting
            if (autoStart)
            {
                settings.autoStart = false;
            }

            disableOnlineSdkConfig = settings.disableOnlineSdkConfig;

            if (settings.captureDeepLinks)
            {
#if !UNITY_EDITOR
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
                RegisterDeepLink();
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
                CheckDeepLinkArguments(settings.GetCustomSchemes());
#endif
#endif
            }

            InitializeNative(settings, moduleNameList);

            //Revert auto start setting
            if (autoStart)
            {
                settings.autoStart = true;
            }
        }

        public void OnInitialized(Action callback)
        {
            SetupModules();

            callback?.Invoke();

            moduleNameList = null;
            moduleSettingsList = null;

            if (autoStart)
            {
                Start();
            }
        }

        private void RegisterDeepLink()
        {
            if (hasRegisteredDeepLink) return;
            hasRegisteredDeepLink = true;

            Application.deepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
        }

        private void UnregisterDeepLink()
        {
            if (!hasRegisteredDeepLink) return;
            hasRegisteredDeepLink = false;

            Application.deepLinkActivated -= OnDeepLinkActivated;
        }

        private void OnDeepLinkActivated(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            SuperfineSDK.OpenURL(url);
        }

        private bool IsDeepLinkURL(string url, List<string> uriSchemes)
        {
            if (uriSchemes == null) return false;

            int numUriSchemes = uriSchemes.Count;
            for (int i = 0; i < numUriSchemes; ++i)
            {
                string prefix = uriSchemes[i] + "://";
                if (url.StartsWith(prefix)) return true;
            }

            return false;
        }

        private void CheckDeepLinkArguments(List<string> uriSchemes)
        {
            if (uriSchemes == null || uriSchemes.Count == 0) return;

            var args = System.Environment.GetCommandLineArgs();
            if (args != null)
            {
                int numArgs = args.Length;
                if (numArgs > 1)
                {
                    string arg = args[1];
                    if (IsDeepLinkURL(arg, uriSchemes))
                    {
                        OnDeepLinkActivated(arg);
                    }
                }
            }
        }

        private bool IsExcludedSdkConfig(SimpleJSON.JSONObject sdkConfig, string platform)
        {
            if (sdkConfig.TryGetValue<SimpleJSON.JSONArray>("excludedPlatforms", out SimpleJSON.JSONArray excludedPlatformsArray))
            {
                int numPlatforms = excludedPlatformsArray.Count;
                for (int i = 0; i < numPlatforms; ++i)
                {
                    SimpleJSON.JSONNode node = excludedPlatformsArray[i];
                    if (node != null && node.IsString)
                    {
                        if (platform == node.Value) return true;
                    }
                }
            }

            return false;
        }

        private bool IsUpdatedSdkConfig(SimpleJSON.JSONObject sdkConfig, string platform)
        {
            if (sdkConfig.TryGetValue<SimpleJSON.JSONArray>("platforms", out SimpleJSON.JSONArray platformsArray))
            {
                int numPlatforms = platformsArray.Count;
                for (int i = 0; i < numPlatforms; ++i)
                {
                    SimpleJSON.JSONNode node = platformsArray[i];
                    if (node != null && node.IsString)
                    {
                        if (platform == node.Value) return true;
                    }
                }
            }

            return false;
        }

        private void ProcessSdkModuleConfig(SimpleJSON.JSONObject sdkConfig)
        {
            if (sdkConfig == null)
            {
                return;
            }

            if (!sdkConfig.TryGetValue("type", out SimpleJSON.JSONNode node))
            {
                return;
            }

            if (!node.IsString) return;

            string type = node.Value;
            if (string.IsNullOrEmpty(type)) return;

            if (moduleSettingsList == null) return;

            if (!moduleSettingsList.TryGetValue(type, out SuperfineSDKModuleSettings moduleSettings)) return;

            if (IsExcludedSdkConfig(sdkConfig, "Unity"))
            {
                moduleSettingsList.Remove(type);
                return;
            }

            if (!IsUpdatedSdkConfig(sdkConfig, "Unity")) return;

            sdkConfig.Remove("type");
            sdkConfig.Remove("platforms");
            sdkConfig.Remove("excludedPlatforms");
            sdkConfig.Remove("excludedWrappers");

            moduleSettings.MergeSdkConfig(sdkConfig);
        }

        private void SetupModules()
        {
            moduleNameList = null;

            if (!disableOnlineSdkConfig)
            {
                string sdkConfig = GetSdkConfig();
                if (!string.IsNullOrEmpty(sdkConfig))
                {
                    SimpleJSON.JSONNode node = SimpleJSON.JSONNode.Parse(sdkConfig);
                    if (node != null && node.IsObject)
                    {
                        SimpleJSON.JSONObject sdkConfigObject = (SimpleJSON.JSONObject)node;
                        if (sdkConfigObject.TryGetValue<SimpleJSON.JSONArray>("modules", out SimpleJSON.JSONArray modulesArray))
                        {
                            int numModules = modulesArray.Count;
                            for (int i = 0; i < numModules; i++)
                            {
                                node = modulesArray[i];
                                if (node != null && node.IsObject)
                                {
                                    SimpleJSON.JSONObject sdkModuleObject = (SimpleJSON.JSONObject)node;
                                    ProcessSdkModuleConfig(sdkModuleObject);
                                }
                            }
                        }
                    }
                }
            }

            modules.Clear();

            if (moduleSettingsList != null)
            {
                foreach (var pair in moduleSettingsList)
                {
                    SuperfineSDKModuleSettings moduleSettings = pair.Value;

                    SuperfineSDKModule module = moduleSettings.CreateModule();
                    if (module == null) continue;

                    modules.Add(module);
                }

                moduleSettingsList = null;
            }
        }

        public SuperfineSDKModule GetModule(string name)
        {
            int numModules = modules.Count;
            for (int i = 0; i < numModules; i++)
            {
                if (modules[i].GetName() == name)
                {
                    return modules[i];
                }
            }

            return null;
        }

        public override void Destroy()
        {
            UnregisterDeepLink();

            int numModules = modules.Count;
            for (int i = 0; i < numModules; ++i)
            {
                modules[i].Destroy();
            }

            modules.Clear();

            base.Destroy();
        }
    }
}
