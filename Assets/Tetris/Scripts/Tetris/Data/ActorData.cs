using System.Collections.Generic;
using GameEngine.Models.Contracts;
using Sirenix.Serialization;
using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = nameof(ActorData), menuName = "Data/" + nameof(ActorData), order = 0)]
    public class ActorData : DbTable<ActorData>
    {
        [OdinSerialize]
        public Dictionary<(int x, int y), TheActor> data { get; set; } = new Dictionary<(int x, int y), TheActor>();
    }
}