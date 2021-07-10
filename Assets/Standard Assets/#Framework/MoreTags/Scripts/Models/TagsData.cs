using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SQLite.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreTags.Models {

[Serializable, Preserve]
public class TagsData : DbData<TagsData> {

    [SerializeField, HideInInspector]
    string m_Caption;

    [Unique, ShowInInspector]
    public string Caption { get => m_Caption; set => m_Caption = value; }

    [OdinSerialize, HideInInspector]
    public HashSet<GameObject> GameObjects = new HashSet<GameObject>();

    // public HashSet<GameObject> GameObjects { get => m_GameObjects; set => m_GameObjects = value; }

    [OdinSerialize, TableColumnWidth(10)]
    Color m_Color;

    public Color Color { get => m_Color; set => m_Color = value; }

}

}