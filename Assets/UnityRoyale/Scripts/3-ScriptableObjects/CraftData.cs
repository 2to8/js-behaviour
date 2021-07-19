using GameEngine.Attributes;
using GameEngine.Models.Contracts;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Utils.Scenes;

namespace UnityRoyale
{
    [PreloadSetting]
    public class CraftData : DbTable<CraftData>
    {
        [FormerlySerializedAs("Icons")]
        public List<RectTransformData> IconPostion = new List<RectTransformData>();

        public List<CraftItemData> items = new List<CraftItemData>();
    }
}