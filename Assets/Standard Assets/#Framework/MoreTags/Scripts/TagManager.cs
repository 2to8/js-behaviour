using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Kernel;
using GameEngine.Models.Contracts;
using MoreTags.Models;
using MoreTags.Types;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using SQLite.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Experimental.SceneManagement;

//using UnityEditor.ShaderGraph.Internal;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace MoreTags
{
    //[ExecuteInEditMode]//, RequireComponent(typeof(ConvertToEntity)),//[ExecuteAlways]
    //ShowOdinSerializedPropertiesInInspector, DefaultExecutionOrder(-56000)
    //, ISerializationCallbackReceiver

    //IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    [PreloadSetting]
    public class TagManager : DbTable<TagManager>
    {
        // public new static TagManager m_Instance;
        // public new static TagManager Instance => m_Instance ??= Core.FindOrCreatePreloadAsset<TagManager>();

        //static TagManager data;

        // public Entity mainEntity;
        // public EntityManager entityManager;
        // public List<AssetLabelReference> Labels;
        //
        // [SerializeField, LabelText("部件目录")]
        // Object ElementDir;

        // [SerializeField, ValueDropdown(nameof(GetWorldNameList))]
        // public string world = "All";

        // [ShowInInspector]
        // public TagComponentData components {
        //     get => m_Components ?? (m_Components = TagComponentData.Instance);
        //     set => m_Components = value;
        // }

        // [SerializeField, HideInInspector]
        // TagComponentData m_Components;

        // [SerializeField]
        // string customWorld;

        // List<string> GetWorldNameList()
        // {
        //     var ret = new List<string>(Enum.GetNames(typeof(WorldName)));
        //
        //     if (!string.IsNullOrEmpty(customWorld.Trim())) {
        //         ret.Add(customWorld.Trim());
        //     }
        //
        //     return ret;
        // }

        // [SerializeField,HideInInspector]
        // WorldName m_WorldName;

        // [ShowInInspector]
        // WorldName setWorld {
        //     get {
        //         if ($"{m_WorldName}" == world) {
        //             return m_WorldName;
        //         }
        //         return WorldName.Custom;
        //     }
        //     set {
        //         m_WorldName = value;
        //         if (value != WorldName.Custom) {
        //             world = $"{value}";
        //         }
        //     }
        // }
        //public static bool hasInstance => Instance.Any();

        // public static TagManager tryGetManager(Scene scene)
        // {
        //     if (!Instance.TryGetValue(scene, out var ret)) {
        //         // var root = scene.GetRootGameObjects().FirstOrDefault(go => go.name == scene.name) ??
        //         //     new GameObject(scene.name).Tap(go => {
        //         //         go.transform.SetAsFirstSibling();
        //         //
        //         //         //go.RequireComponent<SceneAdmin>();
        //         //     });
        //         Instance[scene] = CreateInstance<TagManager>(); // root.RequireComponent<TagManager>();
        //     }
        //
        //     return Instance[scene];
        // }

        //public static Dictionary<Scene, TagManager> Scenes => Scenes;

        //{
        //     get {
        //         if (Core.isQuit || Core.isBuilding) {
        //             Debug.Log("quitting".ToRed());
        //
        //             return null;
        //         }
        //
        //         if (m_Instance == null) {
        //             Debug.Log("get TagManager");
        //             m_Instance = Core.FindObjectOfTypeAll<TagManager>()
        //                 ?? Core.FindInPrefabOrScene<TagManager>();
        //         }
        //
        //         Assert.IsNotNull(m_Instance, "m_Instance != null");
        //
        //         return m_Instance;
        //     }
        //   set => m_Instance = value;
        //}

        [FormerlySerializedAs("m_tags"), SerializeField, TableList(AlwaysExpanded = true)]
        public List<TagData> data = new List<TagData>();

        public static List<TagData> Data => instance.data /* ??= TagData.FetchAll()
            .Of(t => {
                TagSystem.LoadData(t);
            })*/;

        public static void SaveAsset()
        {
        #if UNITY_EDITOR
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
        #endif
        }

        //[OdinSerialize]
        //Dictionary<string, TagData> m_Elements = new Dictionary<string, TagData>();

        //List<GameObject> m_ReferencedPrefabs;
        //
        // public Dictionary<string, TagData> Elements {
        //     get {
        //         //
        //         //     if (Core.isQuit) {
        //         //         return null;
        //         //     }
        //         // #if UNITY_EDITOR
        //         //     if (!m_Elements.Any() && ElementDir != null) {
        //         //         m_Elements["None"] = new TagData();
        //         //         var path = AssetDatabase.GetAssetPath(ElementDir);
        //         //
        //         //         if (!Directory.Exists(path)) {
        //         //             return Instances.m_Elements;
        //         //         }
        //         //
        //         //         AssetDatabase.FindAssets("t:TextAsset")
        //         //             .Select(AssetDatabase.GUIDToAssetPath)
        //         //             .Where(s => s.StartsWith(path + "/") && Path.GetExtension(s) == ".tsx")
        //         //
        //         //             //Directory.GetFiles(path, "*.tsx", SearchOption.AllDirectories)
        //         //             .ForEach(s => {
        //         //                 var tag = s.Substring(path.Length + 1)
        //         //                     .Replace(Path.GetExtension(s), "")
        //         //                     .Trim('\\', '/')
        //         //                     .Replace("/", ".")
        //         //                     .Replace("\\", ".")
        //         //                     ._TagKey();
        //         //                 var row = TagData.FirstOrDefault(t => t.name == tag);
        //         //
        //         //                 if (row.path != s.Substring(path.Length + 1)
        //         //                     || row.type != TagType.Element) {
        //         //                     row.name = tag;
        //         //                     row.color = Color.white;
        //         //                     row.type = TagType.Element;
        //         //                     row.path = s.Substring(path.Length + 1);
        //         //                     row.Insert();
        //         //                 }
        //         //
        //         //                 m_Elements[tag] = row;
        //         //             });
        //         //     }
        //         // #endif
        //         return m_Elements;
        //     }
        // }

        public static void Add(TagData tagsData)
        {
            Data.Add(tagsData);
            SaveAsset();
        }

        [Button, ButtonGroup("SaveLoad")]
        void LoadTags()
        {
            Debug.Log(string.Join(", ", TagSystem.AllTags()));
            data.Clear();

            data = TagData.FetchAll();
            data.Where(t => string.IsNullOrWhiteSpace(t.name))
                .ForEach(t => {
                    TagData.Delete(n => n.Id == t.Id);
                });
            data = TagData.FetchAll();

            // var all = tagData.Where(t => t.Id != 0 && !string.IsNullOrEmpty(t.name));
            Debug.Log($"All: {data.Count()}");
            SaveAsset();

            //TagSystem.LoadDataToTable(m_tags);
        }

        [Button, ButtonGroup("SaveLoad")]
        void SaveTags()
        {
            var data = TagData.FetchAll();
            var ids = new Dictionary<int, TagData>();
            data.ForEach(t => ids[t.Id] = t);
            var tids = new Dictionary<int, TagData>();
            this.data.ForEach(t => tids[t.Id] = t);

            data.ForEach(t => {
                if (!tids.ContainsKey(t.Id)) {
                    TagData.Delete(n => n.Id == t.Id);
                } else {
                    DB.Update(tids[t.Id]);
                }
            });
            this.data.Where(t => t.Id > 0 && !string.IsNullOrWhiteSpace(t.name) && !ids.ContainsKey(t.Id))
                .ForEach(t => {
                    t.name = t.name.Trim();
                    DB.Insert(t);
                });
        }

        public static void SyncLabels()
        {
        #if UNITY_EDITOR
            if (instance == null) {
                Debug.Log("instance not loaded");

                return;
            }
            var config = AddressableAssetSettingsDefaultObject.Settings;

            if (config == null) {
                Debug.LogError("asset settings not found");
            }
            var labels = config.GetLabels();
            labels.ForEach(t => {
                if (Data.All(x => x.name != t)) {
                    TagData.FirstOrInsert(tt => tt.name == t, tt => {
                        tt.name = t;
                        Data.Add(tt);
                        Debug.Log($"tag added {t}".ToRed());
                    });
                }
            });
            Data.ForEach(t => {
                if (!labels.Contains(t.name)) {
                    config.AddLabel(t.name);
                    Debug.Log($"label added {t.name}".ToRed());
                }
            });

            //
            // if (!labels.Contains("TestAddLable")) {
            //     config.AddLabel("TestAddLable");
            //     Debug.Log("added".ToRed());
            // } else {
            //     config.RemoveLabel("TestAddLable");
            //     Debug.Log("removed".ToRed());
            // }
        #endif
        }

    #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void OnInitialize()
        {
            Core.FindOrCreatePreloadAsset<TagManager>();
        }
    #endif

        // #endif
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Instance = m_Instance ??= this;

            //if (data == null) {

            if (!data.Any()) {
                data = TagData.FetchAll();
                Debug.Log("Tag Manager Init".ToGreen());
            }
            TagSystem.LoadData(data,true);
            Debug.Log($"[TagNum] {data.Count} {data.Select(t => t.name).Join()}");

            //  }

            // if (!m_Instances.ContainsKey(SceneManager.GetActiveScene())) {
            //     m_Instances[SceneManager.GetActiveScene()] = this;
            //
            //     // if (Application.isPlaying) {
            //     //     DontDestroyOnLoad(gameObject);
            //     // }
            // }

            // var tm = Core.FindAllOrInPrefab<TagManager>(gameObject);
            //
            // if (tm.Length == 0 || tm.Length == 1 && tm[0] == this) {

            // }
        }

        // protected override void OnAfterDeserialize()
        // {
        //     base.OnAfterDeserialize();
        //
        //     // if (m_Instance.Any()) {
        //     //     Debug.Log("after serialize");
        //     //     TagSystem.LoadDataToTable(m_tags);
        //     // }
        // }
        //
        // protected override void OnBeforeSerialize()
        // {
        //     base.OnBeforeSerialize();
        //
        //     // if (gameObject == null) {
        //     //     return;
        //     // }
        //
        //     TagSystem.BeforeSerialize(ref m_tags, SceneManager.GetActiveScene());
        // }

        /*
        public void CheckDatabase()
        {
            return;

            DB.Table<TagsData>().Delete(t => string.IsNullOrEmpty(t.Caption));
            Debug.Log(m_tags.Count().Red("m_tags"));
            m_tags.ForEach(t => {
                t.name = t.name._TagKey();
                var row = DB.Table<TagsData>().FirstOrDefault(tn => tn.Caption == t.name);
                if (row.Id == 0) {
                    row.Caption = t.name;
                    row.Color = t.color;

                    // if (row.GameObjects == null) {
                    //     row.GameObjects = new HashSet<GameObject>();
                    // }
                    // t.gameObjects.ForEach(go => {
                    //     if (!row.GameObjects.Contains(go)) {
                    //         row.GameObjects.Add(go);
                    //     }
                    // });
                    Debug.Log(
                        $"caption: {row.Caption} color: {row.Color} id: {row.Id} hash: {JsonConvert.SerializeObject(row.GameObjects)}");
                    DB.Insert(row);
                }
            });
        }*/
        // public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        // {
        //     this.referencedPrefabs = referencedPrefabs;
        // }

        // public List<GameObject> referencedPrefabs { get => m_ReferencedPrefabs; set => m_ReferencedPrefabs = value; }

        // public void Convert(Entity entity, EntityManager dstManager,
        //     GameObjectConversionSystem conversionSystem)
        // {
        //     mainEntity = entity;
        //     entityManager = dstManager;
        //     dstManager.AddComponentData(entity, new TagManagerEntry() {
        //         ready = false,
        //         active = true,
        //         instance = entity,
        //     });
        //     TagData.Table()
        //         .Where(t => t.type == TagType.System)
        //         .ForEach(t => {
        //             var e = dstManager.CreateEntity();
        //             dstManager.AddComponentData(e, new ReactSystemEntry() {
        //                 tagId = t.Id,
        //                 name = t.name,
        //                 ready = false,
        //                 active = true,
        //                 instance = e,
        //                 agent = entity,
        //             });
        //         });
        // }
    }
}