using System;
using System.Collections.Generic;
using UnityEngine;

namespace Superfine.Unity
{
    public delegate void RequestAuthorizationTrackingCompleteHandler(AuthorizationTrackingStatus status);

    public class SuperfineSDKManager :
#if UNITY_EDITOR
        SuperfineSDKManagerStub
#elif UNITY_ANDROID
        SuperfineSDKManagerAndroid
#elif UNITY_IPHONE || UNITY_IOS
        SuperfineSDKManagerIos
#elif UNITY_STANDALONE
        SuperfineSDKManagerStandalone
#else
        SuperfineSDKManagerStub
#endif
    {
        private bool hasRegisteredDeepLink = false;

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
            OpenURL(url);
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

        private List<SuperfineSDKModule> modules = new List<SuperfineSDKModule>();

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

        public static SuperfineSDKManager Create()
        {
            return new SuperfineSDKManager();
        }

        protected SuperfineSDKManager()
        {
            //Force create dispatcher
            UnityMainThreadDispatcher.Instance();
        }

        ~SuperfineSDKManager()
        {
            Destroy();
        }

        public override void Initialize(SuperfineSDKSettings settings)
        {
            bool autoStart = false;

            if (settings.autoStart)
            {
                settings.autoStart = false;
                autoStart = true;
            }

            base.Initialize(settings);

            List<Action> startCallbackCache = SuperfineSDK.GetStartCallbackCache();
            if (startCallbackCache != null)
            {
                foreach (Action action in startCallbackCache)
                {
                    AddStartCallback(action);
                }
            }

            List<Action> stopCallbackCache = SuperfineSDK.GetStopCallbackCache();
            if (stopCallbackCache != null)
            {
                foreach (Action action in stopCallbackCache)
                {
                    AddStopCallback(action);
                }
            }

            List<Action> pauseCallbackCache = SuperfineSDK.GetPauseCallbackCache();
            if (pauseCallbackCache != null)
            {
                foreach (Action action in pauseCallbackCache)
                {
                    AddPauseCallback(action);
                }
            }

            List<Action> resumeCallbackCache = SuperfineSDK.GetResumeCallbackCache();
            if (resumeCallbackCache != null)
            {
                foreach (Action action in resumeCallbackCache)
                {
                    AddResumeCallback(action);
                }
            }

            List< Action<string> > deepLinkCallbackCache = SuperfineSDK.GetDeepLinkCallbackCache();
            if (deepLinkCallbackCache != null)
            {
                foreach (Action<string> action in deepLinkCallbackCache)
                {
                    AddDeepLinkCallback(action, false);
                }
            }

            List< Action<string> > pushTokenCallbackCache = SuperfineSDK.GetPushTokenCallbackCache();
            if (pushTokenCallbackCache != null)
            {
                foreach (Action<string> action in pushTokenCallbackCache)
                {
                    AddPushTokenCallback(action, false);
                }
            }

            List< Action<string, string> > sendEventCallbackCache = SuperfineSDK.GetSendEventCallbackCache();
            if (sendEventCallbackCache != null)
            {
                foreach (Action<string, string> action in sendEventCallbackCache)
                {
                    AddSendEventCallback(action);
                }
            }

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

            List<SuperfineSDKModuleSettings> moduleSettingList = settings.modules;
            HashSet<string> moduleNameHash = new HashSet<string>();

            if (moduleSettingList != null)
            {
                int numModules = moduleSettingList.Count;
                for (int i = 0; i < numModules; ++i)
                {
                    SuperfineSDKModuleSettings moduleSettings = moduleSettingList[i];

                    string moduleName = moduleSettings.GetModuleName();
                    if (string.IsNullOrEmpty(moduleName)) continue;

                    if (moduleNameHash.Contains(moduleName)) continue;
                    moduleNameHash.Add(moduleName);

                    SuperfineSDKModule module = moduleSettings.CreateModule();
                    if (module == null) continue;

                    modules.Add(module);
                }
            }

            if (autoStart)
            {
                settings.autoStart = true;
                Start();
            }
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
