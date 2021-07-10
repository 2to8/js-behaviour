using System;

namespace GameEngine.Attributes {

[AttributeUsage(AttributeTargets.All)]
public class DontGenAttribute : Attribute {

    public DontGenAttribute() { }

}

}