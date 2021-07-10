namespace GameEngine.Contacts {

public interface IState {

    IProvider Provider { get; set; }

    void OnActivate();

    void OnDeactivate();

    void OnUpdate();

}

}