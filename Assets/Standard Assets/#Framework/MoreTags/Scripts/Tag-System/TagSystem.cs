using GameEngine.Extensions;
using GameEngine.Kernel;
using JetBrains.Annotations;
using MoreTags.Models;

// using MoreTags.Query;
using MoreTags.Types;

// using NodeCanvas;
// using NodeCanvas.Common.Design;
#if UNITY_EDITOR

// using NodeCanvas.Common.Design.PartialEditor.EditorUtils;
#endif
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.ComponentModel;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using TMPro;
using UniRx.Async;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Component = UnityEngine.Component;
using Object = UnityEngine.Object;

namespace MoreTags
{
    public static class TagSystem
    {
        public static TagPattern pattern => new TagPatternImpl();

        //static TagManager s_TagManager;
#if UNITY_EDITOR
        static void m_ClearInEditor()
        {
            refs.Clear();
        }
#endif

        // 保存所有场景的 gameobject 的引用
        public static TagReference refs {
            get {
                if (!m_References.Any()) {
                    m_References.FetchAll();
                    if (!Application.isPlaying) {
                        Debug.Log("init gameobjects for tags".ToBlue());
                        Core.GetAllLoadedScenes().SelectMany(scene => scene.GetRootGameObjects()).ForEach(go => {
                            go.GetComponentsInChildren<Tags>(true).Where(t => t != null && t.ids.Any()).ForEach(t => {
                                t.gameObject.AddTag(t.ids.ToArray());
                            });
                        });
                    }
                }

                return m_References;
            }
        }

        static TagReference m_References = new TagReference();

        //new Dictionary<string, TagTableData>();
        static GameObject[] s_SearchFrom = null;

        // static Node[] s_SearchFromNode = null;

        public static void Reset()
        {
            refs.Clear();
        }

        static Action m_RunOnce;

        /// <summary>
        /// 保存引用到当前场景
        /// </summary>
        /// <param name="m_tags"></param>
        /// <param name="scene"></param>
        public static void BeforeSerialize(ref List<TagData> m_tags, Scene scene)
        {
            var tmp = new List<TagData>();
            Core.RunOnceOnScriptReload(ref m_RunOnce, () => {
                Debug.Log("before serialize");
                var data = TagData.FetchAll();
                Debug.Log("tags: " + string.Join(", ", data.Select(t => $"{t.name} => {t.Id}")));
                data.ForEach(row => {
                    if (!refs.ContainsKey(row.name)) {
                        refs[row.name] = new TagRefs() {
                            color = row.color.TagColor(),
                            Id = row.Id,

                            // nodes = new HashSet<Node>(),
                            gameObjects = new HashSet<GameObject>(),
                        };
                    }
                    else {
                        refs[row.name].color = row.color.TagColor();
                        refs[row.name].Id = row.Id;
                    }
                });
                refs.Keys.ToList().ForEach(s => {
                    if (data.All(row => row.name != s)) {
                        refs.Remove(s);
                    }
                });
                tmp = refs.Select((kv) => new TagData() {
                    name = kv.Key,
                    color = data.First(t => t.name == kv.Key).color.TagColor(), // kv.Value.color,
                    Id = data.First(t => t.name == kv.Key).Id, // kv.Value.Id,
                    gameObjects = kv.Value.gameObjects.Where(go => go != null && go.scene == scene).ToArray(),
                }).ToList();
            });
            if (tmp.Any()) {
                Debug.Log("reload all tags: " + string.Join(", ", tmp.Select(t => $"{t.Id} = {t.name}")));
                m_tags = tmp;
            }
        }

        public static TagQuery query => new TagQuery();
        public static T Find<T>(params string[] tags) where T : Component => Find<T>(new GameObject[] { }, tags);

        // query.tags(tags)
        // .withTypes(typeof(T))
        // .result.Select(go => go.GetComponent<T>())
        // .FirstOrDefault(t => t != null);

        public static T Find<T>(this GameObject[] parent, params string[] tags) where T : Component =>
            query.Parent(parent).tags(tags).withTypes(typeof(T)).result.Select(go => go.GetComponent<T>())
                .FirstOrDefault(t => t != null);

