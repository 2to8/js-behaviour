using GameEngine.Kernel;

namespace GameEngine.Controllers {

public class ExitSceneCommand : Controller<ExitSceneCommand> {

    public override void Execute(object data)
    {
        //ObjectPool.Instance.UnspawnAll();
    }

}

}