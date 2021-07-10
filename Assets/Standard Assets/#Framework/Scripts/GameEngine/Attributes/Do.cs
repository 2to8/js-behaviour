using System;

namespace GameEngine.Attributes {

public static class Do {

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class InjectAttribute : Attribute {

        public Type type { get; set; }

    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class EventAttribute : Attribute {

        public string EventName;
        public object EventObj;

        public EventAttribute(object name)
        {
            EventObj = name;
            EventName = Core.GetName(name);
        }

        public EventAttribute(string name)
        {
            EventName = name;
        }

        public Type type { get; set; }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateAttribute : Attribute {

        public Type type { get; set; }

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OutputAttribute : Attribute {

        public OutputAttribute(Type type)
        {
            this.type = type;
        }

        public Type type { get; set; }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EnableAttribute : Attribute {

        public Type type { get; set; }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DisableAttribute : Attribute {

        public Type type { get; set; }

    }

}

}