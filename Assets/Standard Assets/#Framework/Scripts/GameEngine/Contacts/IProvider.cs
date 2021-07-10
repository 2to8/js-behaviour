using System;
using System.Collections.Generic;

namespace GameEngine.Contacts {

public interface IProvider : IView {

    List<IState> States { get; set; }
    IState State { get; set; }
    IState Default { get; set; }

    void SetState(Type newStateType);

    void SetState(IState state);

    void SetState<T>();

}

}