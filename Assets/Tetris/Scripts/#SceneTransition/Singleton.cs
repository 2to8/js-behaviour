using UnityEngine;

namespace Tetris
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        public static bool instanceExists => Instance != null;

        protected virtual void Awake()
        {
            // if (instanceExists && Instance.gameObject != gameObject) Destroy (gameObject);
            // else
            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}