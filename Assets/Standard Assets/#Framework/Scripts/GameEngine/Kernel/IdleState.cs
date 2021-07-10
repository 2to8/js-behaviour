using GameEngine.Contacts;

namespace GameEngine.Kernel {

public class IdleState : State<IdleState, IdleProvider>, IState {

    public override void OnActivate() { }

    public override void OnDeactivate() { }

    public override void OnUpdate() { }

    public override IProvider Provider { get; set; }

}

}