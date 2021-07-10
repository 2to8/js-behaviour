//using NodeCanvas.Framework;

using GameEngine.Models.Contracts;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SQLite.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreTags.TagHelpers {

[Serializable]
public class TagPress : DbTable<TagPress> {

    [ReadOnly, OdinSerialize]
    public string code { get; set; }

    [OdinSerialize, Unique]
    public int tid { get; set; }

    // [OdinSerialize, Title("是否启用", HorizontalLine = false)]
    // public new bool Enabled { get; set; }

    [OdinSerialize, Title("监听start", HorizontalLine = false)]
    public bool OnStartListener { get; set; }

    [OdinSerialize, Title("监听Update", HorizontalLine = false)]
    public bool OnUpdateListener { get; set; }

    [NonSerialized]
    public TextAsset module;

    [OdinSerialize, Unique, ReadOnly]
    public string path { get; set; }

    public List<string> findList = new List<string>();
    public List<string> setList = new List<string>();

    [SerializeField, PropertyOrder(-2), FoldoutGroup("Finder")]
    TagList m_WithAll;

    [SerializeField, PropertyOrder(-2), FoldoutGroup("Finder")]
    TagList m_WithAny;

    [SerializeField, PropertyOrder(-2), FoldoutGroup("Finder")]
    TagList m_WithNone;

    [SerializeField, PropertyOrder(-1), FoldoutGroup("Setter")]
    TagList m_ResultAdd;

    [SerializeField, PropertyOrder(-1), FoldoutGroup("Setter")]
    TagList m_ResultRemove;

    [OdinSerialize, HideInInspector]
    public TagList WithAll { get => m_WithAll; set => m_WithAll = value; }

    [OdinSerialize, HideInInspector]
    public TagList WithAny { get => m_WithAny; set => m_WithAny = value; }

    [OdinSerialize, HideInInspector]
    public TagList WithNone { get => m_WithNone; set => m_WithNone = value; }

    [OdinSerialize, HideInInspector]
    public TagList ResultAdd { get => m_ResultAdd; set => m_ResultAdd = value; }

    [OdinSerialize, HideInInspector]
    public TagList ResultRemove { get => m_ResultRemove; set => m_ResultRemove = value; }

    // public void Apply(Action<Node> action) { }
    public void Apply(Action<GameObject> action) { }

    protected override void Awake()
    {
        base.Awake();
        if (WithAll == null) {
            WithAll = new TagList(findList);
        }
        if (WithAny == null) {
            WithAny = new TagList(findList);
        }
        if (WithNone == null) {
            WithNone = new TagList(findList);
        }
        if (ResultAdd == null) {
            ResultAdd = new TagList(setList);
        }
        if (ResultRemove == null) {
            ResultRemove = new TagList(setList);
        }
    }

    void OnValidate()
    {
        _保存表();
        Debug.Log($"saved {Id} {JsonConvert.SerializeObject(this)}");
    }

    // [Button]
    // void test() { }

}

}