using GameEngine.Contacts;
using Sirenix.Utilities;
using System;
using System.Reflection;
using UnityEngine;

namespace GameEngine.Kernel.Attributes {

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class Events : Attribute {

    public Events(object obj)
    {
        EnumObj = obj;
        EventName = obj.GetType().FullName + "." + obj;
    }

    public Events(string eventName)
    {
        EventName = eventName;
    }

    //public Events() { }
    public Events(Type eventName)
    {
        EventName = eventName.Name;
    }

    public string EventName { get; set; }
    public object EnumObj { get; set; }

    public static void Dispatch(string eventName, object target, object sender, object data = null)
    {
        if (target is IState state) {
            state.GetType()
                .GetCustomAttributes<Events>()
                .ForEach(attr => {
                    if (eventName == attr.EventName) {
                        state.Provider.State = state;
                    }
                });
        }

        if (target is IView view) {
            view.controllers.ForEach(tv => {
                doEvent(eventName, tv, sender, data);
            });
        }

        doEvent(eventName, target, sender, data);
    }

    protected static void doEvent(string eventName, object target, object sender, object data = null)
    {
        target.GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .ForEach(info => {
                info.GetCustomAttributes<Events>()
                    .ForEach(attr => {
                        if (eventName == attr.EventName) {
                            try {
                                Debug.Log(
                                    $"Dispatch Event: <color=green>{eventName}</color> from <color=green>{target.GetType().FullName}</color>");

                                info.Invoke(target, new[] { sender, data });
                            } catch (TargetInvocationException ex) {
                                if (ex.InnerException != null) {
                                    throw ex.InnerException;
                                }
                            }
                        }
                    });
            });
    }

}

}