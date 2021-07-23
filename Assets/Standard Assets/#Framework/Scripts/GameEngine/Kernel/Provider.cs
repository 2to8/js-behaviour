using GameEngine.Contacts;
using GameEngine.Kernel.Attributes;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;

namespace GameEngine.Kernel {

public class Provider<T> : BaseView<T>, IProvider where T : Provider<T> {

    IState currentState;

    [FoldoutGroup("Core"), SerializeField]
    IState defaultState;

    // public List<ScriptableObject> controllers = new List<ScriptableObject>();
    // public List<ScriptableObject> models = new List<ScriptableObject>();

    [FoldoutGroup("Core"), ShowInInspector]
    public List<IState> States { get; set; } = new List<IState>();

    [FoldoutGroup("Core"), ShowInInspector]
    public IState State { get => currentState; set => SetState(value); }

    [FoldoutGroup("Core")]
    public IState Default { get => defaultState; set => defaultState = value; }

    //public Dictionary<Type, IState> States = new Dictionary<Type, IState>();

    //Changes the current game state
    public void SetState(Type newStateType)
    {
        if (newStateType == null) {
            return;
        }

        currentState?.OnDeactivate();

        currentState = newStateType == typeof(IdleState)
            ? IdleState.Instance
            : States.FirstOrDefault(t => t.GetType() == newStateType);

        if (currentState == null) {
            if (newStateType.IsAssignableFrom(typeof(IModel))) {
                var prop = newStateType.GetProperty("Instance");

                if (prop != null && prop.GetValue(null) is IState state) {
                    currentState = state;
                }
            } else if (newStateType.IsAssignableFrom(typeof(Component))) {
                currentState = gameObject.AddComponent(newStateType) as IState;
            }
        }

        if (currentState == null) {
            Debug.LogError($"{newStateType.Name} not found", gameObject);
        } else {
        #if LOG_STATE
                Debug.Log($"<color=green>{newStateType.Name}</color> changed", gameObject);
        #endif
            if (!States.Contains(currentState)) {
                States.Add(currentState);
            }
        }

        currentState?.OnActivate();
    }

    public void SetState(IState state)
    {
        SetState(state.GetType());
    }

    public void SetState<TA>()
    {
        SetState(typeof(TA));
    }

    protected override async Task Awake()
    {
        await base.Awake();
        DefaultState.Dispatch(this);

        if (currentState == null && defaultState != null) {
            SetState(defaultState);
        }
    }

    protected override async UniTask Update()
    {
        await base.Update();
        currentState?.OnUpdate();
    }

}

}