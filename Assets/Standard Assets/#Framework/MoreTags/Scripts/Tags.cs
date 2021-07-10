using GameEngine.Extensions;
using GameEngine.Kernel;
using MoreTags.Types;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MoreTags
{
    [AddComponentMenu("MoreTags/Tags", 0)] [ExecuteAlways]
    public partial class Tags : SerializedMonoBehaviour
    {
        // Addressables 异步加载场景时不起作用
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnSceneLoaded()
        {
            var scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects().ForEach(t => t.CheckTagsFromRoot());
        }

        [FormerlySerializedAs("Asset"), FormerlySerializedAs("Prefab")]
        public AssetReference asset;

        [OdinSerialize, NonSerialized, HideInInspector]
        AssetLabelReference[] m_Labels;

        public string Description;

        [ShowInInspector]
        public AssetLabelReference[] Labels {
            get => m_Labels ??= new AssetLabelReference[] { };
            set => m_Labels = value;
        }

        [OdinSerialize, HideInInspector]
        Dictionary<string, GameObject> m_Injects;

        [ShowInInspector]
        public Dictionary<string, GameObject> Injects {
            get => m_Injects ??= new Dictionary<string, GameObject>();
            set => m_Injects = value;
        }

        [OdinSerialize, HideInInspector]
        Dictionary<string, AssetReference> m_Prefabs;

        [ShowInInspector]
        public Dictionary<string, AssetReference> Prefabs {
            get => m_Prefabs ??= new Dictionary<string, AssetReference>();
            set => m_Prefabs = value;
        }

        [OdinSerialize, HideInInspector]
        Dictionary<AssetLabelReference, Dictionary<AssetLabelReference, List<TagState>>> m_States;

        [ShowInInspector]
        public Dictionary<AssetLabelReference, Dictionary<AssetLabelReference, List<TagState>>> states {
            get => m_States ??= new Dictionary<AssetLabelReference, Dictionary<AssetLabelReference, List<TagState>>>();
            set => m_States = value;
        }

        //[OdinSerialize, HideInInspector]
        /// <summary>
        /// 必须用odin的, unity的在场景里覆盖prefab里面的list等等
        /// 字符串那些不用
        /// </summary>
        [NonSerialized, OdinSerialize]
        List<int> m_Ids = new List<int>();

        //[ShowInInspector]
        public List<int> ids {
            get => m_Ids;
            set => m_Ids = value;
        }

        public bool BindScriptOnstart;

        [Range(-100, 100)]
        public int groupIndex;

        public List<string> tags => TagSystem.GetTags(ids).ToList();

        //[ShowIf("@this.asset.IsValid()")]
        [LabelText("清除自身"), Title("保存时自动清除自身", HorizontalLine = false, Subtitle = "场景启动时用 prefab 重新创建")]
        public bool ClearOnSave;

        //[ShowIf("@this.asset.IsValid()")]
        [Title("保存时自动清除子物体", HorizontalLine = false, Subtitle = "场景启动时不创建"), LabelText("清除子物体")]
        public bool ClearChildrenOnSave;

        // public string sid;
        [FoldoutGroup("events")]
        public UnityEvent E_OnStart;

        [FoldoutGroup("events")]
        public UnityEvent E_OnAwake;

        // public UnityEvent E_OnUpdate;
        // public UnityEvent E_OnFixedUpdate;
        // public UnityEvent E_OnLateUpdate;
        // public UnityEvent E_OnFixedLateUpdate;
        [FoldoutGroup("events")]
        public UnityEvent E_OnEnable;

        [FoldoutGroup("events")]
        public UnityEvent E_OnDisable;

        [FoldoutGroup("events")]
        public UnityEvent E_OnDestory;

        // [ShowInInspector, PropertyOrder(1)]
        // public Object SetScriptPath {
        //     get => scriptPath;
        //     set {
        //         scriptPath = value;
        //     #if UNITY_EDITOR
        //         m_ScriptPath = AssetDatabase.GetAssetPath(value);
        //     #endif
        //     }
        // }

        // [SerializeField, ReadOnly, PropertyOrder(2)]
        // string m_ScriptPath;
        //
        // [SerializeField, HideInInspector, PropertyOrder(3)]
        // Object scriptPath;

        [SerializeField]
        AssetLabelReference currentTag;

        [SerializeField]
        AssetLabelReference currentState;

        [SerializeField]
        bool BindScriptOnAwake;

        //List<TagState> m_SetState;

        // public EntityManager entityManager;
        // public Entity mainEntity;

        // [ShowInInspector]
        // public int id => gameObject.GetHashCode();

        // [ShowInInspector]
        // public int id => gameObject.GetInstanceID();

        // [ShowInInspector, TableList(AlwaysExpanded = true)]
        // List<TagState> setState {
        //     get {
        //         if (checkAddItem(out m_SetState)) {
        //             //m_SetState = m_SetState.OrderBy(t => t.order).ToList();
        //         }
        //
        //         return m_SetState;
        //     }
        //     set {
        //         if (checkAddItem(out m_SetState)) {
        //             m_SetState = value;
        //         }
        //     }
        // }

        void KeepSingle()
        {
            Invoke(nameof(CheckSingle), 0);
        }

        void OnButtonClick()
        {
            Debug.Log($"{gameObject.GetPath()}[tags]: {tags.Join()} {ids.Select(t => $"{t}").Join()}");

            // Debug.Log($"[TagNum] {TagManager.Data.Count} - {TagSystem.ids.Count()} {TagManager.Data.Select(t => t.name).Join()}");
        }

        public void RemoveAllListeners()
        {
            GetComponent<Button>()?.onClick.RemoveAllListeners();
        }

        public void Start()
        {
            //RemoveAllListeners();
            GetComponent<Button>()?.onClick.RemoveListener(OnButtonClick);
            GetComponent<Button>()?.onClick.AddListener(OnButtonClick);

            if (BindScriptOnstart && Application.isPlaying) {
                Debug.Log($"load: {gameObject.GetPath()} js onstart");
                Core.js.Call("globalThis.OnTagStart", this);
            }

            E_OnStart?.Invoke();
        }

        public void OnEnable()
        {
            gameObject.AddTag(ids.ToArray());

            E_OnEnable?.Invoke();
        }

        void OnDisable()
        {
            //gameObject.RemoveTag();
            E_OnDisable?.Invoke();
        }

        void OnDestroy()
        {
            E_OnDestory?.Invoke();
        }

        public bool checkAddItem(out List<TagState> value)
        {
            value = new List<TagState>();

            if (states != null && currentTag != null && currentState != null) {
                if (TagSystem.GetByName.TryGetValue(currentTag.labelString, out var tag)) {
                    var id = new AssetLabelReference() { labelString = tag.name };

                    if (!states.ContainsKey(id) || states[id] == null) {
                        states[id] = new Dictionary<AssetLabelReference, List<TagState>>();
                    }

                    if (TagSystem.GetByName.TryGetValue(currentState.labelString, out var state)) {
                        var stateId = new AssetLabelReference() { labelString = state.name };

                        if (!states[id].ContainsKey(stateId) || states[id][stateId] == null) {
                            states[id][stateId] = new List<TagState>();
                        }

                        value = states[id][stateId];

                        return true;
                    }
                }
            }

            return false;
        }

        //
        // [Button]
        // void AddTagStateItem()
        // {
        //     if (checkAddItem(out m_SetState)) {
        //         m_SetState.Add(new TagState());
        //     }
        //
        //     // GetType()
        //     //     .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        //     //     .Where(p => p.GetCustomAttribute<TestAttr>() != null)
        //     //     .ForEach(t => {
        //     //         if (t.Name == "str") {
        //     //             t.SetValue("123", this, null);
        //     //         }
        //     //     });
        // }

        // List<string> getTagsDropdown() => new List<string>() { "None" }.Of(t =>
        //     t.AddRange(TagSystem.GetByName.Where(t => ids.Contains(t.Value.Id)).Select(t => t.Key)));

        //#if UNITY_EDITOR
        void CheckSingle()
        {
            foreach (var comp in GetComponents<Tags>()) {
                if (comp != this) {
                    DestroyImmediate(this);
                }
            }
        }

        public bool isAwaked;

        public void Awake()
        {
            if (isAwaked) {
                return;
            }

            KeepSingle();

            //ids.RemoveAll(t => !TagSystem.ids.Contains(t));

            if (BindScriptOnAwake && Application.isPlaying) {
                Core.js.Call("globalThis.OnTagAwake", this);
            }
            E_OnAwake?.Invoke();
            isAwaked = true;
        }

        public bool Match(params string[] _tags)
        {
            return tags.All(t => _tags.Select(s => s._TagKey()).Contains(t));
        }
    }
}