        public static T Find<T>(this Component parent, params string[] tags) where T : Component =>
            Find<T>(new[] {parent?.gameObject}, tags);

        public static T Find<T>(this GameObject parent, params string[] tags) where T : Component =>
            Find<T>(new[] {parent}, tags);

        public static IEnumerable<GameObject> Query(Transform[] parent = null,
            params (string[] tag, Type[] type)[] query)
        {
            //if (!Application.isPlaying) return new List<GameObject>();
            var result = new List<IEnumerable<GameObject>>();
            IEnumerable<GameObject> ret = null;
            query.ForEach((q, i) => {
                if (ret == null || ret.Any()) {
                    ret = q.tag.SelectMany(t => GetGameObjects(t).Where(go =>
                        go != null && q.tag.All(nt => GetGameObjects(nt).Contains(go)) &&
                        (i == 0 || result[i - 1].Any(p => go.GetParents().Contains(p))) && (q.type == null ||
                            !q.type.Any() || q.type.All(type => go.GetComponent(type) != null))));
                    result.Add(ret);
                }
            });
            return ret?.Where(t => parent == null || parent.All(p => p?.gameObject == null) || parent
                .Where(p => p?.gameObject != null)
                .Any(p => t.transform.GetComponentsInParent<Transform>(true).Contains(p))) ?? new List<GameObject>();
        }

        public static IEnumerable<Node> QueryNodes(Graph[] parent = null, params (string[] tag, Type[] type)[] query)
        {
            //if (!Application.isPlaying) return new List<GameObject>();
            var result = new List<IEnumerable<Node>>();
            IEnumerable<Node> ret = null;
            query.ForEach((q, i) => {
                if (ret == null || ret.Any()) {
                    ret = q.tag.SelectMany(t => GetNodes(t).Where(go =>
                        go != null && q.tag.All(nt => GetNodes(nt).Contains(go)) &&
                        (i == 0 || result[i - 1].Any(p => go.GetType() == p.GetType())) && (q.type == null ||
                            !q.type.Any() || q.type.All(type => go.GetType() == type))));
                    result.Add(ret);
                }
            });
            return ret?.Where(t => parent == null || parent.All(p => p == null) || parent
                .Where(p => p != null).Any(p => p.allNodes.Contains(t))) ?? new List<Node>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void ResetInit()
        {
            if (TagManager.instance != null) {
                LoadData(TagManager.Data);
            }
        }

        public static void LoadData(List<TagData> tagData, bool force = false)
        {
            // if (refs.Any() && !force) {
            //     return;
            // }

            //Reset();
            var all = tagData.Where(t => t.Id != 0 && !string.IsNullOrEmpty(t.name));
            Debug.Log($"All: {all.Count()}");
            foreach (var data in all) {
                var list = data.gameObjects;
                if (refs.ContainsKey(data.name)) {
                    list = refs[data.name].gameObjects.Union(list).ToArray();
                }
                else {
                    refs[data.name] = new TagRefs();
                    refs[data.name].gameObjects ??= new HashSet<GameObject>(list);
                    refs[data.name].gameObjects.AddRange(list);
                }

                if (list.Any()) {
                    Debug.Log($"{data.name}: {list.Count()}");
                }

                refs[data.name].Id = data.Id;
                refs[data.name].color = data.color;
            }
        }

        public static IEnumerable<string> GetTags(IEnumerable<int> ids)
        {
            if (ids == null) ids = new List<int>();
            return refs.Where(t => ids.Contains(t.Value.Id)).Select(t => t.Key._TagKey());
        }

        // public static bool CheckTagManager(this GameObject gameObject)
        // {
        //     // s_TagManager = TagManager.tryGetManager(gameObject.scene);
        //
        //     return true;
        // }

        // public static void CheckTagManager(this Scene scene)
        // {
        //     // if (s_TagManager != null) {
        //     //     return;
        //     // }
        //     //
        //     // if (!scene.isLoaded) {
        //     //     return;
        //     // }
        //     // var manager = TagManager.tryGetManager(scene);
        //     // s_TagManager = manager;
        //
        //     // if (s_TagManager == null) {
        //     //     var go = new GameObject("Tag Manager");
        //     //     SceneManager.MoveGameObjectToScene(go, scene);
        //     //
        //     //     //go.AddComponent<TagManager>();
        //     // }
        // }

        // public static void CheckGameObjectTag(this GameObject go)
        // {
        //     var tags = go.GetComponent<Tags>();
        //
        //     if (tags == null) {
        //         go.AddComponent<Tags>();
        //     }
        // }

        public static void CheckGameObjectTag(this Node node)
        {
            //var tags = node.Tags;
            if (node.Tags == null) {
                node.Tags = new HashSet<string>();
            }

            node.tag.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s)).ForEach(s => {
                    if (!node.Tags.Any(t => t.Equals(s, StringComparison.OrdinalIgnoreCase))) {
                        node.Tags.Add(s);
                    }
                });
            if (node.tag != string.Join(",", node.Tags.Distinct())) {
                node.tag = string.Join(",", node.Tags.Distinct());
                UndoUtility.SetDirty(node.graph);
            }

