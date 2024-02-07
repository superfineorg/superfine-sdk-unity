using UnityEngine;

namespace Superfine.Unity
{
    public abstract class SuperfineSDKModule
    {
        public SuperfineSDKModule(SuperfineSDKModuleSettings settings)
        {
            Initialize(settings);
        }

        protected virtual void Initialize(SuperfineSDKModuleSettings settings)
        {
            name = (settings ? settings.GetModuleName() : string.Empty);
        }

        public virtual void Destroy()
        {
        }

        protected string name = string.Empty;
        public string GetName()
        {
            return name;
        }

        protected T CreateMonoBehaviour<T>(bool forceDisable = false) where T: MonoBehaviour
        {
            GameObject go = new GameObject(name);
            if (forceDisable)
            {
                go.SetActive(false);
            }

            T component = go.AddComponent<T>();
            Object.DontDestroyOnLoad(go);

            return component;
        }
    }
}
