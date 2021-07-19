using Sirenix.OdinInspector;

namespace Common
{
    public partial class ComponentBase : SerializedMonoBehaviour { }

    public partial class Component<T> : ComponentBase where T : Component<T> { }
}