            Debug.Log($"node tags: {node.tag}");
        }

        public static void RemoveUnusedTag()
        {
            RemoveNullGameObject();
            var remove = refs.Where(kv => !refs[kv.Key].gameObjects.Any()).Select(kv => kv.Key).ToArray();
            foreach (var key in remove) {
                refs.Remove(key);
            }
        }

        public static void RemoveNullGameObject()
        {
            foreach (var data in refs) {
                data.Value.gameObjects.RemoveWhere(go => go == null);
            }
        }

        public static void SearchFrom(IEnumerable<GameObject> list = null)
        {
            s_SearchFrom = list?.ToArray();
        }

        public static void AddTag(params string[] tags)
        {
            tags.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ForEach(tag => {
                var data = TagData.FirstOrInsert(t => t.name == tag, add => {
                    add.name = tag;
                    TagManager.Add(add);
                });
                if (!refs.ContainsKey(tag)) {
                    refs.Add(tag, new TagRefs());
                }
            });
        }

        public static Dictionary<int, TagData> GetById {
            get {
                var data = new Dictionary<int, TagData>();
                TagManager.Data.ForEach(t => data[t.Id] = t);
                return data;
            }
        }

        //public static IEnumerable<string> alltags => TagSystem.GetAllTags();

        public static Dictionary<string, TagData> GetByName {
            get {
                var data = new Dictionary<string, TagData>();
                TagManager.Data.ForEach(t => data[t.name] = t);
                return data;
            }
        }

        public static void AddTag(this Node node, params string[] tags)
        {
            Debug.Log("tags: " + string.Join(", ", tags));
            var tt = new List<string>();
            tags.SelectMany(s => s.Split(',')).ForEach(s => tt.Add(s));
            foreach (var _t in tt.Distinct()) {
                var tag = _t._TagKey();
                if (!refs.ContainsKey(tag)) {
                    refs.Add(tag, new TagRefs());
                    TagData.FirstOrInsert(t => t.name == tag, add => { add.name = tag; });
                }

                if (node != null) {
                    refs[tag].nodes.Add(node);
                    if (!node.Tags.Contains(tag)) {
                        node.Tags.Add(tag);
                    }

                    // if (!gameObject.GetTags().Contains(tag)) {
                    //
                    // }
                    // if (!gameObject.tag.Contains(tag)) {
                    //     gameObject.tag += (string.IsNullOrEmpty(gameObject.tag) ? string.Empty : ",")
                    //         + tag;
                    //     UndoUtility.SetDirty(gameObject.graph);
                    // }

                    //gameObject.RequireComponent<Tags>().ids.AddOnce(refs[tag].Id);
                }
            }
        }

        public static void AddTag(this GameObject gameObject, params string[] tags)
        {
            if (gameObject == null) return;
            tags.Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ForEach(tag => {
                if (!refs.ContainsKey(tag)) {
                    refs.Add(tag, new TagRefs());
                    TagData.FirstOrInsert(t => t.name == tag, add => {
                        add.name = tag;
                        TagManager.Add(add);
                    });
                }

                // if (Application.isPlaying) {
                refs[tag].gameObjects.Add(gameObject);

                // }
                gameObject.RequireComponent<Tags>().ids.AddOnce(refs[tag].Id);
            });
        }

        public static void AddTag(this GameObject gameObject, params int[] id)
        {
            AddTag(gameObject, Tags(id));
        }

