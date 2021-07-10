using GameEngine.Controllers.Contracts;

namespace GameEngine.Controllers.Commands {

public class ExitSceneCmd : Controller<ExitSceneCmd> {

    public override void Execute(object data)
    {
        //ObjectPool.Instance.UnspawnAll();
    }

}

}