using System;

namespace Common.EditorHelpers {

[AttributeUsage(AttributeTargets.Class)]
public class ScriptExecutionOrderAttribute : Attribute {
    int order = 0;

    public ScriptExecutionOrderAttribute(int order)
    {
        this.order = order;
    }

    public int GetOrder() => order;
}

}