        /// <summary>
        /// 不带GameObject 参数的直接从系统中移除
        /// </summary>
        /// <param name="tags"></param>
        public static void RemoveTag(params string[] tags)
        {
            var changed = false;
            foreach (var tag in tags) {
                if (refs.ContainsKey(tag)) {
                    refs.Remove(tag);
                    TagData.Delete(t => t.name == tag);
                    TagManager.Data.RemoveAll(t => t.name == tag);
                    changed = true;
                }
            }

            if (changed) {
                TagManager.SaveAsset();
            }
        }

        /// <summary>
        /// 带GameObject 参数的从当前 GameObject 移除
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="tags"></param>
        public static void RemoveTag(this GameObject gameObject, params string[] tags)
        {
            if (!tags.Any()) {
                foreach (var data in refs) {
                    data.Value.gameObjects.RemoveWhere(go => go == null || gameObject == go);
                }
            }
            else {
                foreach (var tag in tags) {
                    if (refs.ContainsKey(tag)) {
                        refs[tag].gameObjects.Remove(gameObject);
                        gameObject.RequireComponent<Tags>().ids.RemoveOnce(refs[tag].Id);
                    }
                }
            }
        }

        public static void RenameTag(string old, string tag)
        {
            if (string.IsNullOrEmpty(tag)) {
                return;
            }

            if (!refs.ContainsKey(old)) {
                return;
            }

            if (refs.ContainsKey(tag) || TagData.FirstOrInsert(t => t.name == tag).Id != 0) {
                return;
            }

            refs[tag] = refs[old];
            refs.Remove(old);
            TagData.FirstOrInsert(t => t.name == old, row => { row.name = tag; });
            GetById[refs[tag].Id].name = tag;
            TagManager.SaveAsset();
        }

        public static void SetTagColor(string tag, Color col)
        {
            refs[tag].color = col;
            TagData.FirstOrInsert(t => t.name == tag, row => row.color = col);
        }

        public static Color GetTagColor(string tag) => refs[tag].color;

        // public static void AddGameObjectTag(GameObject go, params string[] tags)
        // {
        //     AddTag(go, tags);
        // }

        public static void AddGameObjectTag(this Node go, params string[] tags)
        {
            //CheckTagManager(go.scene);
            CheckGameObjectTag(go);
            AddTag(go, go.Tags.ToArray());
            tags.Where(s => go.Tags.All(t => !t.Trim().Equals(s.Trim(), StringComparison.OrdinalIgnoreCase))).ForEach(
                s => {
                    go.Tags.Add(s.Trim());
                    refs[s.Trim()].nodes.Add(go);
                });
            foreach (var tag in tags) {
                if (!refs[tag].nodes.Contains(go)) {
                    refs[tag].nodes.Add(go);
                }
            }
        }

        // public static void RemoveTags(this GameObject go, params string[] tags)
        // {
        //     var component = go.RequireComponent<Tags>();
        //
        //     foreach (var tag in tags) {
        //         if (refs.ContainsKey(tag)) {
        //             if (component.ids.Contains(refs[tag].Id)) {
        //                 component.ids.Remove(refs[tag].Id);
        //             }
        //
        //             refs[tag].gameObjects.Remove(go);
        //         }
        //     }
        // }

        public static void RemoveNodeTag(this Node go, params string[] tags)
        {
            // CheckTagManager(go.scene);
            //tags.Where(s => go.Tags.Contains(s)).ForEach(s => go.Tags.Remove(s));
            var list = go.Tags.ToList();
            list.Where(s => tags.Any(t => t.Trim().Equals(s.Trim(), StringComparison.OrdinalIgnoreCase)))
                .ForEach(s => go.Tags.Remove(s));
            foreach (var tag in tags) {
                if (refs.ContainsKey(tag)) {
                    refs[tag].nodes.Remove(go);
                }

                if (go.Tags.Contains(tag)) {
                    go.Tags.Remove(tag);
                }
            }

            go.tag = string.Join(",", go.Tags);
            CheckGameObjectTag(go);
        }

        public static string[] Tags(this GameObject go)
        {
            return refs.Where(kv => kv.Value.gameObjects.Contains(go)).Select(kv => kv.Key).ToArray();
        }

