namespace GameEngine.Controllers.Contracts {

public abstract class State<T, C> : Controller<C>, IStateBase where T : State<T, C> where C : Controller<C> {

    public virtual void OnActivate() { }

    public virtual void OnDeactivate() { }

    public virtual void OnUpdate() { }

}

}