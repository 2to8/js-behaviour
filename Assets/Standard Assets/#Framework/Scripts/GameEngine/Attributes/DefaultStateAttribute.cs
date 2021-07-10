using System;

namespace GameEngine.Attributes {

[AttributeUsage(AttributeTargets.Class)]
public class DefaultStateAttribute : Attribute {

    public DefaultStateAttribute(Type _type)
    {
        type = _type;
    }

    public Type type { get; set; }

    // public DefaultStateAttribute(object obj) {
    //     var asm = Assembly.GetAssembly(obj.GetType());
    //     type = asm.GetType(obj.ToString());
    // }

}

}