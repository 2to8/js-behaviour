using Sirenix.Utilities;
using System;
using System.Reflection;
using UnityEngine;

namespace GameEngine.Attributes {

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class Inject : Attribute {

    public string name;
    public Type type;

    public Inject(string _name)
    {
        name = _name;
    }

    public Inject(Type _type, string _name = null)
    {
        type = _type;

        if (!_name.IsNullOrWhitespace()) {
            name = _name;
        }
    }

    public static void Dispatch()
    {
        var changed = false;

        Assembly.GetExecutingAssembly()
            .GetCustomAttributes<Inject>()
            .ForEach(t => {
                Debug.Log(t.type);
            });
    }

}

}