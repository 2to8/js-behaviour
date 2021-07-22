using GameEngine.Models.Contracts;
using MoreTags.Types;
using Sirenix.OdinInspector;
using SQLite.Attributes;
using System;
using Sirenix.Serialization;
using UnityEngine;

namespace MoreTags
{
    [Serializable]
    public class TagData : DbData<TagData>
    {
        [SerializeField]
        string m_Name;

        [SerializeField, TableColumnWidth(4)]
        Color m_Color = Color.white;

        [SerializeField, HideInInspector]
        TagType m_Type = TagType.Tag;

        [SerializeField, HideInInspector]
        string m_Path;

        [Unique]
        public string name {
            get => m_Name;
            set => m_Name = value;
        }

        [HideInInspector]
        public TagType type {
            get => m_Type;
            set => m_Type = value;
        }

        [HideInInspector]
        public string path {
            get => m_Path;
            set => m_Path = value;
        }

        public Color color {
            get => m_Color;
            set => m_Color = value;
        }

        [HideInInspector]
        public GameObject[] gameObjects = new GameObject[] { };

        [OdinSerialize]
        public string comment { get; set; }
    }
}