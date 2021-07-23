using GameEngine.Attributes;
using GameEngine.Kernel;
using GameEngine.Views.Contracts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Controllers.Contracts {

public abstract class Controller<T, M> : Controller<T> where T : Controller<T, M> where M : Views.Contracts.BaseView<M> {

    [OdinSerialize, ValueDropdown(nameof(StateTypes))]
    public override Type CurrentState { get; set; } = typeof(M).GetCustomAttribute<DefaultStateAttribute>()?.type ??
        typeof(T).GetCustomAttribute<DefaultStateAttribute>()?.type ?? StateTypes().FirstOrDefault();

    static IEnumerable<Type> StateTypes() => Core.StateTypes<M>();

}

public abstract class Controller<T> : Controller where T : Controller<T> { }

//SerializedScriptableObject { }

/// <summary>
/// 控制层
/// </summary>
///
public abstract class Controller : ControllerBase {

    readonly Dictionary<Type, IStateBase> States = new Dictionary<Type, IStateBase>();
    public virtual Type CurrentState { get; set; }
    public IStateBase State => States.TryGetValue(CurrentState, out var stateBase) ? stateBase : null;

    public void SetState(Type newStateType = null)
    {
        if (newStateType != null) {
            CurrentState = newStateType;
        }

        State?.OnDeactivate();

        // CurrentState = GetComponentInChildren(newStateType) as _StatesBase;
        State?.OnActivate();
    }

}

}