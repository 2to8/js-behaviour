namespace GameEngine.Controllers.Contracts {

public interface IStateBase {

    void OnActivate();

    void OnDeactivate();

    void OnUpdate();

    string ToString();

}

}