        public static string[] Tags() => refs.Keys.ToArray();

        public static string[] Tags(params int[] ids) =>
            ids.Any() ? refs.Where(tk => ids.Contains(tk.Value.Id)).Select(tk => tk.Key).ToArray() : new string[] { };

        public static Dictionary<int, string> TagsId() =>
            new Dictionary<int, string>().Of(tk => { refs.ForEach(k => tk[k.Value.Id] = k.Key); });

        public static IEnumerable<int> ids => refs.Select(tk => tk.Value.Id);
        public static GameObject GetGameObject(string tag) => pattern.With(tag._TagKey()).GameObject();
        public static GameObject[] GetGameObjects(string tag) => pattern.With(tag._TagKey()).GameObjects();
        public static Node[] GetNodes(string tag) => pattern.With(tag._TagKey()).Nodes();

        public static IEnumerable<string> GameObjectTags(this GameObject go)
        {
            // if (Application.isEditor && !Application.isPlaying) {
            //     if (go.TryGetComponent<Tags>(out var tags)) {
            //         return refs.Where(kv => tags.ids.Contains(kv.Value.Id)).Select(kv => kv.Key);
            //     } else {
            //         return new List<string>();
            //     }
            // }
            return refs.Where(kv => kv.Value.gameObjects.Contains(go)).Select(kv => kv.Key);
        }

        public static IEnumerable<string> NodeTags(this Node node)
        {
            return refs.Where(kv => kv.Value.nodes.Contains(node)).Select(kv => kv.Key);
        }

        public static IEnumerable<string> AllTags() => refs.Keys.AsEnumerable();

        #region TagPattern

        class TagPatternImpl : TagPattern
        {
            enum Mode
            {
                And,
                Or,
                Exclude
            }

            Mode m_Mode = Mode.And;
            HashSet<GameObject> m_List = null;
            HashSet<Node> m_NodeList = null;

            HashSet<GameObject> AllGameObject()
            {
                var e = new HashSet<GameObject>(Object.FindObjectsOfType<GameObject>().Where(go => go.scene.isLoaded));
                return s_SearchFrom == null ? e : e.And(s_SearchFrom);
            }

            HashSet<GameObject> Empty() => new HashSet<GameObject>();
            HashSet<Node> EmptyNodes() => new HashSet<Node>();
            public override GameObject GameObject() => GameObjects().FirstOrDefault();

            public override GameObject[] GameObjects()
            {
                var list = m_List ?? Empty();
                return list.ToArray();
            }

            public override Node[] Nodes()
            {
                return (m_NodeList ?? EmptyNodes()).ToArray();
            }

            public override int Count() => m_List.Count;

            public override TagPattern And()
            {
                m_Mode = Mode.And;
                return this;
            }

            public override TagPattern Or()
            {
                m_Mode = Mode.Or;
                return this;
            }

            public override TagPattern Exclude()
            {
                m_Mode = Mode.Exclude;
                return this;
            }

            public override TagPattern And(TagPattern pattern) => Combine(pattern, Mode.And);
            public override TagPattern Or(TagPattern pattern) => Combine(pattern, Mode.Or);
            public override TagPattern Exclude(TagPattern pattern) => Combine(pattern, Mode.Exclude);
            public override TagPattern Combine(TagPattern pattern) => Combine(pattern, m_Mode);

            TagPattern Combine(TagPattern pattern, Mode mode)
            {
                var pat = pattern as TagPatternImpl;
                return Combine(pat.m_List, mode);
            }

            TagPattern Combine(HashSet<GameObject> list) => Combine(list, m_Mode);

            TagPattern Combine(HashSet<GameObject> list, Mode mode)
            {
                list = list == null ? Empty() : list;
                switch (mode) {
                    case Mode.And:
                        m_List = m_List == null ? list : m_List.And(list);
                        break;
                    case Mode.Or:
                        m_List = m_List == null ? list : m_List.Or(list);
                        break;
                    case Mode.Exclude:
                        m_List = m_List == null ? AllGameObject().Exclude(list) : m_List.Exclude(list);
                        break;
                }

                return this;
            }

            public override TagPattern All()
            {
                m_List = AllGameObject();
                return this;
            }

