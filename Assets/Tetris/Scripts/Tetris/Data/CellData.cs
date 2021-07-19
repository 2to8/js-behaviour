using System.Collections.Generic;
using GameEngine.Models.Contracts;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [System.Serializable]
    public class CellData
    {
        public int id;

        [FormerlySerializedAs("cellId")]
        public int colorId;
    }
}