using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Models.Maps {

[CreateAssetMenu(fileName = "MapData", menuName = "App/MapData", order = 0)]
public class MapData : Model<MapData> {

    [ShowInInspector, OdinSerialize]
    public string Version { get; set; }

}

}