using Sirenix.Utilities;
using UnityEngine;

namespace Helpers {

[GlobalConfig("Resources/MyConfigFiles/")]
public class ReplaceGameObjectsConfig : GlobalConfig<ReplaceGameObjectsConfig> {
    public GameObject prefab;
}

}