using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Models.Actors {

[CreateAssetMenu(fileName = "ActorCommon", menuName = "App/ActorCommon", order = 0)]
public class ActorData : Model<ActorData> {

    [ShowInInspector, OdinSerialize]
    public string Title { get; set; }

}

}