using UnityEngine;

namespace Superfine.Unity
{
    public class SuperfineSDKMonoBehaviour : MonoBehaviour
    {
        private SuperfineSDKManagerBase manager = null;

        public void SetManager(SuperfineSDKManagerBase manager)
        {
            this.manager = manager;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationPause(bool pause)
        {
            if (manager != null)
            {
                if (pause) manager.Execute("pause");
                else manager.Execute("resume");
            }
        }

        private void OnDestroy()
        {
            if (manager != null)
            {
                manager.Execute("destroy");
            }
        }
    }
}
