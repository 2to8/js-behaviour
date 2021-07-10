using GameEngine.Contacts;
using SQLite.Attributes;
using System;
using UniRx.Async;
using UnityEngine;

#pragma warning disable 1998

namespace GameEngine.Kernel {

public class StateModel<T, TV> : Model<T>, IState where T : StateModel<T, TV>, new() where TV : IProvider {

    [SerializeField]
    TV m_Provider;

    [Ignore]
    public TV Self => (TV)Provider;

    [Ignore]
    public IProvider Provider { get => m_Provider; set => m_Provider = (TV)value; }

    public virtual void OnActivate() { }

    public virtual void OnDeactivate() { }

    public virtual void OnUpdate() { }

    public virtual async UniTask Awake()
    {
        // await base.Awake();
        // if(m_Provider == null) {
        //     m_Provider = GetComponentInParent<TV>();
        // }

        if (m_Provider != null && !m_Provider.States.Contains((T)this)) {
            m_Provider.States.Add((T)this);
        }
    }

    public void SetState(Type newStateType)
    {
        Self.SetState(newStateType);
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