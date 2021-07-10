using GameEngine.Models.Contracts;
using GameEngine.Utils;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Models {

[CreateAssetMenu(fileName = "TestModel", menuName = "Tests/TestModel", order = 0)]
public class TestDbTable : DbTable<TestDbTable> {

    [OdinSerialize]
    public Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

    public MyCustomDict<GameObject> tests = new MyCustomDict<GameObject>();

}

}