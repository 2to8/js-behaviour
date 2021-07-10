using System.Collections.Generic;
using UniRx.Async;

namespace GameEngine.Contacts {

public interface IController : IApplication {

    IProvider Provider { get; set; }
    List<IState> States { get; set; }

    void Execute(object data);

    void RegisterViewEvents();

    UniTask InitController(IProvider provider);

}

}