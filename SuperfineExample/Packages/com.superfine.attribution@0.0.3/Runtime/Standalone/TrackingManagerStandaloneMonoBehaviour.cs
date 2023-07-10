#if UNITY_STANDALONE && !UNITY_EDITOR
using UnityEngine;

namespace Superfine.Tracking.Unity
{
    public class TrackingManagerStandaloneMonoBehaviour : MonoBehaviour
    {
        private TrackingManagerStandalone manager = null;

        public void SetManager(TrackingManagerStandalone manager)
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
#endif