using GameEngine.Models.Contracts;
using GameEngine.Utils;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Kernel._Appliation {

[CreateAssetMenu(fileName = "Artecs_Config", menuName = "Models/Config", order = 0)]
public class Config : DbTable<Config> {

    public MyCustomDict<GameObject> Prefabs = new MyCustomDict<GameObject>();

    [OdinSerialize]
    public Dictionary<string, GameObject> tests = new Dictionary<string, GameObject>();

}

}