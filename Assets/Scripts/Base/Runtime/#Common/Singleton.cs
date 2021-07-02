using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

namespace Base.Runtime
{
    [System.Serializable]
    public class Singleton<T> : SerializedMonoBehaviour where T : Singleton<T>
    {
        protected Singleton() { }
        static T m_Singleton;

        protected virtual void Awake()
        {
            m_Singleton ??= this as T;
        }

        public static T Inst =>
            m_Singleton ??= FindObjectOfType<T>() ?? SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(t => t.GetComponentInChildren<T>(true)).FirstOrDefault();
    }
}