            public override TagPattern With(string tag) => Combine(WithInternal(tag));

            HashSet<GameObject> WithInternal(string tag)
            {
                var e = string.IsNullOrEmpty(tag) || !refs.ContainsKey(tag) ? Empty() : refs[tag].gameObjects;
                return s_SearchFrom == null ? e : e.And(s_SearchFrom);
            }

            public override TagPattern Both(params string[] tags) => Combine(BothInternal(tags));

            HashSet<GameObject> BothInternal(IEnumerable<string> tags)
            {
                if (!tags.Any()) {
                    return Empty();
                }

                foreach (var tag in tags) {
                    if (!refs.ContainsKey(tag)) {
                        return Empty();
                    }
                }

                var e = WithInternal(tags.First());
                foreach (var tag in tags.Skip(1)) {
                    e = e.And(refs[tag].gameObjects, s_SearchFrom);
                }

                return e;
            }

            public override TagPattern Either(params string[] tags) => Combine(EitherInternal(tags));

            HashSet<GameObject> EitherInternal(IEnumerable<string> tags)
            {
                var list = new List<string>();
                foreach (var tag in tags) {
                    if (refs.ContainsKey(tag)) {
                        list.Add(tag);
                    }
                }

                if (list.Count == 0) {
                    return Empty();
                }

                var e = WithInternal(list.First());
                foreach (var tag in list.Skip(1)) {
                    e = e.Or(refs[tag].gameObjects, s_SearchFrom);
                }

                return e;
            }
        }

        #endregion

        #region HashSet Operation

        static HashSet<T> And<T>(this HashSet<T> a, HashSet<T> b)
        {
            var less = a.Count < b.Count ? a : b;
            var more = a.Count >= b.Count ? a : b;
            var result = new HashSet<T>();
            foreach (var item in less) {
                if (more.Contains(item)) {
                    result.Add(item);
                }
            }

            return result;
        }

        static HashSet<T> Or<T>(this HashSet<T> a, HashSet<T> b)
        {
            var less = a.Count < b.Count ? a : b;
            var more = a.Count >= b.Count ? a : b;
            var result = new HashSet<T>(more);
            foreach (var item in less) {
                result.Add(item);
            }

            return result;
        }

        static HashSet<T> Exclude<T>(this HashSet<T> a, HashSet<T> b)
        {
            var result = new HashSet<T>(a);
            foreach (var item in b) {
                result.Remove(item);
            }

            return result;
        }

        static HashSet<T> And<T>(this HashSet<T> a, IEnumerable<T> b)
        {
            var result = new HashSet<T>();
            foreach (var item in b) {
                if (a.Contains(item)) {
                    result.Add(item);
                }
            }

            return result;
        }

        static HashSet<T> And<T>(this HashSet<T> a, HashSet<T> b, IEnumerable<T> c)
        {
            if (c == null) {
                return a.And(b);
            }

            var result = new HashSet<T>();
            foreach (var item in c) {
                if (a.Contains(item) && b.Contains(item)) {
                    result.Add(item);
                }
            }

            return result;
        }

        static HashSet<T> Or<T>(this HashSet<T> a, HashSet<T> b, IEnumerable<T> c)
        {
            if (c == null) {
                return a.Or(b);
            }

            var result = new HashSet<T>(a);
            foreach (var item in c) {
                if (b.Contains(item)) {
                    result.Add(item);
                }
            }

            return result;
        }

        #endregion

        public static void DoChange(string name, object value)
        {
            TagSystem.query.tags(name).result.ForEach(go => { SetText(go, value); });
        }

        public static void SetText(GameObject go, object value)
        {
            var has = go.GetComponent<TMP_Text>()?.Of(t => t.text = $"{value}") != null;
            has = has || go.GetComponent<Text>()?.Of(t => t.text = $"{value}") != null;
            has = has || go.GetComponent<Slider>()?.Of(t => t.value = Convert.ToInt32(value)) != null;
            has = has || go.GetComponent<RadialSlider>()
                ?.Of(t => go.GetComponent<Image>().fillAmount = Convert.ToSingle(value)) != null;
            if (has && isDisplayDebug) Debug.Log(go.name, go);
        }

        public static bool isDisplayDebug { get; set; }
    }
}