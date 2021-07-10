using GameEngine.Contacts;
using System;
using System.Reflection;
using UnityEngine;

namespace GameEngine.Kernel.Attributes {

[AttributeUsage(AttributeTargets.All)]
public class DefaultState : Attribute {

    public Type Type;

    public DefaultState(Type type)
    {
        Type = type;
    }

    public static void Dispatch(IProvider provider)
    {
        if (provider.GetType().GetCustomAttribute<DefaultState>() is DefaultState attr) {
            if (provider.Default == null) {
                var t = (provider as MonoBehaviour)?.transform;

                provider.Default = t?.GetComponent(attr.Type) as IState ??
                    t?.gameObject.AddComponent(attr.Type) as IState;
            }
        }
    }

}

}