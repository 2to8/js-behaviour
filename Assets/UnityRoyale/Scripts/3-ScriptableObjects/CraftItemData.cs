using GameEngine.Attributes;
using GameEngine.Models.Contracts;
using UnityEngine;
using Sirenix.OdinInspector;

namespace UnityRoyale
{
    [PreloadSetting]
    public class CraftItemData : DbTable<CraftItemData>
    {
        public RectTransform icon { get; set; }
    }
}