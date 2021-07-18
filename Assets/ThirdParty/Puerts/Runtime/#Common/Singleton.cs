using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
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

        public static T instance =>
            m_Singleton ??= FindObjectOfType<T>() ?? SceneManager.GetAllScenes().Where(scene => scene.isLoaded)
                    .SelectMany(scene => scene.GetRootGameObjects()).Select(t => t.GetComponentInChildren<T>(true))
                    .FirstOrDefault() ??
                (GameObject.Find("/" + SceneManager.GetActiveScene().name) ??
                    new GameObject(SceneManager.GetActiveScene().name)).AddComponent<T>();
    }
}