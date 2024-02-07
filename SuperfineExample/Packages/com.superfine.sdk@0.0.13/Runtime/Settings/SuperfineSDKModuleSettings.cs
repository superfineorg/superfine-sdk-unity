using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKModuleSettings : ScriptableObject
    {
        public virtual string GetModuleName()
        {
            return string.Empty;
        }

        public virtual SuperfineSDKModule CreateModule()
        {
            return null;
        }
    }
}
