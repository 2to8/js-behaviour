using GameEngine.Models.Contracts;
using MoreTags.Types;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags.TagHelpers {

[Serializable]
public class TagList : DbData<TagList> {

    [OdinSerialize]
    public List<string> tags { get; set; } = new List<string>();

    public List<int> getById => tags
        .Select(s => TagSystem.GetByName.TryGetValue(s, out var tagData) ? tagData.Id : 0)
        .Where(id => id != 0)
        .ToList();

    [OdinSerialize]
    public TagType tagType { get; set; } = TagType.Tag;

    [SerializeField]
    public List<string> shared = new List<string>();

    // 多个列表共享的tags
    public TagList(List<string> shared)
    {
        this.shared = shared;
    }

    public TagList() { }

    // [SerializeField, HideInInspector]
    // private SerializationData serializationData;
    //
    // SerializationData m_SerializationData;
    //
    // void ISerializationCallbackReceiver.OnAfterDeserialize()
    // {
    //     UnitySerializationUtility.de(this, ref this.serializationData);
    // }
    //
    // void ISerializationCallbackReceiver.OnBeforeSerialize()
    // {
    //     UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    // }
    //
    // public SerializationData SerializationData {
    //     get => m_SerializationData;
    //     set => m_SerializationData = value;
    // }

}

}