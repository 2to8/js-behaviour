#if UNITY_EDITOR
using Consts;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Tetris;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Admin
{
    public class StateWindow<T> : OdinEditorWindow where T : ScriptableObject
    {
        static StateWindow<T> m_Instance;

        [OdinSerialize]
        [ShowIf(nameof(isMain))]
        T m_Object;

        //[ShowInInspector]
        public T Status {
            get => m_Object ??= CreateInstance<T>();
            private set => m_Object = value;
        }

        bool isMain => SceneManager.GetActiveScene().name == $"{SceneName.Main}";

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Instance = this;
        }

        public static StateWindow<T> instance {
            get {
                if (m_Instance == null) OpenWindow<T>();
                return m_Instance;
            }
        }

        //[MenuItem("Debug/StateWindow")]

        public static void OpenWindow<T>() where T : ScriptableObject
        {
            var window = GetWindow<StateWindow<T>>();
            window.titleContent = new GUIContent(typeof(T).Name);
            window.Show();
        }
    }
}
#endif