using GameEngine.Contacts;
using Sirenix.OdinInspector;
using System;
using UniRx.Async;
using UnityEngine;

namespace GameEngine.Kernel {

// public abstract class State<T> : View<T> where T : State<T>, IState
// {
//     [ShowInInspector]
//     public IProvider Provider { get; set; }
//
//     protected override void OnAwake() {
//         if(Provider == null) {
//             Provider = GetComponentInParent<IProvider>();
//         }
//     }
// }

public abstract class State<T, TV> : View<T>, IState where T : State<T, TV> where TV : IProvider {

    [SerializeField]
    TV m_Provider;

    public TV Self => (TV)Provider;

    public virtual void OnActivate() { }

    public virtual void OnDeactivate() { }

    public virtual void OnUpdate() { }

    [ShowInInspector]
    public virtual IProvider Provider { get => m_Provider; set => m_Provider = (TV)value; }

    protected override async UniTask Awake()
    {
        await base.Awake();

        if (m_Provider == null) {
            m_Provider = GetComponentInParent<TV>();
        }

        if (!m_Provider.States.Contains((T)this)) {
            m_Provider.States.Add((T)this);
        }
    }

    public void SetState(Type newStateType)
    {
        Provider.SetState(newStateType);
    }

    public void SetState(IState state)
    {
        SetState(state.GetType());
    }

    public void SetState<TA>()
    {
        SetState(typeof(TA));
    }

}

}