using GameEngine.Kernel;
using UnityEngine;

namespace GameEngine.Controllers {

[CreateAssetMenu(fileName = "TestController", menuName = "Controllers/TestController", order = 0)]
public class TestController : Controller<TestController> {

    public string PrefabName;

    public override void Execute(object data) { }

}

}