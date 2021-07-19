using System.Collections.Generic;
using Consts;
using Sirenix.Serialization;
using Tetris;
using UnityEngine;

namespace Battle
{
    [SceneBind(SceneName.Main)]
    public class ActorLogic : ViewManager<ActorLogic>
    {
        public ActorData ActorData;
        public List<GameObject> WarriorPrefab = new List<GameObject>();
    }
}